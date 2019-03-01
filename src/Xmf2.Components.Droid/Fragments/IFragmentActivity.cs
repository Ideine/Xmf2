using Android.Support.V4.App;

namespace Xmf2.Components.Droid.Fragments
{
	public interface IFragmentActivity
	{
		void ShowFragment<TFragment>(bool addToBackstack = true)
			where TFragment : Fragment, new();
	}
}