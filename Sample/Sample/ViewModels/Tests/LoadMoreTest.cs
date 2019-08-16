using System;
using AiForms.Renderers;
using System.Threading.Tasks;

namespace Sample.ViewModels.Tests
{
    public class LoadMoreTest:TestGroup
    {
        IScrollController ScrollController => VM.ScrollController;
        int _pageCount = 1;
        public LoadMoreTest():base("LoadMore"){}

        public override void SetUp()
        {
            base.SetUp();
            _pageCount = 1;
           
            VM.LoadMoreCommand.Subscribe(_ =>
            {
                if(_pageCount == 3)
                {
                    VM.SetEndLoadMore(true);
                    return;
                }

                for (var i = 0; i < 10; i++)
                {
                    VM.ItemsSource.Add(new PhotoItem
                    {
                        PhotoUrl = $"https://kamusoft.jp/openimage/nativecell/{i + 1}.jpg",
                        Title = $"P{_pageCount} {i + 1}",
                        Category = "AAA",
                    });
                }
                VM.SetEndLoadMore(false);
                _pageCount++;
            });
        }

        [Test(Message = "LoadMore 20 Items And LoadMore Complete")]
        public async void LoadMore()
        {

            ScrollController.ScrollToEnd(true);
            await Task.Delay(500);
            ScrollController.ScrollToEnd(true);
            await Task.Delay(500);
            ScrollController.ScrollToEnd(true);
        }

        [Test(Message = "LoadMoreCompletion reset And LoadMore 20 Items Again.")]
        public async void LoadMoreAgain()
        {
            ScrollController.ScrollToStart(true);
            await Task.Delay(500);
            _pageCount = 1;
            VM.SetEndLoadMore(false);
            ScrollController.ScrollToEnd(true);
            await Task.Delay(500);
            ScrollController.ScrollToEnd(true);
            await Task.Delay(500);
            ScrollController.ScrollToEnd(true);
        }


    }
}
