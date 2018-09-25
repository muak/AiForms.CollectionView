using System;
using System.Collections.Generic;

using Xamarin.Forms;
using Sample.ViewModels;

namespace Sample.Views
{
    public partial class HCollectionViewTest : ContentPage
    {
        public HCollectionViewTest()
        {
            InitializeComponent();
        }

        ScrollToPosition pos = ScrollToPosition.Start;
        void Handle_Clicked(object sender, System.EventArgs e)
        {
            var vm = BindingContext as HCollectionViewTestViewModel;

            collectionView.ScrollTo(vm.ItemsSource[1][0],vm.ItemsSource[1], pos, false);
            collectionView2.ScrollTo(vm.ItemsSource2[4], pos, false);

            if (pos == ScrollToPosition.End)
            {
                pos = ScrollToPosition.Start;
            }
            else
            {
                pos++;
            }
        }
    }
}
