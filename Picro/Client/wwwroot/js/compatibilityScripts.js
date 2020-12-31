window.getComponentCoordinates = (id) => {
    const element = document.getElementById(id);

    return element.getBoundingClientRect();
}