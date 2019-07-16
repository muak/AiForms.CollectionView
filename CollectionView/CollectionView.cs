using System;
using Xamarin.Forms;
using System.Windows.Input;

namespace AiForms.Renderers
{
    /// <summary>
    /// Collection view.
    /// </summary>
    public class CollectionView : ListView
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:AiForms.Renderers.CollectionView"/> class.
        /// </summary>
        /// <param name="cachingStrategy">Caching strategy.</param>
        public CollectionView(ListViewCachingStrategy cachingStrategy):base(cachingStrategy){
            ScrollController = new ScrollController(this);
            SetLoadMoreCompletion = (isEnd) =>
            {
                SetLoadMoreCompletionAction?.Invoke(isEnd);
            };
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:AiForms.Renderers.CollectionView"/> class.
        /// </summary>
        public CollectionView():this(ListViewCachingStrategy.RecycleElement){}

        /// <summary>
        /// The item tap command property.
        /// </summary>
        public static BindableProperty ItemTapCommandProperty =
            BindableProperty.Create(
                nameof(ItemTapCommand),
                typeof(ICommand),
                typeof(CollectionView),
                default(ICommand),
                defaultBindingMode: BindingMode.OneWay
            );

        /// <summary>
        /// Gets or sets the item tap command.
        /// </summary>
        /// <value>The item tap command.</value>
        public ICommand ItemTapCommand {
            get { return (ICommand)GetValue(ItemTapCommandProperty); }
            set { SetValue(ItemTapCommandProperty, value); }
        }

        /// <summary>
        /// The item long tap command property.
        /// </summary>
        public static BindableProperty ItemLongTapCommandProperty =
            BindableProperty.Create(
                nameof(ItemLongTapCommand),
                typeof(ICommand),
                typeof(CollectionView),
                default(ICommand),
                defaultBindingMode: BindingMode.OneWay
            );

        /// <summary>
        /// Gets or sets the item long tap command.
        /// </summary>
        /// <value>The item long tap command.</value>
        public ICommand ItemLongTapCommand {
            get { return (ICommand)GetValue(ItemLongTapCommandProperty); }
            set { SetValue(ItemLongTapCommandProperty, value); }
        }

        /// <summary>
        /// The touch feedback color property.
        /// </summary>
        public static BindableProperty TouchFeedbackColorProperty =
            BindableProperty.Create(
                nameof(TouchFeedbackColor),
                typeof(Color),
                typeof(CollectionView),
                default(Color),
                defaultBindingMode: BindingMode.OneWay
            );

        /// <summary>
        /// Gets or sets the color of the touch feedback.
        /// </summary>
        /// <value>The color of the touch feedback.</value>
        public Color TouchFeedbackColor {
            get { return (Color)GetValue(TouchFeedbackColorProperty); }
            set { SetValue(TouchFeedbackColorProperty, value); }
        }

        /// <summary>
        /// The group first spacing property.
        /// </summary>
        public static BindableProperty GroupFirstSpacingProperty =
            BindableProperty.Create(
                nameof(GroupFirstSpacing),
                typeof(double),
                typeof(CollectionView),
                default(double),
                defaultBindingMode: BindingMode.OneWay
            );

        /// <summary>
        /// Gets or sets the group first spacing.
        /// </summary>
        /// <value>The group first spacing.</value>
        public double GroupFirstSpacing
        {
            get { return (double)GetValue(GroupFirstSpacingProperty); }
            set { SetValue(GroupFirstSpacingProperty, value); }
        }

        /// <summary>
        /// The group last spacing property.
        /// </summary>
        public static BindableProperty GroupLastSpacingProperty =
            BindableProperty.Create(
                nameof(GroupLastSpacing),
                typeof(double),
                typeof(CollectionView),
                default(double),
                defaultBindingMode: BindingMode.OneWay
            );

        /// <summary>
        /// Gets or sets the group last spacing.
        /// </summary>
        /// <value>The group last spacing.</value>
        public double GroupLastSpacing
        {
            get { return (double)GetValue(GroupLastSpacingProperty); }
            set { SetValue(GroupLastSpacingProperty, value); }
        }

        /// <summary>
        /// The scroll controller property.
        /// </summary>
        public static BindableProperty ScrollControllerProperty =
            BindableProperty.Create(
                nameof(ScrollController),
                typeof(IScrollController),
                typeof(CollectionView),
                default(IScrollController),
                defaultBindingMode: BindingMode.OneWayToSource
            );

        /// <summary>
        /// Gets or sets the scroll controller.
        /// </summary>
        /// <value>The scroll controller.</value>
        public IScrollController ScrollController
        {
            get { return (IScrollController)GetValue(ScrollControllerProperty); }
            set { SetValue(ScrollControllerProperty, value); }
        }

        public static BindableProperty LoadMoreCommandProperty =
            BindableProperty.Create(
                nameof(LoadMoreCommand),
                typeof(ICommand),
                typeof(CollectionView),
                default(ICommand),
                defaultBindingMode: BindingMode.OneWay
            );

        public ICommand LoadMoreCommand
        {
            get { return (ICommand)GetValue(LoadMoreCommandProperty); }
            set { SetValue(LoadMoreCommandProperty, value); }
        }

        internal Action<bool> SetLoadMoreCompletionAction;

        public static BindableProperty SetLoadMoreCompletionProperty =
            BindableProperty.Create(
                nameof(SetLoadMoreCompletion),
                typeof(Action<bool>),
                typeof(CollectionView),
                default(Action),
                defaultBindingMode: BindingMode.OneWayToSource
            );

        public Action<bool> SetLoadMoreCompletion
        {
            get { return (Action<bool>)GetValue(SetLoadMoreCompletionProperty); }
            set { SetValue(SetLoadMoreCompletionProperty, value); }
        }

        // kill unused properties
        private new object Header { get; }
        private new DataTemplate HeaderTemplate { get; }
        private new object Footer { get; }
        private new DataTemplate FooterTemplate { get; }
        private new object SelectedItem { get; }
        private new ListViewSelectionMode SelectionMode { get; }
        private new bool HasUnevenRows { get; }
        private new int RowHeight { get; }
        private new SeparatorVisibility SeparatorVisibility { get; }
        private new Color SeparatorColor { get; }
        private new event EventHandler<SelectedItemChangedEventArgs> ItemSelected;
    }
}
