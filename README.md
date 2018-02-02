[logo]: https://raw.githubusercontent.com/Geeksltd/Zebble.FloatingButton/master/Shared/NuGet/Icon.png "Zebble.FloatingButton"


## Zebble.FloatingButton

![logo]

A Zebble plugin to add floating button in Zebble applications.


[![NuGet](https://img.shields.io/nuget/v/Zebble.FloatingButton.svg?label=NuGet)](https://www.nuget.org/packages/Zebble.FloatingButton/)

> This plugin enables you to add floating button in each position that you need. Also, you can add some action menu item to show when user tapped the button in Android, IOS, and UWP platforms.

<br>


### Setup
* Available on NuGet: [https://www.nuget.org/packages/Zebble.FloatingButton/](https://www.nuget.org/packages/Zebble.FloatingButton/)
* Install in your platform client projects.
* Available for iOS, Android and UWP.
<br>


### Api Usage

If you need to add a stand alone floating button without action menus, you can use `Zebble.FloatingButton` object like below:
```csharp
var floatBtn = new FloatingButton
{
    ImagePath = "Image url",
    Color = Colors.Gray,
    Size = FloatingButton.ButtonSize.Big,
    Alignment = FloatingButton.ButtonAlignment.BottomRight
};
```
You can just call `floatBtn.Show()` to add button to each page that you need like below:
```csharp
public async override Task OnInitializing()
{
    await base.OnInitializing();
    
    var floatBtn = new FloatingButton
    {
        ImagePath = "Images/Icons/Share.png",
        Color = Colors.Gray,
        Size = FloatingButton.ButtonSize.Big,
        Alignment = FloatingButton.ButtonAlignment.BottomRight
    };

    await floatBtn.Show();
}
```

##### FloatingButton with action menu item :
To add menu item to `FloatingButton` you need to use `Zebble.FloatingButton<FloatingButton.ActionButtonCollection>` like below:
```csharp
var floatBtn = new FloatingButton<FloatingButton.ActionButtonCollection>
{
    ImagePath = "Images/Icons/Share.png",
    Color = Colors.Gray,
    Size = FloatingButton.ButtonSize.Big,
    Alignment = FloatingButton.ButtonAlignment.BottomRight,
    Source = new FloatingButton.ActionButtonCollection(FloatingButton.ActionButtonAlignment.Top, menuItem1, menuItem2, menuItem3)
};
```
To show floating button with action menu items you need to call `floatBtn.ShowAsActionMenu()` method:
```csharp
public async override Task OnInitializing()
{
    await base.OnInitializing();

    var menuItem1 = new FloatingButton.ActionButton { Color = Colors.LightPink, ImagePath = "Images/Icons/Check.png" };
    var menuItem2 = new FloatingButton.ActionButton { Color = Colors.HotPink, ImagePath = "Images/Icons/Share.png" };

    var floatBtn = new FloatingButton<FloatingButton.ActionButtonCollection>
    {
        ImagePath = "Images/Icons/Share.png",
        Color = Colors.Gray,
        Size = FloatingButton.ButtonSize.Big,
        Alignment = FloatingButton.ButtonAlignment.BottomRight,
        Source = new FloatingButton.ActionButtonCollection(FloatingButton.ActionButtonAlignment.Top, menuItem1, menuItem2)
    };

    await floatBtn.ShowAsActionMenu();
}
```

##### Hiding FloatingButton:
```csharp
await floatBtn.Hide();
```

##### Showing and Hiding FloatingButton action menu items programmatically:
```csharp
//To show action menu items
await floatBtn.ShowActionItems();
// To hide action menu items
await floatBtn.HideActionItems();
```
<br>

### Properties
| Property     | Type         | Android | iOS | Windows |
| :----------- | :----------- | :------ | :-- | :------ |
| IsShowing           | bool          | x       | x   | x       |
| IsActionItemsShowing           | bool          | x       | x   | x       |
| Size           | FloatingButton.ButtonSize          | x       | x   | x       |
| Color           | Color          | x       | x   | x       |
| ImagePath           | bool          | x       | x   | x       |
| Alignment           | FloatingButton.ButtonAlignment          | x       | x   | x       |
| ShadowColor           | Color          | x       | x   | x       |
| Source           | FloatingButton.ActionButtonCollection          | x       | x   | x       |

<br>


### Events
| Event             | Type                                          | Android | iOS | Windows |
| :-----------      | :-----------                                  | :------ | :-- | :------ |
| Tapped            | AsyncEvent<TValue&gt;    | x       | x   | x       |


<br>

### Methods
| Method       | Return Type  | Parameters                          | Android | iOS | Windows |
| :----------- | :----------- | :-----------                        | :------ | :-- | :------ |
| Show         | Task         | -| x       | x   | x       |
| Hide  | Task         | -| x       | x   | x       |
| ShowActionItems  | Task         | -| x       | x   | x       |
| HideActionItems  | Task         | -| x       | x   | x       |