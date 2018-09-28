using System;
using System.Collections.Generic;

namespace Sample.ViewModels
{
    public class DefaultValueTestViewModel
    {
        public PhotoGroup ItemsSource { get; set; }

        public DefaultValueTestViewModel()
        {
            InitializeProperties();
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
        }
    }
}
