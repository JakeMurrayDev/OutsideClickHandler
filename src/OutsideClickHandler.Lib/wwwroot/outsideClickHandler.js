//Fields and Setters
let _element;
let _dotnetHelper;

function setElement(element) {
    _element = element;
}

function setDotnetHelper(dotnetHelper) {
    _dotnetHelper = dotnetHelper;
}
//End Fields and Setters

//Called in Blazor
export function initialize(element, dotnetHelper) {
    setElement(element);
    setDotnetHelper(dotnetHelper);
    window.addEventListener("click", outsideClickEvent);
}


export function dispose() {
    window.removeEventListener("click", outsideClickEvent);
    setElement(null);
    setDotnetHelper(null);
}
//End Called in Blazor

//Calls Blazor Function
function outsideClickEvent(e) {
    if (!_element.contains(e.target)) {
        _dotnetHelper.invokeMethodAsync("InvokeOutsideClick");
    }
}
//End Calls Blazor Function

