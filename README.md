# OutsideClickHandler
Detect click outside of element for Blazor. <br>
*An improvement of [Blazor.OutsideClickHandler](https://github.com/skordesign/Blazor.OutsideClickHandler).* <br>

## What's different?
- There's an issue that after OnClickOutside is invoked, it creates a new addeventlistener everytime creating a flood unused event listeners. This does not, it only creates one and stores OutsideClickHandler instances on a list on first render and removes it when StopPropagation is true or when the component is disposed.
- There's also an issue when OnClickOutside is continuously triggered when unneeded. StopPropagation mitigates that.

## Usage
1. Install [nuget package](https://www.nuget.org/packages/OutsideClickHandler/).
2. Add *@using OutsideClickHandler.Component* to the desired component/page or in _Imports.razor. <br>
![Step1](https://i.imgur.com/zh6AJfE.png) <br>
3. Add js src on index.html in wwwroot folder.
![Step2](https://i.imgur.com/KDUKyvm.png) <br>
4. That's it! You may now use it.
![Step3-1](https://i.imgur.com/sIWldgV.png)![Step3-2](https://i.imgur.com/kp7ncFc.gif)

## Parameters
- OnOutsideClick
  - Accepts void function to be called on outside click.
  - Executed first when both OnOutsideClick and OnOutsideClickTask have value.
- OnOutsideClickTask
  - Accepts awaitable function to be called on outside click.
- StopPropagation
  - Stops <c>OnOutsideClick</c> and <c>OnOutsideClickTask</c> from executing when <c>true</c>.
