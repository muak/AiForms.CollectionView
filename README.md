# CollectionView for Xamarin.Forms

This is a flexible ListView that has a grid and horizontal layout with reusable cells for Android / iOS.

## Available controls

* GridCollectionView
* HCollectionView (HorizontalCollectionView)

### Demo

## Available functions deriving from ListView

### Bindable properties

* ItemsSource
* IsGroupingEnabled
* ItemTemplate
* GroupHeaderTemplate
* BackgroundColor
* IsPullToRefreshEnabled – only GridCollectionView
* RefreshCommand – only GridCollectionView
* IsRefreshing – only GridCollectionView

### Events

* ItemTapped

### Methods

* ScrollTo

## GridCollectionView

This is a ListView that lays out each item in a grid pattern. Though this is similar to [WrapLayout](https://github.com/muak/AiForms.Layouts#wraplayout), is different from it in that cells can be recycled.

### Bindable Properties

* [GridType](#gridtype)
    * Select grid layout type using an enumeration value either UniformGrid or AutoSpacingGrid. (Default: UniformGrid)
* PortraitColumns
    * The number of columns displayed on portrait mode when GridType is UniformGrid.
* LandscapeColumns
    * The number of columns displayed on landscape mode when GridType is UniformGrid.
* RowSpacing
    * The spacing between each row.
* ColumnSpacing
    * The spacing between each column when GridType is UniformGrid or AutoSpacingGrid with setting SpacingType to center.
* ColumnWidth
    * The width of a column when GridType is AutoSpacingGrid.
* ColumnHeight
    * The height of a column. If the value is less than or equal to 5.0, it is used as the ratio of the width: Otherwise used as the absolute size.
* AdditionalHeight
    * The additional height of ColumnHeight. Combining this value with ColumnHeight ratio value, Total height can be set such as 100% + 15px for example.
* GroupHeaderHeight
    * The height of a group header cell.
* [SpacingType](#spacingtype)
    * Select the spacing type using an enumeration value either Between or Center. This is used only when GridType is AutoSpacingGrid.
* PullToRefreshColor
    * The color of the PullToRefresh indicator icon.
* ItemTapCommand
  * The command invoked when an item is tapped.
* ItemLongTapCommand
  * The command invoked when an item is pressed longly.
* TouchFeedbackColor
  * The color rendered when an item is touched.
* [ScrollController](#scrollcontroller)
  * The object for manipulating the scroll from such as ViewModel.

### Special Properties

* ComputedWidth (ReadOnly)
    * The column width after being calculated when using UniformGrid in particular. Note that this value can sometimes make 1 pixel difference from the actual width.
* ComputedHeight (ReadOnly)
  * The column height after being calculated.

### <a href="#gridtype"></a>GridType Enumeration

* UniformGrid
    * The number of columns arranged in each row is specified. Each column width becomes the width obtained by dividing the row width by that value. This number of columns can be set by PortraitColumns and LandscapeColumns properties.
* AutoSpacingGrid
    * Once a column width is specified, each column is arranged until fitting in each row and adjusted automatically each spacing. A column width can be set by ColumnWidth property and Setting SpacingType property can change how to adjust the spacing.

### <a href="#spacingtype"></a>SpacingType Enumeration

* Between
  * Both side items are arranged to each edge without spacing. The other items are uniformly arranged in the remained space.
* Center
  * Each item is uniformly arranged with the specified spacing, and the remained space is assigned to each edge.

### <a href="#scrollcontroller"></a>ScrollController

This is the object which allows methods for scrolling to be called from such as ViewModel.
The following code is the example calling ScrollTo method from ViewModel:

```cs
public class SomeViewModel
{
    public ObservableCollection<string> ItemsSource { get; } = new ObservableCollection<string>{ new List<string>{"A","B","C"} };
    public IScrollController ScrollController { get; set; }

    ...
    public GoToItem(int target)
    {
        // Scroll to the specified item position at the first visible area position with animation. If the target is 1, scroll to "B".
        ScrollController.ScrollTo(ItemsSource[target],ScrollToPosition.Start,true);
    }
    public GoToStart()
    {
        ScrollController.ScrollToStart(true); // scroll to "A" with animation
    }
    public GoToEnd()
    {
        ScrollController.ScrollToEnd(true); // scroll to "C" with animation
    }
}
```

```xml
<ai:GridCollectionView 
    ItemsSource="{Binding ItemsSource}"
    ScrollController="{Binding ScrollController}"
    ...
>
</ai:GridCollectionView>
```

#### IScrollController Methods

* ScrollTo
    * ``void ScrollTo(object sourceItem, ScrollToPosition scrollToPosition, bool animated = false)``
    * ``void ScrollTo(object sourceItem, object sourceGroup ,ScrollToPosition scrollToPosition, bool animated = false)``
    * Scroll to specified item. This method is the same as that of ListView.
    * Note that the argument item is not a cell but an ItemsSource element.
* ScrollToStart
    * ``void ScrollToStart(bool animated = false)``
    * Scroll to the top-most or left-most position.
* ScrollToEnd
    * ``void ScrollToEnd(bool animated = false)``
    * Scroll to the bottom-most or right-most position.


## What CollectionView can do.

### General

* To layout each item in a grid pattern.
* To layout each item in a horizontal pattern.