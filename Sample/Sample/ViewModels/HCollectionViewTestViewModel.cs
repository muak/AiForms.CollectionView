using System;
using System.Collections.ObjectModel;
using Reactive.Bindings;
using static Sample.ViewModels.CollectionViewTestViewModel;
using System.Collections.Generic;
using Prism.Services;
using Xamarin.Forms;
using System.Threading.Tasks;
using System.Linq;
using AiForms.Renderers;

namespace Sample.ViewModels
{
    public class HCollectionViewTestViewModel
    {
        public ObservableCollection<PhotoGroup> ItemsSource { get; set; } = new ObservableCollection<PhotoGroup>();
        public PhotoGroup ItemsSource2 { get; set; }
        public TestFormViewModel TestSource { get; }
        public TestCollection TestList { get; set; } = new TestCollection();
        public ReactiveCommand TapCommand { get; } = new ReactiveCommand();
        public ReactiveCommand LongTapCommand { get; } = new ReactiveCommand();

        public ReactiveCommand AddCommand { get; set; } = new ReactiveCommand();
        public ReactiveCommand DelCommand { get; set; } = new ReactiveCommand();
        public ReactiveCommand RepCommand { get; set; } = new ReactiveCommand();
        public ReactiveCommand MoveCommand { get; set; } = new ReactiveCommand();
        public ReactiveCommand AddSecCommand { get; set; } = new ReactiveCommand();
        public ReactiveCommand DelSecCommand { get; set; } = new ReactiveCommand();

        public ReactivePropertySlim<Color> Background { get; } = new ReactivePropertySlim<Color>(Color.Transparent);
        public ReactivePropertySlim<Color> FeedbackColor { get; } = new ReactivePropertySlim<Color>(Color.Yellow);
        public ReactivePropertySlim<double> ColumnWidth { get; } = new ReactivePropertySlim<double>(100);
        public ReactivePropertySlim<double> RowHeight { get; } = new ReactivePropertySlim<double>(100);
        public ReactivePropertySlim<double> Spacing { get; } = new ReactivePropertySlim<double>(5);
        public ReactivePropertySlim<double> GroupWidth { get; } = new ReactivePropertySlim<double>(50);
        public ReactivePropertySlim<bool> IsInfinite { get; } = new ReactivePropertySlim<bool>(false);

        public IScrollController ScrollController {get;set;}
        public IScrollController ScrollController2 { get; set; }

        PhotoGroup _additionalGroup;
        IPageDialogService _pageDlg;

        public HCollectionViewTestViewModel(IPageDialogService pageDialog)
        {
            _pageDlg = pageDialog;
            TestSource = new TestFormViewModel
            {
                ItemsSource = TestList,
                PageDialog = pageDialog
            };

            InitializeProperties();

            TestList.Add("Test Start").Add(
                "Manipulation Test1. Has SectionB - Title11 been scrolled to Top to Bottom to Center?",
                async () => {
                    ScrollController.ScrollToStart();
                    await Task.Delay(2000);
                    ScrollController.ScrollTo(ItemsSource[1][0], ItemsSource[1],ScrollToPosition.Start, false);
                    await Task.Delay(2000);
                    ScrollController.ScrollTo(ItemsSource[1][0], ItemsSource[1], ScrollToPosition.End, false);
                    await Task.Delay(2000);
                    ScrollController.ScrollTo(ItemsSource[1][0], ItemsSource[1], ScrollToPosition.Center, false);
                }
            ).Add(
                "Manipulation Test2. Has SectionB - Title11 been scrolled to Top to Bottom to Center with animation?",
                async () => {
                    ScrollController.ScrollToStart(true);
                    await Task.Delay(2000);
                    ScrollController.ScrollTo(ItemsSource[1][0], ItemsSource[1], ScrollToPosition.Start, true);
                    await Task.Delay(2000);
                    ScrollController.ScrollTo(ItemsSource[1][0], ItemsSource[1], ScrollToPosition.End, true);
                    await Task.Delay(2000);
                    ScrollController.ScrollTo(ItemsSource[1][0], ItemsSource[1], ScrollToPosition.Center, true);
                }
            ).Add(
                "Manipulation Test3. Has SectionC - Title7 been scrolled to Top to Bottom to Center with animation?",
                async () => {
                    ScrollController.ScrollToStart(true);
                    await Task.Delay(2000);
                    ScrollController.ScrollTo(ItemsSource[2][1], ItemsSource[2], ScrollToPosition.Start, true);
                    await Task.Delay(2000);
                    ScrollController.ScrollTo(ItemsSource[2][1], ItemsSource[2], ScrollToPosition.End, true);
                    await Task.Delay(2000);
                    ScrollController.ScrollTo(ItemsSource[2][1], ItemsSource[2], ScrollToPosition.Center, true);
                }
            ).Add(
                "Manipulation Test4. Has Title5 been scrolled to Top to Bottom to Center?",
                async () => {
                    ScrollController2.ScrollToStart();
                    await Task.Delay(2000);
                    ScrollController2.ScrollTo(ItemsSource2[4], ScrollToPosition.Start, false);
                    await Task.Delay(2000);
                    ScrollController2.ScrollTo(ItemsSource2[4], ScrollToPosition.End, false);
                    await Task.Delay(2000);
                    ScrollController2.ScrollTo(ItemsSource2[4],  ScrollToPosition.Center, false);
                }
            ).Add(
                "Manipulation Test5. Has Title5 been scrolled to Top to Bottom to Center with animation?",
                async () => {
                    ScrollController2.ScrollToStart(true);
                    await Task.Delay(2000);
                    ScrollController2.ScrollTo(ItemsSource2[4], ScrollToPosition.Start, true);
                    await Task.Delay(2000);
                    ScrollController2.ScrollTo(ItemsSource2[4], ScrollToPosition.End, true);
                    await Task.Delay(2000);
                    ScrollController2.ScrollTo(ItemsSource2[4], ScrollToPosition.Center, true);
                }
            ).Add(
                "Manipulation Test6. Has new cell been inserted before Title1?",
                async () =>
                {
                    ScrollController.ScrollToStart(true);
                    ScrollController2.ScrollToStart(true);
                    await Task.Delay(2000);
                    ItemsSource[0].Insert(0, GetAdditionalItem());
                    ItemsSource2.Insert(0, GetAdditionalItem());
                }
            ).Add(
                "Manipulation Test7. Has new cell been inserted after Title20?",
                async () =>
                {
                    ScrollController.ScrollTo(ItemsSource[0][20], ItemsSource[0], ScrollToPosition.Center, true);
                    ScrollController2.ScrollToEnd(true);
                    await Task.Delay(2000);
                    ItemsSource[0].Add(GetAdditionalItem());
                    ItemsSource2.Add(GetAdditionalItem());
                }
            ).Add(
                "Manipulation Test8. Has new cell been inserted between Title8 and Title9?",
                async () =>
                {
                    ScrollController.ScrollTo(ItemsSource[0][8], ItemsSource[0], ScrollToPosition.Center, true);
                    ScrollController2.ScrollTo(ItemsSource2[8], ScrollToPosition.Center, true);
                    await Task.Delay(2000);
                    ItemsSource[0].Insert(9, GetAdditionalItem());
                    ItemsSource2.Insert(9, GetAdditionalItem());
                }
            ).Add(
                "Manipulation Test9. Has the first cell been deleted?",
                async () =>
                {
                    ScrollController.ScrollToStart(true);
                    ScrollController2.ScrollToStart(true);
                    await Task.Delay(2000);
                    ItemsSource[0].RemoveAt(0);
                    ItemsSource2.RemoveAt(0);
                }
            ).Add(
                "Manipulation Test10. Has the last cell in SectionA been deleted?",
                async () =>
                {
                    ScrollController.ScrollTo(ItemsSource[0][21], ItemsSource[0], ScrollToPosition.End, true);
                    ScrollController2.ScrollToEnd();
                    await Task.Delay(2000);
                    ItemsSource[0].RemoveAt(ItemsSource[0].Count - 1);
                    ItemsSource2.RemoveAt(ItemsSource2.Count - 1);
                }
            ).Add(
                "Manipulation Test11. Has the cell between Title8 and Title9 in SectionA been deleted?",
                async () =>
                {
                    ScrollController.ScrollTo(ItemsSource[0][8], ItemsSource[0], ScrollToPosition.Center, true);
                    ScrollController2.ScrollTo(ItemsSource2[8], ScrollToPosition.Center, true);
                    await Task.Delay(2000);
                    ItemsSource[0].RemoveAt(8);
                    ItemsSource2.RemoveAt(8);
                }
            ).Add(
                "Manipulation Test12. Has the first cell been changed to AddItem?",
                async () =>
                {
                    ScrollController.ScrollToStart(true);
                    ScrollController2.ScrollToStart(true);
                    await Task.Delay(2000);
                    ItemsSource[0][0] = GetAdditionalItem();
                    ItemsSource2[0] = GetAdditionalItem();
                }
            ).Add(
                "Manipulation Test13. Has the first cell been moved to after Title5?",
                async () =>
                {
                    ScrollController.ScrollToStart(true);
                    ScrollController2.ScrollToStart(true);
                    await Task.Delay(2000);
                    ItemsSource[0].Move(0, 4);
                    ItemsSource2.Move(0, 4);
                }
            ).Add(
                "Manipulation Test14. Has new section been inserted at last?",
                async () =>
                {
                    await Task.Delay(2000);
                    ItemsSource.Add(_additionalGroup);
                    await Task.Delay(2000);
                    ScrollController.ScrollToEnd(true);
                }
            ).Add(
                "Manipulation Test15. Has the last section been deleted?",
                async () =>
                {
                    await Task.Delay(2000);
                    ItemsSource.Remove(_additionalGroup);
                    await Task.Delay(2000);
                    ScrollController.ScrollToEnd(true);
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
                "Has Spacing width been twice as large as previous?",
                () =>
                {
                    Spacing.Value *= 2;
                },
                () =>
                {
                    Spacing.Value /= 2;
                }
            ).Add(
                "Has Column width been twice as large as previous?",
                () =>
                {
                    ColumnWidth.Value *= 2;
                },
                () => { ColumnWidth.Value /= 2; }
            ).Add(
                "Has GroupHeaderWidth been twice as large as previous?",
                () =>
                {
                    GroupWidth.Value *= 2;
                },
                () =>
                {
                    GroupWidth.Value /= 2;
                }
            ).Add(
                "Has Row height been one and a half as large as previous?",
                () =>
                {
                    RowHeight.Value = 150;
                },
                () => { RowHeight.Value = 100; }
            ).Add(
                "Has Scroll come to circulate?",
                () =>
                {
                    IsInfinite.Value = true;
                },
                () =>
                {
                    IsInfinite.Value = false;
                }
            ).Add(
                "Change the device orientation Landscape, and Back to Portrait. Hasn't the layout broken?"
            );




        }

        void InitializeProperties() {
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
            var list4 = new List<PhotoItem>();
            for (var i = 1; i < 10; i++)
            {
                list4.Add(new PhotoItem
                {
                    PhotoUrl = $"https://kamusoft.jp/openimage/nativecell/{i + 1}.jpg",
                    Title = $"Title {i + 1}",
                    Category = "DDD",
                });
            }

            var noGroupList = new List<PhotoItem>();
            for (var i = 0; i < 20; i++)
            {
                noGroupList.Add(new PhotoItem
                {
                    PhotoUrl = $"https://kamusoft.jp/openimage/nativecell/{i + 1}.jpg",
                    Title = $"Title {i + 1}",
                    Category = "AAA",
                });
            }
            ItemsSource2 = new PhotoGroup(noGroupList);

            var group1 = new PhotoGroup(list1) { Head = "SecA" };
            var group2 = new PhotoGroup(list2) { Head = "SecB" };
            var group3 = new PhotoGroup(list3) { Head = "SecC" };
            ItemsSource.Add(group1);
            ItemsSource.Add(group2);
            ItemsSource.Add(group3);
            _additionalGroup = new PhotoGroup(list4) { Head = "SEC4" };

            TapCommand.Subscribe(async item =>
            {
                var photo = item as PhotoItem;
                await _pageDlg.DisplayAlertAsync("", $"Tap {photo.Title}", "OK");
            });

            LongTapCommand.Subscribe(async item =>
            {
                var photo = item as PhotoItem;
                await _pageDlg.DisplayAlertAsync("", $"LongTap {photo.Title}", "OK");
            });

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
                        group1.Add(addItem);
                        break;
                    case 1:
                        group1.Insert(0, addItem);
                        break;
                    case 2:
                        group1.Insert(group1.Count / 2, addItem);
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
                        group1.RemoveAt(0);
                        break;
                    case 1:
                        group1.RemoveAt(group1.Count / 2);
                        break;
                    case 2:
                        group1.RemoveAt(group1.Count - 1);
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
                group1[0] = addItem;
            });

            MoveCommand.Subscribe(_ =>
            {
                group1.Move(0, 3);
            });


            AddSecCommand.Subscribe(_ =>
            {
                ItemsSource.Add(_additionalGroup);
            });

            DelSecCommand.Subscribe(_ =>
            {
                ItemsSource.RemoveAt(ItemsSource.Count - 1);
            });
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
