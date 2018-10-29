using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using Reactive.Bindings;
using Prism.Services;
using System.Threading.Tasks;
using Xamarin.Forms;
using AiForms.Renderers;
using Prism.Navigation;
using System.Linq;
using Prism.Mvvm;

namespace Sample.ViewModels
{
    public class CollectionViewTestViewModel:BindableBase, INavigatingAware
    {
        public PhotoGroup ItemsSource { get; set; }
        public ObservableCollection<PhotoGroup> ItemsGroupSource { get; set; }
        public ReactiveCommand TapCommand { get; } = new ReactiveCommand();
        public ReactiveCommand LongTapCommand { get; } = new ReactiveCommand();
        public ReactiveProperty<bool> IsRefreshing { get; } = new ReactiveProperty<bool>(false);
        public AsyncReactiveCommand RefreshCommand { get; } = new AsyncReactiveCommand();
        public TestFormViewModel TestSource { get; }
        public TestCollection TestList { get; set; } = new TestCollection();

        public ReactiveCommand AddCommand { get; set; } = new ReactiveCommand();
        public ReactiveCommand DelCommand { get; set; } = new ReactiveCommand();
        public ReactiveCommand RepCommand { get; set; } = new ReactiveCommand();
        public ReactiveCommand MoveCommand { get; set; } = new ReactiveCommand();
        public ReactiveCommand NextCommand { get; set; } = new ReactiveCommand();
        public ReactiveCommand RepeatCommand { get; set; } = new ReactiveCommand();

        public ReactivePropertySlim<double> HeaderHeight { get; } = new ReactivePropertySlim<double>(36);
        public ReactivePropertySlim<Color> Background { get; } = new ReactivePropertySlim<Color>(Color.Transparent);
        public ReactivePropertySlim<Color> FeedbackColor { get; } = new ReactivePropertySlim<Color>(Color.Yellow);
        public ReactivePropertySlim<GridType> GridType { get; } = new ReactivePropertySlim<GridType>(AiForms.Renderers.GridType.UniformGrid);
        public ReactivePropertySlim<SpacingType> SpacingType { get; } = new ReactivePropertySlim<SpacingType>(AiForms.Renderers.SpacingType.Between);
        public ReactivePropertySlim<int> PortraitColumns { get; } = new ReactivePropertySlim<int>(2);
        public ReactivePropertySlim<int> LandscapeColumns { get; } = new ReactivePropertySlim<int>(5);
        public ReactivePropertySlim<double> ColumnSpacing { get; } = new ReactivePropertySlim<double>(4);
        public ReactivePropertySlim<double> ColumnWidth { get; } = new ReactivePropertySlim<double>(150);
        public ReactivePropertySlim<double> ColumnHeight { get; } = new ReactivePropertySlim<double>(1.0);
        public ReactivePropertySlim<double> RowSpacing { get; } = new ReactivePropertySlim<double>(4);
        public ReactivePropertySlim<bool> EnabledPullToRefresh { get; } = new ReactivePropertySlim<bool>(false);
        public ReactivePropertySlim<Color> RefreshIconColor { get; } = new ReactivePropertySlim<Color>(Color.DimGray);
        public ReactivePropertySlim<double> AdditionalHeight { get; } = new ReactivePropertySlim<double>(0);
        public IScrollController ScrollController { get; set; }
        public ReactivePropertySlim<double> GroupFirstSpacing { get; } = new ReactivePropertySlim<double>(0);
        public ReactivePropertySlim<double> GroupLastSpacing { get; } = new ReactivePropertySlim<double>(0);
        public ReactivePropertySlim<bool> IsGroupHeaderSticky { get; } = new ReactivePropertySlim<bool>(true);
        public ReactivePropertySlim<double> BothSidesMargin { get; } = new ReactivePropertySlim<double>(0);

        public List<TestSection> TestSections { get; set; }
        IEnumerator<TestItem> _testEnumerator;

        IPageDialogService _pageDlg;

        public CollectionViewTestViewModel(IPageDialogService pageDialog)
        {
            _pageDlg = pageDialog;
            InitializeProperties();

            Action currentAction = null;
            NextCommand.Subscribe(async _ =>
            {
                if (!_testEnumerator.MoveNext())
                {
                    await pageDialog.DisplayAlertAsync("", "Finished", "OK");
                    return;
                }
                currentAction = _testEnumerator.Current.Run;
                currentAction?.Invoke();

            });
            RepeatCommand.Subscribe(_ =>
            {
                currentAction?.Invoke();
            });
        }

        public virtual void OnNavigatingTo(NavigationParameters parameters)
        {
            TestSections = parameters.GetValue<List<TestSection>>("tests");

            _testEnumerator = TestSections.SelectMany(x => x).Where(y => y.Check.Value).SelectMany(z => z).GetEnumerator();

            TestSections.ForEach(x => x.SetViewModel(this));

            RaisePropertyChanged(nameof(TestSections));
        }

        public PhotoItem GetAdditionalItem()
        {
            return new PhotoItem
            {
                PhotoUrl = $"https://kamusoft.jp/openimage/nativecell/13.jpg",
                Title = $"AddItem",
                Category = "AAA"
            };
        }

        void InitializeProperties()
        {

            var list = new List<PhotoItem>();
            for (var i = 0; i < 20; i++)
            {
                list.Add(new PhotoItem
                {
                    PhotoUrl = $"https://kamusoft.jp/openimage/nativecell/{i + 1}.jpg",
                    Title = $"Title {i + 1}",
                    Category = "AAA",
                });
            }


            ItemsSource = new PhotoGroup(list);


            TapCommand.Subscribe(async item => {
                var photo = item as PhotoItem;
                await _pageDlg.DisplayAlertAsync("", $"Tap {photo.Title}", "OK");
            });

            LongTapCommand.Subscribe(async item => {
                var photo = item as PhotoItem;
                await _pageDlg.DisplayAlertAsync("", $"LongTap {photo.Title}", "OK");
            });

            RefreshCommand.Subscribe(async _ => {
                await Task.Delay(3000);
                IsRefreshing.Value = false;
            });
        }
    }
}
