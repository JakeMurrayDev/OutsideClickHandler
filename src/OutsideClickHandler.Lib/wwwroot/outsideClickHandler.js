let hasOutsideClickEvent = false;
const elementRefs = [];

class ElementRef {
    constructor(element, dotnetHelper) {
        this.element = element;
        this.dotnetHelper = dotnetHelper;
    }
}
//Functions to be invoked in blazor
function addOutsideClickEvent(element, dotnetHelper) {
    const er = new ElementRef(element, dotnetHelper);
    elementRefs.push(er);
    if (!hasOutsideClickEvent) {
        window.addEventListener("click", outsideClickEvent);
        hasOutsideClickEvent = true;
    }
    return elementRefs.length - 1;
}

function removeOutsideClickEvent(index) {
    elementRefs.slice(index, 1);
    if (elementRefs.length === 0) {
        window.removeEventListener("click", outsideClickEvent);
        hasOutsideClickEvent = false;
    }
}
//End Functions to be invoked in blazor
function outsideClickEvent(e) {
    for (var i = 0; i < elementRefs.length; i++) {
        const er = elementRefs[i];
        if (!er.element.contains(e.target)) {
            er.dotnetHelper.invokeMethodAsync("InvokeOutsideClick");
        }
    }
}