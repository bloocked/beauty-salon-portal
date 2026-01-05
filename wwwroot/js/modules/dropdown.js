export function populateCities(dropwdown, Cities) {
    Object.values(Cities).forEach(city => {
    const option = document.createElement("option");
    option.value = city;
    option.textContent = city;

    dropwdown.appendChild(option);
});
}

export function populateServices(dropwdown, array) {
    array.forEach(item => {
        const option = document.createElement("option");
        option.value = item.name;
        option.textContent = item.name;

        dropwdown.appendChild(option);
    })
}