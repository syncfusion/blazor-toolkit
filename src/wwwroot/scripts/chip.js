const observers = new Map();

function lengthPx(value) {
    const parsed = Number.parseFloat(value);
    if (!Number.isFinite(parsed) || parsed === 0) {
        return 0;
    }
    if (String(value).trim().endsWith("rem")) {
        const rootSize = Number.parseFloat(getComputedStyle(document.documentElement).fontSize);
        return parsed * (Number.isFinite(rootSize) ? rootSize : 16);
    }
    return parsed;
}

function gapPx(root) {
    const style = getComputedStyle(root);
    const gap = style.columnGap && style.columnGap !== "normal" ? style.columnGap : "0.4rem";
    return lengthPx(gap);
}

function contentWidth(root) {
    const style = getComputedStyle(root);
    const box = root.getBoundingClientRect();
    const padStart = lengthPx(style.paddingInlineStart);
    const padEnd = lengthPx(style.paddingInlineEnd);
    const rtl = style.direction === "rtl";
    return {
        start: rtl ? box.right - padStart : box.left + padStart,
        end: rtl ? box.left + padEnd : box.right - padEnd,
        rtl,
        width: box.width - padStart - padEnd,
    };
}

function naturalWidth(item) {
    const hidden = item.classList.contains("e-chip-collapsed");
    if (hidden) {
        item.classList.remove("e-chip-collapsed");
    }
    const width = item.getBoundingClientRect().width;
    if (hidden) {
        item.classList.add("e-chip-collapsed");
    }
    return width;
}

function measureVisible(root, maximum) {
    const items = [...root.querySelectorAll("[data-testid='chip-item']")];
    if (items.length === 0) {
        return 0;
    }
    const content = contentWidth(root);
    const gap = gapPx(root);
    const overflow = root.querySelector("[data-testid='chip-overflow']");
    const reserve = overflow ? overflow.getBoundingClientRect().width + gap : 0;
    const cap = Number.isFinite(maximum) && maximum > 0 ? Math.min(items.length, maximum) : items.length;
    let used = 0;
    let visible = 0;
    for (let index = 0; index < cap; index += 1) {
        const width = naturalWidth(items[index]);
        const needsOverflow = index + 1 < items.length;
        const next = used + width + (visible > 0 ? gap : 0);
        if (visible > 0 && next + (needsOverflow ? reserve : 0) > content.width + 0.5) {
            break;
        }
        used = next;
        visible += 1;
    }
    return Math.min(cap, Math.max(items.length === 0 ? 0 : 1, visible));
}

export function observe(rootId, dotnet, maximum) {
    const root = document.getElementById(rootId);
    if (!root || typeof ResizeObserver === "undefined" || observers.has(rootId)) {
        return;
    }
    const measure = () => {
        if (!observers.has(rootId)) {
            return;
        }
        const visible = measureVisible(root, maximum);
        if (visible > 0) {
            dotnet.invokeMethodAsync("OnOverflowMeasured", visible).catch(() => undefined);
        }
    };
    const observer = new ResizeObserver(measure);
    observer.observe(root);
    observers.set(rootId, observer);
    measure();
}

export function dispose(rootId) {
    const observer = observers.get(rootId);
    if (observer) {
        observer.disconnect();
        observers.delete(rootId);
    }
}

export function scrollIntoView(id) {
    const node = document.getElementById(id);
    if (node && node.scrollIntoView) {
        node.scrollIntoView({ block: "nearest", inline: "nearest" });
    }
}
