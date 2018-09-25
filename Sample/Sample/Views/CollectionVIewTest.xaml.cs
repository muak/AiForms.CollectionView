using System;
using System.Collections.Generic;
using Sample.ViewModels;
using Xamarin.Forms;
using System.Linq;

namespace Sample.Views
{
    public partial class CollectionViewTest : ContentPage
    {
        public CollectionViewTest()
        {
            InitializeComponent();
        }

        void Handle_ItemTapped(object sender, Xamarin.Forms.ItemTappedEventArgs e)
        {
            var photo = e.Item as PhotoItem;
            DisplayAlert("", $"ItemTapped {photo.Category} {photo.Title}", "OK");
        }

        ScrollToPosition pos = ScrollToPosition.Start;
        void Handle_Clicked(object sender, System.EventArgs e)
        {
            var vm = BindingContext as CollectionViewTestViewModel;

            collectionView.ScrollTo(vm.ItemsSource[9], pos, false);

            if(pos == ScrollToPosition.End) {
                pos = ScrollToPosition.Start;
            }
            else{
                pos++;
            }
        }
    }
}
