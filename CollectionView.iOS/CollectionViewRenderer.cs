using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Threading.Tasks;
using CoreGraphics;
using Foundation;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Platform.iOS;
using System.Linq;

namespace AiForms.Renderers.iOS
{
    [Foundation.Preserve(AllMembers = true)]
    public class CollectionViewRenderer : ViewRenderer<CollectionView, UICollectionView>,IUIScrollViewDelegate
    {
        public const string SectionHeaderId = "SectionHeader";
        protected CollectionViewSource DataSource;
        protected UICollectionViewFlowLayout ViewLayout;
        protected ITemplatedItemsView<Cell> TemplatedItemsView => Element;
        protected bool IsReachedBottom;
        bool _disposed;

        protected override void OnElementChanged(ElementChangedEventArgs<CollectionView> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null)
            {
                var templatedItems = ((ITemplatedItemsView<Cell>)e.OldElement).TemplatedItems;
                templatedItems.CollectionChanged -= OnCollectionChanged;
                templatedItems.GroupedCollectionChanged -= OnGroupedCollectionChanged;
                e.OldElement.ScrollToRequested -= OnScrollToRequested;
                e.OldElement.EndLoadingAction = null;
                (Control as UIScrollView).Delegate = null;
            }

            if (e.NewElement != null)
            {

                var templatedItems = ((ITemplatedItemsView<Cell>)e.NewElement).TemplatedItems;

                templatedItems.CollectionChanged += OnCollectionChanged;
                templatedItems.GroupedCollectionChanged += OnGroupedCollectionChanged;
                e.NewElement.ScrollToRequested += OnScrollToRequested;
                (Control as UIScrollView).Delegate = this;
                e.NewElement.EndLoadingAction = () =>
                {
                    IsReachedBottom = false;
                };

                UpdateBackgroundColor();
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }

            if (disposing)
            {             
                (Control as UIScrollView).Delegate = null;
                ViewLayout?.Dispose();
                ViewLayout = null;

                foreach (UIView subview in Subviews)
                {
                    DisposeSubviews(subview);
                }

                if (Element != null)
                {
                    var templatedItems = TemplatedItemsView.TemplatedItems;
                    templatedItems.CollectionChanged -= OnCollectionChanged;
                    templatedItems.GroupedCollectionChanged -= OnGroupedCollectionChanged;
                    Element.ScrollToRequested -= OnScrollToRequested;
                    Element.EndLoadMore = null;
                }
            }

            _disposed = true;

            base.Dispose(disposing);
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            if (e.PropertyName == Xamarin.Forms.ListView.IsGroupingEnabledProperty.PropertyName)
            {
                Control.ReloadData();
            }
            else if (e.PropertyName == VisualElement.BackgroundColorProperty.PropertyName)
            {
                UpdateBackgroundColor();
            }
        }

        [Foundation.Export("scrollViewDidScroll:")]
        public virtual void Scrolled(UIKit.UIScrollView scrollView)
        {
            if(IsReachedBottom || Element.LoadMoreCommand == null)
            {
                return;
            }

            if(ViewLayout.ScrollDirection == UICollectionViewScrollDirection.Horizontal)
            {
                if(scrollView.ContentSize.Width <= scrollView.ContentOffset.X + scrollView.Bounds.Width)
                {
                    RaiseReachedBottom();
                }
            }
            else
            {
                if(scrollView.ContentSize.Height <= scrollView.ContentOffset.Y + scrollView.Bounds.Height)
                {
                    RaiseReachedBottom();
                }
            }
        }

        void RaiseReachedBottom()
        {
            IsReachedBottom = true;
            Element?.LoadMoreCommand?.Execute(null);
        }

        protected virtual async void OnScrollToRequested(object sender, ScrollToRequestedEventArgs e)
        {
            if (Superview == null)
            {
                return;
            }

            var position = GetScrollPosition(e.Position);
            var scrollArgs = (ITemplatedItemsListScrollToRequestedEventArgs)e;

            if (scrollArgs.Item == null)
            {
                // What the Item is null means that either ScrollToStart or ScrollToEnd has been sent from FormsView.
                if (e.Position == ScrollToPosition.Start)
                {
                    Control.SetContentOffset(CGPoint.Empty, e.ShouldAnimate);
                    return;
                }

                CGPoint offsetPoint = CGPoint.Empty;
                if (ViewLayout.ScrollDirection == UICollectionViewScrollDirection.Horizontal)
                {
                    offsetPoint = new CGPoint(Control.ContentSize.Width - Control.Frame.Size.Width, 0);
                }
                else
                {
                    offsetPoint = new CGPoint(0, Control.ContentSize.Height - Control.Frame.Size.Height);
                }

                Control.SetContentOffset(offsetPoint, e.ShouldAnimate);

                return;
            }

            var templatedItems = TemplatedItemsView.TemplatedItems;
            if (Element.IsGroupingEnabled)
            {
                var result = templatedItems.GetGroupAndIndexOfItem(scrollArgs.Group, scrollArgs.Item);
                if (result.Item1 != -1 && result.Item2 != -1)
                {
                    if (result.Item2 + result.Item1 == 0 && e.Position == ScrollToPosition.Start)
                    {
                        Control.SetContentOffset(CGPoint.Empty, e.ShouldAnimate);
                    }
                    else if (result.Item2 == 0 && e.Position == ScrollToPosition.Start)
                    {
                        var attr = Control.GetLayoutAttributesForSupplementaryElement(UICollectionElementKindSectionKey.Header, NSIndexPath.FromRowSection(0, result.Item1));
                        var offset = 0d;
                        CGPoint headerPoint = CGPoint.Empty;
                        if (ViewLayout.ScrollDirection == UICollectionViewScrollDirection.Horizontal)
                        {
                            offset = Math.Min(Math.Max(0, attr.Frame.X), Control.ContentSize.Width - Control.Frame.Size.Width);
                            headerPoint = new CGPoint(offset, 0);
                        }
                        else
                        {
                            offset = Math.Min(Math.Max(0, attr.Frame.Y), Control.ContentSize.Height - Control.Frame.Size.Height);
                            headerPoint = new CGPoint(0, offset);
                        }

                        Control.SetContentOffset(headerPoint, e.ShouldAnimate);
                    }
                    else
                    {
                        Control.ScrollToItem(NSIndexPath.FromRowSection(result.Item2, result.Item1), position, e.ShouldAnimate);
                    }
                }
            }
            else
            {
                var index = templatedItems.GetGlobalIndexOfItem(scrollArgs.Item);
                if (index != -1)
                {
                    Control.Layer.RemoveAllAnimations();

                    await Task.Delay(1); // iOS11 hack

                    Control.ScrollToItem(NSIndexPath.FromRowSection(index, 0), position, e.ShouldAnimate);
                }
            }
        }

        protected virtual UICollectionViewScrollPosition GetScrollPosition(ScrollToPosition position)
        {
            return UICollectionViewScrollPosition.None;
        }

        protected virtual void DisposeSubviews(UIView view)
        {
            var ver = view as IVisualElementRenderer;

            if (ver == null)
            {
                // VisualElementRenderers should implement their own dispose methods that will appropriately dispose and remove their child views.
                // Attempting to do this work twice could cause a SIGSEGV (only observed in iOS8), so don't do this work here.
                // Non-renderer views, such as separator lines, etc., can be removed here.
                foreach (UIView subView in view.Subviews)
                {
                    DisposeSubviews(subView);
                }

                view.RemoveFromSuperview();
            }

            view.Dispose();
        }

        protected virtual void UpdateBackgroundColor()
        {
            if (Element.BackgroundColor.IsDefault)
            {
                Control.BackgroundColor = UIColor.Clear;
            }
            else
            {
                Control.BackgroundColor = Element.BackgroundColor.ToUIColor();
            }
        }

        protected virtual void OnGroupedCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            var til = (TemplatedItemsList<ItemsView<Cell>, Cell>)sender;

            var templatedItems = TemplatedItemsView.TemplatedItems;

            // On iOS, sometimes the groupIndex can't be correctly got using 
            // "templatedItems.IndexOf(til.HeaderContent)" for some reason.
            // So BindingContext instead of HeaderContent is used and got the group index.
            var groupIndex = TemplatedItemsView.TemplatedItems.ListProxy.IndexOf(til.BindingContext);
            UpdateItems(e, groupIndex, false);
        }

        protected virtual void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            UpdateItems(e, 0, true);
        }

        protected virtual void UpdateItems(NotifyCollectionChangedEventArgs e, int section, bool resetWhenGrouped, bool forceReset = false)
        {
            var exArgs = e as NotifyCollectionChangedEventArgsEx;
            if (exArgs != null)
            {
                DataSource.Counts[section] = exArgs.Count;
            }

            // This means the UITableView hasn't rendered any cells yet
            // so there's no need to synchronize the rows on the UITableView
            if (Control.IndexPathsForVisibleItems == null && e.Action != NotifyCollectionChangedAction.Reset)
            {
                return;
            }

            var groupReset = (resetWhenGrouped && Element.IsGroupingEnabled) || forceReset;

            // HACK: When an item is added for the first time, UICollectionView is sometimes crashed for some reason.
            // So, in that case, ReloadData is called.
            if (!Control.IndexPathsForVisibleItems.Any())
            {
                groupReset = true;
            }

            // We can't do this check on grouped lists because the index doesn't match the number of rows in a section.
            // Likewise, we can't do this check on lists using RecycleElement because the number of rows in a section will remain constant because they are reused.
            if (!groupReset && Element.CachingStrategy == ListViewCachingStrategy.RetainElement)
            {
                var lastIndex = Control.NumberOfItemsInSection(section);
                if (e.NewStartingIndex > lastIndex || e.OldStartingIndex > lastIndex)
                    throw new ArgumentException(
                        $"Index '{Math.Max(e.NewStartingIndex, e.OldStartingIndex)}' is greater than the number of rows '{lastIndex}'.");
            }

            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:

                    if (e.NewStartingIndex == -1 || groupReset)
                    {
                        goto case NotifyCollectionChangedAction.Reset;
                    }

                    Control.PerformBatchUpdates(() =>
                    {
                        Control.InsertItems(GetPaths(section, e.NewStartingIndex, e.NewItems.Count));
                    }, (finished) =>
                    {
                    });

                    break;

                case NotifyCollectionChangedAction.Remove:
                    if (e.OldStartingIndex == -1 || groupReset)
                    {
                        goto case NotifyCollectionChangedAction.Reset;
                    }
                    Control.PerformBatchUpdates(() =>
                    {
                        Control.DeleteItems(GetPaths(section, e.OldStartingIndex, e.OldItems.Count));
                    }, (finished) =>
                    {
                    });

                    break;

                case NotifyCollectionChangedAction.Move:
                    if (e.OldStartingIndex == -1 || e.NewStartingIndex == -1 || groupReset)
                    {
                        goto case NotifyCollectionChangedAction.Reset;
                    }
                    Control.PerformBatchUpdates(() =>
                    {
                        for (var i = 0; i < e.OldItems.Count; i++)
                        {
                            var oldi = e.OldStartingIndex;
                            var newi = e.NewStartingIndex;

                            if (e.NewStartingIndex < e.OldStartingIndex)
                            {
                                oldi += i;
                                newi += i;
                            }

                            Control.MoveItem(NSIndexPath.FromRowSection(oldi, section), NSIndexPath.FromRowSection(newi, section));
                        }
                    }, (finished) =>
                    {
                    });

                    break;

                case NotifyCollectionChangedAction.Replace:
                    if (e.OldStartingIndex == -1 || groupReset)
                    {
                        goto case NotifyCollectionChangedAction.Reset;
                    }

                    Control.PerformBatchUpdates(() =>
                    {
                        Control.ReloadItems(GetPaths(section, e.OldStartingIndex, e.OldItems.Count));
                    }, (finished) =>
                    {
                    });

                    break;

                case NotifyCollectionChangedAction.Reset:
                    Control.ReloadData();
                    Control.CollectionViewLayout.InvalidateLayout(); // for iOS10
                    return;
            }
        }

        // TODO: For the next version, group changed event will appropriately be done, too.  
        protected virtual void UpdateGroups(NotifyCollectionChangedEventArgs e)
        {
            var exArgs = e as NotifyCollectionChangedEventArgsEx;
            if (exArgs != null)
                DataSource.Counts[e.NewStartingIndex] = exArgs.Count;

            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:

                    Control.PerformBatchUpdates(() =>
                    {
                        Control.InsertSections(NSIndexSet.FromIndex(e.NewStartingIndex));
                    }, (finished) =>
                    {
                    });

                    break;
                case NotifyCollectionChangedAction.Remove:

                    Control.PerformBatchUpdates(() =>
                    {
                        Control.DeleteSections(NSIndexSet.FromIndex(e.OldStartingIndex));
                    }, (finished) =>
                    {
                    });

                    break;
                // the other pattern
            }
        }

        protected virtual NSIndexPath[] GetPaths(int section, int index, int count)
        {
            var paths = new NSIndexPath[count];
            for (var i = 0; i < paths.Length; i++)
            {
                paths[i] = NSIndexPath.FromRowSection(index + i, section);
            }

            return paths;
        }
    }
}
