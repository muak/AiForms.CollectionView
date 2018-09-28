using Xamarin.Forms;

namespace AiForms.Renderers
{
    /// <summary>
    /// Grid type.
    /// </summary>
    public enum GridType
    {
        /// <summary>
        /// The uniform grid.
        /// </summary>
        UniformGrid,
        /// <summary>
        /// The auto spacing grid.
        /// </summary>
        AutoSpacingGrid
    }

    /// <summary>
    /// Spacing type.
    /// </summary>
    public enum SpacingType
    {
        /// <summary>
        /// The between.
        /// </summary>
        Between,
        /// <summary>
        /// The center.
        /// </summary>
        Center
    }

    /// <summary>
    /// Grid collection view.
    /// </summary>
    public class GridCollectionView : CollectionView
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:AiForms.Renderers.GridCollectionView"/> class.
        /// </summary>
        /// <param name="cachingStrategy">Caching strategy.</param>
        public GridCollectionView(ListViewCachingStrategy cachingStrategy) : base(cachingStrategy) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:AiForms.Renderers.GridCollectionView"/> class.
        /// </summary>
        public GridCollectionView() : base(ListViewCachingStrategy.RecycleElement) { }

        /// <summary>
        /// The portrait columns property.
        /// </summary>
        public static BindableProperty PortraitColumnsProperty =
            BindableProperty.Create(
                nameof(PortraitColumns),
                typeof(int),
                typeof(GridCollectionView),
                2,
                defaultBindingMode: BindingMode.OneWay
            );

        /// <summary>
        /// Gets or sets the portrait columns.
        /// </summary>
        /// <value>The portrait columns.</value>
        public int PortraitColumns
        {
            get { return (int)GetValue(PortraitColumnsProperty); }
            set { SetValue(PortraitColumnsProperty, value); }
        }

        /// <summary>
        /// The landscape columns property.
        /// </summary>
        public static BindableProperty LandscapeColumnsProperty =
            BindableProperty.Create(
                nameof(LandscapeColumns),
                typeof(int),
                typeof(GridCollectionView),
                4,
                defaultBindingMode: BindingMode.OneWay
            );

        /// <summary>
        /// Gets or sets the landscape columns.
        /// </summary>
        /// <value>The landscape columns.</value>
        public int LandscapeColumns
        {
            get { return (int)GetValue(LandscapeColumnsProperty); }
            set { SetValue(LandscapeColumnsProperty, value); }
        }

        /// <summary>
        /// The row spacing property.
        /// </summary>
        public static BindableProperty RowSpacingProperty =
            BindableProperty.Create(
                nameof(RowSpacing),
                typeof(double),
                typeof(GridCollectionView),
                default(double),
                defaultBindingMode: BindingMode.OneWay
            );

        /// <summary>
        /// Gets or sets the row spacing.
        /// </summary>
        /// <value>The row spacing.</value>
        public double RowSpacing
        {
            get { return (double)GetValue(RowSpacingProperty); }
            set { SetValue(RowSpacingProperty, value); }
        }

        /// <summary>
        /// The column spacing property.
        /// </summary>
        public static BindableProperty ColumnSpacingProperty =
            BindableProperty.Create(
                nameof(ColumnSpacing),
                typeof(double),
                typeof(GridCollectionView),
                default(double),
                defaultBindingMode: BindingMode.OneWay
            );

        /// <summary>
        /// Gets or sets the column spacing.
        /// </summary>
        /// <value>The column spacing.</value>
        public double ColumnSpacing
        {
            get { return (double)GetValue(ColumnSpacingProperty); }
            set { SetValue(ColumnSpacingProperty, value); }
        }

        /// <summary>
        /// The column width property.
        /// </summary>
        public static BindableProperty ColumnWidthProperty =
            BindableProperty.Create(
                nameof(ColumnWidth),
                typeof(double),
                typeof(GridCollectionView),
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
        /// The column height property.
        /// </summary>
        public static BindableProperty ColumnHeightProperty =
            BindableProperty.Create(
                nameof(ColumnHeight),
                typeof(double),
                typeof(GridCollectionView),
                1.0d,
                defaultBindingMode: BindingMode.OneWay
        );

        /// <summary>
        /// Gets or sets the height of the column.
        /// </summary>
        /// <value>The height of the column.</value>
        public double ColumnHeight
        {
            get { return (double)GetValue(ColumnHeightProperty); }
            set { SetValue(ColumnHeightProperty, value); }
        }

        /// <summary>
        /// The additional height property.
        /// </summary>
        public static BindableProperty AdditionalHeightProperty =
            BindableProperty.Create(
                nameof(AdditionalHeight),
                typeof(double),
                typeof(GridCollectionView),
                default(double),
                defaultBindingMode: BindingMode.OneWay
            );

        /// <summary>
        /// Gets or sets the height of the additional.
        /// </summary>
        /// <value>The height of the additional.</value>
        public double AdditionalHeight
        {
            get { return (double)GetValue(AdditionalHeightProperty); }
            set { SetValue(AdditionalHeightProperty, value); }
        }

        /// <summary>
        /// The computed width property.
        /// </summary>
        public static BindableProperty ComputedWidthProperty =
            BindableProperty.Create(
                nameof(ComputedWidth),
                typeof(double),
                typeof(GridCollectionView),
                default(double),
                defaultBindingMode: BindingMode.OneWayToSource
            );

        /// <summary>
        /// Gets the width of the computed.
        /// </summary>
        /// <value>The width of the computed.</value>
        public double ComputedWidth
        {
            get { return (double)GetValue(ComputedWidthProperty); }
        }

        /// <summary>
        /// The computed height property.
        /// </summary>
        public static BindableProperty ComputedHeightProperty =
            BindableProperty.Create(
                nameof(ComputedHeight),
                typeof(double),
                typeof(GridCollectionView),
                default(double),
                defaultBindingMode: BindingMode.OneWayToSource
            );

        /// <summary>
        /// Gets the height of the computed.
        /// </summary>
        /// <value>The height of the computed.</value>
        public double ComputedHeight
        {
            get { return (double)GetValue(ComputedHeightProperty); }
        }

        /// <summary>
        /// The group header height property.
        /// </summary>
        public static BindableProperty GroupHeaderHeightProperty =
            BindableProperty.Create(
                nameof(GroupHeaderHeight),
                typeof(double),
                typeof(GridCollectionView),
                36d,
                defaultBindingMode: BindingMode.OneWay
            );

        /// <summary>
        /// Gets or sets the height of the group header.
        /// </summary>
        /// <value>The height of the group header.</value>
        public double GroupHeaderHeight
        {
            get { return (double)GetValue(GroupHeaderHeightProperty); }
            set { SetValue(GroupHeaderHeightProperty, value); }
        }

        /// <summary>
        /// The grid type property.
        /// </summary>
        public static BindableProperty GridTypeProperty =
            BindableProperty.Create(
                nameof(GridType),
                typeof(GridType),
                typeof(GridCollectionView),
                GridType.AutoSpacingGrid,
                defaultBindingMode: BindingMode.OneWay
            );

        /// <summary>
        /// Gets or sets the type of the grid.
        /// </summary>
        /// <value>The type of the grid.</value>
        public GridType GridType
        {
            get { return (GridType)GetValue(GridTypeProperty); }
            set { SetValue(GridTypeProperty, value); }
        }

        /// <summary>
        /// The spacing type property.
        /// </summary>
        public static BindableProperty SpacingTypeProperty =
            BindableProperty.Create(
                nameof(SpacingType),
                typeof(SpacingType),
                typeof(GridCollectionView),
                SpacingType.Between,
                defaultBindingMode: BindingMode.OneWay
            );

        /// <summary>
        /// Gets or sets the type of the spacing.
        /// </summary>
        /// <value>The type of the spacing.</value>
        public SpacingType SpacingType
        {
            get { return (SpacingType)GetValue(SpacingTypeProperty); }
            set { SetValue(SpacingTypeProperty, value); }
        }

        /// <summary>
        /// The pull to refresh color property.
        /// </summary>
        public static BindableProperty PullToRefreshColorProperty =
            BindableProperty.Create(
                nameof(PullToRefreshColor),
                typeof(Color),
                typeof(GridCollectionView),
                default(Color),
                defaultBindingMode: BindingMode.OneWay
            );

        /// <summary>
        /// Gets or sets the color of the pull to refresh.
        /// </summary>
        /// <value>The color of the pull to refresh.</value>
        public Color PullToRefreshColor
        {
            get { return (Color)GetValue(PullToRefreshColorProperty); }
            set { SetValue(PullToRefreshColorProperty, value); }
        }

    }
}
