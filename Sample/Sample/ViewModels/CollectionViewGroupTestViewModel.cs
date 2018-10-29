using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Reactive.Bindings;
using Prism.Services;
using System.Threading.Tasks;
using Xamarin.Forms;
using Prism.Navigation;
using System.Linq;
using System.Diagnostics;

namespace Sample.ViewModels
{
    public class CollectionViewGroupTestViewModel:CollectionViewTestViewModel
    {

        public ReactiveCommand AddSecCommand { get; set; } = new ReactiveCommand();
        public ReactiveCommand DelSecCommand { get; set; } = new ReactiveCommand();

        IPageDialogService _pageDlg;

        public CollectionViewGroupTestViewModel(IPageDialogService pageDialog):base(pageDialog)
        {
            _pageDlg = pageDialog;
            InitializeProperties();

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

            var group1 = new PhotoGroup(list1) { Head = "SectionA" };
            var group2 = new PhotoGroup(list2) { Head = "SectionB" };
            var group3 = new PhotoGroup(list3) { Head = "SectionC" };

            ItemsGroupSource.Add(group1);
            ItemsGroupSource.Add(group2);
            ItemsGroupSource.Add(group3);
        }
    }
}
