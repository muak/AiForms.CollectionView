using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AiForms.Renderers;

namespace Sample.ViewModels.Tests
{
    public class LoadMoreGroupTest:TestGroup
    {
        IScrollController ScrollController => VM.ScrollController;
        IDisposable loadMoreSub;


        public LoadMoreGroupTest():base("LoadMore")
        {
        }

        public override void SetUp()
        {
            base.SetUp();
            loadMoreSub?.Dispose();
        }

        void CommandLoadMoreItems()
        {          

            for (var i = 0; i < 10; i++)
            {
                VM.ItemsGroupSource[2].Add(new PhotoItem
                {
                    PhotoUrl = $"https://kamusoft.jp/openimage/nativecell/{i + 1}.jpg",
                    Title = $"P1 {i + 1}",
                    Category = "AAA",
                });
            }
        }

        void CommandLoadMoreGroup()
        {
            var list = new List<PhotoItem>();
            for (var i = 5; i < 15; i++)
            {
                list.Add(new PhotoItem
                {
                    PhotoUrl = $"https://kamusoft.jp/openimage/nativecell/{i + 1}.jpg",
                    Title = $"P2 {i + 1}",
                    Category = "DDD",
                });
            }

            VM.ItemsGroupSource.Add(new PhotoGroup(list) { Head = "MoreSection" });
        }

        [Test(Message = "LoadMore 10 Items And Complete LoadMore.")]
        public async void LoadMoreItems()
        {
            loadMoreSub = VM.LoadMoreCommand.Subscribe(CommandLoadMoreItems);


            ScrollController.ScrollToEnd(true);
            await Task.Delay(500);
            ScrollController.ScrollToEnd(true);
        }

        [Test(Message = "LoadMoreCompletion reset And LoadMore 1 Group")]
        public async void LoadMoreGroup()
        {
            ScrollController.ScrollToStart(true);
            await Task.Delay(500);

            VM.SetEndLoadMore(false);
            loadMoreSub.Dispose();
            loadMoreSub = VM.LoadMoreCommand.Subscribe(CommandLoadMoreGroup);

            ScrollController.ScrollToEnd(true);
            await Task.Delay(500);
            ScrollController.ScrollToEnd(true);
        }
    }
}
