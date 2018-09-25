using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows.Input;
using AiForms.Renderers;
using AiForms.Renderers.iOS;
using AiForms.Renderers.iOS.Cells;
using CoreGraphics;
using Foundation;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Platform.iOS;
using RectangleF = CoreGraphics.CGRect;
using System.Linq;

[assembly: ExportRenderer(typeof(GridCollectionView), typeof(GridCollectionViewRenderer))]
namespace AiForms.Renderers.iOS
{
    [Foundation.Preserve(AllMembers = true)]
    public class GridCollectionViewRenderer : CollectionViewRenderer
    {
        UICollectionView _collectionView;
        UIRefreshControl _refreshControl;
        KeyboardInsetTracker _insetTracker;
        RectangleF _previousFrame;
        bool _disposed;
        GridCollectionView _gridCollectionView => (GridCollectionView)Element;
        GridCollectionViewSource _gridSource => DataSource as GridCollectionViewSource;
        bool _isRatioHeight => _gridCollectionView.ColumnHeight <= 5.0;


        protected override void OnElementChanged(ElementChangedEventArgs<CollectionView> e)
        {
            if (e.NewElement != null)
            {
                ViewLayout = new UICollectionViewFlowLayout();
                ViewLayout.ScrollDirection = UICollectionViewScrollDirection.Vertical;
                ViewLayout.SectionInset = new UIEdgeInsets(0, 0, 0, 0);
                ViewLayout.MinimumLineSpacing = 0.0f;
                ViewLayout.MinimumInteritemSpacing = 0.0f;
                ViewLayout.EstimatedItemSize = UICollectionViewFlowLayout.AutomaticSize;
                ViewLayout.SectionHeadersPinToVisibleBounds = true; // fix header cell


                _refreshControl = new UIRefreshControl();
                _refreshControl.ValueChanged += RefreshControl_ValueChanged;

                _collectionView = new UICollectionView(CGRect.Empty, ViewLayout);
                _collectionView.RegisterClassForCell(typeof(ContentCellContainer), typeof(ContentCell).FullName);
                _collectionView.RegisterClassForSupplementaryView(typeof(ContentCellContainer), UICollectionElementKindSection.Header, SectionHeaderId);

                _collectionView.AllowsSelection = true;

                SetNativeControl(_collectionView);

                _insetTracker = new KeyboardInsetTracker(_collectionView, () => Control.Window, insets => Control.ContentInset = Control.ScrollIndicatorInsets = insets, point =>
                {
                    var offset = Control.ContentOffset;
                    offset.Y += point.Y;
                    Control.SetContentOffset(offset, true);
                });


                DataSource = new GridCollectionViewSource(e.NewElement, _collectionView);
                _collectionView.Source = DataSource;

                UpdateRowSpacing();
                UpdatePullToRefreshEnabled();
                UpdatePullToRefreshColor();
            }

            base.OnElementChanged(e);
        }

        public override void LayoutSubviews()
        {
            if (_previousFrame != Frame)
            {
                _previousFrame = Frame;
                UpdateGridType();
                UpdateGroupHeaderHeight();
                _insetTracker?.UpdateInsets();
            }
            base.LayoutSubviews();
        }

        public override SizeRequest GetDesiredSize(double widthConstraint, double heightConstraint)
        {
            return Control.GetSizeRequest(widthConstraint, heightConstraint, 50, 50);
        }

        protected override void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }

            if (disposing)
            {
                _insetTracker?.Dispose();
                _insetTracker = null;

                DataSource?.Dispose();
                DataSource = null;

                if (_refreshControl != null)
                {
                    _refreshControl.ValueChanged -= RefreshControl_ValueChanged;
                    _refreshControl.Dispose();
                    _refreshControl = null;
                }

                _collectionView?.Dispose();
                _collectionView = null;
            }

            _disposed = true;

            base.Dispose(disposing);
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            if (e.PropertyName == GridCollectionView.GroupHeaderHeightProperty.PropertyName)
            {
                UpdateGroupHeaderHeight();
                ViewLayout.InvalidateLayout();
            }
            else if (e.PropertyName == GridCollectionView.GridTypeProperty.PropertyName ||
                     e.PropertyName == GridCollectionView.PortraitColumnsProperty.PropertyName ||
                     e.PropertyName == GridCollectionView.LandscapeColumnsProperty.PropertyName ||
                     e.PropertyName == GridCollectionView.ColumnSpacingProperty.PropertyName ||
                     e.PropertyName == GridCollectionView.ColumnHeightProperty.PropertyName ||
                     e.PropertyName == GridCollectionView.SpacingTypeProperty.PropertyName )
            {
                UpdateGridType();
                ViewLayout.InvalidateLayout();
            }
            else if (e.PropertyName == GridCollectionView.RowSpacingProperty.PropertyName)
            {
                UpdateRowSpacing();
                ViewLayout.InvalidateLayout();
            }
            else if (e.PropertyName == GridCollectionView.ColumnWidthProperty.PropertyName)
            {
                if (_gridCollectionView.GridType != GridType.UniformGrid)
                {
                    UpdateGridType();
                    ViewLayout.InvalidateLayout();
                }
            }
            else if (e.PropertyName == ListView.IsPullToRefreshEnabledProperty.PropertyName)
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
        }

        protected override UICollectionViewScrollPosition GetScrollPosition(ScrollToPosition position)
        {
            switch (position)
            {
                case ScrollToPosition.Center:
                    return UICollectionViewScrollPosition.CenteredVertically;
                case ScrollToPosition.End:
                    return UICollectionViewScrollPosition.Bottom;
                case ScrollToPosition.Start:
                    return UICollectionViewScrollPosition.Top;
                case ScrollToPosition.MakeVisible:
                default:
                    return UICollectionViewScrollPosition.None;
            }
        }

        void RefreshControl_ValueChanged(object sender, EventArgs e)
        {
            if (_refreshControl.Refreshing)
            {
                _gridCollectionView.SendRefreshing();
            }
            _gridCollectionView.IsRefreshing = _refreshControl.Refreshing;
        }

        void UpdateIsRefreshing()
        {
            var refreshing = Element.IsRefreshing;
            if (_gridCollectionView == null)
            {
                return;
            }
            if (refreshing)
            {
                if (!_refreshControl.Refreshing)
                {
                    _refreshControl.BeginRefreshing();
                }
            }
            else
            {
                _refreshControl.EndRefreshing();
            }

        }

        void UpdatePullToRefreshColor()
        {
            if (!_gridCollectionView.PullToRefreshColor.IsDefault)
            {
                _refreshControl.TintColor = _gridCollectionView.PullToRefreshColor.ToUIColor();
            }
        }

        void UpdatePullToRefreshEnabled()
        {
            _refreshControl.Enabled = Element.IsPullToRefreshEnabled && (Element as IListViewController).RefreshAllowed;
            if (_refreshControl.Enabled)
            {
                _collectionView.RefreshControl = _refreshControl;
            }
            else
            {
                _collectionView.RefreshControl = null;
            }
        }

        void UpdateRowSpacing()
        {
            ViewLayout.MinimumLineSpacing = (System.nfloat)_gridCollectionView.RowSpacing;
        }

        void UpdateGroupHeaderHeight()
        {
            if (_gridCollectionView.IsGroupingEnabled)
            {
                // TODO: 細かいサイズ調整をする場合はサブクラスで対応する
                ViewLayout.HeaderReferenceSize = new CGSize(Bounds.Width, _gridCollectionView.GroupHeaderHeight);
            }
        }

        void UpdateGridType()
        {
            ViewLayout.SectionInset = new UIEdgeInsets(0, 0, 0, 0); // Reset insets
            ViewLayout.MinimumInteritemSpacing = 0;
            CGSize itemSize = CGSize.Empty;

            if (_gridCollectionView.GridType == GridType.UniformGrid)
            {
                switch (UIApplication.SharedApplication.StatusBarOrientation)
                {
                    case UIInterfaceOrientation.Portrait:
                    case UIInterfaceOrientation.PortraitUpsideDown:
                    case UIInterfaceOrientation.Unknown:
                        itemSize = GetUniformItemSize(_gridCollectionView.PortraitColumns);

                        break;
                    case UIInterfaceOrientation.LandscapeLeft:
                    case UIInterfaceOrientation.LandscapeRight:
                        itemSize = GetUniformItemSize(_gridCollectionView.LandscapeColumns);
                        break;
                }
                ViewLayout.MinimumInteritemSpacing = (System.nfloat)_gridCollectionView.ColumnSpacing;
            }
            else
            {
                itemSize = GetAutoSpacingItemSize();
            }

            DataSource.CellSize = itemSize;
        }

        double CalcurateColumnHeight(double itemWidth)
        {
            if (_isRatioHeight)
            {
                return itemWidth * _gridCollectionView.ColumnHeight;
            }

            return _gridCollectionView.ColumnHeight;
        }

        CGSize GetUniformItemSize(int columns)
        {
            float width = (float)Frame.Width - (float)_gridCollectionView.ColumnSpacing * (float)(columns - 1.0f);

            _gridSource.SurplusPixel = (int)width % columns;

            var itemWidth = Math.Floor((float)(width / (float)columns));
            var itemHeight = CalcurateColumnHeight(itemWidth);
            return new CGSize(itemWidth, itemHeight);
        }

        CGSize GetAutoSpacingItemSize()
        {
            var itemWidth = (float)Math.Min(Frame.Width, _gridCollectionView.ColumnWidth);
            var itemHeight = CalcurateColumnHeight(itemWidth);
            if (_gridCollectionView.SpacingType == SpacingType.Between)
            {
                return new CGSize(itemWidth, itemHeight);
            }


            var leftSize = (float)Frame.Width;
            var spacing = (float)_gridCollectionView.ColumnSpacing;
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

            var contentWidth = itemWidth * columnCount + spacing * (columnCount - 1f);

            var insetSum = Frame.Width - contentWidth;
            var insetSurplus = (int)insetSum % 2;
            var inset = (float)Math.Floor(insetSum / 2.0f);

            ViewLayout.SectionInset = new UIEdgeInsets(0, inset + (float)insetSurplus, 0, inset);

            return new CGSize(itemWidth, itemHeight);
        }
    }
}
