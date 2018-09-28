using System;
using Xamarin.Forms;

namespace AiForms.Renderers
{
    /// <summary>
    /// HCollection view.
    /// </summary>
    public class HCollectionView:CollectionView
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:AiForms.Renderers.HCollectionView"/> class.
        /// </summary>
        /// <param name="cachingStrategy">Caching strategy.</param>
        public HCollectionView(ListViewCachingStrategy cachingStrategy) : base(cachingStrategy) 
        {
            VerticalOptions = LayoutOptions.Start;
            HorizontalOptions = LayoutOptions.FillAndExpand;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:AiForms.Renderers.HCollectionView"/> class.
        /// </summary>
        public HCollectionView() : this(ListViewCachingStrategy.RecycleElement) {}

        /// <summary>
        /// The column width property.
        /// </summary>
        public static BindableProperty ColumnWidthProperty =
            BindableProperty.Create(
                nameof(ColumnWidth),
                typeof(double),
                typeof(HCollectionView),
                100d,
                defaultBindingMode: BindingMode.OneWay
            );

        /// <summary>
        /// Gets or sets the width of the column.
        /// </summary>
        /// <value>The width of the column.</value>
        public double ColumnWidth
        {
            get { return (double)GetValue(ColumnWidthProperty); }
            set { SetValue(ColumnWidthProperty, value); }
        }

        /// <summary>
        /// The spacing property.
        /// </summary>
        public static BindableProperty SpacingProperty =
            BindableProperty.Create(
                nameof(Spacing),
                typeof(double),
                typeof(HCollectionView),
                default(double),
                defaultBindingMode: BindingMode.OneWay
            );

        /// <summary>
        /// Gets or sets the spacing.
        /// </summary>
        /// <value>The spacing.</value>
        public double Spacing
        {
            get { return (double)GetValue(SpacingProperty); }
            set { SetValue(SpacingProperty, value); }
        }

        /// <summary>
        /// The group header width property.
        /// </summary>
        public static BindableProperty GroupHeaderWidthProperty =
            BindableProperty.Create(
                nameof(GroupHeaderWidth),
                typeof(double),
                typeof(HCollectionView),
                100d,
                defaultBindingMode: BindingMode.OneWay
            );

        /// <summary>
        /// Gets or sets the width of the group header.
        /// </summary>
        /// <value>The width of the group header.</value>
        public double GroupHeaderWidth
        {
            get { return (double)GetValue(GroupHeaderWidthProperty); }
            set { SetValue(GroupHeaderWidthProperty, value); }
        }

        /// <summary>
        /// The is infinite property.
        /// </summary>
        public static BindableProperty IsInfiniteProperty =
            BindableProperty.Create(
                nameof(IsInfinite),
                typeof(bool),
                typeof(HCollectionView),
                default(bool),
                defaultBindingMode: BindingMode.OneWay
            );

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="T:AiForms.Renderers.HCollectionView"/> is infinite.
        /// </summary>
        /// <value><c>true</c> if is infinite; otherwise, <c>false</c>.</value>
        public bool IsInfinite
        {
            get { return (bool)GetValue(IsInfiniteProperty); }
            set { SetValue(IsInfiniteProperty, value); }
        }
    }
}
