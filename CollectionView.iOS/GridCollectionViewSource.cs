using System;
using System.Collections.Generic;
using CoreGraphics;
using Foundation;
using UIKit;

namespace AiForms.Renderers.iOS
{
    [Foundation.Preserve(AllMembers = true)]
    public class GridCollectionViewSource:CollectionViewSource
    {
        public int SurplusPixel { get; set; } = 0;
        public List<int> AdjustCellSizeList { get; set; } = new List<int>();
        GridCollectionView _gridCollectionView => CollectionView as GridCollectionView;

        public GridCollectionViewSource(CollectionView collectionView, UICollectionView uiCollectionView)
            :base(collectionView,uiCollectionView)
        {
        }

        public override CGSize GetSizeForItem(UICollectionView collectionView, UICollectionViewLayout layout, NSIndexPath indexPath)
        {
            if(_gridCollectionView.GridType != GridType.UniformGrid){
                return base.GetSizeForItem(collectionView, layout, indexPath);
            }

            var totalColumns = 0;

            switch (UIApplication.SharedApplication.StatusBarOrientation)
            {
                case UIInterfaceOrientation.Portrait:
                case UIInterfaceOrientation.PortraitUpsideDown:
                case UIInterfaceOrientation.Unknown:
                    totalColumns = _gridCollectionView.PortraitColumns;

                    break;
                case UIInterfaceOrientation.LandscapeLeft:
                case UIInterfaceOrientation.LandscapeRight:
                    totalColumns = _gridCollectionView.LandscapeColumns;
                    break;
            }

            var column = indexPath.Row % totalColumns; 

            if(column <= SurplusPixel - 1) {
                // assign 1px to the cell width in order from the first cell until the surplus is gone.
                return new CGSize(CellSize.Width + 1, CellSize.Height);
            }

            return CellSize;
        }
    }
}
