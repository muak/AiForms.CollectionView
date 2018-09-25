using System;
using Xamarin.Forms;
using System.Windows.Input;

namespace AiForms.Renderers
{
    public class CollectionView : ListView
    {
        public CollectionView(ListViewCachingStrategy cachingStrategy):base(cachingStrategy){
            ScrollController = new ScrollController(this);
        }

        public CollectionView():this(ListViewCachingStrategy.RecycleElement){}

        public static BindableProperty ItemTapCommandProperty =
            BindableProperty.Create(
                nameof(ItemTapCommand),
                typeof(ICommand),
                typeof(CollectionView),
                default(ICommand),
                defaultBindingMode: BindingMode.OneWay
            );

        public ICommand ItemTapCommand {
            get { return (ICommand)GetValue(ItemTapCommandProperty); }
            set { SetValue(ItemTapCommandProperty, value); }
        }

        public static BindableProperty ItemLongTapCommandProperty =
            BindableProperty.Create(
                nameof(ItemLongTapCommand),
                typeof(ICommand),
                typeof(CollectionView),
                default(ICommand),
                defaultBindingMode: BindingMode.OneWay
            );

        public ICommand ItemLongTapCommand {
            get { return (ICommand)GetValue(ItemLongTapCommandProperty); }
            set { SetValue(ItemLongTapCommandProperty, value); }
        }

        public static BindableProperty TouchFeedbackColorProperty =
            BindableProperty.Create(
                nameof(TouchFeedbackColor),
                typeof(Color),
                typeof(CollectionView),
                default(Color),
                defaultBindingMode: BindingMode.OneWay
            );

        public Color TouchFeedbackColor {
            get { return (Color)GetValue(TouchFeedbackColorProperty); }
            set { SetValue(TouchFeedbackColorProperty, value); }
        }

        public static BindableProperty ScrollControllerProperty =
            BindableProperty.Create(
                nameof(ScrollController),
                typeof(IScrollController),
                typeof(CollectionView),
                default(IScrollController),
                defaultBindingMode: BindingMode.OneWayToSource
            );

        public IScrollController ScrollController
        {
            get { return (IScrollController)GetValue(ScrollControllerProperty); }
            set { SetValue(ScrollControllerProperty, value); }
        }


        // kill unused properties
        new object Header { get; }
        new DataTemplate HeaderTemplate { get; }
        new object Footer { get; }
        new DataTemplate FooterTemplate { get; }
        new object SelectedItem { get; }
        new ListViewSelectionMode SelectionMode { get; }
        new bool HasUnevenRows { get; }
        new int RowHeight { get; }
        new SeparatorVisibility SeparatorVisibility { get; }
        new Color SeparatorColor { get; }
        new event EventHandler<SelectedItemChangedEventArgs> ItemSelected;
    }
}
