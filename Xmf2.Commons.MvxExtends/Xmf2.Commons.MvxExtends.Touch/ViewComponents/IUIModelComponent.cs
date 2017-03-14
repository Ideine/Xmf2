namespace Xmf2.Commons.MvxExtends.Touch.ViewComponents
{

	public interface IUIModelComponent<TModel> : IUIComponent where TModel : class
	{
		TModel Model { get; set; }
	}
}
