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

function measureVisible(root) {
    const items = [...root.querySelectorAll("[data-testid='chip-item']")].filter((item) => {
        return getComputedStyle(item).display !== "none";
    });
    if (items.length === 0) {
        return 0;
    }
    const content = contentWidth(root);
    const gap = gapPx(root);
    const overflow = root.querySelector("[data-testid='chip-overflow']");
    const reserve = overflow ? overflow.getBoundingClientRect().width + gap : 0;
    let cursor = content.start;
    let visible = 0;
    for (const item of items) {
        const box = item.getBoundingClientRect();
        const width = box.width;
        const itemStart = content.rtl ? cursor - width : cursor;
        const crosses = content.rtl ? itemStart < content.end - 0.5 : itemStart + width > content.end + 0.5;
        const needsOverflow = visible + 1 < items.length;
        const reservedEnd = content.rtl ? content.end + reserve : content.end - reserve;
        const hitsReserve = needsOverflow && (content.rtl ? itemStart < reservedEnd - 0.5 : itemStart + width > reservedEnd + 0.5);
        if (visible > 0 && (crosses || hitsReserve)) {
            break;
        }
        cursor = content.rtl ? cursor - width - gap : cursor + width + gap;
        visible += 1;
    }
    return Math.min(items.length, Math.max(1, visible));
}

export function observe(rootId, dotnet) {
    const root = document.getElementById(rootId);
    if (!root || typeof ResizeObserver === "undefined") {
        return;
    }
    const measure = () => {
        const visible = measureVisible(root);
        if (visible > 0) {
            dotnet.invokeMethodAsync("OnOverflowMeasured", visible);
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
