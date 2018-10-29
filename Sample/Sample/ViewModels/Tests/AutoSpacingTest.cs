using System;
using System.Threading.Tasks;

namespace Sample.ViewModels.Tests
{
    public class AutoSpacingTest:TestGroup
    {
        public AutoSpacingTest():base("AutoSpacing")
        {
        }

        public override void Initialize()
        {
            base.Initialize();
            VM.GridType.Value = AiForms.Renderers.GridType.AutoSpacingGrid;
            VM.ColumnWidth.Value = 150;
            VM.SpacingType.Value = AiForms.Renderers.SpacingType.Between;
            VM.ColumnSpacing.Value = 4;
        }

        public override void Destroy()
        {
            base.Destroy();
            Initialize();
        }

        [Test(Message = "Has Colum width been changed 150 to 120 to 90 to 60 with keeping Between?")]
        public async void ColumnNumberDecrement()
        {
            VM.ColumnWidth.Value = 120;
            await Task.Delay(1000);
            VM.ColumnWidth.Value = 90;
            await Task.Delay(1000);
            VM.ColumnWidth.Value = 60;
        }

        [Test(Message = "Has SpacingType been changed to Center?")]
        public void ChangeTypeCenter()
        {
            VM.SpacingType.Value = AiForms.Renderers.SpacingType.Center;
        }

        [Test(Message = "Has Colum width been changed 60 to 90 to 120 to 150 with keeping Center?")]
        public async void ColumnNumberIncrement()
        {
            VM.ColumnWidth.Value = 90;
            await Task.Delay(1000);
            VM.ColumnWidth.Value = 120;
            await Task.Delay(1000);
            VM.ColumnWidth.Value = 150;
        }

        [Test(Message = "Has ColumnSpacing been changed 0?")]
        public void ChangeSpacing0()
        {
            VM.ColumnSpacing.Value = 0;
        }

        [Test(Message = "Has Colum width been changed 150 to 120 to 90 to 60 with keeping Center?")]
        public async void ColumnNumberDecrement2()
        {
            VM.ColumnWidth.Value = 120;
            await Task.Delay(1000);
            VM.ColumnWidth.Value = 90;
            await Task.Delay(1000);
            VM.ColumnWidth.Value = 60;
        }

        [Test(Message = "Change Landscape.")]
        public void ChangeLandscape()
        {
            VM.ColumnSpacing.Value = 4;
        }

        [Test(Message = "Has Colum width been changed 60 to 90 to 120 to 150 with keeping Center?")]
        public async void ColumnNumberIncrement2()
        {
            VM.ColumnWidth.Value = 90;
            await Task.Delay(1000);
            VM.ColumnWidth.Value = 120;
            await Task.Delay(1000);
            VM.ColumnWidth.Value = 150;
        }

        [Test(Message = "Has SpacingType been changed to Between?")]
        public void ChnageBetweeen()
        {
            VM.SpacingType.Value = AiForms.Renderers.SpacingType.Between;
        }

        [Test(Message = "Has Colum width been changed 150 to 120 to 90 to 60 with keeping Between?")]
        public async void ColumnNumberDecrement3()
        {
            VM.ColumnWidth.Value = 120;
            await Task.Delay(1000);
            VM.ColumnWidth.Value = 90;
            await Task.Delay(1000);
            VM.ColumnWidth.Value = 60;
        }

        [Test(Message = "Change Portrait")]
        public void ChangePortrait(){}
    }
}
