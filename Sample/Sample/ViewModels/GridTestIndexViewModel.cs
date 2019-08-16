using System;
using System.Collections.Generic;
using Sample.ViewModels.Tests;
using System.Linq;
using Reactive.Bindings;
using Xamarin.Forms;
using Prism.Navigation;

namespace Sample.ViewModels
{
    public class GridTestIndexViewModel
    {
        public ReactiveCommand RunCommand { get; } = new ReactiveCommand();
        public ReactiveCommand AllCheckCommand { get; } = new ReactiveCommand();
        public ReactiveCommand NoneCheckCommand { get; } = new ReactiveCommand();
        public ReactiveCommand SaveCommand { get; } = new ReactiveCommand();

        public List<TestSection> TestSections { get; } = new List<TestSection>();
        public GridTestIndexViewModel(INavigationService navigationService)
        {
            var Section = new TestSection { SectionTitle = "Fundamental" };
            Section.Add(new PullToRefreshTest());
            Section.Add(new RowSpacingAndHeightTest());
            Section.Add(new ColorTest());
            Section.Add(new LoadMoreTest());

            TestSections.Add(Section);

            Section = new TestSection { SectionTitle = "GridType" };
            Section.Add(new UniformGridTest());
            Section.Add(new AutoSpacingTest());

            TestSections.Add(Section);

            Section = new TestSection { SectionTitle = "Individual" };
            Section.Add(new GridManipulationTest());

            TestSections.Add(Section);

            var groups = TestSections.SelectMany(x => x).ToList();


            void CheckChange(bool turned)
            {
                foreach (var grp in groups)
                {
                    grp.Check.Value = turned;
                }
            }

            AllCheckCommand.Subscribe(_ => CheckChange(true));
            NoneCheckCommand.Subscribe(_ => CheckChange(false));

            SaveCommand.Subscribe(_ =>
            {
                for (var i = 0; i < groups.Count; i++)
                {
                    Application.Current.Properties[$"check{i}"] = groups[i].Check.Value;
                }
                Application.Current.SavePropertiesAsync();
            });

            for (var i = 0; i < groups.Count; i++)
            {
                if (Application.Current.Properties.TryGetValue($"check{i}", out var check))
                {
                    groups[i].Check.Value = (bool)check;
                }
            }

            RunCommand.Subscribe(async _ =>
            {
                var param = new NavigationParameters();
                param.Add("tests", TestSections);
                await navigationService.NavigateAsync("CollectionViewTest", param);
            });
        }
    }
}
