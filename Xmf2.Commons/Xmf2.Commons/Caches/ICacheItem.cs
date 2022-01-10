using System.Threading;
using System.Threading.Tasks;

namespace Xmf2.Commons.Caches
{
	public interface ICacheItem
	{
		void Invalidate();
	}

	public interface ICacheItem<T> : ICacheItem
	{
		T Value { get; }

		Task<T> Load(bool force = false);

		Task<T> Load(CancellationToken ct, bool force = false);
	}

	public interface ICacheItem<TValue, in TParam> : ICacheItem
	{
		TValue Value { get; }

		Task<TValue> Load(TParam param, bool force = false);

		Task<TValue> Load(TParam param, CancellationToken ct, bool force = false);
	}
}