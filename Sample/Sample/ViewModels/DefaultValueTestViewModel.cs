using System;
using System.Collections.Generic;
using Prism.Mvvm;
using Prism.Navigation;
using Reactive.Bindings;
using System.Threading.Tasks;


namespace Sample.ViewModels
{
    public class DefaultValueTestViewModel:BindableBase,INavigatedAware
    {
        public ReactiveCollection<PhotoItem> ItemsSource { get; set; } = new ReactiveCollection<PhotoItem>();

        public DefaultValueTestViewModel()
        {
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


            ItemsSource.AddRangeOnScheduler(list);
        }

        public void OnNavigatingTo(NavigationParameters parameters)
        {
        }

        public void OnNavigatedFrom(NavigationParameters parameters)
        {
        }

        public async void OnNavigatedTo(NavigationParameters parameters)
        {
            await Load();
        }

        internal async Task Load()
        {
            try
            {
                InitializeProperties();
            }
            finally {; }
        }
    }
}
