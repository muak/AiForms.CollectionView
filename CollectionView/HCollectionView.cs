using System;
using Xamarin.Forms;

namespace AiForms.Renderers
{
    public class HCollectionView:CollectionView
    {
        public HCollectionView(ListViewCachingStrategy cachingStrategy) : base(cachingStrategy) 
        {
            VerticalOptions = LayoutOptions.Start;
            HorizontalOptions = LayoutOptions.FillAndExpand;
        }

        public HCollectionView() : this(ListViewCachingStrategy.RecycleElement) {}

        public static BindableProperty ColumnWidthProperty =
            BindableProperty.Create(
                nameof(ColumnWidth),
                typeof(double),
                typeof(HCollectionView),
                -1d,
                defaultBindingMode: BindingMode.OneWay
            );

        public double ColumnWidth
        {
            get { return (double)GetValue(ColumnWidthProperty); }
            set { SetValue(ColumnWidthProperty, value); }
        }

        public static BindableProperty SpacingProperty =
            BindableProperty.Create(
                nameof(Spacing),
                typeof(double),
                typeof(HCollectionView),
                default(double),
                defaultBindingMode: BindingMode.OneWay
            );

        public double Spacing
        {
            get { return (double)GetValue(SpacingProperty); }
            set { SetValue(SpacingProperty, value); }
        }

        public static BindableProperty GroupHeaderWidthProperty =
            BindableProperty.Create(
                nameof(GroupHeaderWidth),
                typeof(double),
                typeof(HCollectionView),
                default(double),
                defaultBindingMode: BindingMode.OneWay
            );

        public double GroupHeaderWidth
        {
            get { return (double)GetValue(GroupHeaderWidthProperty); }
            set { SetValue(GroupHeaderWidthProperty, value); }
        }

        public static BindableProperty IsInfiniteProperty =
            BindableProperty.Create(
                nameof(IsInfinite),
                typeof(bool),
                typeof(HCollectionView),
                default(bool),
                defaultBindingMode: BindingMode.OneWay
            );

        public bool IsInfinite
        {
            get { return (bool)GetValue(IsInfiniteProperty); }
            set { SetValue(IsInfiniteProperty, value); }
        }
    }
}
