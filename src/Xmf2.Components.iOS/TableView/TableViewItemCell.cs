using System;
using Foundation;
using UIKit;

using static UIKit.NSLayoutAttribute;
using static UIKit.NSLayoutRelation;

namespace Xmf2.Components.iOS.TableView
{
	public class TableViewItemCell : UITableViewCell
	{
		public const string CELL_IDENTIFIER = nameof(TableViewItemCell);
		public static readonly NSString NsCellIdentifier = new NSString(CELL_IDENTIFIER);

		private NSLayoutConstraint[] _childConstraints;
		private UIView _child;

		protected TableViewItemCell(IntPtr handle) : base(handle) { }

		public void SetContent(UIView child)
		{
			if(_child == child)
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
			var vConstraint1 = NSLayoutConstraint.Create(ContentView, CenterY, Equal, _child, CenterY, 1f, 0f).WithAutomaticIdentifier();
			var vConstraint2 = NSLayoutConstraint.Create(ContentView, Height, Equal, _child, Height, 1f, 0f).WithAutomaticIdentifier();
			//La priorité de la contraint doit être inférieur a 1000 (Required)...
			//...autrement de nombreuses erreur de contrainte peuvent s'afficher...
			//... au layout see https://stackoverflow.com/questions/24984650/autolayout-breaks-constraints-when-layoutifneeded-is-called
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