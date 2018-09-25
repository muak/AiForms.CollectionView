﻿using System;
using System.ComponentModel;
using AiForms.Renderers;
using AiForms.Renderers.Droid;
using Android.Content;
using Android.Content.Res;
using Android.Support.V4.Widget;
using Android.Support.V7.Widget;
using Android.Views;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using AView = Android.Views.View;

[assembly: ExportRenderer(typeof(GridCollectionView), typeof(GridCollectionViewRenderer))]
namespace AiForms.Renderers.Droid
{
    [Android.Runtime.Preserve(AllMembers = true)]
    public class GridCollectionViewRenderer : CollectionViewRenderer, SwipeRefreshLayout.IOnRefreshListener
    {
        SwipeRefreshLayout _refresh;
        GridLayoutManager _gridLayoutManager => LayoutManager as GridLayoutManager;
        CollectionViewSpanSizeLookup _spanSizeLookup;
        GridCollectionItemDecoration _itemDecoration;
        IListViewController Controller => Element;
        GridCollectionView _gridCollectionView => (GridCollectionView)Element;
        bool _isRatioHeight => _gridCollectionView.ColumnHeight <= 5.0;
        bool _disposed;

        public int RowSpacing { get; set; }
        public int ColumnSpacing { get; set; }


        public GridCollectionViewRenderer(Context context) : base(context)
        {
            CellWidth  = LayoutParams.MatchParent;
            GroupHeaderWidth = LayoutParams.MatchParent;
        }

        protected override void Dispose(bool disposing)
        {
            if(_disposed)
            {
                return;
            }

            if(disposing)
            {
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
            base.OnElementChanged(e);

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
                     e.PropertyName == GridCollectionView.SpacingTypeProperty.PropertyName)
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

            //_recyclerView.RequestLayout();
            //_recyclerView.Invalidate();
            //_refresh.Invalidate();
            //_refresh.RequestLayout();
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


        void UpdatePullToRefreshColor()
        {
            if (!_gridCollectionView.PullToRefreshColor.IsDefault)
            {
                var color = _gridCollectionView.PullToRefreshColor.ToAndroid();
                _refresh.SetColorSchemeColors(color, color, color, color);
            }
        }

        void UpdatePullToRefreshEnabled()
        {
            if (_refresh != null)
                _refresh.Enabled = Element.IsPullToRefreshEnabled && (Element as IListViewController).RefreshAllowed;
        }

        void UpdateIsRefreshing(bool isInitialValue = false)
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

        void UpdateGroupHeaderHeight()
        {
            if (_gridCollectionView.IsGroupingEnabled)
            {
                GroupHeaderHeight = (int)Context.ToPixels(_gridCollectionView.GroupHeaderHeight);
            }
        }

        void UpdateGridType(int containerWidth = 0)
        {
            containerWidth = containerWidth == 0 ? Width : containerWidth;
            if (containerWidth <= 0)
            {
                return;
            }
            RecyclerView.SetPadding(0, 0, 0, 0);
            RowSpacing = (int)Context.ToPixels(_gridCollectionView.RowSpacing);

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

        double CalcurateColumnHeight(double itemWidth)
        {
            if (_isRatioHeight)
            {
                return itemWidth * _gridCollectionView.ColumnHeight;
            }

            return Context.ToPixels(_gridCollectionView.ColumnHeight);
        }

        int GetUniformItemHeight(int containerWidth, int columns)
        {
            float actualWidth = containerWidth - (float)ColumnSpacing * (float)(columns - 1.0f);
            var itemWidth = (float)(actualWidth / (float)columns);
            return (int)CalcurateColumnHeight(itemWidth);
        }

        (int spanCount, int columnSpacing, int rowHeight) GetAutoSpacingItemSize(double containerWidth)
        {
            var columnWidth = Context.ToPixels(_gridCollectionView.ColumnWidth);
            var columnHeight = Context.ToPixels(_gridCollectionView.ColumnHeight);

            var itemWidth = Math.Min(containerWidth, columnWidth);
            var itemHeight = CalcurateColumnHeight(itemWidth);

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


        internal class CollectionViewSpanSizeLookup : GridLayoutManager.SpanSizeLookup
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

        internal class GridCollectionItemDecoration : RecyclerView.ItemDecoration
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
                if(disposing)
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

                if (spanSize == _spanCount)
                {
                    var headparams = view.LayoutParameters as ViewGroup.MarginLayoutParams;
                    var margin = 0;
                    if (_gridCollectionView.GridType == GridType.AutoSpacingGrid &&
                       _gridCollectionView.SpacingType == SpacingType.Center)
                    {
                        margin = _parentRenderer.RecyclerView.PaddingLeft * -1;
                        headparams.SetMargins(margin, headparams.TopMargin, margin, headparams.BottomMargin);
                        view.LayoutParameters = headparams;
                    }
                    else if (headparams.LeftMargin < 0) {
                        headparams.SetMargins(margin, headparams.TopMargin, margin, headparams.BottomMargin);
                        view.LayoutParameters = headparams;
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

                outRect.Bottom = _parentRenderer.RowSpacing;
            }
        }
    }
}