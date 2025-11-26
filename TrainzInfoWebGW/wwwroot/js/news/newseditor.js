window.initCKEditor = (textareaId, dotNetHelper, methodName) => {
    ClassicEditor
        .create(document.querySelector(`#${textareaId}`))
        .then(editor => {
            editor.model.document.on('change:data', () => {
                dotNetHelper.invokeMethodAsync(methodName, editor.getData());
            });
        })
        .catch(error => console.error(error));
};
