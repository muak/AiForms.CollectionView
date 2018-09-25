using System;

namespace AiForms.Renderers.Droid
{
    [Android.Runtime.Preserve(AllMembers = true)]
    public interface ICollectionViewRenderer
    {
        int GroupHeaderHeight { get; set; }
        int GroupHeaderWidth { get; set; }
        int CellHeight { get; set; }
        int CellWidth { get; set; }
    }
}
