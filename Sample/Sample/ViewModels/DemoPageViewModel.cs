using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using AiForms.Extras.Abstractions;
using Reactive.Bindings;
using Prism.Services;
using Sample.Views;

namespace Sample.ViewModels
{
    public class DemoPageViewModel
    {
        public ObservableCollection<PhotoGroup> ItemsSource { get; set; } = new ObservableCollection<PhotoGroup>();
        public ReactiveCommand TapCommand { get; } = new ReactiveCommand();
        public ReactiveCommand LongTapCommand { get; } = new ReactiveCommand();
        public ReactiveProperty<bool> IsRefreshing { get; } = new ReactiveProperty<bool>(false);
        public AsyncReactiveCommand RefreshCommand { get; } = new AsyncReactiveCommand();
        public AsyncReactiveCommand NextCommand { get; } = new AsyncReactiveCommand();

        public TestCollection TestList { get; set; } = new TestCollection();

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
                "PullToRefresh can't be work."
            ).Add(
                "Can PullToRefresh be work?",
                () =>
                {
                    //EnabledPullToRefresh.Value = true;
                }
            );
        }
    }
}
