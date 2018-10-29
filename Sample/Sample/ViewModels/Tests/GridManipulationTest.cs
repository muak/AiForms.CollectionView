using System;
using System.Linq;
using System.Threading.Tasks;
using AiForms.Renderers;
using Xamarin.Forms;

namespace Sample.ViewModels.Tests
{
    public class GridManipulationTest:TestGroup
    {
        IScrollController ScrollController => VM.ScrollController;
        PhotoGroup ItemsSource => VM.ItemsSource;

        public GridManipulationTest():base("GridManipulation")
        {
        }

        [Test(Message = "Has Title10 been scrolled to Top to Bottom To Center?")]
        public async void ScrollTest()
        {
            ScrollController.ScrollTo(ItemsSource.First(), ScrollToPosition.Start, false);
            await Task.Delay(2000);
            ScrollController.ScrollTo(ItemsSource[9], ScrollToPosition.Start, false);
            await Task.Delay(2000);
            ScrollController.ScrollTo(ItemsSource[9], ScrollToPosition.End, false);
            await Task.Delay(2000);
            ScrollController.ScrollTo(ItemsSource[9], ScrollToPosition.Center, false);
        }

        [Test(Message = "Has Title10 been scrolled to Top to Bottom To Center with animation?")]
        public async void ScrollAnimeTest()
        {
            ScrollController.ScrollTo(ItemsSource.First(), ScrollToPosition.Start, true);
            await Task.Delay(2000);
            ScrollController.ScrollTo(ItemsSource[9], ScrollToPosition.Start, true);
            await Task.Delay(2000);
            ScrollController.ScrollTo(ItemsSource[9], ScrollToPosition.End, true);
            await Task.Delay(2000);
            ScrollController.ScrollTo(ItemsSource[9], ScrollToPosition.Center, true);
        }

        [Test(Message = "Has new cell been inserted before Title1?")]
        public async void InsertTest()
        {
            ScrollController.ScrollTo(ItemsSource.First(), ScrollToPosition.Start, true);
            await Task.Delay(2000);
            ItemsSource.Insert(0, VM.GetAdditionalItem());
        }

        [Test(Message = "Has new cell been inserted after Title20?")]
        public async void InsertTest2()
        {
            ScrollController.ScrollTo(ItemsSource.Last(), ScrollToPosition.End, true);
            await Task.Delay(2000);
            ItemsSource.Add(VM.GetAdditionalItem());
        }

        [Test(Message = "Has new cell been inserted between Title8 and Title9?")]
        public async void InsertTest3()
        {
            ScrollController.ScrollTo(ItemsSource[8], ScrollToPosition.Center, true);
            await Task.Delay(2000);
            ItemsSource.Insert(9, VM.GetAdditionalItem());
        }

        [Test(Message = "Has the first cell been deleted?")]
        public async void DeleteTest()
        {
            ScrollController.ScrollTo(ItemsSource.First(), ScrollToPosition.Start, true);
            await Task.Delay(2000);
            ItemsSource.RemoveAt(0);
        }

        [Test(Message = "Has the last cell been deleted?")]
        public async void DeleteTest2()
        {
            ScrollController.ScrollTo(ItemsSource.Last(), ScrollToPosition.End, true);
            await Task.Delay(2000);
            ItemsSource.RemoveAt(ItemsSource.Count - 1);
        }

        [Test(Message = "Has the cell between Title8 and Title9 been deleted?")]
        public async void DeleteTest3()
        {
            ScrollController.ScrollTo(ItemsSource[8], ScrollToPosition.Center, true);
            await Task.Delay(2000);
            ItemsSource.RemoveAt(8);
        }

        [Test(Message = "Has the first cell been changed to AddItem?")]
        public async void ReplaceTest()
        {
            ScrollController.ScrollTo(ItemsSource.First(), ScrollToPosition.Start, true);
            await Task.Delay(2000);
            ItemsSource[0] = VM.GetAdditionalItem();
        }

        [Test(Message = "Has the first cell been moved to after Title5?")]
        public async void MoveTest()
        {
            ScrollController.ScrollTo(ItemsSource.First(), ScrollToPosition.Start, true);
            await Task.Delay(2000);
            ItemsSource.Move(0, 4);
        }
    }
}
