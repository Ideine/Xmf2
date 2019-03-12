using Android.OS;
using Android.Views;
using Android.Graphics;
using Xmf2.Core.Droid.Helpers;
using Xmf2.Core.Subscriptions;
using Android.Graphics.Drawables;

namespace Xmf2.Core.Droid.Extensions
{
    public static class FeedbackExtensions
    {
        #region Elevation highlight

        public static T WithElevationHighlight<T>(this T view, Xmf2Disposable disposer, int highlightElevationInDp = 4) where T : View
        {
            var highlightElevation = UIHelper.DpToPx(view.Context, highlightElevationInDp);

            view.Elevation = highlightElevation;

            new EventSubscriber<View>(
                view,
                btn => btn.SetOnTouchListener(new TouchViewListener<View>(btn).DisposeWith(disposer).WithTouchHighlight(FromHighlight, ToHighlight)),
                btn => btn.SetOnTouchListener(null)
            ).DisposeEventWith(disposer);

            void ToHighlight(View highlightView)
            {
                highlightView.ScaleX = highlightView.ScaleY = 0.95f;
                SetElevation(highlightView, 0);
            }

            void FromHighlight(View highlightView)
            {
                highlightView.ScaleX = highlightView.ScaleY = 1f;
                SetElevation(highlightView, highlightElevation);
            }

            void SetElevation(View elevationView, int value)
            {
                if (Build.VERSION.SdkInt >= BuildVersionCodes.Lollipop)
                {
                    elevationView.Elevation = value;
                }
            }

            return view;
        }

        #endregion

        #region Background highlight 

        public static T WithFadeHighlight<T>(this T view, Xmf2Disposable disposer) where T : View
        {
            new EventSubscriber<View>(
                view,
                btn => btn.SetOnTouchListener(new TouchViewListener<View>(btn).DisposeWith(disposer).WithTouchHighlight(FromFadeHighlight, ToFadeHighlight)),
                btn => btn.SetOnTouchListener(null)
            ).DisposeEventWith(disposer);

            void ToFadeHighlight(View highlightView) => highlightView.Alpha = 0.58f;
            void FromFadeHighlight(View highlightView) => highlightView.Alpha = 1f;

            return view;
        }

        public static void SetBackgroundWithHighlight(this View view, Color backgroundColor, Color highlightColor, Xmf2Disposable disposer)
        {
            var st = new StateListDrawable();

            st.AddState(new int[] { Android.Resource.Attribute.StatePressed }, new ColorDrawable(highlightColor).DisposeWith(disposer));
            st.AddState(new int[] { }, new ColorDrawable(backgroundColor).DisposeWith(disposer));

            view.Background = st;
        }

        public static void SetBackgroundWithHighlight(this View view, Drawable background, Drawable highlight, Drawable disabled = null)
        {
            var st = new StateListDrawable();

            st.AddState(new int[] { Android.Resource.Attribute.StatePressed }, highlight);
            st.AddState(new int[] { -Android.Resource.Attribute.StateEnabled }, disabled);
            st.AddState(new int[] { }, background);

            view.Background = st;
        }

        #endregion
    }
}
