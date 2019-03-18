using Android.OS;
using Android.Views;
using Android.Graphics;
using Xmf2.Core.Droid.Helpers;
using Xmf2.Core.Subscriptions;
using Android.Graphics.Drawables;
using Android.Widget;
using Android.Content.Res;

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

        public static void SetBackgroundWithHighlight(this View view, Drawable background, Drawable highlight, Drawable selected = null, Drawable selectedHighlight = null, Drawable disabled = null)
        {
            var st = new StateListDrawable();

            st.AddState(new int[] { Android.Resource.Attribute.StatePressed, Android.Resource.Attribute.StateSelected }, selectedHighlight);
            st.AddState(new int[] { Android.Resource.Attribute.StatePressed }, highlight);
            st.AddState(new int[] { Android.Resource.Attribute.StateSelected }, selected);
            st.AddState(new int[] { -Android.Resource.Attribute.StateEnabled }, disabled);
            st.AddState(new int[] { }, background);

            view.Background = st;
        }

        public static void SetImageDrawableWithHighlight(this ImageView view, Drawable normal, Drawable selected = null, Drawable highlight = null, Drawable disabled = null)
        {
            var st = new StateListDrawable();

            st.AddState(new int[] { Android.Resource.Attribute.StatePressed }, highlight);
            st.AddState(new int[] { Android.Resource.Attribute.StateSelected }, selected);
            st.AddState(new int[] { -Android.Resource.Attribute.StateEnabled }, disabled);
            st.AddState(new int[] { }, normal);

            view.SetImageDrawable(st);
        }


        public static void SetTextColors(this TextView textView, Color normal, Color selected)
        {
            int[][] states = new int[][]
            {
                new int[] { Android.Resource.Attribute.StateSelected },
                new int[] { }
            };
            int[] colors = new int[] { selected, normal };

            var st = new ColorStateList(states, colors);

            textView.SetTextColor(st);
        }

        #endregion
    }
}
