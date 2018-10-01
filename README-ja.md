# CollectionView for Xamarin.Forms

CollectionViewは、Xamarin.Formsで使用できるGridや水平方向レイアウトに対応した柔軟なListViewです。AndroidとiOSに対応しています。

## コントロール

* [GridCollectionView](#gridcollectionview)
    * Grid状にアイテムを並べるListView
* [HCollectionView](#hcollectionview) (HorizontalCollectionView)
    * 水平方向にアイテムを並べるListView

## 最小対応バージョン 

iOS: iOS10  
Android: version 5.1.1 (only FormsAppcompatActivity) / API22

## デモ

<img src="images/SS_ios.jpg" height="800" /> <img src="images/SS_android.jpg" height="800" />

<a href="https://www.youtube.com/watch?feature=player_embedded&v=qF4sVnE5Dao
" target="_blank"><img src="https://img.youtube.com/vi/qF4sVnE5Dao/0.jpg" 
alt="" width="480" height="360" border="0" /></a>

## 使用方法

### Nuget でのインストール

[https://www.nuget.org/packages/AiForms.CollectionView/](https://www.nuget.org/packages/AiForms.CollectionView/)

```bash
Install-Package AiForms.CollectionView -pre
```

.NETStandardプロジェクトと各プラットフォームのプロジェクトにインストールする必要があります。

### For iOS project

iOSの場合は AppDelegate.cs に以下の記述が必要です。

```csharp
public override bool FinishedLaunching(UIApplication app, NSDictionary options) {
    global::Xamarin.Forms.Forms.Init();

    AiForms.Renderers.iOS.CollectionViewInit.Init(); //need to write here

    LoadApplication(new App(new iOSInitializer()));

    return base.FinishedLaunching(app, options);
}
```

### Xaml からの使用方法 

#### For GridCollectionView

```xml
<ContentPage 
    xmlns="http://xamarin.com/schemas/2014/forms" 
    xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml" 
    xmlns:ai="clr-namespace:AiForms.Renderers;assembly=CollectionView"
    x:Class="Sample.Views.Test">
    <ai:GridCollectionView 
        ItemsSource="{Binding ItemsSource}" TouchFeedbackColor="Yellow"
        ColumnWidth="100" ColumnHeight="1.0" >
        <ListView.ItemTemplate>
            <DataTemplate>
                <ai:ContentCell>
                    <Label Text="{Binding Name}" />
                </ai:ContentCell>
            </DataTemplate>
        </ListView.ItemTemplate>
    </ai:GridCollectionView>
</ContentPage>
```

#### For HCollectionView

```xml
    ...
    <ai:HCollectionView 
        ItemsSource="{Binding ItemsSource}" TouchFeedbackColor="Yellow"
        ColumnWidth="100" HeightRequest="100" Spacing="4" IsInfinite="true" >
        <ListView.ItemTemplate>
            <DataTemplate>
                <ai:ContentCell>
                    <Label Text="{Binding Name}" />
                </ai:ContentCell>
            </DataTemplate>
        </ListView.ItemTemplate>
    </ai:HCollectionView>
```

#### For GridCollectionView (グループ化)

```xml
    ...
    <ai:GridCollectionView 
        ItemsSource="{Binding ItemsSource}" TouchFeedbackColor="Yellow"
        ColumnWidth="100" ColumnHeight="1.0"
        IsGroupingEnabled="true" GroupHeaderHeight="36"   >
        <ListView.GroupHeaderTemplate>
            <DataTemplate>
                <ai:ContentCell>
                    <Label Text="{Binding Category}" BackgroundColor="#E6DAB9" />
                </ai:ContentCell>
            </DataTemplate>
        </ListView.GroupHeaderTemplate>
        <ListView.ItemTemplate>
            <DataTemplate>
                <ai:ContentCell>
                    <Label Text="{Binding Name}" />
                </ai:ContentCell>
            </DataTemplate>
        </ListView.ItemTemplate>
    </ai:GridCollectionView>
```

HCollectionViewのグループでの使用は上と同じです。

> DataTemplateのルートには ViewCell ではなく ContentCell を配置する必要があることに注意してください。

### グループ化した ItemsSouce の作成例

```cs
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

public class SomeViewModel
{
    public ObservableCollection<PhotoGroup> ItemsSource { get; } = new ObservableCollection<PhotoGroup>();

    public SomeViewModel()
    {
        var list1 = new List<PhotoItem>();
        for (var i = 0; i < 20; i++)
        {
            list1.Add(new PhotoItem
            {
                PhotoUrl = $"https://example.com/{i + 1}.jpg",
                Title = $"Title {i + 1}",
                Category = "AAA",
            });
        }
        var list2 = new List<PhotoItem>();
        for (var i = 20; i < 40; i++)
        {
            list2.Add(new PhotoItem
            {
                PhotoUrl = $"https://example.com/{i + 1}.jpg",
                Title = $"Title {i + 1}",
                Category = "BBB",
            });
        }

        var group1 = new PhotoGroup(list1) { Head = "SectionA" };
        var group2 = new PhotoGroup(list2) { Head = "SectionB" };

        ItemsSource.Add(group1);
        ItemsSource.Add(group2);
    }
}
```

## 使用可能なListViewの機能

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

### Caching Strategy

ListViewのCachingStrategyはGridCollectionViewとHCollectionViewの両方で使用可能です。
ただし、ListViewはデフォルトでRetainElementを使用しますが、CollectionViewではRecycleElementを使用することに注意してください。

ListViewCachingStrategyはプロパティではないので、もし変更する場合は、"x:Arguments" 構文を使用して設定する必要があります。

```xml
<ai:HCollectionView ...>
    <x:Arguments>
        <ListViewCachingStrategy>RetainElement</ListViewCachingStrategy>
    </x:Arguments>
    ...
</ai:HCollectionView>
```

あるいは、C#からコードで設定します。

```cs
var collectionView = new HCollectionView(ListViewCachingStrategy.RetainElement);
```

## 画像を使う場合

DataTemplateの要素に画像を使用する場合は、**[FFImageLoading](https://github.com/luberda-molinet/FFImageLoading)** を利用することを強く推奨します。
CollectionViewには画像を非同期で処理したりキャッシュしたりする機能がないためです。

## GridCollectionView

Grid状に各要素を配置するListViewです。これは [WrapLayout](https://github.com/muak/AiForms.Layouts#wraplayout) に似ていますが、セルをリサイクルできるという点などで異なります。

### Bindable Properties

* [GridType](#gridtype-enumeration)
    * Gridレイアウトのタイプを UniformGrid か AutoSpacingGrid より選択します。(Default: AutoSpacingGrid)
* PortraitColumns
    * 縦向きの時に1行に表示する列数です。GridTypeがUniformGridの時のみ有効です。
* LandscapeColumns
    * 横向きの時に1行に表示する列数です。GridTypeがUniformGridの時のみ有効です。
* RowSpacing
    * 各行の間隔。
* ColumnSpacing
    * 各列の間隔。GridType が UniformGrid のときと、AutoSpacingGrid で SpacingType に Center が設定されているときのみ有効です。
* ColumnWidth
    * 各アイテムの列幅。GridType が AutoSpacingGrid のときのみ有効です。
* ColumnHeight
    * 各アイテムの高さ。5.0以下の場合は、幅に対する比率として扱われ、それより大きい値の場合は絶対サイズとして扱われます。
* AdditionalHeight
    * 追加のアイテムの高さ。この値と ColumnHeight を組み合わせると、高さを幅の100% + 15px にする といったような設定ができます。
* GroupHeaderHeight
    * グループヘッダーのセルの高さ。
* [SpacingType](#spacingtype-enumeration)
    * 列間の間隔の決め方を、Between と Center から選択します。GridType が AutoSpacingGrid のときのみ有効です。(Default: Between)
* PullToRefreshColor
    * PullToRefreshのインジケータに使用する色。
* ItemTapCommand
  * アイテムがタップられた時に発火するコマンド。
* ItemLongTapCommand
  * アイテムがロングタップされた時に発火するコマンド。
* TouchFeedbackColor
  * アイテムをタッチした時に表示するエフェクト色。
* [ScrollController](#scrollcontroller)
  * ViewModelなどでCollectionViewのスクロールを制御する場合に使用するオブジェクト。

### Special Properties

* ComputedWidth – ReadOnly
    * 計算後の列幅。UniformGrid の場合に、実際のおおよその列幅を参照する場合などに使います。
  
    > この値は、実際の幅より1pxほど異なることがあります。

* ComputedHeight – ReadOnly
  * 計算後のアイテムの高さ。

### <a href="#gridtype"></a>GridType Enumeration

* UniformGrid
    * 1行に配置する列数を指定します。各列幅は1行の幅をこの値で割った値になります。この列数は PortraitColumns と LandscapeColumns プロパティで指定できます。
* AutoSpacingGrid
    * 列幅を指定し、1行を満たすまでアイテムを配置して、各間隔を自動で調整します。列幅は ColumnWidth プロパティで指定でき、間隔の調整方法は、SpacingType プロパティで設定できます。

### <a href="#spacingtype"></a>SpacingType Enumeration

* Between
  * 両端に余白なしでアイテムを配置し、残りのスペースに他のアイテムを余白が均等になるように配置します。
* Center
  * 各アイテムはColumnSpacingで設定された間隔で均等に配置され、両端に残りのスペースを割り当てます。

## HCollectionView

水平方向にアイテムを配置するListViewです。IsInfinite プロパティを設定することでスクロールを循環させる（無限にする）ことができます。HCollectionViewもセルをリサイクルします。

### Bindable Properties

* ColumnWidth
    * 列幅。
* Spacing
    * 各列の間隔。
* GroupHeaderWidth
    * グループヘッダーの幅。
* IsInfinite
    * スクロールを循環させるかどうか。 (Default: false)

    > iOSの場合、コンテナ幅を十分に満たす数のセルが必要です。
    > Androidの場合、完全に無限ではないので長時間スクロールすると端に到達することがあります。

* ItemTapCommand
  * アイテムがタップられた時に発火するコマンド。
* ItemLongTapCommand
  * アイテムがロングタップされた時に発火するコマンド。
* TouchFeedbackColor
  * アイテムをタッチした時に表示するエフェクト色。
* [ScrollController](#scrollcontroller)
  * ViewModelなどでCollectionViewのスクロールを制御する場合に使用するオブジェクト。

### 行の高さについて

HCollectionView の行の高さは、HeightRequestの値や、自身のサイズによって決定されます。固定値にする場合はHeightRequestを指定してください。

## <a href="#scrollcontroller"></a>ScrollController

これは、ViewModelなどからスクロールのメソッドを呼び出すことができるオブジェクトです。
以下のコードは、ViewModelから ScrollTo メソッドを呼び出している例です。

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

### IScrollController Methods

* ScrollTo
    * ``void ScrollTo(object sourceItem, ScrollToPosition scrollToPosition, bool animated = true)``
    * ``void ScrollTo(object sourceItem, object sourceGroup ,ScrollToPosition scrollToPosition, bool animated = true)``
    * アイテムを指定して、スクロールします。これはListViewのそれと同じです。
    * 引数のアイテムはセルではなくItemsSourceの要素であることに注意してください。
* ScrollToStart
    * ``void ScrollToStart(bool animated = true)``
    * 一番上か一番左にスクロールします。
* ScrollToEnd
    * ``void ScrollToEnd(bool animated = true)``
    * 一番下か一番右にスクロールします。

## License

The MIT Licensed.

コードの一部は [Xamarin.Forms](https://github.com/xamarin/Xamarin.Forms) から取得しています.