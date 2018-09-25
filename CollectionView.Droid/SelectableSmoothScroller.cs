using System;
using Android.Content;
using Android.Support.V7.Widget;
using Android.Views;
using Xamarin.Forms;

namespace AiForms.Renderers.Droid
{
    public class SelectableSmoothScroller:LinearSmoothScroller
    {
        public ScrollToPosition SnapPosition { get; set; } = ScrollToPosition.Start;

        public SelectableSmoothScroller(Context context) : base(context)
        {
        }

        protected override int VerticalSnapPreference => ToSnapPreference();
        protected override int HorizontalSnapPreference => ToSnapPreference();

        public override int CalculateDtToFit(int viewStart, int viewEnd, int boxStart, int boxEnd, int snapPreference)
        {
            if(SnapPosition == ScrollToPosition.Center){
                return (boxStart + (boxEnd - boxStart) / 2) - (viewStart + (viewEnd - viewStart) / 2);
            }

            return base.CalculateDtToFit(viewStart, viewEnd, boxStart, boxEnd, snapPreference);
        }

        int ToSnapPreference() 
        {
            switch (SnapPosition)
            {
                case ScrollToPosition.Start:
                    return SnapToStart;
                case ScrollToPosition.End:
                    return SnapToEnd;
                default:
                    return SnapToAny;
            }
        }
    }
}
