const navigationOptionSelector = ".sbd-stage .sbd-nav li[role='option']";

document.addEventListener(
    "keydown",
    (event) => {
        if (
            (event.key === "ArrowUp" || event.key === "ArrowDown") &&
            event.target instanceof Element &&
            event.target.closest(navigationOptionSelector)
        ) {
            event.preventDefault();
        }
    },
    true
);
