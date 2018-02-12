[logo]: https://raw.githubusercontent.com/Geeksltd/Zebble.FloatingButton/master/Shared/NuGet/Icon.png "Zebble.FloatingButton"


## Zebble.FloatingButton

![logo]

A plugin to add floating action buttons in Zebble applications.


[![NuGet](https://img.shields.io/nuget/v/Zebble.FloatingButton.svg?label=NuGet)](https://www.nuget.org/packages/Zebble.FloatingButton/)

> Floating action buttons (or FAB) are: “A special case of promoted actions. They are distinguished by a circled icon floating above the UI and have special motion behaviors, related to morphing, launching, and its transferring anchor point.”

<br>


### Setup
* Available on NuGet: [https://www.nuget.org/packages/Zebble.FloatingButton/](https://www.nuget.org/packages/Zebble.FloatingButton/)
* Install in your platform client projects.
* Available for iOS, Android and UWP.
<br>


### Api Usage


##### Stand alone FloatingButton (Without actions):
If you need to add a stand alone floating button without actions, you can use `Zebble.FloatingButton` object like below:
```csharp
var floatBtn = new FloatingButton
{
    ImagePath = "Image url",
    BackgroundColor = Colors.Gray,
    Position = FloatingButtonPosition.BottomRight
};
```
You can just call `floatBtn.Show()` to add button to the page:
```csharp
await floatBtn.Show();
```
Or you can add `FloatingButton` to the page in the markup like this:
```xml
<FloatingButton ImagePath="Images/Icons/Share.png" />
```
<br/>

##### FloatingButton with action menu item :
To add actions to `FloatingButton` you need to use `Zebble.FloatingButton`:
```csharp
var floatBtn = new FloatingButton(FloatingButtonFlow.Top, menuItem1, menuItem2)
{
    ImagePath = "Images/Icons/Share.png",
    BackgroundColor = Colors.Gray,
    Position = FloatingButtonPosition.BottomRight
};
```
To show floating button with action menu items you need to call `floatBtn.ShowAsActionMenu()` method:
```csharp
var menuItem1 = new FloatingButton.Action{ BackgroundColor = Colors.LightPink, ImagePath = "Images/Icons/Check.png" };
var menuItem2 = new FloatingButton.Action{ BackgroundColor = Colors.HotPink, ImagePath = "Images/Icons/Share.png" };

var floatBtn = new FloatingButton(FloatingButton.ActionButtonAlignments.Top, menuItem1, menuItem2)
{
    ImagePath = "Images/Icons/Share.png",
    BackgroundColor = Colors.Gray,
    Alignment = FloatingButton.ButtonAlignments.BottomRight
};

await floatBtn.ShowAsActionMenu();
```
Or simply using Zebble markup:
```xml
<FloatingButton ImagePath="Images/Icons/Share.png">
    <FloatingButton.Action ImagePath="Images/Icons/Check.png" Style="background: pink;" />
    <FloatingButton.Action ImagePath="Images/Icons/Add.png" Style="background: red;" />
</FloatingButton>
```
<br/>

##### Hiding FloatingButton:
```csharp
await floatBtn.Hide();
```

##### Showing and Hiding FloatingButton Actions programmatically:
```csharp
//To show Actions
await floatBtn.ShowActions();
// To hide Actions
await floatBtn.HideActions();
```
<br>

### Properties
| Property     | Type         | Android | iOS | Windows |
| :----------- | :----------- | :------ | :-- | :------ |
| IsShowing           | bool          | x       | x   | x       |
| IsActionsShowing           | bool          | x       | x   | x       |
| ImagePath           | bool          | x       | x   | x       |
| Flow | FloatingButtonFlow | x       | x   | x       |
| Position           | FloatingButtonPosition          | x       | x   | x       |
| ShadowColor           | Color          | x       | x   | x       |
| Actions           | List<Action&gt;          | x       | x   | x       |

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
| ShowAsActionMenu  | Task         | -| x       | x   | x       |
| ShowActions  | Task         | -| x       | x   | x       |
| HideActions  | Task         | -| x       | x   | x       |