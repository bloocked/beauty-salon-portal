import { getResource } from "./modules/utils.js";
import { populateCities, populateServices } from "./modules/dropdown.js";

const cityDropDown = document.getElementById("cities");
const servicesDropdown = document.getElementById("services");
const searchForm = document.getElementById("searchForm");
const salonsContainer = document.getElementById("salonsContainer");

const Cities = {
    Vilnius: "Vilnius",
    Kaunas: "Kaunas"
};

searchForm.addEventListener("submit", (event) => {
    event.preventDefault();
    searchSalons();
});

salonsContainer.addEventListener("click", event =>{
    event.preventDefault();
    const card = event.target.closest(".card");

    const salonId = card.dataset.id;
    window.alert(salonId);

    const query = new URLSearchParams();
    query.append("salonId", salonId);
    
    window.location.href = `/salon.html?${query.toString()}`
});

function populateSalons(container, array) {
    container.innerHTML = "";
    array.forEach(salon => {
        const card = document.createElement("div");
        const name = document.createElement("h3");
        const fullAddress = document.createElement("h4");

        card.dataset.id = salon.id;

        card.className = "card";
        salonsContainer.appendChild(card);
        name.innerHTML = salon.name;
        card.appendChild(name);
        fullAddress.innerHTML = `${salon.address}, ${salon.city}`;
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
        populateSalons(salonsContainer, salons);
    }
}

//not sure if this is needed but saw a recommendation
async function init() {
    const services = await getResource("api/services");
    const salons = await getResource("api/salons");

    populateCities(cityDropDown, Cities);
    populateServices(servicesDropdown, services);
    populateSalons(salonsContainer, salons);
}

init();