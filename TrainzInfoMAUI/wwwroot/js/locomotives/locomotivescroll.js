window.addLocomotiveScrollListener = function (dotnetHelper) {
    const container = document.getElementById('locomotiveContainer');

    if (!container) {
        console.error("❌ locomotiveContainer not found!");
        return;
    }

    function revealCards() {
        const cards = container.querySelectorAll('.loco-card');
        const windowBottom = window.innerHeight + window.scrollY;

        cards.forEach(card => {
            if (!card.classList.contains('visible')) {
                const cardTop = card.getBoundingClientRect().top + window.scrollY;
                if (windowBottom > cardTop + 100) {
                    card.classList.add('visible');
                }
            }
        });
    }

    // Lazy load при скролі
    window.addEventListener('scroll', () => {
        const scrollTop = document.documentElement.scrollTop;
        const scrollHeight = document.documentElement.scrollHeight;
        const clientHeight = document.documentElement.clientHeight;

        if (scrollTop + clientHeight >= scrollHeight - 50 && dotnetHelper) {
            dotnetHelper.invokeMethodAsync('OnScrollNearBottom');
        }

        revealCards();
    });

    // Функція для Blazor: показати всі нові картки одразу
    window.revealNewLocoCards = function () {
        container.querySelectorAll('.loco-card').forEach(card => card.classList.add('visible'));
    };

    // Початковий reveal для вже наявних карток
    revealCards();

    // Спостерігач для доданих вузлів
    const observer = new MutationObserver(revealCards);
    observer.observe(container, { childList: true, subtree: true });
};
