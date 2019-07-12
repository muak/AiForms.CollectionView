using System;
using Android.Support.V7.Widget;
namespace CollectionView.Droid
{
    public class CollectionViewScrollListener:RecyclerView.OnScrollListener
    {
        public CollectionViewScrollListener()
        {
        }

        protected override void Dispose(bool disposing)
        {
            if(disposing)
            {

            }
            base.Dispose(disposing);
        }

        public override void OnScrolled(RecyclerView recyclerView, int dx, int dy)
        {
            base.OnScrolled(recyclerView, dx, dy);

            if(dx < 0 || dy < 0)
            {
                return;
            }

            var layoutManager = recyclerView.GetLayoutManager() as LinearLayoutManager;

            var visibleItemCount = recyclerView.ChildCount;
            var totalItemCount = layoutManager.ItemCount;
            var firstVisibleItem = layoutManager.FindFirstVisibleItemPosition();

            if(totalItemCount - visibleItemCount <= firstVisibleItem)
            {

            }
        }
    }
}
