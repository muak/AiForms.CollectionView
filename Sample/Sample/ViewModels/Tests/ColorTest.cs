using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Sample.ViewModels.Tests
{
    public class ColorTest:TestGroup
    {
        public ColorTest():base("Color")
        {
        }

        public override void SetUp()
        {
            base.SetUp();
            VM.Background.Value = Color.White;
            VM.FeedbackColor.Value = Color.Yellow;
        }

        [Test(Message = "Has Background turned from Yellow to White to Green to transparent?")]
        public async void BackgroundColor()
        {
            VM.Background.Value = Color.Yellow;
            await Task.Delay(1000);
            VM.Background.Value = Color.White;
            await Task.Delay(1000);
            VM.Background.Value = Color.Green;
            await Task.Delay(1000);
            VM.Background.Value = Color.Transparent;
        }

        [Test(Message = "Tap some cells. Has FeedBack color turned Red?")]
        public void FeedbackColor()
        {
            VM.FeedbackColor.Value = Color.Red;
        }
    }
}
