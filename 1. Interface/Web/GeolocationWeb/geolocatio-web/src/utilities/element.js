export default class DOMElement {
    static makeButton =
        (id, className, caption) => {
            const button = this.makeElementWith('button', {type: 'button', id: id, class: className});
            button.appendChild(document.createTextNode(caption));
            return button;
        }

    static makeTextInput =
        (id, className) =>
            this.makeElementWith('input', {type: 'text', id: id, class: className})

    static makeParagraph =
        (id, className, value) => {
            const paragraph = this.makeElementWith('p', {id: id, class: className});
            paragraph.appendChild(document.createTextNode(value));
            return paragraph;
        }

    static makeElementWith =
        (tag, props) => {
            const element = document.createElement(tag);
            for (let prop in props) {
                const value = props[prop];
                if (!value)
                    continue;
                element.setAttribute(prop, value);
            }
            return element;
        }

    static removeAllChild =
        (element) => {
            console.log('removeAllChild');
            console.log(element.lastChild);
            while (element.lastChild) {
                element.removeChild(element.lastChild);
            }
            console.log(element);
        }
}