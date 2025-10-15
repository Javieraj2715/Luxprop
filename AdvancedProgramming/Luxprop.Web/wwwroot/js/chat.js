window.scrollToBottom = (element) => {
    if (element) {
        element.scrollTop = element.scrollHeight;
    }
};

// Helper used by login page to read latest input values even if onchange didn't fire
window.luxpropGetInputValue = (id) => {
    const el = document.getElementById(id);
    return el ? el.value : '';
};
