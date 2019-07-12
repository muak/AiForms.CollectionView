using System;
using Android.Content;
using Android.Support.V7.Widget;
using AiForms.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using AView = Android.Views.View;
using Android.Views;

namespace AiForms.Renderers.Droid
{
    [Android.Runtime.Preserve(AllMembers = true)]
    public abstract class CollectionViewRenderer : ViewRenderer<CollectionView, AView>, ICollectionViewRenderer
    {
        public int GroupHeaderHeight { get; set; }
        public int GroupHeaderWidth { get; set; }
        public int CellHeight { get; set; }
        public int CellWidth { get; set; }

        protected RecyclerView RecyclerView;
        protected CollectionViewAdapter Adapter;
        protected LinearLayoutManager LayoutManager;
        protected bool IsAttached;
        protected ITemplatedItemsView<Cell> TemplatedItemsView => Element;

        CollectionViewScrollListener _scrollListener;
        SelectableSmoothScroller _scroller;
        ScrollToRequestedEventArgs _pendingScrollTo;

        bool _disposed;

        public CollectionViewRenderer(Context context) : base(context)
        {
            AutoPackage = false;
        }

        protected override void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }

            if (disposing)
            {
                _scroller?.Dispose();
                _scroller = null;

                RecyclerView.RemoveOnScrollListener(_scrollListener);
                _scrollListener?.Dispose();
                _scrollListener = null;

                Adapter?.Dispose();
                Adapter = null;

                LayoutManager?.Dispose();
                LayoutManager = null;

                ((IListViewController)Element).ScrollToRequested -= OnScrollToRequested;
            }

            _disposed = true;
            base.Dispose(disposing);
        }

        protected override void OnElementChanged(ElementChangedEventArgs<CollectionView> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null)
            {
                ((IListViewController)e.OldElement).ScrollToRequested -= OnScrollToRequested;
                if (Adapter != null)
                {
                    Adapter?.Dispose();
                    Adapter = null;
                }
                e.OldElement.EndLoadingAction = null;
            }

            if (e.NewElement != null)
            {
                ((IListViewController)e.NewElement).ScrollToRequested += OnScrollToRequested;
                _scroller = new SelectableSmoothScroller(Context);
                _scrollListener = new CollectionViewScrollListener(e.NewElement);
                RecyclerView.AddOnScrollListener(_scrollListener);
                e.NewElement.EndLoadingAction = () => _scrollListener.IsReachedBottom = false;
            }
        }

        protected override void OnAttachedToWindow()
        {
            base.OnAttachedToWindow();

            IsAttached = true;
            Adapter.IsAttachedToWindow = IsAttached;
        }

        protected override void OnDetachedFromWindow()
        {
            base.OnDetachedFromWindow();

            IsAttached = false;
            Adapter.IsAttachedToWindow = IsAttached;
        }

        protected virtual void OnScrollToRequested(object sender, ScrollToRequestedEventArgs e)
        {
            if (!IsAttached)
            {
                _pendingScrollTo = e;
                return;
            }

            Cell cell; // TODO: this is not used at current version. If used individual size, this must be used.
            int position;
            var scrollArgs = (ITemplatedItemsListScrollToRequestedEventArgs)e;

            if (scrollArgs.Item == null)
            {
                if (e.Position == ScrollToPosition.Start)
                {
                    ExecuteScroll(0, e);
                }
                else
                {
                    ExecuteScroll(Adapter.ItemCount - 1, e);
                }
                return;
            }

            var templatedItems = TemplatedItemsView.TemplatedItems;
            if (Element.IsGroupingEnabled)
            {
                var results = templatedItems.GetGroupAndIndexOfItem(scrollArgs.Group, scrollArgs.Item);
                if (results.Item1 == -1 || results.Item2 == -1)
                    return;

                var group = templatedItems.GetGroup(results.Item1);
                cell = group[results.Item2];

                position = templatedItems.GetGlobalIndexForGroup(group) + results.Item2 + 1;
            }
            else
            {
                position = templatedItems.GetGlobalIndexOfItem(scrollArgs.Item);
                if (position == -1)
                    return;

                cell = templatedItems[position];
            }


            ExecuteScroll(position, e);
        }

        protected virtual void ExecuteScroll(int targetPosition, ScrollToRequestedEventArgs eventArgs)
        {
            if (eventArgs.Position == ScrollToPosition.MakeVisible)
            {
                if (eventArgs.ShouldAnimate)
                    RecyclerView.SmoothScrollToPosition(targetPosition);
                else
                    RecyclerView.ScrollToPosition(targetPosition);
                return;
            }

            if (eventArgs.ShouldAnimate)
            {
                _scroller.SnapPosition = eventArgs.Position;
                _scroller.TargetPosition = targetPosition;
                LayoutManager.StartSmoothScroll(_scroller);
            }
            else
            {
                LayoutManager.ScrollToPositionWithOffset(targetPosition, CalculateScrollOffset(eventArgs.Position));
            }
        }


        protected virtual int CalculateScrollOffset(ScrollToPosition snapPosition)
        {
            // TODO:
            // If variable cell height is wanted to use, must calculate real size here.
            int cellSize = LayoutManager.Orientation == LinearLayoutManager.Horizontal ? CellWidth : CellHeight;
            var containerSize = LayoutManager.Orientation == LinearLayoutManager.Horizontal ? RecyclerView.Width : RecyclerView.Height;

            var offset = 0;

            if (snapPosition == ScrollToPosition.Center)
            {
                offset = containerSize / 2 - cellSize / 2;
            }
            else if (snapPosition == ScrollToPosition.End)
            {
                offset = containerSize - cellSize;
            }

            return offset;
        }
    }
}
