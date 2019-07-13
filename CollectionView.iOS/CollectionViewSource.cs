using System;
using System.Collections;
using System.Collections.Generic;
using AiForms.Renderers.iOS.Cells;
using CoreGraphics;
using Foundation;
using ObjCRuntime;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace AiForms.Renderers.iOS
{
    [Foundation.Preserve(AllMembers = true)]
    public class CollectionViewSource : UICollectionViewSource, IUICollectionViewDelegateFlowLayout,IUIScrollViewDelegate
    {
        static int s_dataTemplateIncrementer = 2; // lets start at not 0 because

        public CGSize CellSize { get; set; }
        public Dictionary<int, int> Counts { get; set; }
        public bool IsReachedBottom { get; set; }

        protected CollectionView CollectionView;
        protected ITemplatedItemsView<Cell> TemplatedItemsView => CollectionView;


        const int DefaultItemTemplateId = 1;
        bool _isLongTap;
        bool _disposed;

        UICollectionView _uiCollectionView;
        Dictionary<DataTemplate, int> _templateToId = new Dictionary<DataTemplate, int>();

        public CollectionViewSource(CollectionView collectionView, UICollectionView uiCollectionView)
        {
            CollectionView = collectionView;
            _uiCollectionView = uiCollectionView;
            Counts = new Dictionary<int, int>();
            _uiCollectionView.RegisterClassForCell(typeof(ContentCellContainer), DefaultItemTemplateId.ToString());
        }

        protected override void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }
            if (disposing)
            {
                Counts = null;
                _templateToId = null;
                CollectionView = null;
                _uiCollectionView = null;

            }

            _disposed = true;

            base.Dispose(disposing);
        }

        public override void Scrolled(UIScrollView scrollView)
        {
            if (IsReachedBottom || CollectionView.LoadMoreCommand == null)
            {
                return;
            }
        }

        protected void RaiseReachedBottom()
        {
            IsReachedBottom = true;
            CollectionView?.LoadMoreCommand?.Execute(null);
        }

        public override nint NumberOfSections(UICollectionView collectionView)
        {
            if (TemplatedItemsView.TemplatedItems.Count == 0) 
            {
                return 0;
            }

            if (CollectionView.IsGroupingEnabled)
            {
                return TemplatedItemsView.TemplatedItems.Count;
            }

            return 1;
        }

        public override nint GetItemsCount(UICollectionView collectionView, nint section)
        {
            int countOverride;
            if (Counts.TryGetValue((int)section, out countOverride))
            {
                Counts.Remove((int)section);
                return countOverride;
            }

            var templatedItems = CollectionView.TemplatedItems;
            if (CollectionView.IsGroupingEnabled)
            {
                var group = (IList)((IList)templatedItems)[(int)section];
                return group.Count;
            }

            return templatedItems.Count;
        }

        public override void ItemHighlighted(UICollectionView collectionView, NSIndexPath indexPath)
        {
            _isLongTap = false;
            var cell = collectionView.CellForItem(indexPath);
            (cell as ContentCellContainer)?.SelectedAnimation(0.4, 0, 0.5);
        }

        public override void ItemUnhighlighted(UICollectionView collectionView, NSIndexPath indexPath)
        {
            if (_isLongTap)
            {
                return;
            }
            var cell = collectionView.CellForItem(indexPath);
            (cell as ContentCellContainer)?.SelectedAnimation(0.4, 0.5, 0);
        }

        public override bool ShouldShowMenu(UICollectionView collectionView, NSIndexPath indexPath)
        {
            // Detected long tap
            if (CollectionView.ItemLongTapCommand == null)
            {
                return false;
            }

            _isLongTap = true;
            var cell = collectionView.CellForItem(indexPath) as ContentCellContainer;
            var formsCell = cell.ContentCell;

            if (CollectionView.ItemLongTapCommand != null && CollectionView.ItemLongTapCommand.CanExecute(formsCell.BindingContext))
            {
                CollectionView.ItemLongTapCommand.Execute(formsCell.BindingContext);
            }

            (cell as ContentCellContainer)?.SelectedAnimation(1.0, 0.5, 0);

            return true;
        }

        public override bool CanPerformAction(UICollectionView collectionView, Selector action, NSIndexPath indexPath, NSObject sender)
        {
            return false;
        }

        public override void PerformAction(UICollectionView collectionView, Selector action, NSIndexPath indexPath, NSObject sender) { }

        public override void ItemSelected(UICollectionView collectionView, NSIndexPath indexPath)
        {
            var cell = collectionView.CellForItem(indexPath) as ContentCellContainer;

            if (cell == null)
                return;

            var formsCell = cell.ContentCell;

            if (CollectionView.ItemTapCommand != null && CollectionView.ItemTapCommand.CanExecute(formsCell.BindingContext))
            {
                CollectionView.ItemTapCommand.Execute(formsCell.BindingContext);
            }

            var realIndexPath = GetRealIndexPath(indexPath);
            CollectionView.NotifyRowTapped(realIndexPath.Section, realIndexPath.Row, formsCell);

            collectionView.DeselectItem(indexPath, false);
        }

        public override UICollectionReusableView GetViewForSupplementaryElement(UICollectionView collectionView, NSString elementKind, NSIndexPath indexPath)
        {
            if (!CollectionView.IsGroupingEnabled)
                return null;

            if (elementKind == "UICollectionElementKindSectionFooter")
            {
                return null;
            }

            ContentCell cell;
            ContentCellContainer nativeCell;

            var realIndexPath = GetRealIndexPath(indexPath);

            Performance.Start(out string reference);


            var cachingStrategy = CollectionView.CachingStrategy;
            if (cachingStrategy == ListViewCachingStrategy.RetainElement)
            {
                nativeCell = GetNativeHeaderCell(realIndexPath);
            }
            else if ((cachingStrategy & ListViewCachingStrategy.RecycleElement) != 0)
            {
                // Here is used the argument indexPath as it is because header cell will be got not to displayed when IsInfinite.
                nativeCell = collectionView.DequeueReusableSupplementaryView(
                    UICollectionElementKindSection.Header,
                    CollectionViewRenderer.SectionHeaderId,
                    indexPath
                ) as ContentCellContainer;

                if (nativeCell.ContentCell == null)
                {
                    nativeCell = GetNativeHeaderCell(realIndexPath);
                }
                else
                {
                    var templatedList = TemplatedItemsView.TemplatedItems.GetGroup(realIndexPath.Section);

                    cell = (ContentCell)((INativeElementView)nativeCell).Element;
                    cell.SendDisappearing();

                    templatedList.UpdateHeader(cell, realIndexPath.Section);
                    cell.SendAppearing();
                }
            }
            else
            {
                throw new NotSupportedException();
            }

            Performance.Stop(reference);

            return nativeCell;
        }

        public override UICollectionViewCell GetCell(UICollectionView collectionView, NSIndexPath indexPath)
        {
            ContentCell cell;
            ContentCellContainer nativeCell;

            var realIndexPath = GetRealIndexPath(indexPath);

            Performance.Start(out string reference);

            var cachingStrategy = CollectionView.CachingStrategy;
            if (cachingStrategy == ListViewCachingStrategy.RetainElement)
            {
                cell = GetCellForPath(realIndexPath);
                nativeCell = GetNativeCell(cell, realIndexPath);
            }
            else if ((cachingStrategy & ListViewCachingStrategy.RecycleElement) != 0)
            {

                var id = TemplateIdForPath(realIndexPath);


                nativeCell = collectionView.DequeueReusableCell(id.ToString(), indexPath) as ContentCellContainer;
                if (nativeCell.ContentCell == null)
                {
                    cell = GetCellForPath(realIndexPath);

                    nativeCell = GetNativeCell(cell, realIndexPath, true, id.ToString());
                }
                else
                {
                    var templatedList = TemplatedItemsView.TemplatedItems.GetGroup(realIndexPath.Section);

                    cell = (ContentCell)((INativeElementView)nativeCell).Element;
                    cell.SendDisappearing();

                    templatedList.UpdateContent(cell, realIndexPath.Row);
                    cell.SendAppearing();
                }
            }
            else
            {
                throw new NotSupportedException();
            }

            Performance.Stop(reference);
            return nativeCell;
        }

        [Export("collectionView:layout:sizeForItemAtIndexPath:")]
        public virtual CGSize GetSizeForItem(UICollectionView collectionView, UICollectionViewLayout layout, NSIndexPath indexPath)
        {
            return CellSize;
        }

        protected virtual NSIndexPath GetRealIndexPath(NSIndexPath indexPath)
        {
            return indexPath;
        }

        protected virtual ContentCellContainer GetNativeHeaderCell(NSIndexPath indexPath)
        {
            var cell = TemplatedItemsView.TemplatedItems[(int)indexPath.Section] as ContentCell;

            var renderer = (ContentCellRenderer)Xamarin.Forms.Internals.Registrar.Registered.GetHandlerForObject<IRegisterable>(cell);

            var reusableCell = _uiCollectionView.DequeueReusableSupplementaryView(UICollectionElementKindSection.Header, CollectionViewRenderer.SectionHeaderId, indexPath) as ContentCellContainer;

            var nativeCell = renderer.GetCell(cell, reusableCell, _uiCollectionView) as ContentCellContainer;

            var cellWithContent = nativeCell;

            // Sometimes iOS for returns a dequeued cell whose Layer is hidden. 
            // This prevents it from showing up, so lets turn it back on!
            if (cellWithContent.Layer.Hidden)
                cellWithContent.Layer.Hidden = false;

            // Because the layer was hidden we need to layout the cell by hand
            if (cellWithContent != null)
                cellWithContent.LayoutSubviews();

            return nativeCell;
        }

        protected virtual ContentCellContainer GetNativeCell(ContentCell cell, NSIndexPath indexPath, bool recycleCells = false, string templateId = "")
        {
            var id = recycleCells ? templateId : cell.GetType().FullName;

            var renderer = (ContentCellRenderer)Xamarin.Forms.Internals.Registrar.Registered.GetHandlerForObject<IRegisterable>(cell);

            // Note that UICollectionView returns the instance even if called in the first time, unlike UITableView.
            var reusableCell = _uiCollectionView.DequeueReusableCell(id, indexPath) as ContentCellContainer;

            var nativeCell = renderer.GetCell(cell, reusableCell, _uiCollectionView) as ContentCellContainer;

            var cellWithContent = nativeCell;

            // Sometimes iOS for returns a dequeued cell whose Layer is hidden. 
            // This prevents it from showing up, so lets turn it back on!
            if (cellWithContent.Layer.Hidden)
                cellWithContent.Layer.Hidden = false;

            // Because the layer was hidden we need to layout the cell by hand
            if (cellWithContent != null)
                cellWithContent.LayoutSubviews();

            return nativeCell;
        }

        protected virtual int TemplateIdForPath(NSIndexPath indexPath)
        {
            var itemTemplate = CollectionView.ItemTemplate;
            var selector = itemTemplate as DataTemplateSelector;
            if (selector == null)
            {
                return DefaultItemTemplateId;
            }

            var templatedList = GetTemplatedItemsListForPath(indexPath);
            var item = templatedList.ListProxy[indexPath.Row];

            itemTemplate = selector.SelectTemplate(item, CollectionView);
            int key;
            if (!_templateToId.TryGetValue(itemTemplate, out key))
            {
                s_dataTemplateIncrementer++;
                key = s_dataTemplateIncrementer;
                _templateToId[itemTemplate] = key;
                _uiCollectionView.RegisterClassForCell(typeof(ContentCellContainer), key.ToString());
            }

            return key;
        }

        protected virtual ContentCell GetCellForPath(NSIndexPath indexPath)
        {
            var templatedItems = GetTemplatedItemsListForPath(indexPath);
            var cell = templatedItems[indexPath.Row] as ContentCell;
            return cell;
        }

        protected virtual ITemplatedItemsList<Cell> GetTemplatedItemsListForPath(NSIndexPath indexPath)
        {
            var templatedItems = TemplatedItemsView.TemplatedItems;
            if (CollectionView.IsGroupingEnabled)
            {
                templatedItems = (ITemplatedItemsList<Cell>)((IList)templatedItems)[indexPath.Section];
            }

            return templatedItems;
        }
    }
}
