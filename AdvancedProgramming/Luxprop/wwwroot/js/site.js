window.scrollToBottom = (element) => {
    try {
        if (element) element.scrollTop = element.scrollHeight;
    } catch (e) { }
};
