window.addElectricsScrollListener = function (dotnetHelper) {
    const container = document.getElementById('trainsContainer');
    if (!container) return console.error("trainsContainer not found!");

    function revealCards() {
        const cards = container.querySelectorAll('.train-card');
        const windowBottom = window.innerHeight + window.scrollY;
        cards.forEach(card => {
            if (!card.classList.contains('visible')) {
                const cardTop = card.getBoundingClientRect().top + window.scrollY;
                if (windowBottom > cardTop + 100) card.classList.add('visible');
            }
        });
    }

    window.addEventListener('scroll', () => {
        const scrollTop = document.documentElement.scrollTop;
        const scrollHeight = document.documentElement.scrollHeight;
        const clientHeight = document.documentElement.clientHeight;

        if (scrollTop + clientHeight >= scrollHeight - 50 && dotnetHelper) {
            dotnetHelper.invokeMethodAsync('OnScrollNearBottom');
        }

        revealCards();
    });

    // Функція, яку можна викликати з Blazor після Load()
    window.revealNewElectricTrainCards = function () {
        container.querySelectorAll('.train-card').forEach(card => card.classList.add('visible'));
    };

    revealCards();
};
