using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Xmf2.Commons.Caches
{
    public class LoaderCache<T>
    {
        /// <summary>
        /// Durée de validité du cache en second
        /// </summary>
        private readonly int _cacheDurationInSecond;
        /// <summary>
        /// Fonction permettant de loader la donnée
        /// </summary>
        private Func<CancellationToken, Task<T>> _loaderFunc;

        /// <summary>
        /// Lock général du cache
        /// </summary>
        private object _lockObject = new object();
        /// <summary>
        /// 
        /// </summary>
        private Task<T> _currentLoadTask;
        private long _lastLoadNumber = 0;
        private long _lastInvalidateOnloadNumber = 0;

        private long _currentValueLoadNumber = 0;
        private T _currentValue;
        private DateTime _currentValueInvalidationDate = DateTime.MinValue;

        public LoaderCache(Func<CancellationToken, Task<T>> loaderFunc, int cacheDurationInSecond = 60)
        {
            _loaderFunc = loaderFunc;
            _cacheDurationInSecond = cacheDurationInSecond;
        }

        public async Task<T> Get(CancellationToken ct, bool withInvalidation = false)
        {
            if (withInvalidation)
                this.Invalidate();

            Task<T> currentLoadTask = null;
            lock (_lockObject)
            {
                // La donnée en cache est encore bonne
                if (DateTime.Now < _currentValueInvalidationDate)
                {
                    return _currentValue;
                }

                // S'il n'y a pas de tache de load en attente on en crée une
                if (_currentLoadTask == null)
                {
                    _currentLoadTask = this.Load(ct);
                }
                currentLoadTask = _currentLoadTask;
            }

            return await currentLoadTask;
        }

        private async Task<T> Load(CancellationToken ct)
        {
            _lastLoadNumber++;
            var currentLoadNumber = _lastLoadNumber;

            return await Task.Run<T>(async () =>
            {
                T loadedValue = await _loaderFunc.Invoke(ct).ConfigureAwait(false);

                lock (_lockObject)
                {
                    // Le cache n'a pas été invalidé et une tache de load plus récente n'a pas terminé avant
                    if (_lastInvalidateOnloadNumber < currentLoadNumber
                        && _currentValueLoadNumber < currentLoadNumber)
                    {
                        _currentValueLoadNumber = currentLoadNumber;
                        _currentValue = loadedValue;
                        _currentValueInvalidationDate = DateTime.Now.AddSeconds(_cacheDurationInSecond);
                    }

                    _currentLoadTask = null;
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
                _currentLoadTask = null;
                _lastInvalidateOnloadNumber = _lastLoadNumber;
            }
        }
    }
}
