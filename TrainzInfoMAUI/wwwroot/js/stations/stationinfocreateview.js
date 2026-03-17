window.initCKEditorForStationInfo = (textareaId, dotNetHelper, methodName, initialData) => {
    const el = document.querySelector(`#${textareaId}`);
    if (!el) return;

    ClassicEditor
        .create(el)
        .then(editor => {
            if (initialData) editor.setData(initialData); // встановлюємо початкові дані, якщо є
            editor.model.document.on('change:data', () => {
                dotNetHelper.invokeMethodAsync(methodName, editor.getData());
            });
        })
        .catch(error => console.error(error));
};
