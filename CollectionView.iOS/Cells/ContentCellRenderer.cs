using AiForms.Renderers;
using AiForms.Renderers.iOS.Cells;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

[assembly: ExportRenderer(typeof(ContentCell), typeof(ContentCellRenderer))]
namespace AiForms.Renderers.iOS.Cells
{
    [Foundation.Preserve(AllMembers = true)]
    public class ContentCellRenderer : IRegisterable
    {
        public virtual UICollectionViewCell GetCell(ContentCell item, ContentCellContainer reusableCell, UICollectionView cv)
        {
            Performance.Start(out string reference);

            if (reusableCell.ContentCell != null)
            {
                ClearPropertyChanged(reusableCell);
            }

            reusableCell.ContentCell = item;

            SetUpPropertyChanged(reusableCell);

            reusableCell.UpdateNativeCell();

            Performance.Stop(reference);

            return reusableCell;
        }

        protected virtual void SetUpPropertyChanged(ContentCellContainer nativeCell)
        {
            var formsCell = nativeCell.ContentCell as ContentCell;
            var parentElement = formsCell?.Parent as CollectionView;

            formsCell.PropertyChanged += nativeCell.CellPropertyChanged;

            if (parentElement != null)
            {
                parentElement.PropertyChanged += nativeCell.ParentPropertyChanged;
            }
        }

        protected virtual void ClearPropertyChanged(ContentCellContainer nativeCell)
        {
            var formsCell = nativeCell.ContentCell as ContentCell;
            var parentElement = formsCell.Parent as CollectionView;

            formsCell.PropertyChanged -= nativeCell.CellPropertyChanged;

            if (parentElement != null)
            {
                parentElement.PropertyChanged -= nativeCell.ParentPropertyChanged;
            }
        }
    }


}
