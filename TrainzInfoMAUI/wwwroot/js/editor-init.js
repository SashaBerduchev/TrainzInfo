document.addEventListener("DOMContentLoaded", function () {
    const editorElements = document.querySelectorAll('textarea[data-editor]');

    editorElements.forEach(el => {
        ClassicEditor
            .create(el, {
                toolbar: [
                    'undo', 'redo', '|',
                    'heading', '|',
                    'bold', 'italic', 'underline', '|',
                    'link', 'bulletedList', 'numberedList', '|',
                    'blockQuote', 'insertTable'
                ]
            })
            .catch(error => {
                console.error(error);
            });
    });
});