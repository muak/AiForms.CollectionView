using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Sample.ViewModels
{
    public class DataTemplateTestViewModel
    {
        public ObservableCollection<PhotoGroup> ItemsSource { get; set; } = new ObservableCollection<PhotoGroup>();

        public DataTemplateTestViewModel()
        {
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

            var group1 = new PhotoGroup(list1) { Head = "SecA" };
            var group2 = new PhotoGroup(list2) { Head = "SecB" };
            var group3 = new PhotoGroup(list3) { Head = "SecC" };
            ItemsSource.Add(group1);
            ItemsSource.Add(group2);
            ItemsSource.Add(group3);
        }
    }
}
