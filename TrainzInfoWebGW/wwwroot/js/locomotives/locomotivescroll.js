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
            if (!card) return;

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
            if (dotnetHelper) {
                dotnetHelper.invokeMethodAsync('OnScrollNearBottom');
            } else {
                console.error("❌ dotnetHelper is null");
            }
        }

        revealCards();
    });

    revealCards();

    const observer = new MutationObserver(revealCards);
    observer.observe(container, { childList: true, subtree: true });
};
