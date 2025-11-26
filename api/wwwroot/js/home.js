const cityDropDown = document.getElementById("cities");
const servicesDropdown = document.getElementById("services");

const Cities = {
    Vilnius: "Vilnius",
    Kaunas: "Kaunas"
};

function populateCities() {
    Object.values(Cities).forEach(city => {
    const option = document.createElement("option");
    option.value = city;
    option.textContent = city;

    cityDropDown.appendChild(option);
});
}

//refactor, look into adding helpers like an apiGet with error handling
async function loadServices() {
    try {
    const response = await fetch("api/services");

    if(!response.ok) {
        error = await response.text();
        window.alert(error);
        return;
    }
    
    const services = await response.json();
    console.log(services); //ommit after debugging

    return services;

    } catch (e) {
        console.error(e);
    }
}

// probably merge cities and services into one func for populating
function populateSelect(objects) {
    objects.forEach(item => {
        const option = document.createElement("option");
        option.value = item.name;
        option.textContent = item.name;

        servicesDropdown.appendChild(option);
    })
}

//not sure if this is needed but saw a recommendation
async function init() {
    const services = await loadServices();

    populateCities();
    populateSelect(services);
}

init();