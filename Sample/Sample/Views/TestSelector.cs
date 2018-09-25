using System;
using Xamarin.Forms;
using AiForms.Renderers;
using Sample.ViewModels;

namespace Sample.Views
{
    public class TestSelector : DataTemplateSelector
    {
        public DataTemplate TemplateA { get; set; }
        public DataTemplate TemplateB { get; set; }
            

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            var hoge = item as PhotoItem;
            return hoge.Title.Contains("1") ? TemplateB : TemplateA;
        }
    }
}
