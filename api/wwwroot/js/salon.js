import { getResource } from "./modules/utils.js";
import { populateServices } from "./modules/dropdown.js";

const servicesDropdown = document.getElementById("services");
const specialistsContainer = document.getElementById("specialistsContainer");
const searchForm = document.getElementById("searchForm");


searchForm.addEventListener("submit", event => {
    event.preventDefault();
    searchSpecialists();
});

specialistsContainer.addEventListener("click", event => { //event delegation is based
    event.preventDefault();
    const card = event.target.closest(".card");

    if (!card) return;

    const query = new URLSearchParams();
    query.append("specialistId", card.dataset.specialistId);
    query.append("specialistServiceId", card.dataset.serviceId);
    
    window.location.href = `specialist.html?${query.toString()}`;
});

async function searchSpecialists() {
    const service = servicesDropdown.value;

    const query = new URLSearchParams(window.location.search);

    if (service) query.append("service", service);

    // call getResource with params
    const specialists = await getResource(`api/specialists?${query.toString()}`);

    if (specialists && specialists.length > 0) {
        populateSpecialists(specialistsContainer, specialists);
    }
    else {
        specialistsContainer.innerHTML = "<p>No specialists found.</p>";
    }
}

// SEARCH AND POPULATE NEED LOOKING INTO, FLOW DOESNT MAKE SENSE ATM

function populateSpecialists(container, array) {
    container.innerHTML = "";

    const selected = servicesDropdown.value;

    array.forEach(specialist => {
        const matchedService = specialist.services.find(s => s.name == selected);

        if (matchedService) {
            const card = document.createElement("div");
            const body = document.createElement("div");
            const name = document.createElement("h5");
            const service = document.createElement("p");

            card.dataset.specialistId = specialist.id;
            card.dataset.serviceId = matchedService.id;

            card.className = "card h-100 col";
            specialistsContainer.appendChild(card);

            body.className = "card-body";
            card.appendChild(body);

            name.className = "card-title mb-2";
            name.textContent = specialist.name;
            body.appendChild(name);

            service.className = "card-text text-body-secondary mb-0";
            service.textContent = `${matchedService.name}, ${matchedService.cost}`;
            body.appendChild(service);
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