using System;
using Reactive.Bindings;
using System.Collections.Generic;
using Unity.Attributes;
using Prism.Services;
using System.Linq;
namespace Sample.ViewModels
{
    public class TestFormViewModel
    {
        public ReactiveCommand<TestItem> OKCommand { get; set; } = new ReactiveCommand<TestItem>();
        public ReactiveCommand<TestItem> NGCommand { get; set; } = new ReactiveCommand<TestItem>();
        public ReactiveCommand SelectedCommand { get; set; } = new ReactiveCommand();
        public TestCollection ItemsSource { get; set; }
        public ReactivePropertySlim<int> Position { get; set; } = new ReactivePropertySlim<int>();
        public IPageDialogService PageDialog { get; set; }

        TestItem _prevItem;

        public TestFormViewModel()
        {
            OKCommand.Subscribe(item =>
            {
                item.Result = true;
                if(Position.Value == ItemsSource.Count - 1){
                    TestFinished();
                    return;
                }
                Position.Value++;
            });
            NGCommand.Subscribe(item =>
            {
                item.Result = false;
            });
            Position.Subscribe(pos =>
            {
                if (ItemsSource == null) return;
                _prevItem?.CancelAction?.Invoke();
                ItemsSource[pos].Action?.Invoke();
                _prevItem = ItemsSource[pos];
            });
        }

        async void TestFinished() {
            var text = "Passed All";
            if(!ItemsSource.All(x=>x.Result)){
                text = "Failed";
            }

            await PageDialog.DisplayAlertAsync("", text, "OK");
        }
    }

    public class TestCollection:List<TestItem>
    {
        public TestCollection Add(string message,Action action = null,Action cancel = null) {
            this.Add(new TestItem
            {
                Message = message,
                Action = action,
                CancelAction = cancel,
            });

            return this;
        }
    }

    public class TestItem
    {
        public string Message { get; set; }
        public bool Result { get; set; }
        public Action Action { get; set; }
        public Action CancelAction { get; set; }
    }
}
