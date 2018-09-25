using AiForms.Renderers;
using AiForms.Renderers.Droid.Cells;
using Android.Content;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using AView = Android.Views.View;

[assembly: ExportRenderer(typeof(ContentCell), typeof(ContentCellRenderer))]
namespace AiForms.Renderers.Droid.Cells
{
    [Android.Runtime.Preserve(AllMembers = true)]
    public class ContentCellRenderer:IRegisterable
    {
        static readonly BindableProperty RendererProperty = BindableProperty.CreateAttached("Renderer", typeof(ContentCellRenderer), typeof(ContentCell), null);

        public AView GetCell(ContentCell formsCell, ContentCellContainer nativeCell, Android.Views.ViewGroup parent, Context context)
        {
            Performance.Start(out string reference);

            if(nativeCell.ContentCell != null)
            {
                ClearPropertyChanged(nativeCell);
            }

            nativeCell.ContentCell = formsCell;

            SetUpPropertyChanged(nativeCell);

            nativeCell.UpdateNativeCell();

            Performance.Stop(reference);

            return nativeCell;
        }

        protected virtual void SetUpPropertyChanged(ContentCellContainer nativeCell)
        {
            var formsCell = nativeCell.ContentCell as ContentCell;
            var parentElement = formsCell?.Parent as CollectionView;

            formsCell.PropertyChanged += nativeCell.CellPropertyChanged;

            if (parentElement != null) {
                parentElement.PropertyChanged += nativeCell.ParentPropertyChanged;
            }
        }

        protected virtual void ClearPropertyChanged(ContentCellContainer nativeCell)
        {
            var formsCell = nativeCell.ContentCell as ContentCell;
            var parentElement = formsCell.Parent as CollectionView;

            formsCell.PropertyChanged -= nativeCell.CellPropertyChanged;
            if (parentElement != null) {
                parentElement.PropertyChanged -= nativeCell.ParentPropertyChanged;
            }
        }

        internal static ContentCellRenderer GetRenderer(BindableObject cell)
        {
            return (ContentCellRenderer)cell.GetValue(RendererProperty);
        }

        internal static void SetRenderer(BindableObject cell, ContentCellRenderer renderer)
        {
            cell.SetValue(RendererProperty, renderer);
        }

    }
}
