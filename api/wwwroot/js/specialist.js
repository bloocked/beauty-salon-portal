import { getResource, getUserFromValidToken } from "./modules/utils.js";

const schedule = document.getElementById("scheduleContainer");
const selectedDate = document.getElementById("calendarDate");
const title = document.getElementById("title");

const queryString = new URLSearchParams(window.location.search);
const specialistId = queryString.get("specialistId");
const specialistServiceId = queryString.get("specialistServiceId");

const slots = [];
let occupiedIntervals = [];

schedule.addEventListener("click", async event => {
    const currentUser = getUserFromValidToken();
    
    const slot = event.target.closest(".slotBtn");

    if (!slot) return;

    const slotDate = slot.dataset.date;
    if (currentUser === null) {
        window.alert("Please log in to make a reservation.");
        return;
    }

    const reservation = { 
        specialistId: Number(specialistId),
        specialistServiceId: Number(specialistServiceId),
        clientId: Number(currentUser.nameid),        // PLACEHOLDER, WILL USE JWT DATA LATER
        startTime: formatLocalDateTime(new Date(slotDate))  // should fix incorrect post dates
    };

    console.log("Reservation object to send:", reservation);

    try {
        const response = await fetch("/api/reservations", {
        method: "POST",
        headers: { 
            "Content-Type": "application/json",
            "Authorization": `Bearer ${localStorage.getItem("jwt")}`
         },
        body: JSON.stringify(reservation)
        })

        console.log(response);
        if (response.ok) {
            window.alert("Reservation success!");
            occupiedIntervals = await getOccupiedSlots();
            disableOccupied(slots, occupiedIntervals);
        } else {
            const error = await response.text();
            window.alert(error);
        }
    } catch (e) {
        console.error(e);
    }
});

selectedDate.addEventListener("change", async event => {
    slots.length = 0;
    occupiedIntervals = await getOccupiedSlots();
    populateSlots(slots, occupiedIntervals);
});

function setDefaultDate() {
    selectedDate.valueAsDate = new Date();
}

function populateSlots(slots, occupiedIntervals) {
    schedule.innerHTML = "";
    slots.length = 0;

    const minuteInterval = 15;
    const dateToDisplay = new Date(selectedDate.value);
    dateToDisplay.setHours(0, 0, 0, 0); // start of the workday

    const dayEnd = new Date(dateToDisplay);
    dayEnd.setHours(24, 0, 0, 0);       // end of the workday (look at enforcing specific times in backend)

    let curr = new Date(dateToDisplay);

    while (curr < dayEnd) {
        const slot = document.createElement("button");
        slot.type = "button";
        slot.className = "slotBtn btn btn-outline-secondary col py-2";

        const hours = curr.getHours().toString().padStart(2, "0");
        const minutes = curr.getMinutes().toString().padStart(2, "0");
        slot.textContent = `${hours}:${minutes}`;
        schedule.appendChild(slot);

        slot.dataset.date = curr.toISOString();
        slots.push({ element: slot, date: new Date(curr) });

        curr.setMinutes(curr.getMinutes() + minuteInterval);
    }

    disableOccupied(slots, occupiedIntervals);
}

function disableOccupied(slots, occupiedIntervals) {
    slots.forEach(slot => {
        const date = slot.date;
        const element = slot.element;

        const disabled = occupiedIntervals.some(interval => {
            const intervalStart = new Date(interval.startTime);
            const intervalEnd = new Date(interval.endTime);

            return date >= intervalStart && date < intervalEnd;
        });

        if (disabled) {
            element.classList.remove("btn-outline-secondary");
            element.classList.add("btn-secondary");
            element.setAttribute("disabled", "disabled");
        } else {
            element.classList.add("btn-outline-secondary");
            element.classList.remove("btn-secondary");
            element.removeAttribute("disabled");
        }
    });
}

async function setTitle() {
    const specialist = await getResource(`api/specialists/${specialistId}`);
    console.log(`Specialis:  ${specialist}`);
    title.innerText = `${specialist.name}'s schedule:`
}

async function getOccupiedSlots() {
    return await getResource(
        `api/specialists/${specialistId}/occupied-slots?date=${selectedDate.value}`
    );
}

 // used for converting from UTC to local time, fixes time misalignment
 // getMonth() etc. takes UTC and turns into local time
 // possibly a thing to look at later
function formatLocalDateTime(date) {
    const yyyy = date.getFullYear();
    const mm = String(date.getMonth() + 1).padStart(2, '0');
    const dd = String(date.getDate()).padStart(2, '0');
    const hh = String(date.getHours()).padStart(2, '0');
    const min = String(date.getMinutes()).padStart(2, '0');

    return `${yyyy}-${mm}-${dd}T${hh}:${min}:00`;
}


async function init() {
    await setTitle();
    setDefaultDate();

    occupiedIntervals = await getOccupiedSlots();

    populateSlots(slots, occupiedIntervals);
}

init();