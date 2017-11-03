namespace Xmf2.Commons.Droid.Helpers.CustomAnimations
{
	public class RevealValueProperty : Android.Util.Property
	{
		public RevealValueProperty() : base(Java.Lang.Class.FromType(typeof(Java.Lang.Float)),nameof(IRevealValues.Percentage))
		{
			
		}

		protected RevealValueProperty(System.IntPtr javaReference, Android.Runtime.JniHandleOwnership transfer) : base(javaReference, transfer)
		{
			
		}

		public override void Set(Java.Lang.Object source, Java.Lang.Object value)
		{
			if (!(source is IRevealValues item))
			{
				return;
			}
			Java.Lang.Float f = (Java.Lang.Float) value;
			float val = f.FloatValue();
			item.Percentage = val;
			(item.Target.Parent as IRevealViewGroup)?.ForceDraw();
		}

		public override Java.Lang.Object Get(Java.Lang.Object source)
		{
			if (!(source is IRevealValues item))
			{
				return null;
			}
			return item.Percentage;
		}
	}
}