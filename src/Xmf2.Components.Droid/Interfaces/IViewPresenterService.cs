using Android.Content;
using Xmf2.Components.Interfaces;
using Android.Support.V4.App;
using Android.Support.V7.App;
using Xmf2.Components.Droid.Fragments;

namespace Xmf2.Components.Droid.Interfaces
{
	public interface IViewPresenterService : IPresenterService
	{
		bool IsCurrentViewFor<TViewModel>();

		void Show(Intent intent);
		void ShowView<TActivity>(bool clearHistory = false) where TActivity : AppCompatActivity;
		void ShowView(DialogFragment fragment, string tag);
		void ShowView<TFragment>(string tag) where TFragment : DialogFragment, new();

		void ShowFragment<TActivity, TFragment>() where TFragment : Fragment, new() where TActivity : AppCompatActivity, IFragmentActivity;

		void ShowFragment<TActivity, TRootFragment, TFragment>()
			where TRootFragment : Fragment, new()
			where TFragment : Fragment, new()
			where TActivity : AppCompatActivity, IFragmentActivity;

		void ShowFragment<TActivity, TRootFragment, TFragment, TFragmentBis>(bool forceCreate = false)
			where TRootFragment : Fragment, new()
			where TFragment : Fragment, new()
			where TFragmentBis : Fragment, new()
			where TActivity : AppCompatActivity, IFragmentActivity;

		void ShowFragment<TActivity, TRootFragment, TFragment, TFragmentBis, TFragmentTer>(bool forceCreateFirst = false, bool forceCreateSecond = false)
			where TRootFragment : Fragment, new()
			where TFragment : Fragment, new()
			where TFragmentBis : Fragment, new()
			where TFragmentTer : Fragment, new()
			where TActivity : AppCompatActivity, IFragmentActivity;

		void ShowRootFragment<TActivity, TFragment>() where TFragment : Fragment, new() where TActivity : AppCompatActivity, IFragmentActivity;


		void PushFragment<TActivity, TFragment>() where TFragment : Fragment, new() where TActivity : AppCompatActivity, IFragmentActivity;
		void ReplaceFragment<TActivity, TFragment>() where TFragment : Fragment, new() where TActivity : AppCompatActivity, IFragmentActivity;
	}
}