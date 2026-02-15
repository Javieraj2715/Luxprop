window.scrollToBottom = (element) => {
    try {
        if (element) element.scrollTop = element.scrollHeight;
    } catch (e) { }
};

window.downloadFile = (fileName, contentType, base64Data) => {
    const link = document.createElement('a');
    link.href = "data:" + contentType + ";base64," + base64Data;
    link.download = fileName;
    document.body.appendChild(link);
    link.click();
    document.body.removeChild(link);
};

window.forceDownload = (url) => {
    const a = document.createElement("a");
    a.href = url;
    a.download = ""; // fuerza intento de descarga
    a.target = "_blank"; // respaldo si el browser bloquea download cross-domain
    a.rel = "noopener";
    document.body.appendChild(a);
    a.click();
    a.remove();
};
