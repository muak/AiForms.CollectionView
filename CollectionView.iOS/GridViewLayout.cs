using System;
using Foundation;
using UIKit;
using CoreGraphics;
using System.Linq;
namespace AiForms.Renderers.iOS
{
    public class GridViewLayout:UICollectionViewFlowLayout
    {
        public override UICollectionViewLayoutAttributes[] LayoutAttributesForElementsInRect(CGRect rect)
        {
            var attributes = base.LayoutAttributesForElementsInRect(rect);
            attributes.Where(x => x.RepresentedElementCategory == UICollectionElementCategory.Cell);
            for (var i = 0; i < attributes.Length;i++)
            {
                if(attributes[i].RepresentedElementCategory == UICollectionElementCategory.Cell && attributes[i].IndexPath.Row == 0)
                {
                    attributes[i] = LayoutAttributesForItem(attributes[i].IndexPath);
                }
            }

            return attributes;
        }

        public override UICollectionViewLayoutAttributes LayoutAttributesForItem(NSIndexPath indexPath)
        {
            var curAttributes = base.LayoutAttributesForItem(indexPath);
            if(indexPath.Row == 0) 
            {
                var rect = curAttributes.Frame;
                curAttributes.Frame = new CGRect(SectionInset.Left, rect.Y, rect.Width, rect.Height);
                return curAttributes;
            }

            return curAttributes;
        }
    }
}
