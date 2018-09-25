using System;
using Xamarin.Forms;

namespace AiForms.Renderers
{
    public class ScrollController:IScrollController
    {
        WeakReference<CollectionView> _refView;
        public ScrollController(CollectionView collectionView)
        {
            _refView = new WeakReference<CollectionView>(collectionView);
        }

        public void ScrollTo(object sourceItem, ScrollToPosition scrollToPosition, bool animated = false)
        {
            if(_refView.TryGetTarget(out var collection)){
                collection.ScrollTo(sourceItem, scrollToPosition,animated);
            }
        }

        public void ScrollTo(object sourceItem, object sourceGroup, ScrollToPosition scrollToPosition, bool animated = false)
        {
            if (_refView.TryGetTarget(out var collection))
            {
                collection.ScrollTo(sourceItem, sourceGroup, scrollToPosition, animated);
            }
        }

        public void ScrollToStart(bool animated = false)
        {
            if (_refView.TryGetTarget(out var collection))
            {
                collection.ScrollTo(null,ScrollToPosition.Start,animated);
            }
        }

        public void ScrollToEnd(bool animated = false)
        {
            if (_refView.TryGetTarget(out var collection))
            {
                collection.ScrollTo(null, ScrollToPosition.End, animated);
            }
        }
    }

    /// <summary>
    /// Scroll controller.
    /// </summary>
    public interface IScrollController
    {
        /// <summary>
        /// Scrolls to.
        /// </summary>
        /// <param name="sourceItem">An item in ItemsSource</param>
        /// <param name="scrollToPosition">Scroll to position.</param>
        /// <param name="animated">If set to <c>true</c> animated.</param>
        void ScrollTo(object sourceItem, ScrollToPosition scrollToPosition, bool animated = false);
        /// <summary>
        /// Scrolls to.
        /// </summary>
        /// <param name="sourceItem">An item in ItemsSource in ItemsSouceGroup</param>
        /// <param name="sourceGroup">A group in ItemsSouceGroup.</param>
        /// <param name="scrollToPosition">Scroll to position.</param>
        /// <param name="animated">If set to <c>true</c> animated.</param>
        void ScrollTo(object sourceItem, object sourceGroup ,ScrollToPosition scrollToPosition, bool animated = false);

        void ScrollToStart(bool animated = false);

        void ScrollToEnd(bool animated = false);
    }
}
