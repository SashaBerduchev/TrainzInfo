window.initCKEditorForEditNews = (textareaEl, dotNetHelper, methodName, initialData) => {
    if (!textareaEl) return;

    ClassicEditor.create(textareaEl)
        .then(editor => {
            if (initialData) editor.setData(initialData);
            editor.model.document.on('change:data', () => {
                dotNetHelper.invokeMethodAsync(methodName, editor.getData());
            });
        })
        .catch(console.error);
};