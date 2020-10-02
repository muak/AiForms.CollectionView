using System;
using System.ComponentModel;
using AiForms.Renderers;
using AiForms.Renderers.Droid;
using Android.Content;
using Android.Content.Res;
using Android.Views;
using AndroidX.RecyclerView.Widget;
using AndroidX.SwipeRefreshLayout.Widget;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(GridCollectionView), typeof(GridCollectionViewRenderer))]
namespace AiForms.Renderers.Droid
{
    [Android.Runtime.Preserve(AllMembers = true)]
    public class GridCollectionViewRenderer : CollectionViewRenderer, SwipeRefreshLayout.IOnRefreshListener
    {
        public int RowSpacing { get; set; }
        public int ColumnSpacing { get; set; }

        int _bothSidesMargin;
        int _firstSpacing;
        int _lastSpacing;

        SwipeRefreshLayout _refresh;
        GridLayoutManager _gridLayoutManager => LayoutManager as GridLayoutManager;
        CollectionViewSpanSizeLookup _spanSizeLookup;
        GridCollectionItemDecoration _itemDecoration;
        IListViewController Controller => Element;
        GridCollectionView _gridCollectionView => (GridCollectionView)Element;
        bool _isRatioHeight => _gridCollectionView.ColumnHeight <= 5.0;
        bool _disposed;

        public GridCollectionViewRenderer(Context context) : base(context)
        {
            CellWidth = LayoutParams.MatchParent;
            GroupHeaderWidth = LayoutParams.MatchParent;
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
                _gridLayoutManager?.SetSpanSizeLookup(null);

                _spanSizeLookup?.Dispose();
                _spanSizeLookup = null;

                _itemDecoration?.Dispose();
                _itemDecoration = null;
            }

            _disposed = true;

            base.Dispose(disposing);
        }

        protected override void OnElementChanged(ElementChangedEventArgs<CollectionView> e)
        {
            if (e.NewElement != null)
            {
                if (RecyclerView == null)
                {
                    RecyclerView = new RecyclerView(Context);
                    _refresh = new SwipeRefreshLayout(Context);
                    _refresh.SetOnRefreshListener(this);
                    _refresh.AddView(RecyclerView, LayoutParams.MatchParent, LayoutParams.MatchParent);

                    SetNativeControl(_refresh);
                }

                LayoutManager = new GridLayoutManager(Context, 2);
                _spanSizeLookup = new CollectionViewSpanSizeLookup(this);
                _gridLayoutManager.SetSpanSizeLookup(_spanSizeLookup);

                RecyclerView.Focusable = false;
                RecyclerView.DescendantFocusability = Android.Views.DescendantFocusability.AfterDescendants;
                RecyclerView.OnFocusChangeListener = this;
                RecyclerView.SetClipToPadding(false);

                _itemDecoration = new GridCollectionItemDecoration(this);
                RecyclerView.AddItemDecoration(_itemDecoration);

                Adapter = new CollectionViewAdapter(Context, _gridCollectionView, RecyclerView, this);

                RecyclerView.SetAdapter(Adapter);
                RecyclerView.SetLayoutManager(_gridLayoutManager);

                Adapter.IsAttachedToWindow = IsAttached;

                UpdateGroupHeaderHeight();
                UpdatePullToRefreshEnabled();
                UpdatePullToRefreshColor();
            }

            base.OnElementChanged(e);
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            // avoid running the vain process when popping a page.
            if ((sender as BindableObject)?.BindingContext == null)
            {
                return;
            }

            if (e.PropertyName == Xamarin.Forms.ListView.IsGroupingEnabledProperty.PropertyName)
            {
                RefreshAll();
            }
            else if (e.PropertyName == GridCollectionView.GroupHeaderHeightProperty.PropertyName)
            {
                UpdateGroupHeaderHeight();
                RefreshAll();
            }
            else if (e.PropertyName == GridCollectionView.GridTypeProperty.PropertyName ||
                     e.PropertyName == GridCollectionView.PortraitColumnsProperty.PropertyName ||
                     e.PropertyName == GridCollectionView.LandscapeColumnsProperty.PropertyName ||
                     e.PropertyName == GridCollectionView.ColumnSpacingProperty.PropertyName ||
                     e.PropertyName == GridCollectionView.RowSpacingProperty.PropertyName ||
                     e.PropertyName == GridCollectionView.ColumnHeightProperty.PropertyName ||
                     e.PropertyName == GridCollectionView.ColumnWidthProperty.PropertyName ||
                     e.PropertyName == GridCollectionView.SpacingTypeProperty.PropertyName ||
                     e.PropertyName == GridCollectionView.AdditionalHeightProperty.PropertyName ||
                     e.PropertyName == CollectionView.GroupFirstSpacingProperty.PropertyName ||
                     e.PropertyName == CollectionView.GroupLastSpacingProperty.PropertyName ||
                     e.PropertyName == GridCollectionView.BothSidesMarginProperty.PropertyName)
            {
                UpdateGridType();
                RefreshAll();
            }
            else if (e.PropertyName == Xamarin.Forms.ListView.IsPullToRefreshEnabledProperty.PropertyName)
            {
                UpdatePullToRefreshEnabled();
            }
            else if (e.PropertyName == GridCollectionView.PullToRefreshColorProperty.PropertyName)
            {
                UpdatePullToRefreshColor();
            }
            else if (e.PropertyName == Xamarin.Forms.ListView.IsRefreshingProperty.PropertyName)
            {
                UpdateIsRefreshing();
            }
            else if (e.PropertyName == VisualElement.BackgroundColorProperty.PropertyName)
            {
                UpdateBackgroundColor();
            }
        }

        protected virtual void RefreshAll()
        {

            RecyclerView.RemoveItemDecoration(_itemDecoration);
            _gridLayoutManager.GetSpanSizeLookup().InvalidateSpanIndexCache();

            Adapter.OnDataChanged();
            RecyclerView.AddItemDecoration(_itemDecoration);
            RequestLayout();
            Invalidate();
        }

        protected override void OnLayout(bool changed, int l, int t, int r, int b)
        {
            if (changed)
            {
                UpdateGridType(r - l);
            }

            base.OnLayout(changed, l, t, r, b);
        }

        protected override void OnConfigurationChanged(Configuration newConfig)
        {
            base.OnConfigurationChanged(newConfig);

            UpdateGridType();

            // HACK: run after a bit time because of not refreshing immediately
            Device.StartTimer(TimeSpan.FromMilliseconds(1000), () =>
            {
                RefreshAll();
                return false;
            });
        }

        void SwipeRefreshLayout.IOnRefreshListener.OnRefresh()
        {
            IListViewController controller = Element;
            controller.SendRefreshing();
        }

        protected override void OnAttachedToWindow()
        {
            base.OnAttachedToWindow();

            UpdateIsRefreshing(true);
        }


        protected virtual void UpdatePullToRefreshColor()
        {
            if (!_gridCollectionView.PullToRefreshColor.IsDefault)
            {
                var color = _gridCollectionView.PullToRefreshColor.ToAndroid();
                _refresh.SetColorSchemeColors(color, color, color, color);
            }
        }

        protected virtual void UpdatePullToRefreshEnabled()
        {
            if (_refresh != null)
                _refresh.Enabled = Element.IsPullToRefreshEnabled && (Element as IListViewController).RefreshAllowed;
        }

        protected virtual void UpdateIsRefreshing(bool isInitialValue = false)
        {
            if (_refresh != null)
            {
                var isRefreshing = Element.IsRefreshing;
                if (isRefreshing && isInitialValue)
                {
                    _refresh.Refreshing = false;
                    _refresh.Post(() =>
                    {
                        _refresh.Refreshing = true;
                    });
                }
                else
                    _refresh.Refreshing = isRefreshing;
            }
        }

        protected virtual void UpdateGroupHeaderHeight()
        {
            if (_gridCollectionView.IsGroupingEnabled)
            {
                GroupHeaderHeight = (int)Context.ToPixels(_gridCollectionView.GroupHeaderHeight);
            }
        }

        protected virtual void UpdateGridType(int containerWidth = 0)
        {
            containerWidth = containerWidth == 0 ? Width : containerWidth;
            if (containerWidth <= 0)
            {
                return;
            }
            RecyclerView.SetPadding(0, 0, 0, 0);
            RowSpacing = (int)Context.ToPixels(_gridCollectionView.RowSpacing);

            _firstSpacing = (int)Context.ToPixels(_gridCollectionView.GroupFirstSpacing);
            _lastSpacing = (int)Context.ToPixels(_gridCollectionView.GroupLastSpacing);

            int spanCount = 0;
            if (_gridCollectionView.GridType == GridType.UniformGrid)
            {
                var orientation = Context.Resources.Configuration.Orientation;
                switch (orientation)
                {
                    case Orientation.Portrait:
                    case Orientation.Square:
                    case Orientation.Undefined:
                        spanCount = _gridCollectionView.PortraitColumns;
                        break;
                    case Orientation.Landscape:
                        spanCount = _gridCollectionView.LandscapeColumns;
                        break;
                }
                _bothSidesMargin = (int)Context.ToPixels(_gridCollectionView.BothSidesMargin);
                ColumnSpacing = (int)(Context.ToPixels(_gridCollectionView.ColumnSpacing));
                CellHeight = GetUniformItemHeight(containerWidth, spanCount);
                System.Diagnostics.Debug.WriteLine($"Decided RowHeight {CellHeight}");
            }
            else
            {
                var autoSpacingSize = GetAutoSpacingItemSize(containerWidth);
                spanCount = autoSpacingSize.spanCount;
                ColumnSpacing = autoSpacingSize.columnSpacing;
                CellHeight = autoSpacingSize.rowHeight;
            }

            _gridLayoutManager.SpanCount = spanCount;
            _spanSizeLookup.SpanSize = spanCount;
        }

        protected virtual double CalcurateColumnHeight(double itemWidth)
        {
            var height = _isRatioHeight ? itemWidth * _gridCollectionView.ColumnHeight :
                                          Context.ToPixels(_gridCollectionView.ColumnHeight);

            var actualHeight = height + Context.ToPixels(_gridCollectionView.AdditionalHeight);
            _gridCollectionView.SetValue(GridCollectionView.ComputedHeightProperty,
                                         Context.FromPixels(actualHeight));

            return actualHeight;
        }

        protected virtual int GetUniformItemHeight(int containerWidth, int columns)
        {
            RecyclerView.SetPadding(_bothSidesMargin, 0, _bothSidesMargin, 0);
            float actualWidth = containerWidth - _bothSidesMargin * 2f - (float)ColumnSpacing * (float)(columns - 1.0f);
            var itemWidth = (float)(actualWidth / (float)columns);
            _gridCollectionView.SetValue(GridCollectionView.ComputedWidthProperty, Context.FromPixels(itemWidth));
            return (int)CalcurateColumnHeight(itemWidth);
        }

        protected virtual (int spanCount, int columnSpacing, int rowHeight) GetAutoSpacingItemSize(double containerWidth)
        {
            var columnWidth = Context.ToPixels(_gridCollectionView.ColumnWidth);
            var columnHeight = Context.ToPixels(_gridCollectionView.ColumnHeight);

            var itemWidth = Math.Min(containerWidth, columnWidth);
            var itemHeight = CalcurateColumnHeight(itemWidth);

            _gridCollectionView.SetValue(GridCollectionView.ComputedWidthProperty, Context.FromPixels(itemWidth));

            var leftSize = containerWidth;
            var spacing = _gridCollectionView.SpacingType == SpacingType.Between ? 0 : Context.ToPixels(_gridCollectionView.ColumnSpacing);
            int columnCount = 0;
            do
            {
                leftSize -= itemWidth;
                if (leftSize < 0)
                {
                    break;
                }
                columnCount++;
                if (leftSize - spacing < 0)
                {
                    break;
                }
                leftSize -= spacing;
            } while (true);

            double contentWidth = 0;
            double columnSpacing = 0;
            if (_gridCollectionView.SpacingType == SpacingType.Between)
            {
                contentWidth = itemWidth * columnCount;
                columnSpacing = (containerWidth - contentWidth) / (columnCount - 1);
                return (columnCount, (int)columnSpacing, (int)itemHeight);
            }

            contentWidth = itemWidth * columnCount + spacing * (columnCount - 1f);
            var inset = (containerWidth - contentWidth) / 2.0f;
            RecyclerView.SetPadding((int)inset, 0, (int)inset, 0);
            columnSpacing = spacing;

            return (columnCount, (int)columnSpacing, (int)itemHeight);
        }


        public class CollectionViewSpanSizeLookup : GridLayoutManager.SpanSizeLookup
        {
            GridCollectionViewRenderer _parent;
            GridCollectionView _gridCollectionView => _parent._gridCollectionView;

            public int SpanSize { get; set; }
            public int SpanCount { get; set; }

            public CollectionViewSpanSizeLookup(GridCollectionViewRenderer parent)
            {
                _parent = parent;
                SpanIndexCacheEnabled = false;
            }

            public override int GetSpanSize(int position)
            {
                if (_parent._gridCollectionView.IsGroupingEnabled)
                {
                    var group = _parent.TemplatedItemsView.TemplatedItems.GetGroupIndexFromGlobal(position, out var row);
                    if (row == 0)
                    {
                        return SpanSize;
                    }
                }

                return 1;
            }

            protected override void Dispose(bool disposing)
            {
                base.Dispose(disposing);
                if (disposing)
                {
                    _parent = null;
                }
            }
        }

        public class GridCollectionItemDecoration : RecyclerView.ItemDecoration
        {
            public bool IncludeEdge { get; set; }

            GridCollectionViewRenderer _parentRenderer;
            GridCollectionView _gridCollectionView => _parentRenderer._gridCollectionView;
            CollectionViewSpanSizeLookup _spanLookUp => _parentRenderer._spanSizeLookup;
            int _spanCount => _parentRenderer._gridLayoutManager.SpanCount;

            public GridCollectionItemDecoration(GridCollectionViewRenderer parentRenderer)
            {
                _parentRenderer = parentRenderer;
            }

            protected override void Dispose(bool disposing)
            {
                if (disposing)
                {
                    _parentRenderer = null;
                }
                base.Dispose(disposing);
            }

            public override void GetItemOffsets(Android.Graphics.Rect outRect, Android.Views.View view, RecyclerView parent, RecyclerView.State state)
            {
                var param = view.LayoutParameters as GridLayoutManager.LayoutParams;
                var spanIndex = param.SpanIndex;
                var spanSize = param.SpanSize;
                var position = parent.GetChildAdapterPosition(view);

                if (spanSize == _spanCount)
                {
                    var headparams = view.LayoutParameters as ViewGroup.MarginLayoutParams;
                    var margin = 0;
                    if (_gridCollectionView.GridType == GridType.AutoSpacingGrid && _gridCollectionView.SpacingType == SpacingType.Center || 
                       _gridCollectionView.GridType == GridType.UniformGrid && _gridCollectionView.BothSidesMargin > 0)
                    {
                        margin = _parentRenderer.RecyclerView.PaddingLeft * -1;
                        headparams.SetMargins(margin, headparams.TopMargin, margin, headparams.BottomMargin);
                        view.LayoutParameters = headparams;
                    }
                    else if (headparams.LeftMargin < 0)
                    {
                        headparams.SetMargins(margin, headparams.TopMargin, margin, headparams.BottomMargin);
                        view.LayoutParameters = headparams;
                    }

                    outRect.Bottom = _parentRenderer._firstSpacing;
                    if(position != 0) {
                        outRect.Top = _parentRenderer._lastSpacing;
                    }
                    return;
                }

                if (_spanCount == 1)
                {
                    return;
                }

                if (IncludeEdge)
                {
                    outRect.Left = _parentRenderer.ColumnSpacing - spanIndex * _parentRenderer.ColumnSpacing / _spanCount; // spacing - column * ((1f / spanCount) * spacing)
                    outRect.Right = (spanIndex + 1) * _parentRenderer.ColumnSpacing / _spanCount; // (column + 1) * ((1f / spanCount) * spacing)
                }
                else
                {
                    outRect.Left = spanIndex * _parentRenderer.ColumnSpacing / _spanCount; // column * ((1f / spanCount) * spacing)
                    outRect.Right = _parentRenderer.ColumnSpacing - (spanIndex + 1) * _parentRenderer.ColumnSpacing / _spanCount; // spacing - (column + 1) * ((1f /    spanCount) * spacing)
                }

                // Disabled grouping top spacing is applied at the first row cells.
                if (!_parentRenderer.Element.IsGroupingEnabled && position < _spanCount)
                {
                    outRect.Top = _parentRenderer._firstSpacing;
                }

                // Group bottom or single bottom spacing is applied at the last row cells.
                if (position >= _parentRenderer.Adapter.ItemCount - _spanCount)
                {
                    outRect.Bottom = _parentRenderer._lastSpacing;
                }

                if (position  < _spanCount || _parentRenderer.Adapter.FirstSectionItems.Contains(position - spanIndex)) {
                    return;
                }

                outRect.Top = _parentRenderer.RowSpacing;
            }
        }
    }
}
