// Measurement is a text boundary, not an HTML template. Only these exact legacy
// formatting tokens create nodes; attributes, entities and all other markup stay text.
export function setMeasurementText(element, value) {
    const text = value == null ? '' : String(value);
    const fragment = document.createDocumentFragment();
    const parents = [fragment];
    const tokens = /<br\/>|<br>|<b>|<\/b>|<strong>|<\/strong>/g;
    let offset = 0;
    let match;
    while ((match = tokens.exec(text)) !== null) {
        let parent = parents[parents.length - 1];
        parent.appendChild(document.createTextNode(text.slice(offset, match.index)));
        const token = match[0];
        if (token === '<br>' || token === '<br/>') {
            parent.appendChild(document.createElement('br'));
        } else if (token === '<b>' || token === '<strong>') {
            const bold = document.createElement(token === '<b>' ? 'b' : 'strong');
            parent.appendChild(bold);
            parents.push(bold);
        } else if (parents.length > 1 && parent.nodeName.toLowerCase() === token.slice(2, -1)) {
            parents.pop();
        } else {
            // Unmatched closing tags are data, never browser-parser recovery input.
            parent.appendChild(document.createTextNode(token));
        }
        offset = tokens.lastIndex;
    }
    parents[parents.length - 1].appendChild(document.createTextNode(text.slice(offset)));
    element.replaceChildren(fragment);
}
