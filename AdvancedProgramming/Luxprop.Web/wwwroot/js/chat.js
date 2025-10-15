window.scrollToBottom = (element) => {
    if (element) {
        element.scrollTop = element.scrollHeight;
    }
};

// Utility used by Login.razor to fetch input values in case the browser
// autofills without firing 'input' events (common on some browsers)
window.luxpropGetInputValue = (id) => {
    const el = document.getElementById(id);
    return el ? el.value : '';
};
