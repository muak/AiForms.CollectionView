using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Sample.ViewModels.Tests
{
    public class PullToRefreshTest:TestGroup
    {
        public PullToRefreshTest():base("PullToRefresh"){}

        public override void SetUp()
        {
            base.SetUp();
            VM.EnabledPullToRefresh.Value = true;
            VM.RefreshIconColor.Value = Color.DimGray;
        }

        [Test(Message = "PullToRefresh can't be work.")]
        public void Disabled()
        {
            VM.EnabledPullToRefresh.Value = false;
        }

        [Test(Message = "Can PullToRefresh be work?")]
        public void Enabled()
        {
            VM.EnabledPullToRefresh.Value = true;
        }

        [Test(Message = "Has Refresh icon turned red?")]
        public void TurnIconColor()
        {
            VM.RefreshIconColor.Value = Color.Red;
        }
    }
}
