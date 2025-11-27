const cityDropDown = document.getElementById("cities");
const servicesDropdown = document.getElementById("services");
const searchForm = document.getElementById("searchForm");
const salonsContainer = document.getElementById("salons-container");

const Cities = {
    Vilnius: "Vilnius",
    Kaunas: "Kaunas"
};

searchForm.addEventListener("submit", (event) => {
    event.preventDefault();
    searchSalons();
});

function populateCities() {
    Object.values(Cities).forEach(city => {
    const option = document.createElement("option");
    option.value = city;
    option.textContent = city;

    cityDropDown.appendChild(option);
});
}

//refactor, look into adding helpers like an apiGet with error handling
async function getResource(endpoint) {
    try {
    const response = await fetch(endpoint);

    if(!response.ok) {
        error = await response.text();
        window.alert(error);
        return [];
    }
    
    const resource = await response.json();
    console.log(resource); //ommit after debugging

    return resource;

    } catch (e) {
        console.error(e);
        return [];
    }
}

function populateServices(array) {
    array.forEach(item => {
        const option = document.createElement("option");
        option.value = item.name;
        option.textContent = item.name;

        servicesDropdown.appendChild(option);
    })
}

function populateSalons(array) {
    array.forEach(item => {
        const card = document.createElement("div");
        const name = document.createElement("h3");
        const fullAddress = document.createElement("h4");

        card.className = "card";
        salonsContainer.appendChild(card);
        name.innerHTML = item.name;
        card.appendChild(name);
        fullAddress.innerHTML = `${item.address}, ${item.city}`;
        card.appendChild(fullAddress);
    });
}

async function searchSalons() {
    const city = cityDropDown.value;
    const service = servicesDropdown.value;

    const query = new URLSearchParams();

    if (city) query.append("city", city);
    if (service) query.append("service", service);

    // call getResource with params
    const salons = await getResource(`api/salons?${query.toString()}`);

    if (salons && salons.length > 0) {
        salonsContainer.innerHTML = ""; //clear previous results
        populateSalons(salons);
    }
}

//not sure if this is needed but saw a recommendation
async function init() {
    const services = await getResource("api/services");
    const salons = await getResource("api/salons");

    populateCities();
    populateServices(services);
    populateSalons(salons);
}

init();