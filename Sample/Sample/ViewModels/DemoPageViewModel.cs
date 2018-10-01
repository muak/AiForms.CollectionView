using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using AiForms.Extras.Abstractions;
using Reactive.Bindings;
using Prism.Services;
using Sample.Views;
using Xamarin.Forms;
using AiForms.Renderers;

namespace Sample.ViewModels
{
    public class DemoPageViewModel
    {
        public ObservableCollection<PhotoGroup> ItemsSource { get; set; } = new ObservableCollection<PhotoGroup>();
        public ObservableCollection<PhotoGroup> ItemsSourceH { get; set; } = new ObservableCollection<PhotoGroup>();
        public ReactiveCommand TapCommand { get; } = new ReactiveCommand();
        public ReactiveCommand LongTapCommand { get; } = new ReactiveCommand();
        public ReactiveProperty<bool> IsRefreshing { get; } = new ReactiveProperty<bool>(false);
        public AsyncReactiveCommand RefreshCommand { get; } = new AsyncReactiveCommand();
        public AsyncReactiveCommand NextCommand { get; } = new AsyncReactiveCommand();

        public TestCollection TestList { get; set; } = new TestCollection();

        public ReactivePropertySlim<Color> Background { get; } = new ReactivePropertySlim<Color>(Color.Transparent);
        public ReactivePropertySlim<Color> FeedbackColor { get; } = new ReactivePropertySlim<Color>(Color.Yellow);
        public ReactivePropertySlim<GridType> GridType { get; } = new ReactivePropertySlim<GridType>(AiForms.Renderers.GridType.UniformGrid);
        public ReactivePropertySlim<SpacingType> SpacingType { get; } = new ReactivePropertySlim<SpacingType>(AiForms.Renderers.SpacingType.Between);
        public ReactivePropertySlim<int> PortraitColumns { get; } = new ReactivePropertySlim<int>(3);
        public ReactivePropertySlim<int> LandscapeColumns { get; } = new ReactivePropertySlim<int>(5);
        public ReactivePropertySlim<double> ColumnSpacing { get; } = new ReactivePropertySlim<double>(4);
        public ReactivePropertySlim<double> ColumnWidth { get; } = new ReactivePropertySlim<double>(150);
        public ReactivePropertySlim<double> ColumnHeight { get; } = new ReactivePropertySlim<double>(1.0);
        public ReactivePropertySlim<double> RowSpacing { get; } = new ReactivePropertySlim<double>(4);
        public ReactivePropertySlim<bool> EnabledPullToRefresh { get; } = new ReactivePropertySlim<bool>(true);
        public ReactivePropertySlim<Color> RefreshIconColor { get; } = new ReactivePropertySlim<Color>(Color.DimGray);
        public ReactivePropertySlim<double> AdditionalHeight { get; } = new ReactivePropertySlim<double>(0);
        public ReactivePropertySlim<bool> IsInfinite { get; } = new ReactivePropertySlim<bool>(false);

        public IScrollController ScrollController { get; set; }
        public IScrollController ScrollControllerH { get; set; }

        IToast _toast;
        IPageDialogService _pageDlg;

        public DemoPageViewModel(IPageDialogService pageDialog, IToast toast)
        {
            _toast = toast;
            _pageDlg = pageDialog;

            var list1 = new List<PhotoItem>();
            for (var i = 0; i < 20; i++)
            {
                list1.Add(new PhotoItem
                {
                    PhotoUrl = $"https://kamusoft.jp/openimage/nativecell/{i + 1}.jpg",
                    Title = $"Title {i + 1}",
                    Category = "AAA",
                });
            }
            var list2 = new List<PhotoItem>();
            for (var i = 10; i < 15; i++)
            {
                list2.Add(new PhotoItem
                {
                    PhotoUrl = $"https://kamusoft.jp/openimage/nativecell/{i + 1}.jpg",
                    Title = $"Title {i + 1}",
                    Category = "BBB",
                });
            }
            var list3 = new List<PhotoItem>();
            for (var i = 5; i < 20; i++)
            {
                list3.Add(new PhotoItem
                {
                    PhotoUrl = $"https://kamusoft.jp/openimage/nativecell/{i + 1}.jpg",
                    Title = $"Title {i + 1}",
                    Category = "CCC",
                });
            }

            var group1 = new PhotoGroup(list1) { Head = "SectionA" };
            var group2 = new PhotoGroup(list2) { Head = "SectionB" };
            var group3 = new PhotoGroup(list3) { Head = "SectionC" };
            ItemsSource.Add(group1);
            ItemsSource.Add(group2);
            ItemsSource.Add(group3);
            ItemsSourceH.Add(group1);
            ItemsSourceH.Add(group2);

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

            NextCommand.Subscribe(NextAction);

            SetDemoItems();
        }

        int _position = 0;
        async Task NextAction()
        {
            if(_position == TestList.Count) {
                await _pageDlg.DisplayAlertAsync("", "Completed", "OK");
                return;
            } 
            _toast.Show<MyToastView>(TestList[_position]);
            TestList[_position].Action?.Invoke();
            _position++;
            await Task.Delay(250);
        }

        void SetDemoItems()
        {
            TestList.Add(
                "Tap & Long Tap"
            ).Add(
                "The touch color can be changed.",
                () => {
                    FeedbackColor.Value = Color.Red;
                },
                () => { FeedbackColor.Value = Color.Yellow; }
            ).Add(
                "PullToRefresh"
            ).Add(
                "PullToRefresh icon color can be changed.",
                () =>
                {
                    RefreshIconColor.Value = Color.Red;
                },
                () => { RefreshIconColor.Value = Color.DimGray; }
            ).Add(
                "Insert a new cell", async () =>
                {
                    ScrollController.ScrollToStart();
                    ScrollControllerH.ScrollToStart();
                    await Task.Delay(2000);
                    ItemsSource[0].Insert(0, GetAdditionalItem());
                    await Task.Delay(2000);
                    ScrollController.ScrollTo(ItemsSource[0][20], ItemsSource[0], ScrollToPosition.Center);
                    ScrollControllerH.ScrollTo(ItemsSourceH[0][20], ItemsSourceH[0], ScrollToPosition.Center);
                    await Task.Delay(2000);
                    ItemsSource[0].Add(GetAdditionalItem());
                    await Task.Delay(2000);
                    ScrollController.ScrollTo(ItemsSource[0][8], ItemsSource[0], ScrollToPosition.Center);
                    ScrollControllerH.ScrollTo(ItemsSourceH[0][8], ItemsSourceH[0], ScrollToPosition.Center);
                    await Task.Delay(2000);
                    ItemsSource[0].Insert(9, GetAdditionalItem());
                }
            ).Add(
                "Delete a cell", async () =>
                {
                    ScrollController.ScrollToStart();
                    ScrollControllerH.ScrollToStart();
                    await Task.Delay(2000);
                    ItemsSource[0].RemoveAt(0);
                    await Task.Delay(2000);
                    ScrollController.ScrollTo(ItemsSource[0][21], ItemsSource[0], ScrollToPosition.Center);
                    ScrollControllerH.ScrollTo(ItemsSourceH[0][21], ItemsSourceH[0], ScrollToPosition.Center);
                    await Task.Delay(2000);
                    ItemsSource[0].RemoveAt(ItemsSource[0].Count - 1);
                    await Task.Delay(2000);
                    ScrollController.ScrollTo(ItemsSource[0][8], ItemsSource[0], ScrollToPosition.Center);
                    ScrollControllerH.ScrollTo(ItemsSourceH[0][8], ItemsSourceH[0], ScrollToPosition.Center);
                    await Task.Delay(2000);
                    ItemsSource[0].RemoveAt(8);
                }
            ).Add(
                "Replace a cell", async () =>
                {
                    ScrollController.ScrollToStart();
                    ScrollControllerH.ScrollToStart();
                    await Task.Delay(2000);
                    ItemsSource[0][0] = GetAdditionalItem();
                }
            ).Add(
                "Move a cell", async () =>
                {
                    ScrollController.ScrollToStart();
                    ScrollControllerH.ScrollToStart();
                    await Task.Delay(2000);
                    ItemsSource[0].Move(0, 4);
                }
            ).Add(
                "HCollectionView can be infinite scroll.",
                () => {
                    IsInfinite.Value = true;
                }
            ).Add(
                "UniformGrid. The colums number is being changed.",
                async () => {
                    await Task.Delay(1250);
                    PortraitColumns.Value = 4;
                    await Task.Delay(1250);
                    PortraitColumns.Value = 5;
                    await Task.Delay(1250);
                    PortraitColumns.Value = 6;
                    await Task.Delay(1250);
                    PortraitColumns.Value = 7;
                }
            ).Add(
                "AutoSpacingGrid. SpacingType is between.",
                () => {
                    GridType.Value = AiForms.Renderers.GridType.AutoSpacingGrid;
                    SpacingType.Value = AiForms.Renderers.SpacingType.Between;
                    ColumnWidth.Value = 180;
                }
            ).Add(
                "AutoSpacingGrid. The colum width is being changed.",
                async () => {
                    await Task.Delay(1250);
                    ColumnWidth.Value = 150;
                    await Task.Delay(1250);
                    ColumnWidth.Value = 120;
                    await Task.Delay(1250);
                    ColumnWidth.Value = 90;
                    await Task.Delay(1250);
                    ColumnWidth.Value = 60;
                }
            ).Add(
                "AutoSpacingGrid. SpacingType is center.",
                () => {
                    SpacingType.Value = AiForms.Renderers.SpacingType.Center;
                }
            ).Add(
                "AutoSpacingGrid. The colum width is being changed.",
                async () => {
                    await Task.Delay(1250);
                    ColumnWidth.Value = 90;
                    await Task.Delay(1250);
                    ColumnWidth.Value = 120;
                    await Task.Delay(1250);
                    ColumnWidth.Value = 150;
                    await Task.Delay(1250);
                    ColumnWidth.Value = 180;
                }
            )
                    ;
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
    }
}
