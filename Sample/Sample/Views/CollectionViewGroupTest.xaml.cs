using System;
using System.Collections.Generic;

using Xamarin.Forms;
using Sample.ViewModels;

namespace Sample.Views
{
    public partial class CollectionViewGroupTest : ContentPage
    {
        public CollectionViewGroupTest()
        {
            InitializeComponent();
        }

        void Handle_ItemTapped(object sender, Xamarin.Forms.ItemTappedEventArgs e)
        {
            var photo = e.Item as PhotoItem;
            DisplayAlert("", $"ItemTapped {photo.Category} {photo.Title}", "OK");
        }
    }
}
