window.addScrollListener = function (dotnetHelper) {
    window.addEventListener('scroll', () => {
        const scrollTop = document.documentElement.scrollTop;
        const scrollHeight = document.documentElement.scrollHeight;
        const clientHeight = document.documentElement.clientHeight;

        if (scrollTop + clientHeight >= scrollHeight - 50) { // близько до низу
            dotnetHelper.invokeMethodAsync('OnScrollNearBottom');
        }
    });
};
