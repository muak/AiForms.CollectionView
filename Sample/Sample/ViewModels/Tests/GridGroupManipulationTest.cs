using System;
using System.Threading.Tasks;
using AiForms.Renderers;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using System.Collections.Generic;

namespace Sample.ViewModels.Tests
{
    public class GridGroupManipulationTest:TestGroup
    {

        IScrollController ScrollController => VM.ScrollController;
        PhotoGroup ItemsSource => VM.ItemsSource;
        ObservableCollection<PhotoGroup> ItemsGroupSource => VM.ItemsGroupSource;
        PhotoGroup _additionalGroup;

        public GridGroupManipulationTest():base("GridGroupManipulation")
        {
        }

        public override void Initialize()
        {
            base.Initialize();
            var list = new List<PhotoItem>();
            for (var i = 5; i < 15; i++)
            {
                list.Add(new PhotoItem
                {
                    PhotoUrl = $"https://kamusoft.jp/openimage/nativecell/{i + 1}.jpg",
                    Title = $"Title {i + 1}",
                    Category = "DDD",
                });
            }
            _additionalGroup = new PhotoGroup(list) { Head = "SectionD" };

        }

        [Test(Message = "Has SectionB been scrolled to Top to Bottom To Center?")]
        public async void ScrollTest()
        {
            ScrollController.ScrollToStart();
            await Task.Delay(2000);
            ScrollController.ScrollTo(ItemsGroupSource[1][0], ItemsGroupSource[1], ScrollToPosition.Start, false);
            await Task.Delay(2000);
            ScrollController.ScrollTo(ItemsGroupSource[1][0], ItemsGroupSource[1], ScrollToPosition.End, false);
            await Task.Delay(2000);
            ScrollController.ScrollTo(ItemsGroupSource[1][0], ItemsGroupSource[1], ScrollToPosition.Center, false);
        }

        [Test(Message = "Has SectionB been scrolled to Top to Bottom To Center with animation?")]
        public async void ScrollTest2()
        {
            ScrollController.ScrollToStart(true);
            await Task.Delay(2000);
            ScrollController.ScrollTo(ItemsGroupSource[1][0], ItemsGroupSource[1], ScrollToPosition.Start, true);
            await Task.Delay(2000);
            ScrollController.ScrollTo(ItemsGroupSource[1][0], ItemsGroupSource[1], ScrollToPosition.End, true);
            await Task.Delay(2000);
            ScrollController.ScrollTo(ItemsGroupSource[1][0], ItemsGroupSource[1], ScrollToPosition.Center, true);
        }

        [Test(Message = "Has SectionC - Title14 been scrolled to Top to Bottom To Center?")]
        public async void ScrollTest3()
        {
            ScrollController.ScrollToStart(false);
            await Task.Delay(2000);
            ScrollController.ScrollTo(ItemsGroupSource[2][3], ItemsGroupSource[2], ScrollToPosition.Start, false);
            await Task.Delay(2000);
            ScrollController.ScrollTo(ItemsGroupSource[2][3], ItemsGroupSource[2], ScrollToPosition.End, false);
            await Task.Delay(2000);
            ScrollController.ScrollTo(ItemsGroupSource[2][3], ItemsGroupSource[2], ScrollToPosition.Center, false);
        }

        [Test(Message = "Has SectionC - Title14 been scrolled to Top to Bottom To Center with animation?")]
        public async void ScrollTest4()
        {
            ScrollController.ScrollToStart(true);
            await Task.Delay(2000);
            ScrollController.ScrollTo(ItemsGroupSource[2][3], ItemsGroupSource[2], ScrollToPosition.Start, true);
            await Task.Delay(2000);
            ScrollController.ScrollTo(ItemsGroupSource[2][3], ItemsGroupSource[2], ScrollToPosition.End, true);
            await Task.Delay(2000);
            ScrollController.ScrollTo(ItemsGroupSource[2][3], ItemsGroupSource[2], ScrollToPosition.Center, true);
        }

        [Test(Message = "Has new cell been inserted before Title1?")]
        public async void InsertTest()
        {
            ScrollController.ScrollToStart(true);
            await Task.Delay(2000);
            ItemsGroupSource[0].Insert(0, VM.GetAdditionalItem());
        }

        [Test(Message = "Has new cell been inserted after Title20?")]
        public async void InsertTest2()
        {
            ScrollController.ScrollTo(ItemsGroupSource[0][20], ItemsGroupSource[0], ScrollToPosition.End, true);
            await Task.Delay(2000);
            ItemsGroupSource[0].Add(VM.GetAdditionalItem());
        }

        [Test(Message = "Has new cell been inserted between Title8 and Title9?")]
        public async void InsertTest3()
        {
            ScrollController.ScrollTo(ItemsGroupSource[0][8], ItemsGroupSource[0], ScrollToPosition.Center, true);
            await Task.Delay(2000);
            ItemsGroupSource[0].Insert(9, VM.GetAdditionalItem());
        }

        [Test(Message = "Has the first cell been deleted?")]
        public async void DeleteTest()
        {
            ScrollController.ScrollToStart(true);
            await Task.Delay(2000);
            ItemsGroupSource[0].RemoveAt(0);
        }

        [Test(Message = "Has the last cell in SectionA been deleted?")]
        public async void DeleteTest2()
        {
            ScrollController.ScrollTo(ItemsGroupSource[0][21], ItemsGroupSource[0], ScrollToPosition.End, true);
            await Task.Delay(2000);
            ItemsGroupSource[0].RemoveAt(ItemsGroupSource[0].Count - 1);
        }

        [Test(Message = "Has the cell between Title8 and Title9 in SectionA been deleted?")]
        public async void DeleteTest3()
        {
            ScrollController.ScrollTo(ItemsGroupSource[0][8], ItemsGroupSource[0], ScrollToPosition.Center, true);
            await Task.Delay(2000);
            ItemsGroupSource[0].RemoveAt(8);
        }

        [Test(Message = "Has the first cell been changed to AddItem?")]
        public async void ReplaceTest()
        {
            ScrollController.ScrollToStart(true);
            await Task.Delay(2000);
            ItemsGroupSource[0][0] = VM.GetAdditionalItem();
        }

        [Test(Message = "Has the first cell been moved to after Title5?")]
        public async void MoveTest()
        {
            ScrollController.ScrollToStart(true);
            await Task.Delay(2000);
            ItemsGroupSource[0].Move(0, 4);
        }

        [Test(Message = "Has new section been inserted at last?")]
        public async void SectionInsertTest()
        {
            await Task.Delay(2000);
            ItemsGroupSource.Add(_additionalGroup);
            await Task.Delay(2000);
            ScrollController.ScrollToEnd(true);
        }

        [Test(Message = "Has the last section been deleted?")]
        public async void SectionDeleteTest()
        {
            await Task.Delay(2000);
            ItemsGroupSource.Remove(_additionalGroup);
            await Task.Delay(2000);
            ScrollController.ScrollToEnd(true);
        }


    }
}
