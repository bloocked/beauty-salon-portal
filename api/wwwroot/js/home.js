import { getResource, getUserFromValidToken } from "./modules/utils.js";
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
        const body = document.createElement("div");
        const name = document.createElement("h5");
        const fullAddress = document.createElement("p");

        card.dataset.id = salon.id;

        card.className = "card h-100 col";
        salonsContainer.appendChild(card);

        body.className = "card-body";
        card.appendChild(body);

        name.className = "card-title mb-2";
        name.textContent = salon.name;
        body.appendChild(name);

        fullAddress.className = "card-text text-body-secondary mb-0";
        fullAddress.textContent = `${salon.address}, ${salon.city}`;
        body.appendChild(fullAddress);
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
    getUserFromValidToken(); // prevent lingering trash tokens
    const services = await getResource("api/services");
    const salons = await getResource("api/salons");

    populateCities(cityDropDown, Cities);
    populateServices(servicesDropdown, services);
    populateSalons(salonsContainer, salons);
}

init();