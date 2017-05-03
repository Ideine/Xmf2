namespace MvvmCross.Platform
{
	public static class MvxExtensions
	{
		public static TService ResolveOrDefault<TService>(TService defaultValue = default(TService)) where TService : class
		{
			TService service;
			if (Mvx.TryResolve(out service))
			{
				return service;
			}
			return defaultValue;
		}
	}
}
