namespace Xmf2.iOS.Controls.ItemControls
{
	public interface IUIComponent
	{
		void AutoLayout();
		void Bind();
		void ViewDidLoad();
		void ViewDidAppear();
		void ViewWillDisappear();
		void ViewDidDisappear();
	}

	public interface IUIModelComponent<TModel> : IUIComponent where TModel : class
	{
		TModel Model { get; set; }
	}
}
