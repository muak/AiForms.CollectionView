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

namespace Sample.ViewModels
{
    public class CollectionViewTestViewModel:INavigatingAware
    {
        public PhotoGroup ItemsSource { get; set; }
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

        public IScrollController ScrollController { get; set; }

        IPageDialogService _pageDlg;

        public CollectionViewTestViewModel(IPageDialogService pageDialog)
        {
            _pageDlg = pageDialog;
            InitializeProperties();

            TestSource = new TestFormViewModel
            {
                ItemsSource = TestList,
                PageDialog = pageDialog
            };


        }

        void CommontTest() {
            TestList.Add(
                "PullToRefresh can't be work."
            ).Add(
                "Can PullToRefresh be work?",
                () => {
                    EnabledPullToRefresh.Value = true;
                }
            ).Add(
                "Has Refresh icon turned red?",
                () => {
                    RefreshIconColor.Value = Color.Red;
                },
                () => {
                    RefreshIconColor.Value = Color.DimGray;
                }
            ).Add(
                "Has RowSpacing become much larger?",
                () => {
                    RowSpacing.Value = 25;
                },
                () => {
                    RowSpacing.Value = 4;
                }
            ).Add(
                "Has ColumnHeight been changed to absolute value(250px)?",
                () => {
                    ColumnHeight.Value = 250;
                }
            ).Add(
                "Has ColumnHeight been changed to relative value(0.5)?",
                () => {
                    ColumnHeight.Value = 0.5;
                },
                () => {
                    ColumnHeight.Value = 1.0;
                }
            ).Add(
                "Has Background turned from Yellow to White to Green to transparent?",
                 async () =>
                 {
                     Background.Value = Color.Yellow;
                     await Task.Delay(1000);
                     Background.Value = Color.White;
                     await Task.Delay(1000);
                     Background.Value = Color.Green;
                     await Task.Delay(1000);
                     Background.Value = Color.Transparent;
                 }
            ).Add(
                "Tap some cells. Has FeedBack color turned Red?",
                () =>
                {
                    FeedbackColor.Value = Color.Red;
                },
                () =>
                {
                    FeedbackColor.Value = Color.Yellow;
                }
            ).Add(
                "UniformGrid Test1. GridType is UniformGrid. PortraitColumns are 2.",
                () => {
                    GridType.Value = AiForms.Renderers.GridType.UniformGrid;
                    PortraitColumns.Value = 2;
                }
            ).Add(
                "UniformGrid Test2. Has Colums number been changed 2 to 3 to 4 to 5 to 6 on Portrait?",
                async () => {
                    PortraitColumns.Value = 3;
                    await Task.Delay(1000);
                    PortraitColumns.Value = 4;
                    await Task.Delay(1000);
                    PortraitColumns.Value = 5;
                    await Task.Delay(1000);
                    PortraitColumns.Value = 6;
                }
            ).Add(
                "UniformGrid Test3. Has ColumnSpacing been changed 0?",
                () => {
                    ColumnSpacing.Value = 0;
                }
            ).Add(
                "UniformGrid Test4. Has Colums number been changed 6 to 5 to 4 to 3 to 2 on Portrait?",
                async () => {
                    PortraitColumns.Value = 5;
                    await Task.Delay(1000);
                    PortraitColumns.Value = 4;
                    await Task.Delay(1000);
                    PortraitColumns.Value = 3;
                    await Task.Delay(1000);
                    PortraitColumns.Value = 2;
                }
            ).Add(
                "AutoSpacingGrid Test1. Column width is 150px. SpacingType is Between.",
                () => {
                    GridType.Value = AiForms.Renderers.GridType.AutoSpacingGrid;
                    ColumnWidth.Value = 150;
                    SpacingType.Value = AiForms.Renderers.SpacingType.Between;
                    ColumnSpacing.Value = 4;
                }
            ).Add(
                "AutoSpacingGrid Test2. Has Colum width been changed 150 to 120 to 90 to 60 with keeping Between?",
                async () => {
                    ColumnWidth.Value = 120;
                    await Task.Delay(1000);
                    ColumnWidth.Value = 90;
                    await Task.Delay(1000);
                    ColumnWidth.Value = 60;
                }
            ).Add(
                "AutoSpacingGrid Test3. Has SpacingType been changed to Center?",
                () => {
                    SpacingType.Value = AiForms.Renderers.SpacingType.Center;
                }
            ).Add(
                "AutoSpacingGrid Test4. Has Colum width been changed 60 to 90 to 120 to 150 with keeping Center?",
                async () => {
                    ColumnWidth.Value = 90;
                    await Task.Delay(1000);
                    ColumnWidth.Value = 120;
                    await Task.Delay(1000);
                    ColumnWidth.Value = 150;
                }
            ).Add(
                "AutoSpacingGrid Test5. Has ColumnSpacing been changed 0?",
                () => {
                    ColumnSpacing.Value = 0;
                }
            ).Add(
                "AutoSpacingGrid Test6. Has Colum width been changed 150 to 120 to 90 to 60 with keeping Center?",
                async () => {
                    ColumnWidth.Value = 120;
                    await Task.Delay(1000);
                    ColumnWidth.Value = 90;
                    await Task.Delay(1000);
                    ColumnWidth.Value = 60;
                }
            ).Add(
                "UniformGrid Test5. Change Landscape",
                () => {
                    GridType.Value = AiForms.Renderers.GridType.UniformGrid;
                    ColumnSpacing.Value = 4;
                }
            ).Add(
                "UniformGrid Test6. Has Colums number been changed 5 to 6 to 7 to 8 on Landscape?",
                async () => {
                    LandscapeColumns.Value = 6;
                    await Task.Delay(1000);
                    LandscapeColumns.Value = 7;
                    await Task.Delay(1000);
                    LandscapeColumns.Value = 8;
                    await Task.Delay(1000);
                }
            ).Add(
                "UniformGrid Test7. Has ColumnSpacing been changed 0?",
                () => {
                    ColumnSpacing.Value = 0;
                }
            ).Add(
                "UniformGrid Test8. Has Colums number been changed 8 to 7 to 6 to 5 to 4 on Landscape?",
                async () => {
                    LandscapeColumns.Value = 7;
                    await Task.Delay(1000);
                    LandscapeColumns.Value = 6;
                    await Task.Delay(1000);
                    LandscapeColumns.Value = 5;
                    await Task.Delay(1000);
                    LandscapeColumns.Value = 4;
                }
            ).Add(
                "AutoSpacingGrid Test7. Landscape Test. SpacingType is Center and ColumnSpacing is 4.",
                () => {
                    GridType.Value = AiForms.Renderers.GridType.AutoSpacingGrid;
                    SpacingType.Value = AiForms.Renderers.SpacingType.Center;
                    ColumnSpacing.Value = 4;
                }
            ).Add(
                "AutoSpacingGrid Test8. Has Colum width been changed 60 to 90 to 120 to 150 with keeping Center?",
                async () => {
                    ColumnWidth.Value = 90;
                    await Task.Delay(1000);
                    ColumnWidth.Value = 120;
                    await Task.Delay(1000);
                    ColumnWidth.Value = 150;
                }
            ).Add(
                "AutoSpacingGrid Test9. Has SpacingType been changed to Between?",
                () => {
                    SpacingType.Value = AiForms.Renderers.SpacingType.Between;
                }
            ).Add(
                "AutoSpacingGrid Test10. Has Colum width been changed 150 to 120 to 90 to 60 with keeping Between?",
                async () => {
                    ColumnWidth.Value = 120;
                    await Task.Delay(1000);
                    ColumnWidth.Value = 90;
                    await Task.Delay(1000);
                    ColumnWidth.Value = 60;
                }
            );
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

        public virtual void IndividualTest()
        {


            TestList.Add("Test Start").Add(
                "Manipulation Test1. Has Title10 been scrolled to Top to Bottom To Center?",
                async () => {
                    ScrollController.ScrollTo(ItemsSource.First(), ScrollToPosition.Start, false);
                    await Task.Delay(2000);
                    ScrollController.ScrollTo(ItemsSource[9], ScrollToPosition.Start, false);
                    await Task.Delay(2000);
                    ScrollController.ScrollTo(ItemsSource[9], ScrollToPosition.End, false);
                    await Task.Delay(2000);
                    ScrollController.ScrollTo(ItemsSource[9], ScrollToPosition.Center, false);
                }
            ).Add(
                "Manipulation Test2. Has Title10 been scrolled to Top to Bottom To Center with animation?",
                async () => {

                    ScrollController.ScrollTo(ItemsSource.First(), ScrollToPosition.Start, true);
                    await Task.Delay(2000);
                    ScrollController.ScrollTo(ItemsSource[9], ScrollToPosition.Start, true);
                    await Task.Delay(2000);
                    ScrollController.ScrollTo(ItemsSource[9], ScrollToPosition.End, true);
                    await Task.Delay(2000);
                    ScrollController.ScrollTo(ItemsSource[9], ScrollToPosition.Center, true);
                }
            ).Add(
                "Manipulation Test3. Has new cell been inserted before Title1?",
                async () => {

                    ScrollController.ScrollTo(ItemsSource.First(), ScrollToPosition.Start, true);
                    await Task.Delay(2000);
                    ItemsSource.Insert(0, GetAdditionalItem());  
                }
            ).Add(
                "Manipulation Test4. Click + again. Has new cell been inserted after Title20?",
                async () => {

                    ScrollController.ScrollTo(ItemsSource.Last(), ScrollToPosition.End, true);
                    await Task.Delay(2000);
                    ItemsSource.Add(GetAdditionalItem());
                }
            ).Add(
                "Manipulation Test5. Click + again. Has new cell been inserted between Title8 and Title9?",
                async () => {

                    ScrollController.ScrollTo(ItemsSource[8], ScrollToPosition.Center, true);
                    await Task.Delay(2000);
                    ItemsSource.Insert(9, GetAdditionalItem());
                }
            ).Add(
                "Manipulation Test4. Click -. Has the first cell been deleted?",
                async () => {

                    ScrollController.ScrollTo(ItemsSource.First(), ScrollToPosition.Start, true);
                    await Task.Delay(2000);
                    ItemsSource.RemoveAt(0);
                }
            ).Add(
                "Manipulation Test6. Click - again. Has the last cell been deleted?",
                async () => {

                    ScrollController.ScrollTo(ItemsSource.Last(), ScrollToPosition.End, true);
                    await Task.Delay(2000);
                    ItemsSource.RemoveAt(ItemsSource.Count - 1);
                }
            ).Add(
                "Manipulation Test7. Click - again. Has the cell between Title8 and Title9 been deleted?",
                async () => {

                    ScrollController.ScrollTo(ItemsSource[8], ScrollToPosition.Center, true);
                    await Task.Delay(2000);
                    ItemsSource.RemoveAt(8);
                }
            ).Add(
                "Manipulation Test8. Click R. Has the first cell been changed to AddItem?",
                async () => {

                    ScrollController.ScrollTo(ItemsSource.First(), ScrollToPosition.Start, true);
                    await Task.Delay(2000);
                    ItemsSource[0] = GetAdditionalItem();
                }
            ).Add(
                "Manipulation Test9. Click M. Has the first cell been moved to after Title5?",
                async () => {

                    ScrollController.ScrollTo(ItemsSource.First(), ScrollToPosition.Start, true);
                    await Task.Delay(2000);
                    ItemsSource.Move(0, 4);
                }
            );
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

        public virtual void InitializeCommand()
        {
            var addItem = new PhotoItem
            {
                PhotoUrl = $"https://kamusoft.jp/openimage/nativecell/1.jpg",
                Title = $"AddItem",
                Category = "AAA"
            };

            var addPtn = 0;
            AddCommand.Subscribe(_ =>
            {
                switch (addPtn)
                {
                    case 0:
                        ItemsSource.Insert(0, addItem);
                        break;
                    case 1:
                        ItemsSource.Add(addItem);
                        break;
                    case 2:
                        ItemsSource.Insert(9, addItem);
                        break;

                }

                addPtn++;
                if (addPtn > 2)
                {
                    addPtn = 0;
                }
            });

            var delPtn = 0;
            DelCommand.Subscribe(_ =>
            {
                switch (delPtn)
                {
                    case 0:
                        ItemsSource.RemoveAt(0);
                        break;
                    case 1:
                        ItemsSource.RemoveAt(ItemsSource.Count - 1);
                        break;
                    case 2:
                        ItemsSource.RemoveAt(8);
                        break;
                }
                delPtn++;
                if (delPtn > 2)
                {
                    delPtn = 0;
                }
            });

            RepCommand.Subscribe(_ =>
            {
                ItemsSource[0] = addItem;
            });

            MoveCommand.Subscribe(_ =>
            {
                ItemsSource.Move(0, 4);
            });
        }



        public virtual void OnNavigatingTo(NavigationParameters parameters)
        {
            InitializeCommand();
            IndividualTest();
            CommontTest();
        }
    }
}
