window.addDieselsScrollListener = function (dotnetHelper) {
    const container = document.getElementById('dieselContainer');

    if (!container) {
        console.error("❌ dieselContainer not found!");
        return;
    }

    function revealCards() {
        const cards = container.querySelectorAll('.diesel-card');
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

    // Lazy load + reveal
    function onScroll() {
        const scrollTop = document.documentElement.scrollTop;
        const scrollHeight = document.documentElement.scrollHeight;
        const clientHeight = document.documentElement.clientHeight;

        if (scrollTop + clientHeight >= scrollHeight - 50 && dotnetHelper) {
            dotnetHelper.invokeMethodAsync('OnScrollNearBottom');
        }

        revealCards();
    }

    window.addEventListener('scroll', onScroll);

    // 🔥 Доступно з Blazor після ApplyFilter / ClearFilter
    window.revealNewDieselCards = function () {
        container
            .querySelectorAll('.diesel-card')
            .forEach(card => card.classList.add('visible'));
    };

    // Початковий reveal
    revealCards();

    // Для динамічно доданих карток
    const observer = new MutationObserver(revealCards);
    observer.observe(container, { childList: true, subtree: true });
};
