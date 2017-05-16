using MvvmCross.Binding.BindingContext;
using UIKit;
using Xmf2.Commons.MvxExtends.ViewModels;

namespace Xmf2.Commons.MvxExtends.Touch.ViewComponents
{
	public class SimpleNavbarComponent : BaseUIComponent<BaseViewModel>
	{
		private readonly UIButton _backButton;
		private readonly UILabel _titleLabel;
		private readonly UIView _navBarSeparator;
		private readonly UIView _container;

		public UIColor SeparatorColor
		{
			get { return _navBarSeparator.BackgroundColor; }
			set { _navBarSeparator.WithBackgroundColor(value); }
		}

		public UIColor TextColor
		{
			get { return _titleLabel.TextColor; }
			set
			{
				_titleLabel.WithTextColor(value);
				_backButton.WithTextColor(value);
			}
		}

		public UIColor TextColorHighlight
		{
			get { return _backButton.TitleColor(UIControlState.Highlighted); }
			set
			{
				_backButton.WithTextColorHighlight(value);
			}
		}

		public UIFont TextFont
		{
			get { return _titleLabel.Font; }
			set { _titleLabel.WithFont(value); }
		}

		public UIFont ButtonFont
		{
			get { return _backButton.Font; }
			set { _backButton.WithFont(value); }
		}

		public string Title
		{
			get { return _titleLabel.Text; }
			set { _titleLabel.WithText(value); }
		}

		public SimpleNavbarComponent(string titleText, string backImage, string backText = "")
		{
			this.WithBackgroundColor(UIColor.Clear);
			_navBarSeparator = this.CreateView().WithBackgroundColor(UIColor.LightGray);
			_container = this.CreateView();
			_backButton = this.CreateButton()
			                  .WithTitle(backText ?? string.Empty);
			if (backImage != null)
			{
				_backButton.WithImage(backImage);
			}
			
			_backButton.TitleEdgeInsets = new UIEdgeInsets(0, 0, 0, 0);
			_backButton.HorizontalAlignment = UIControlContentHorizontalAlignment.Left;

			_titleLabel = this.CreateLabel()
							  .WithSystemFont(16, UIFontWeight.Medium)
							  .WithAlignment(UITextAlignment.Center)
							  .WithTextColor(UIColor.Black)
							  .WithText(titleText);
			_container.AddSubviews(_titleLabel, _backButton);
			AddSubviews(_container, _navBarSeparator);
		}

		public override void AutoLayout()
		{
			base.AutoLayout();

			_navBarSeparator.ConstrainHeight(1);

			_container.CenterVertically(_backButton)
					  .CenterVertically(_titleLabel)
					  .AnchorLeft(_backButton, 10)
					  .CenterAndFillWidth(_titleLabel);
			this.AnchorTop(_container, 20)
			    .VerticalSpace(_container, _navBarSeparator)
				.AnchorBottom(_navBarSeparator)
				.AnchorLeft(_container)
				.CenterAndFillWidth(_container, _navBarSeparator);

			_backButton.ConstrainHeight(45).ConstrainWidth(100);
			this.ConstrainHeight(65);
		}

		public override void Bind()
		{
			base.Bind();

			var binding = this.CreateBindingSet<SimpleNavbarComponent, BaseViewModel>();

			binding.Bind(_backButton).To(vm => vm.CloseCommand);

			binding.Apply();
		}
	}
}
