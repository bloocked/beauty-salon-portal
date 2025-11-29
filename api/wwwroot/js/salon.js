import { getResource } from "./modules/utils.js";
import { populateServices } from "./modules/dropdown.js";

const servicesDropdown = document.getElementById("services");
const specialistsContainer = document.getElementById("specialistsContainer");
const searchForm = document.getElementById("searchForm");


searchForm.addEventListener("submit", event => {
    event.preventDefault();
    searchSpecialists();
})

async function searchSpecialists() {
    const service = servicesDropdown.value;

    const query = new URLSearchParams();

    if (service) query.append("service", service);

    // call getResource with params
    const specialists = await getResource(`api/specialists?${query.toString()}`);

    if (specialists && specialists.length > 0) {
        populateSpecialists(salonsContainer, specialists);
    }
}

function populateSpecialists(container, array) {
    container.innerHTML = "";
    array.forEach(item => {
        const card = document.createElement("div");
        const name = document.createElement("h3");
        const service = document.createElement("h4");

        card.className = "card";
        specialistsContainer.appendChild(card);
        name.innerHTML = item.name;
        card.appendChild(name);
        service.innerHTML = `${item.services[0].name}, ${item.services[0].cost}`;
        card.appendChild(service);
    });
}

async function init() {
    const services = await getResource("api/services");
    const specialists = await getResource("api/specialists");

    populateServices(servicesDropdown, services);
    populateSpecialists(specialistsContainer, specialists);
}

init();