using Xamarin.Forms;

namespace AiForms.Renderers
{
    public enum GridType
    {
        UniformGrid,
        AutoSpacingGrid
    }
    public enum SpacingType
    {
        Between,
        Center
    }

    public class GridCollectionView : CollectionView
    {
        public GridCollectionView(ListViewCachingStrategy cachingStrategy) : base(cachingStrategy) { }

        public GridCollectionView() : base(ListViewCachingStrategy.RecycleElement) { }

        public static BindableProperty PortraitColumnsProperty =
            BindableProperty.Create(
                nameof(PortraitColumns),
                typeof(int),
                typeof(GridCollectionView),
                2,
                defaultBindingMode: BindingMode.OneWay
            );

        public int PortraitColumns
        {
            get { return (int)GetValue(PortraitColumnsProperty); }
            set { SetValue(PortraitColumnsProperty, value); }
        }

        public static BindableProperty LandscapeColumnsProperty =
            BindableProperty.Create(
                nameof(LandscapeColumns),
                typeof(int),
                typeof(GridCollectionView),
                4,
                defaultBindingMode: BindingMode.OneWay
            );

        public int LandscapeColumns
        {
            get { return (int)GetValue(LandscapeColumnsProperty); }
            set { SetValue(LandscapeColumnsProperty, value); }
        }

        public static BindableProperty RowSpacingProperty =
            BindableProperty.Create(
                nameof(RowSpacing),
                typeof(double),
                typeof(GridCollectionView),
                default(double),
                defaultBindingMode: BindingMode.OneWay
            );

        public double RowSpacing
        {
            get { return (double)GetValue(RowSpacingProperty); }
            set { SetValue(RowSpacingProperty, value); }
        }

        public static BindableProperty ColumnSpacingProperty =
            BindableProperty.Create(
                nameof(ColumnSpacing),
                typeof(double),
                typeof(GridCollectionView),
                default(double),
                defaultBindingMode: BindingMode.OneWay
            );

        public double ColumnSpacing
        {
            get { return (double)GetValue(ColumnSpacingProperty); }
            set { SetValue(ColumnSpacingProperty, value); }
        }

        public static BindableProperty ColumnWidthProperty =
            BindableProperty.Create(
                nameof(ColumnWidth),
                typeof(double),
                typeof(GridCollectionView),
                -1d,
                defaultBindingMode: BindingMode.OneWay
            );

        public double ColumnWidth
        {
            get { return (double)GetValue(ColumnWidthProperty); }
            set { SetValue(ColumnWidthProperty, value); }
        }

        public static BindableProperty ColumnHeightProperty =
            BindableProperty.Create(
                nameof(ColumnHeight),
                typeof(double),
                typeof(GridCollectionView),
                -1d,
                defaultBindingMode: BindingMode.OneWay
        );

        public double ColumnHeight
        {
            get { return (double)GetValue(ColumnHeightProperty); }
            set { SetValue(ColumnHeightProperty, value); }
        }

        public static BindableProperty GroupHeaderHeightProperty =
            BindableProperty.Create(
                nameof(GroupHeaderHeight),
                typeof(double),
                typeof(GridCollectionView),
                default(double),
                defaultBindingMode: BindingMode.OneWay
            );

        public double GroupHeaderHeight
        {
            get { return (double)GetValue(GroupHeaderHeightProperty); }
            set { SetValue(GroupHeaderHeightProperty, value); }
        }

        public static BindableProperty GridTypeProperty =
            BindableProperty.Create(
                nameof(GridType),
                typeof(GridType),
                typeof(GridCollectionView),
                GridType.UniformGrid,
                defaultBindingMode: BindingMode.OneWay
            );

        public GridType GridType
        {
            get { return (GridType)GetValue(GridTypeProperty); }
            set { SetValue(GridTypeProperty, value); }
        }

        public static BindableProperty SpacingTypeProperty =
            BindableProperty.Create(
                nameof(SpacingType),
                typeof(SpacingType),
                typeof(GridCollectionView),
                SpacingType.Between,
                defaultBindingMode: BindingMode.OneWay
            );

        public SpacingType SpacingType
        {
            get { return (SpacingType)GetValue(SpacingTypeProperty); }
            set { SetValue(SpacingTypeProperty, value); }
        }

        public static BindableProperty PullToRefreshColorProperty =
            BindableProperty.Create(
                nameof(PullToRefreshColor),
                typeof(Color),
                typeof(GridCollectionView),
                default(Color),
                defaultBindingMode: BindingMode.OneWay
            );

        public Color PullToRefreshColor
        {
            get { return (Color)GetValue(PullToRefreshColorProperty); }
            set { SetValue(PullToRefreshColorProperty, value); }
        }

    }
}
