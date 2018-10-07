using System.ComponentModel;
using AiForms.Renderers;
using AiForms.Renderers.Droid;
using Android.Content;
using Android.Graphics;
using Android.Support.V7.Widget;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(HCollectionView), typeof(HCollectionViewRenderer))]
namespace AiForms.Renderers.Droid
{
    [Android.Runtime.Preserve(AllMembers = true)]
    public class HCollectionViewRenderer : CollectionViewRenderer, ICollectionViewRenderer
    {
        HSpacingDecoration _itemDecoration;
        HCollectionView _hCollectionView => Element as HCollectionView;
        HCollectionViewAdapter _hAdapter => Adapter as HCollectionViewAdapter;
        int _spacing;
        bool _disposed;
        int _firstSpacing;
        int _lastSpacing;

        public HCollectionViewRenderer(Context context) : base(context)
        {
        }

        protected override void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }
            if (disposing)
            {
                RecyclerView?.StopScroll();
                RecyclerView?.SetAdapter(null);
                RecyclerView?.RemoveItemDecoration(_itemDecoration);

                _itemDecoration?.Dispose();
                _itemDecoration = null;
            }
            _disposed = true;
            base.Dispose(disposing);
        }

        protected override void OnElementChanged(ElementChangedEventArgs<CollectionView> e)
        {
            base.OnElementChanged(e);

            if (e.NewElement != null)
            {
                if (Control == null)
                {
                    RecyclerView = new RecyclerView(Context);
                    LayoutManager = new LinearLayoutManager(Context);
                    LayoutManager.Orientation = LinearLayoutManager.Horizontal;


                    SetNativeControl(RecyclerView);

                    RecyclerView.Focusable = false;
                    RecyclerView.DescendantFocusability = Android.Views.DescendantFocusability.AfterDescendants;
                    RecyclerView.SetClipToPadding(false);
                    RecyclerView.HorizontalScrollBarEnabled = false;

                    _itemDecoration = new HSpacingDecoration(this);
                    RecyclerView.AddItemDecoration(_itemDecoration);

                    Adapter = new HCollectionViewAdapter(Context, e.NewElement, RecyclerView, this);
                    RecyclerView.SetAdapter(Adapter);

                    RecyclerView.SetLayoutManager(LayoutManager);

                    UpdateIsInfinite();
                    UpdateSpacing();
                }
            }
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            if (e.PropertyName == HCollectionView.ColumnWidthProperty.PropertyName ||
               e.PropertyName == VisualElement.HeightRequestProperty.PropertyName)
            {
                UpdateCellSize();
                RefreshAll();
            }
            else if (e.PropertyName == HCollectionView.GroupHeaderWidthProperty.PropertyName)
            {
                UpdateGroupHeaderWidth();
                RefreshAll();
            }
            else if (e.PropertyName == HCollectionView.SpacingProperty.PropertyName ||
                     e.PropertyName == CollectionView.GroupFirstSpacingProperty.PropertyName ||
                     e.PropertyName == CollectionView.GroupLastSpacingProperty.PropertyName)
            {
                UpdateSpacing();
                RefreshAll();
            }
            else if (e.PropertyName == HCollectionView.IsInfiniteProperty.PropertyName)
            {
                RefreshAll();
                UpdateIsInfinite();
            }
        }

        protected override void OnLayout(bool changed, int l, int t, int r, int b)
        {
            if (changed)
            {
                UpdateCellSize();
                UpdateGroupHeaderWidth();
                RefreshAll();
            }

            base.OnLayout(changed, l, t, r, b);
        }

        protected virtual void RefreshAll()
        {
            RecyclerView.RemoveItemDecoration(_itemDecoration);

            Adapter.OnDataChanged();
            RecyclerView.AddItemDecoration(_itemDecoration);
            RequestLayout();
            Invalidate();
        }

        protected override void ExecuteScroll(int targetPosition, ScrollToRequestedEventArgs eventArgs)
        {
            if (_hCollectionView.IsInfinite)
            {
                int fixPosition = _hAdapter.GetInitialPosition();
                if (targetPosition == Adapter.ItemCount - 1)
                {
                    fixPosition += Adapter.RealItemCount - 1;
                }
                else
                {
                    fixPosition += targetPosition;
                }
                base.ExecuteScroll(fixPosition, eventArgs);
                return;
            }
            base.ExecuteScroll(targetPosition, eventArgs);
        }

        protected virtual void UpdateIsInfinite()
        {
            if (_hCollectionView.IsInfinite)
            {
                LayoutManager.ScrollToPositionWithOffset(_hAdapter.GetInitialPosition(), 0);
            }
        }

        protected virtual void UpdateCellSize()
        {
            if (Element.Height < 0)
            {
                return;
            }
            var height = Element.HeightRequest >= 0 ? Element.HeightRequest : Element.Height;
            CellWidth = (int)Context.ToPixels(_hCollectionView.ColumnWidth);
            CellHeight = (int)Context.ToPixels(height);
        }

        protected virtual void UpdateSpacing()
        {
            _spacing = (int)Context.ToPixels(_hCollectionView.Spacing);
            _firstSpacing = (int)Context.ToPixels(_hCollectionView.GroupFirstSpacing);
            _lastSpacing = (int)Context.ToPixels(_hCollectionView.GroupLastSpacing);
        }

        protected virtual void UpdateGroupHeaderWidth()
        {
            if (_hCollectionView.IsGroupingEnabled)
            {
                GroupHeaderWidth = (int)Context.ToPixels(_hCollectionView.GroupHeaderWidth);
                GroupHeaderHeight = (int)Context.ToPixels(Element.Height);
            }
        }

        public class HSpacingDecoration : RecyclerView.ItemDecoration
        {
            HCollectionViewRenderer _renderer;

            public HSpacingDecoration(HCollectionViewRenderer renderer)
            {
                _renderer = renderer;
            }

            protected override void Dispose(bool disposing)
            {
                if (disposing)
                {
                    _renderer = null;
                }
                base.Dispose(disposing);
            }

            public override void GetItemOffsets(Rect outRect, Android.Views.View view, RecyclerView parent, RecyclerView.State state)
            {
                var holder = parent.GetChildViewHolder(view) as ContentViewHolder;
                var position = parent.GetChildAdapterPosition(view);
                var realPosition = position;
                if (_renderer._hCollectionView.IsInfinite)
                {
                    realPosition = _renderer.Adapter.GetRealPosition(position);
                }

                if(holder.IsHeader)
                {
                    outRect.Right = _renderer._firstSpacing;
                    if (position != 0)
                    {
                        outRect.Left = _renderer._lastSpacing;
                    }

                    return;
                }

                if (position == 0 || _renderer.Adapter.FirstSectionItems.Contains(realPosition))
                {
                    return;
                }

                outRect.Left = _renderer._spacing;
            }
        }

    }
}
