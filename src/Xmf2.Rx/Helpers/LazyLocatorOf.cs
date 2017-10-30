using Splat;
using System;

namespace Xmf2.Rx.Helpers
{
	public class LazyLocatorOf<TService>
    {
		private readonly Lazy<TService> _service = new Lazy<TService>(Locator.Current.GetService<TService>);
		public TService Value { get => _service.Value; }
    }
}
