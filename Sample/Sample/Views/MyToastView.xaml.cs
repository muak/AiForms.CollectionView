using System;
using System.Collections.Generic;
using Xamarin.Forms;
using System.Threading.Tasks;
using AiForms.Dialogs.Abstractions;

namespace Sample.Views
{
    public partial class MyToastView : ToastView
    {
        public MyToastView()
        {
            InitializeComponent();
            Duration = 3500;
            BackgroundColor = Color.FromRgba(0, 0, 0, 0.8);
            OffsetY = 50;
            CornerRadius = 5;
        }
    }
}
