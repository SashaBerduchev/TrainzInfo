window.addCitiesScrollListener = function (dotnetHelper) {
    window.addEventListener('scroll', () => {
        const scrollTop = document.documentElement.scrollTop || document.body.scrollTop;
        const scrollHeight = document.documentElement.scrollHeight;
        const clientHeight = document.documentElement.clientHeight;

        // Если до конца страницы осталось 100px
        if (scrollTop + clientHeight >= scrollHeight - 100) {
            if (dotnetHelper) {
                dotnetHelper.invokeMethodAsync('OnScrollNearBottom');
            }
        }
    });
};