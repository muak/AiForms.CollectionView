using System;
using System.Collections.Generic;
using AiForms.Extras.Abstractions;
using Xamarin.Forms;
using System.Threading.Tasks;

namespace Sample.Views
{
    public partial class MyToastView : ToastView
    {
        public MyToastView()
        {
            InitializeComponent();
            ToastWidth = 0.9;
            ToastHeight = 30;
            Duration = 2000;
            LayoutAlignment = LayoutAlignment.Center;
            BackgroundColor = Color.FromRgba(0, 0, 0, 0.8);
            Opacity = 0;
            CornerRadius = 5;
            TranslationY = 30;
        }

        public override void RunPresentationAnimation()
        {
            Task.WhenAll(
                this.FadeTo(1),this.TranslateTo(0,0));
        }
    }
}
