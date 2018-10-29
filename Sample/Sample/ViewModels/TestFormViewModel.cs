using System;
using Reactive.Bindings;
using System.Collections.Generic;
using Unity.Attributes;
using Prism.Services;
using System.Linq;
using Xamarin.Forms;
using System.Reflection;
using System.Threading.Tasks;
using Sample.Views;
using AiForms.Dialogs;

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

        public bool IsFirstItem { get; set; }
        public bool IsLastItem { get; set; }
        public LayoutAlignment VAlign { get; set; } = LayoutAlignment.Center;
        public TestGroup Parent { get; set; }

        public async void Run()
        {
            if(IsFirstItem)
            {
                Toast.Instance.Show<MyToastView>(new {Message=Parent.GroupTitle,VAlign=LayoutAlignment.Start});
                Parent?.Initialize();
                await Task.Delay(500);
            }
            Toast.Instance.Show<MyToastView>(this);
            Parent.SetUp();
            await Task.Delay(500);
            Action?.Invoke();

            if(IsLastItem)
            {
                Parent?.Destroy();
            }
        }
    }

    public class TestAttribute:Attribute{
        public string Message { get; set; }
    }

    public class TestGroup:List<TestItem>
    {
        public CollectionViewTestViewModel VM { get; set; }
        public string GroupTitle { get; set; }
        public ReactivePropertySlim<bool> Check { get; } = new ReactivePropertySlim<bool>();


        public TestGroup(string groupTitle)
        {
            GroupTitle = groupTitle;
            var methods = this.GetType().GetMethods().Where(x => x.GetCustomAttribute<TestAttribute>() != null);

            foreach (var method in methods)
            {
                var methodAction = (Action)method.CreateDelegate(typeof(Action), this);
                var testAttr = method.GetCustomAttribute<TestAttribute>();

                Add(new TestItem { Parent = this,Action = methodAction, Message = testAttr.Message });
            }

            this[0].IsFirstItem = true;
            this[methods.Count() - 1].IsLastItem = true;
        }

        public virtual void Initialize(){}

        public virtual void Destroy(){}

        public virtual void SetUp(){}

    }

    public class TestSection:List<TestGroup>
    {
        public string SectionTitle { get; set; }

        public void SetViewModel(CollectionViewTestViewModel vm)
        {
            foreach(var group in this)
            {
                group.VM = vm;
            }
        }
    }
}
