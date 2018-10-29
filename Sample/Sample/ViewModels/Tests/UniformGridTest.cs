using System;
using System.Threading.Tasks;

namespace Sample.ViewModels.Tests
{
    public class UniformGridTest : TestGroup
    {
        public UniformGridTest() : base("UniformGrid")
        {
        }

        public override void Initialize()
        {
            base.Initialize();
            VM.GridType.Value = AiForms.Renderers.GridType.UniformGrid;
            VM.PortraitColumns.Value = 2;
            VM.LandscapeColumns.Value = 4;
            VM.BothSidesMargin.Value = 10;
            VM.ColumnSpacing.Value = 4;
        }

        public override void Destroy()
        {
            base.Destroy();
            Initialize();
        }

        public override void SetUp()
        {
            base.SetUp();
        }

        [Test(Message = "Has Colums number been changed 2 to 3 to 4 to 5 to 6 on Portrait?")]
        public async void ColumnNumberIncrement()
        {
            VM.PortraitColumns.Value = 3;
            await Task.Delay(1000);
            VM.PortraitColumns.Value = 4;
            await Task.Delay(1000);
            VM.PortraitColumns.Value = 5;
            await Task.Delay(1000);
            VM.PortraitColumns.Value = 6;
        }

        [Test(Message = "Has ColumnSpacing / BothSidesMargin been changed 0?")]
        public void SpacingAndMargin()
        {
            VM.ColumnSpacing.Value = 0;
            VM.BothSidesMargin.Value = 0;
        }

        [Test(Message = "Has Colums number been changed 6 to 5 to 4 to 3 to 2 on Portrait?")]
        public async void ColumnNumberDecrement()
        {
            VM.PortraitColumns.Value = 5;
            await Task.Delay(1000);
            VM.PortraitColumns.Value = 4;
            await Task.Delay(1000);
            VM.PortraitColumns.Value = 3;
            await Task.Delay(1000);
            VM.PortraitColumns.Value = 2;
        }

        [Test(Message = "Change Landscape. Is Columns number 4?")]
        public void ChangeLandscape() { }

        [Test(Message = "Has Colums number been changed 5 to 6 to 7 to 8 on Landscape?")]
        public async void LandscapeColumnNumber()
        {
            VM.LandscapeColumns.Value = 5;
            await Task.Delay(1000);
            VM.LandscapeColumns.Value = 6;
            await Task.Delay(1000);
            VM.LandscapeColumns.Value = 7;
            await Task.Delay(1000);
            VM.LandscapeColumns.Value = 8;
            await Task.Delay(1000);
        }

        [Test(Message = "Have ColumnSpacing and BothSidesMargin been changed 4 and 10?")]
        public void SpacingAndMargin2()
        {
            VM.ColumnSpacing.Value = 4;
            VM.BothSidesMargin.Value = 10;
        }

        [Test(Message = "Has Colums number been changed 8 to 7 to 6 to 5 to 4 on Landscape?")]
        public async void LandscapeColumnNumber2()
        {
            VM.LandscapeColumns.Value = 7;
            await Task.Delay(1000);
            VM.LandscapeColumns.Value = 6;
            await Task.Delay(1000);
            VM.LandscapeColumns.Value = 5;
            await Task.Delay(1000);
            VM.LandscapeColumns.Value = 4;
        }

        [Test(Message = "Change Portrait")]
        public void ChangePortrait() { }
    }
}
