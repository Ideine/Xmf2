using Foundation;
using UIKit;
using Xmf2.Core.iOS.Extensions;
using Xmf2.iOS.Extensions.Constraints;
using Xmf2.iOS.Extensions.Extensions;
using static UIKit.NSLayoutAttribute;
using static UIKit.NSLayoutRelation;

namespace Xmf2.Components.iOS.TableView
{
	public class TableViewItemCell : UITableViewCell
	{
		public const string CELL_IDENTIFIER = nameof(TableViewItemCell);
		public static readonly NSString NsCellIdentifier = new(CELL_IDENTIFIER);

		private NSLayoutConstraint[] _childConstraints;
		private UIView _child;

#if NET7_0_OR_GREATER
		protected TableViewItemCell(ObjCRuntime.NativeHandle handle) : base(handle) { }
#else
		protected TableViewItemCell(System.IntPtr handle) : base(handle) { }
#endif

		public void SetContent(UIView child)
		{
			if (_child == child)
			{
				return;
			}

			ContentView.EnsureRemove(_childConstraints);
			ContentView.EnsureRemove(_child);

			if (child != null)
			{
				_child = child;
				_childConstraints = InstantiateConstraints();

				ContentView.EnsureAdd(child)
					.EnsureAdd(_childConstraints);

				child.TranslatesAutoresizingMaskIntoConstraints = false;
			}

			ContentView.WithBackgroundColor(UIColor.Clear);
			this.WithBackgroundColor(UIColor.Clear);
		}

		public virtual NSLayoutConstraint[] InstantiateConstraints()
		{
			NSLayoutConstraint vConstraint1 = NSLayoutConstraint.Create(ContentView, CenterY, Equal, _child, CenterY, 1f, 0f).WithAutomaticIdentifier();
			NSLayoutConstraint vConstraint2 = NSLayoutConstraint.Create(ContentView, Height, Equal, _child, Height, 1f, 0f).WithAutomaticIdentifier();
			//La priorité de la contraint doit être inférieur à 1000 (Required)...
			//...autrement de nombreuses erreurs de contrainte peuvent s'afficher...
			//... au layout, see https://stackoverflow.com/questions/24984650/autolayout-breaks-constraints-when-layoutifneeded-is-called
			vConstraint1.Priority = (float)UILayoutPriority.DefaultHigh;
			vConstraint2.Priority = (float)UILayoutPriority.DefaultHigh;
			return new[]
			{
				NSLayoutConstraint.Create(ContentView, CenterX, Equal, _child, CenterX, 1f, 0f).WithAutomaticIdentifier(),
				NSLayoutConstraint.Create(ContentView, Width, Equal, _child, Width, 1, 0f).WithAutomaticIdentifier(),
				vConstraint1,
				vConstraint2
			};
		}
	}
}