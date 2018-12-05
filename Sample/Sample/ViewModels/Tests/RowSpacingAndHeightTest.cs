using System;
namespace Sample.ViewModels.Tests
{
    public class RowSpacingAndHeightTest:TestGroup
    {
        public RowSpacingAndHeightTest():base("RowSpacingAndHeight")
        {
        }

        public override void SetUp()
        {
            base.SetUp();
            VM.RowSpacing.Value = 4.0;
            VM.GroupFirstSpacing.Value = 0;
            VM.GroupLastSpacing.Value = 0;
            VM.ColumnHeight.Value = 1.0;
            VM.IsGroupHeaderSticky.Value = true;
            VM.AdditionalHeight.Value = 0;
            VM.HeaderHeight.Value = 36;
        }

        [Test(Message = "Has RowSpacing become much larger?")]
        public void RowSpacing()
        {
            VM.RowSpacing.Value = 25;
        }

        [Test(Message = "Has Group First Spacing been 30px in Grouped?")]
        public void FirstSpacing()
        {
            VM.GroupFirstSpacing.Value = 30;
        }

        [Test(Message = "Has Group Last Spacing been 30px in Grouped?")]
        public void LastSpacing()
        {
            VM.GroupLastSpacing.Value = 30;
        }

        [Test(Message = "Has ColumnHeight been changed to absolute value(250px)?")]
        public void ColumnHeightAbsolute()
        {
            VM.ColumnHeight.Value = 250;
        }

        [Test(Message = "Has ColumnHeight been changed to relative value(0.5)?")]
        public void ColumnHeightProportional()
        {
            VM.ColumnHeight.Value = 0.5;
        }

        [Test(Message = "Has ColumnHeight been changed to 1.0 + 50px by AdditionalHeight?")]
        public void AdditionalHeight()
        {
            VM.AdditionalHeight.Value = 50;
        }

        [Test(Message = "Has GroupHeaderHeight been twice as large as the privious?")]
        public void GroupHeaderHeight()
        {
            VM.HeaderHeight.Value *= 2;
        }

        //[Test(Message = "Has HeaderCell position been released from sticky? (iOS)")]
        //public void Sticky()
        //{
        //    VM.IsGroupHeaderSticky.Value = false;
        //}

    }
}
