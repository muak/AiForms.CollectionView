using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Sample.ViewModels
{
    public class PhotoGroup : ObservableCollection<PhotoItem>
    {
        public string Head { get; set; }
        public PhotoGroup(IEnumerable<PhotoItem> list) : base(list) { }
    }

    public class PhotoItem
    {
        public string PhotoUrl { get; set; }
        public string Title { get; set; }
        public string Category { get; set; }

    }
}
