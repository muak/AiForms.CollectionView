# CollectionView for Xamarin.Forms

This is a flexible ListView that has a grid and horizontal layout with reusable cells for Android / iOS.

## Available controls

* GridCollectionView
* HCollectionView (HorizontalCollectionView)

### Demo

## Available bindable properties deriving from ListView

* ItemsSource
* IsGroupingEnabled
* ItemTemplate
* GroupHeaderTemplate
* BackgroundColor
* IsPullToRefreshEnabled – only GridCollectionView
* RefreshCommand – only GridCollectionView
* IsRefreshing – only GridCollectionView

## GridCollectionView

This is a ListView that lays out each item in a grid pattern. Though this is similar to [WrapLayout](https://github.com/muak/AiForms.Layouts#wraplayout), is different from it in that cells can be recycled.

### Bindable Properties

* [GridType](#gridtype)
    * Select grid layout type using an enumeration value either UniformGrid or AutoSpacingGrid. (Default: UniformGrid)
* PortraitColumns
    *  Specify the number of columns displayed on portrait mode when GridType is UniformGrid.
* LandscapeColumns
    * Specify the number of columns displayed on landscape mode when GridType is UniformGrid.
* RowSpacing
    * Specify the spacing between each row.
* ColumnSpacing
    * Specify the spacing between each column when GridType is UniformGrid or AutoSpacingGrid with setting SpacingType to center.
* ColumnWidth
    * Specify the width of a column when GridType is AutoSpacingGrid.
* ColumnHeight
    * Specify the height of a column. If the value is less than or equal to 5.0, it is used as the ratio of the width: Otherwise used as the absolute size.
* GroupHeaderHeight
    * Specify the height of a group header cell.
* [SpacingType](#spacingtype)
    * Select the spacing type using an enumeration value either Between or Center. This is used only when GridType is AutoSpacingGrid.
* PullToRefreshColor
    * Specify a color of the PullToRefresh indicator icon.

### <a href="#gridtype"></a>GridType enumeration

* UniformGrid
    * The number of columns arranged in each row is specified. Each column width becomes the width obtained by dividing the row width by that value. This number of columns can be set by PortraitColumns and LandscapeColumns properties.
* AutoSpacingGrid
    * Once a column width is specified, each column is arranged until fitting in each row and adjusted automatically each spacing. A column width can be set by ColumnWidth property and Setting SpacingType property can change how to adjust the spacing.

### <a href="#spacingtype"></a>SpacingType enumeration

* Between
  * Both side items are arranged to each edge without spacing. The other items are uniformly arranged in the remained space.
* Center
  * Each item is uniformly arranged with the specified spacing, and the remained space is assigned to each edge.

## What CollectionView can do.

### General

* To layout each item in a grid pattern.
* To layout each item in a horizontal pattern.