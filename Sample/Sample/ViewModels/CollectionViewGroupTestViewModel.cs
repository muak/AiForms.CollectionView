using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Reactive.Bindings;
using Prism.Services;
using System.Threading.Tasks;
using Xamarin.Forms;
using Prism.Navigation;
using System.Linq;

namespace Sample.ViewModels
{
    public class CollectionViewGroupTestViewModel:CollectionViewTestViewModel
    {
        public ObservableCollection<PhotoGroup> ItemsGroupSource { get; set; }
        public ReactiveCommand AddSecCommand { get; set; } = new ReactiveCommand();
        public ReactiveCommand DelSecCommand { get; set; } = new ReactiveCommand();

        public ReactivePropertySlim<double> HeaderHeight { get; } = new ReactivePropertySlim<double>(36);

        PhotoGroup _additionalGroup;
        IPageDialogService _pageDlg;

        public CollectionViewGroupTestViewModel(IPageDialogService pageDialog):base(pageDialog)
        {
            _pageDlg = pageDialog;
            InitializeProperties();

        }

        public override void IndividualTest()
        {
            TestList.Add("Test Start").Add(
                "Manipulation Test1. Has SectionB been scrolled to Top to Bottom To Center?",
                async () =>
                {
                    ScrollController.ScrollToStart();
                    await Task.Delay(2000);
                    ScrollController.ScrollTo(ItemsGroupSource[1][0], ItemsGroupSource[1], ScrollToPosition.Start, false);
                    await Task.Delay(2000);
                    ScrollController.ScrollTo(ItemsGroupSource[1][0], ItemsGroupSource[1], ScrollToPosition.End, false);
                    await Task.Delay(2000);
                    ScrollController.ScrollTo(ItemsGroupSource[1][0], ItemsGroupSource[1], ScrollToPosition.Center, false);
                }
            ).Add(
                "Manipulation Test2. Has SectionB been scrolled to Top to Bottom To Center with animation?",
                async () =>
                {

                    ScrollController.ScrollToStart(true);
                    await Task.Delay(2000);
                    ScrollController.ScrollTo(ItemsGroupSource[1][0], ItemsGroupSource[1], ScrollToPosition.Start, true);
                    await Task.Delay(2000);
                    ScrollController.ScrollTo(ItemsGroupSource[1][0], ItemsGroupSource[1], ScrollToPosition.End, true);
                    await Task.Delay(2000);
                    ScrollController.ScrollTo(ItemsGroupSource[1][0], ItemsGroupSource[1], ScrollToPosition.Center, true);

                }
            ).Add(
                "Manipulation Test3. Has SectionC - Title14 been scrolled to Top to Bottom To Center?",
                async () =>
                {
                    ScrollController.ScrollToStart(false);
                    await Task.Delay(2000);
                    ScrollController.ScrollTo(ItemsGroupSource[2][3], ItemsGroupSource[2], ScrollToPosition.Start, false);
                    await Task.Delay(2000);
                    ScrollController.ScrollTo(ItemsGroupSource[2][3], ItemsGroupSource[2], ScrollToPosition.End, false);
                    await Task.Delay(2000);
                    ScrollController.ScrollTo(ItemsGroupSource[2][3], ItemsGroupSource[2], ScrollToPosition.Center, false);
                }
            ).Add(
                "Manipulation Test4. Has SectionC - Title14 been scrolled to Top to Bottom To Center with animation?",
                async () =>
                {
                    ScrollController.ScrollToStart(true);
                    await Task.Delay(2000);
                    ScrollController.ScrollTo(ItemsGroupSource[2][3], ItemsGroupSource[2], ScrollToPosition.Start, true);
                    await Task.Delay(2000);
                    ScrollController.ScrollTo(ItemsGroupSource[2][3], ItemsGroupSource[2], ScrollToPosition.End, true);
                    await Task.Delay(2000);
                    ScrollController.ScrollTo(ItemsGroupSource[2][3], ItemsGroupSource[2], ScrollToPosition.Center, true);
                }
            ).Add(
                "Manipulation Test5. Has new cell been inserted before Title1?",
                async () =>
                {
                    ScrollController.ScrollToStart(true);
                    await Task.Delay(2000);
                    ItemsGroupSource[0].Insert(0, GetAdditionalItem());
                }
            ).Add(
                "Manipulation Test6. Has new cell been inserted after Title20?",
                async () =>
                {
                    ScrollController.ScrollTo(ItemsGroupSource[0][20],ItemsGroupSource[0], ScrollToPosition.End, true);
                    await Task.Delay(2000);
                    ItemsGroupSource[0].Add(GetAdditionalItem());
                }
            ).Add(
                "Manipulation Test7. Has new cell been inserted between Title8 and Title9?",
                async () =>
                {
                    ScrollController.ScrollTo(ItemsGroupSource[0][8],ItemsGroupSource[0], ScrollToPosition.Center, true);
                    await Task.Delay(2000);
                    ItemsGroupSource[0].Insert(9, GetAdditionalItem());
                }
            ).Add(
                "Manipulation Test8. Has the first cell been deleted?",
                async () =>
                {
                    ScrollController.ScrollToStart(true);
                    await Task.Delay(2000);
                    ItemsGroupSource[0].RemoveAt(0);
                }
            ).Add(
                "Manipulation Test9. Has the last cell in SectionA been deleted?",
                async () =>
                {
                    ScrollController.ScrollTo(ItemsGroupSource[0][21], ItemsGroupSource[0], ScrollToPosition.End, true);
                    await Task.Delay(2000);
                    ItemsGroupSource[0].RemoveAt(ItemsGroupSource[0].Count - 1);
                }
            ).Add(
                "Manipulation Test10. Has the cell between Title8 and Title9 in SectionA been deleted?",
                async () =>
                {
                    ScrollController.ScrollTo(ItemsGroupSource[0][8], ItemsGroupSource[0], ScrollToPosition.Center, true);
                    await Task.Delay(2000);
                    ItemsGroupSource[0].RemoveAt(8);
                }
            ).Add(
                "Manipulation Test11. Has the first cell been changed to AddItem?",
                async () =>
                {
                    ScrollController.ScrollToStart(true);
                    await Task.Delay(2000);
                    ItemsGroupSource[0][0] = GetAdditionalItem();
                }
            ).Add(
                "Manipulation Test12. Has the first cell been moved to after Title5?",
                async () =>
                {
                    ScrollController.ScrollToStart(true);
                    await Task.Delay(2000);
                    ItemsGroupSource[0].Move(0, 4);
                }
            ).Add(
                "Manipulation Test13. Has new section been inserted at last?",
                async () =>
                {
                    await Task.Delay(2000);
                    ItemsGroupSource.Add(_additionalGroup);
                    await Task.Delay(2000);
                    ScrollController.ScrollToEnd(true);
                }
            ).Add(
                "Manipulation Test14. Has the last section been deleted?",
                async () =>
                {
                    await Task.Delay(2000);
                    ItemsGroupSource.Remove(_additionalGroup);
                    await Task.Delay(2000);
                    ScrollController.ScrollToEnd(true);
                }
            ).Add(
                "Has GroupHeaderHeight been twice as large as the privious?",
                async () =>
                {
                    await Task.Delay(2000);
                    HeaderHeight.Value *= 2;
                },
                () => {
                    HeaderHeight.Value /= 2;
                }
            );
        }

        void InitializeProperties() {
            ItemsGroupSource = new ObservableCollection<PhotoGroup>();

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
            for (var i = 0; i < 20; i++)
            {
                list2.Add(new PhotoItem
                {
                    PhotoUrl = $"https://kamusoft.jp/openimage/nativecell/{i + 1}.jpg",
                    Title = $"Title {i + 1}",
                    Category = "BBB",
                });
            }
            var list3 = new List<PhotoItem>();
            for (var i = 10; i < 20; i++)
            {
                list3.Add(new PhotoItem
                {
                    PhotoUrl = $"https://kamusoft.jp/openimage/nativecell/{i + 1}.jpg",
                    Title = $"Title {i + 1}",
                    Category = "CCC",
                });
            }
            var list4 = new List<PhotoItem>();
            for (var i = 5; i < 15; i++)
            {
                list4.Add(new PhotoItem
                {
                    PhotoUrl = $"https://kamusoft.jp/openimage/nativecell/{i + 1}.jpg",
                    Title = $"Title {i + 1}",
                    Category = "DDD",
                });
            }

            var group1 = new PhotoGroup(list1) { Head = "SectionA" };
            var group2 = new PhotoGroup(list2) { Head = "SectionB" };
            var group3 = new PhotoGroup(list3) { Head = "SectionC" };
            _additionalGroup = new PhotoGroup(list4) { Head = "SectionD" };
            ItemsGroupSource.Add(group1);
            ItemsGroupSource.Add(group2);
            ItemsGroupSource.Add(group3);

        }

        public override void InitializeCommand()
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
                        ItemsGroupSource[0].Insert(0, addItem);                       
                        break;
                    case 1:
                        ItemsGroupSource[0].Add(addItem);
                        break;
                    case 2:
                        ItemsGroupSource[0].Insert(ItemsGroupSource[0].Count / 2, addItem);
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
                        ItemsGroupSource[0].RemoveAt(0);
                        break;
                    case 1:
                        ItemsGroupSource[0].RemoveAt(ItemsGroupSource[0].Count - 1);
                        break;
                    case 2:
                        ItemsGroupSource[0].RemoveAt(ItemsGroupSource[0].Count / 2);
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
                ItemsGroupSource[0][0] = addItem;
            });

            MoveCommand.Subscribe(_ =>
            {
                ItemsGroupSource[0].Move(0, 4);
            });


            AddSecCommand.Subscribe(_ =>
            {
                ItemsGroupSource.Add(_additionalGroup);
            });

            DelSecCommand.Subscribe(_ =>
            {
                ItemsGroupSource.RemoveAt(ItemsSource.Count - 1);
            });
        }
    }
}
