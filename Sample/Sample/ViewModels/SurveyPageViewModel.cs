using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Sample.ViewModels
{
    public class SurveyPageViewModel
    {
        public ObservableCollection<PhotoItem> ItemsSource { get; }

        public SurveyPageViewModel()
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

            ItemsSource = new ObservableCollection<PhotoItem>(list1);
        }
    }
}
