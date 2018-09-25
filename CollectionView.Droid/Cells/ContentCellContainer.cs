using System;
using System.ComponentModel;
using System.Linq;
using Android.Content;
using Android.Views;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Platform.Android;
using Android.Widget;
using Android.Graphics.Drawables;

namespace AiForms.Renderers.Droid.Cells
{
    [Android.Runtime.Preserve(AllMembers = true)]
    public class ContentCellContainer : FrameLayout, INativeElementView
    {
        // Get internal members
        static Type DefaultRenderer = typeof(Platform).Assembly.GetType("Xamarin.Forms.Platform.Android.Platform+DefaultRenderer");

        public ContentViewHolder ViewHolder { get; set; }

        IVisualElementRenderer _contentViewRenderer;
        ContentCell _contentCell;


        public ContentCell ContentCell
        {
            get { return _contentCell; }
            set
            {
                if (_contentCell == value)
                    return;
                UpdateCell(value);
            }
        }
        CollectionView CellParent => ContentCell.Parent as CollectionView;
        ICellController _CellController => ContentCell;

        public Element Element => ContentCell;
        public bool IsEmpty => _contentCell == null;

        public ContentCellContainer(Context context) : base(context)
        {
            Clickable = true;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_contentCell != null)
                {
                    _contentCell.PropertyChanged -= CellPropertyChanged;
                    CellParent.PropertyChanged -= ParentPropertyChanged;
                    _contentCell = null;
                }

                ViewHolder = null;

                _contentViewRenderer?.View?.RemoveFromParent();
                _contentViewRenderer?.Dispose();
                _contentViewRenderer = null;


            }
            base.Dispose(disposing);
        }

        protected override void OnLayout(bool changed, int l, int t, int r, int b)
        {
            if (IsEmpty)
            {
                return;
            }
            Performance.Start(out string reference);

            double width = Context.FromPixels(r - l);
            double height = Context.FromPixels(b - t);

            Performance.Start(reference, "Element.Layout");
            var orientation = Context.Resources.Configuration.Orientation;
            //System.Diagnostics.Debug.WriteLine($"{orientation} BoxSize:{width} / {height}");
            Xamarin.Forms.Layout.LayoutChildIntoBoundingRegion(_contentViewRenderer.Element, new Rectangle(0, 0, width, height));
            Performance.Stop(reference, "Element.Layout");

            _contentViewRenderer.UpdateLayout();
            Performance.Stop(reference);
        }

        protected override void OnMeasure(int widthMeasureSpec, int heightMeasureSpec)
        {
            Performance.Start(out string reference);

            int width = ViewHolder.CellWidth < 0 ? MeasureSpec.GetSize(widthMeasureSpec) : ViewHolder.CellWidth;

            // TODO: If more detail size process is needed,  write here.
            SetMeasuredDimension(width, ViewHolder.CellHeight);

            Performance.Stop(reference);
        }

        public virtual void CellPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == Cell.IsEnabledProperty.PropertyName)
                UpdateIsEnabled();
        }

        public virtual void ParentPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            // avoid running the vain process when popping a page.
            if ((sender as BindableObject)?.BindingContext == null)
            {
                return;
            }

            if (e.PropertyName == CollectionView.TouchFeedbackColorProperty.PropertyName)
                UpdateTouchFeedbackColor();
        }

        public virtual void UpdateNativeCell()
        {
            UpdateTouchFeedbackColor();
            UpdateIsEnabled();
        }

        public void UpdateIsEnabled()
        {
            Enabled = _contentCell.IsEnabled;
        }

        protected virtual void UpdateTouchFeedbackColor()
        {
            if (ViewHolder.IsHeader || CellParent.TouchFeedbackColor.IsDefault)
            {
                return;
            }
            var feedbackColor = CellParent.TouchFeedbackColor.MultiplyAlpha(0.5).ToAndroid();
            if (Foreground == null)
            {
                Foreground = DrawableUtility.CreateRipple(feedbackColor);
            }
            else
            {
                var ripple = Foreground as RippleDrawable;
                ripple.SetColor(DrawableUtility.GetPressedColorSelector(feedbackColor));
            }
        }

        public override bool OnInterceptTouchEvent(MotionEvent ev)
        {
            if (!Enabled)
                return true;

            return base.OnInterceptTouchEvent(ev);
        }

        void CreateNewRenderer(ContentCell cell)
        {
            if (cell.View == null)
            {
                throw new InvalidOperationException($"ViewCell must have a {nameof(cell.View)}");
            }

            _contentCell = cell;
            _contentViewRenderer = Platform.CreateRendererWithContext(cell.View, Context);
            AddView(_contentViewRenderer.View);
            Platform.SetRenderer(cell.View, _contentViewRenderer);

            cell.View.IsPlatformEnabled = true;
        }

        public void UpdateCell(ContentCell cell)
        {
            Performance.Start(out string reference);

            if (!IsEmpty)
            {
                _CellController.SendDisappearing();
            }

            if (_contentViewRenderer == null)
            {
                CreateNewRenderer(cell);
                _CellController.SendAppearing();
                Performance.Stop(reference);
                return;
            }

            var renderer = GetChildAt(0) as IVisualElementRenderer;
            var viewHandlerType = Registrar.Registered.GetHandlerTypeForObject(cell.View) ?? DefaultRenderer;
            var reflectableType = renderer as System.Reflection.IReflectableType;
            var rendererType = reflectableType != null ? reflectableType.GetTypeInfo().AsType() : (renderer != null ? renderer.GetType() : typeof(System.Object));
            if (renderer != null && rendererType == viewHandlerType)
            {
                Performance.Start(reference, "Reuse");
                _contentCell = cell;

                cell.View.DisableLayout = true;
                foreach (VisualElement c in cell.View.Descendants())
                    c.DisableLayout = true;

                Performance.Start(reference, "Reuse.SetElement");
                renderer.SetElement(cell.View);
                Performance.Stop(reference, "Reuse.SetElement");

                Platform.SetRenderer(cell.View, _contentViewRenderer);

                cell.View.DisableLayout = false;
                foreach (VisualElement c in cell.View.Descendants())
                    c.DisableLayout = false;

                var viewAsLayout = cell.View as Layout;
                if (viewAsLayout != null)
                    viewAsLayout.ForceLayout();

                Invalidate();
                _CellController.SendAppearing();
                Performance.Stop(reference, "Reuse");
                Performance.Stop(reference);
                return;
            }

            RemoveView(_contentViewRenderer.View);
            Platform.SetRenderer(_contentCell.View, null);
            _contentCell.View.IsPlatformEnabled = false;
            _contentViewRenderer.View.Dispose();

            _contentCell = cell;
            _contentViewRenderer = Platform.CreateRendererWithContext(_contentCell.View, Context);

            Platform.SetRenderer(_contentCell.View, _contentViewRenderer);
            AddView(_contentViewRenderer.View);

            _CellController.SendAppearing();
            Performance.Stop(reference);
        }

        static bool HasTapGestureRecognizers(Xamarin.Forms.View view)
        {
            return view.GestureRecognizers.Any(t => t is TapGestureRecognizer)
                || view.LogicalChildren.OfType<Xamarin.Forms.View>().Any(HasTapGestureRecognizers);
        }

    }
}
