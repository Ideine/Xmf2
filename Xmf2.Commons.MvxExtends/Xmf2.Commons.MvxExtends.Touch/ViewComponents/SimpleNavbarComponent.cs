using MvvmCross.Binding.BindingContext;
using UIKit;
using Xmf2.Commons.MvxExtends.ViewModels;

namespace Xmf2.Commons.MvxExtends.Touch.ViewComponents
{
	public class SimpleNavbarComponent : BaseUIComponent<BaseViewModel<object>>
	{
		private readonly UIButton _backButton;
		private readonly UILabel _titleLabel;
		private readonly UIView _navBarSeparator;

		public UIColor SeparatorColor
		{
			get { return _navBarSeparator.BackgroundColor; }
			set { _navBarSeparator.WithBackgroundColor(value); }
		}

		public UIColor TextColor
		{
			get { return _titleLabel.TextColor; }
			set { _titleLabel.WithTextColor(value); }
		}

		public UIFont TextFont
		{
			get { return _titleLabel.Font; }
			set { _titleLabel.WithFont(value); }
		}

		public string Title
		{
			get { return _titleLabel.Text; }
			set { _titleLabel.WithText(value); }
		}

		public SimpleNavbarComponent(string titleText, string backImage)
		{
			this.WithBackgroundColor(UIColor.Clear);
			_navBarSeparator = this.CreateView().WithBackgroundColor(UIColor.LightGray);
			_backButton = this.CreateButton().WithImage(backImage);
			_titleLabel = this.CreateLabel()
							  .WithSystemFont(16, UIFontWeight.Medium)
							  .WithAlignment(UITextAlignment.Center)
							  .WithTextColor(UIColor.Black)
							  .WithText(titleText);
			AddSubviews(_titleLabel, _backButton, _navBarSeparator);
		}

		public override void AutoLayout()
		{
			base.AutoLayout();

			_navBarSeparator.ConstrainHeight(1);

			this.CenterVertically(_backButton)
				.CenterVertically(_titleLabel)
				.AnchorBottom(_navBarSeparator)
				.AnchorLeft(_backButton)
				.CenterAndFillWidth(_titleLabel, _navBarSeparator);

			_backButton.ConstrainWidth(43).ConstrainHeight(45);
			this.ConstrainHeight(46);
		}

		public override void Bind()
		{
			base.Bind();

			var binding = this.CreateBindingSet<SimpleNavbarComponent, BaseViewModel<object>>();

			binding.Bind(_backButton).To(vm => vm.CloseCommand);

			binding.Apply();
		}
	}
}
