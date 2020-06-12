using System;
using System.Threading;
using System.Threading.Tasks;

namespace Xmf2.Commons.Caches
{
	public abstract class LoaderParameters
    {
        public abstract bool ParametersAreTheSameAs(LoaderParameters thoseOnes);
    }

    public class LoaderWithParametersCache<T, TParams> where TParams : LoaderParameters
    {
        /// <summary>
        /// Durée de validité du cache en second
        /// </summary>
        private readonly int _cacheDurationInSecond;
        /// <summary>
        /// Fonction permettant de loader la donnée
        /// </summary>
        private Func<CancellationToken, TParams, Task<T>> _loaderFunc;

        /// <summary>
        /// Lock général du cache
        /// </summary>
        private object _lockObject = new object();

        private Task<T> _currentLoadTask;
        private TParams _currentLoadParams;
        private long _lastLoadNumber = 0;
        private long _lastInvalidateOnloadNumber = 0;

        private long _currentValueLoadNumber = 0;
        private T _currentValue;
        private TParams _currentValueParams;
        private DateTime _currentValueInvalidationDate = DateTime.MinValue;

        public LoaderWithParametersCache(Func<CancellationToken, TParams, Task<T>> loaderFunc, int cacheDurationInSecond = 60)
        {
            _loaderFunc = loaderFunc;
            _cacheDurationInSecond = cacheDurationInSecond;
        }

        public async Task<T> Get(CancellationToken ct, TParams loaderParams, bool withInvalidation = false)
        {
            if (withInvalidation)
            {
	            this.Invalidate();
            }

            Task<T> currentLoadTask = null;
            lock (_lockObject)
            {
                // La donnée en cache est encore bonne
                if (DateTime.Now < _currentValueInvalidationDate
                    && loaderParams.ParametersAreTheSameAs(_currentValueParams))
                {
                    return _currentValue;
                }

                // S'il n'y a pas de tache de load en attente on en crée une
                if (_currentLoadTask == null
                    || !loaderParams.ParametersAreTheSameAs(_currentValueParams))
                {
                    _currentLoadTask = this.Load(ct, loaderParams);
                }
                currentLoadTask = _currentLoadTask;
            }

            return await currentLoadTask;
        }

        private async Task<T> Load(CancellationToken ct, TParams loaderParams)
        {
            _lastLoadNumber++;
            var currentLoadNumber = _lastLoadNumber;
            _currentLoadParams = loaderParams;

            return await Task.Run<T>(async () =>
            {
                T loadedValue = default(T);
                try
                {
                    loadedValue = await _loaderFunc.Invoke(ct, loaderParams).ConfigureAwait(false);
                }
                catch
                {
                    lock (_lockObject)
                    {
                        // Le cache n'a pas été invalidé et une tache de load plus récente n'a pas terminé avant
                        if (_lastInvalidateOnloadNumber < currentLoadNumber
                            && _currentValueLoadNumber < currentLoadNumber)
                        {
                            _currentLoadTask = null;
                        }
                    }

                    throw;
                }

                lock (_lockObject)
                {
                    // Le cache n'a pas été invalidé et une tache de load plus récente n'a pas terminé avant
                    if (_lastInvalidateOnloadNumber < currentLoadNumber
                        && _currentValueLoadNumber < currentLoadNumber)
                    {
                        _currentValueLoadNumber = currentLoadNumber;
                        _currentValue = loadedValue;
                        _currentValueParams = loaderParams;
                        _currentValueInvalidationDate = DateTime.Now.AddSeconds(_cacheDurationInSecond);
                    }

                    _currentLoadTask = null;
                    _currentLoadParams = null;
                }

                return loadedValue;
            });
        }

        public void Invalidate()
        {
            lock (_lockObject)
            {
                _currentValue = default(T);
                _currentValueInvalidationDate = DateTime.MinValue;
                _currentValueParams = null;
                _currentLoadParams = null;
                _currentLoadTask = null;
                _lastInvalidateOnloadNumber = _lastLoadNumber;
            }
        }
    }
}
