import { getResource } from "./modules/utils.js";
import { populateServices } from "./modules/dropdown.js";

const servicesDropdown = document.getElementById("services");
const specialistsContainer = document.getElementById("specialistsContainer");
const searchForm = document.getElementById("searchForm");


searchForm.addEventListener("submit", event => {
    event.preventDefault();
    searchSpecialists();
})

specialistsContainer.addEventListener("click", event => { //event delegation is based
    event.preventDefault();
    const card = event.target.closest(".card");

    if (!card) return;

    const query = new URLSearchParams();
    query.append("specialistId", card.dataset.specialistId);
    query.append("specialistServiceId", card.dataset.serviceId);
    
    window.location.href = `specialist.html?${query.toString()}`;
})

async function searchSpecialists() {
    const service = servicesDropdown.value;

    const query = new URLSearchParams(window.location.search);

    if (service) query.append("service", service);

    // call getResource with params
    const specialists = await getResource(`api/specialists?${query.toString()}`);

    if (specialists && specialists.length > 0) {
        populateSpecialists(specialistsContainer, specialists);
    }
}

function populateSpecialists(container, array) {
    container.innerHTML = "";
    array.forEach(specialist => {

        const selected = servicesDropdown.value;
        const matchedService = specialist.services.find(s => s.name == selected);

        if (matchedService) {

            const card = document.createElement("div");
            const name = document.createElement("h3");
            const service = document.createElement("h4");

            card.dataset.specialistId = specialist.userId;
            card.dataset.serviceId = matchedService.id;

            card.className = "card";
            specialistsContainer.appendChild(card);
            name.innerHTML = specialist.name;
            card.appendChild(name);

            card.className = "card";
            specialistsContainer.appendChild(card);
            name.innerHTML = specialist.name;
            card.appendChild(name);
            service.innerHTML = `${matchedService.name}, ${matchedService.cost}`;

            card.appendChild(service);
        }
    });
}

async function init() {
    const services = await getResource("api/services");
    const specialists = await getResource("api/specialists");

    populateServices(servicesDropdown, services);
    populateSpecialists(specialistsContainer, specialists);
}

init();