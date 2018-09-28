using System;
using Xamarin.Forms;

namespace AiForms.Renderers
{
    /// <summary>
    /// Scroll controller.
    /// </summary>
    public class ScrollController:IScrollController
    {
        WeakReference<CollectionView> _refView;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:AiForms.Renderers.ScrollController"/> class.
        /// </summary>
        /// <param name="collectionView">Collection view.</param>
        public ScrollController(CollectionView collectionView)
        {
            _refView = new WeakReference<CollectionView>(collectionView);
        }

        /// <summary>
        /// Scrolls to.
        /// </summary>
        /// <param name="sourceItem">Source item.</param>
        /// <param name="scrollToPosition">Scroll to position.</param>
        /// <param name="animated">If set to <c>true</c> animated.</param>
        public void ScrollTo(object sourceItem, ScrollToPosition scrollToPosition, bool animated = true)
        {
            if(_refView.TryGetTarget(out var collection)){
                collection.ScrollTo(sourceItem, scrollToPosition,animated);
            }
        }

        /// <summary>
        /// Scrolls to.
        /// </summary>
        /// <param name="sourceItem">Source item.</param>
        /// <param name="sourceGroup">Source group.</param>
        /// <param name="scrollToPosition">Scroll to position.</param>
        /// <param name="animated">If set to <c>true</c> animated.</param>
        public void ScrollTo(object sourceItem, object sourceGroup, ScrollToPosition scrollToPosition, bool animated = true)
        {
            if (_refView.TryGetTarget(out var collection))
            {
                collection.ScrollTo(sourceItem, sourceGroup, scrollToPosition, animated);
            }
        }

        /// <summary>
        /// Scrolls to start.
        /// </summary>
        /// <param name="animated">If set to <c>true</c> animated.</param>
        public void ScrollToStart(bool animated = true)
        {
            if (_refView.TryGetTarget(out var collection))
            {
                collection.ScrollTo(null,ScrollToPosition.Start,animated);
            }
        }

        /// <summary>
        /// Scrolls to end.
        /// </summary>
        /// <param name="animated">If set to <c>true</c> animated.</param>
        public void ScrollToEnd(bool animated = true)
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
        void ScrollTo(object sourceItem, ScrollToPosition scrollToPosition, bool animated = true);
        /// <summary>
        /// Scrolls to.
        /// </summary>
        /// <param name="sourceItem">An item in ItemsSource in ItemsSouceGroup</param>
        /// <param name="sourceGroup">A group in ItemsSouceGroup.</param>
        /// <param name="scrollToPosition">Scroll to position.</param>
        /// <param name="animated">If set to <c>true</c> animated.</param>
        void ScrollTo(object sourceItem, object sourceGroup ,ScrollToPosition scrollToPosition, bool animated = true);

        /// <summary>
        /// Scrolls to start.
        /// </summary>
        /// <param name="animated">If set to <c>true</c> animated.</param>
        void ScrollToStart(bool animated = true);

        /// <summary>
        /// Scrolls to end.
        /// </summary>
        /// <param name="animated">If set to <c>true</c> animated.</param>
        void ScrollToEnd(bool animated = true);
    }
}
