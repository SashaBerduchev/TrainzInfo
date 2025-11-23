window.addNewsScrollListener = function (dotnetHelper) {
    const container = document.getElementById('newsContainer');

    if (!container) {
        console.error("❌ newsContainer not found!");
        return;
    }
    function revealCards() {
        const cards = container.querySelectorAll('.news-card');
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

    window.addEventListener('scroll', () => {
        const scrollTop = document.documentElement.scrollTop;
        const scrollHeight = document.documentElement.scrollHeight;
        const clientHeight = document.documentElement.clientHeight;

        if (scrollTop + clientHeight >= scrollHeight - 50) {
            dotnetHelper.invokeMethodAsync('OnScrollNearBottom');
        }

        revealCards();
    });

    // Перевірка на старті
    revealCards();

    // Слідкуємо за новими картками
    const observer = new MutationObserver(revealCards);
    observer.observe(container, { childList: true, subtree: true });
};
