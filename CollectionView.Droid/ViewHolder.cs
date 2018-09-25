using AiForms.Renderers.Droid.Cells;
using Android.Support.V7.Widget;
using Android.Views;
using AView = Android.Views.View;

namespace AiForms.Renderers.Droid
{
    [Android.Runtime.Preserve(AllMembers = true)]
    public class ViewHolder : RecyclerView.ViewHolder
    {
        public ViewHolder(AView view) : base(view) { }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                ItemView?.Dispose();
                ItemView = null;
            }
            base.Dispose(disposing);
        }
    }

    [Android.Runtime.Preserve(AllMembers = true)]
    public class ContentViewHolder : ViewHolder
    {
        ICollectionViewRenderer _renderer;
        public bool IsHeader => ItemViewType >= CollectionViewAdapter.DefaultGroupHeaderTemplateId;
        public int CellHeight => IsHeader ? _renderer.GroupHeaderHeight : _renderer.CellHeight;
        public int CellWidth => IsHeader ? _renderer.GroupHeaderWidth : _renderer.CellWidth;

        public ContentViewHolder(ICollectionViewRenderer renderer, ContentCellContainer view) : base(view)
        {
            _renderer = renderer;

            view.LayoutParameters = new ViewGroup.LayoutParams(CellWidth, CellHeight);
            view.ViewHolder = this;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                ItemView?.SetOnClickListener(null);
                ItemView?.SetOnLongClickListener(null);
                _renderer = null;
            }
            base.Dispose(disposing);
        }

    }
}
