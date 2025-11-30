const schedule = document.getElementById("scheduleContainer");
const selectedDate = document.getElementById("calendarDate");

schedule.addEventListener("click", async event => {
    const slot = event.target.closest(".slotBtn");

    if (!slot) return;
    const queryString = new URLSearchParams(window.location.search);

    const specialistId = queryString.get("specialistId");
    const specialistServiceId = queryString.get("specialistServiceId");
    const slotDate = slot.dataset.date;

    const reservation = { 
        specialistId: Number(specialistId),
        specialistServiceId: Number(specialistServiceId),
        clientId: 2, // PLACEHOLDER, WILL USE JWT DATA LATER
        startTime: formatLocalDateTime(new Date(slotDate)) // should fix incorrect post dates
    }

    try {
        const response = await fetch("/api/reservations", {
        method: "POST",
        headers: { "Content-Type": "application/json" }, // WRAP ALL OF THIS IN A DTO LATER
        body: JSON.stringify(reservation)
        })

        if (response.ok) window.alert("Reservation success!")
    } catch (e) {
        console.error(e);
    }
})

function SetDefaultDate() {
    selectedDate.valueAsDate = new Date();
}

function PopulateSlots(array = []) {
    schedule.innerHTML = "";

    const minuteInterval = 15;
    const dateToDisplay = new Date(selectedDate.value);
    dateToDisplay.setHours(0, 0, 0, 0); // mark the start

    const dayEnd = new Date(dateToDisplay);
    dayEnd.setHours(24, 0, 0, 0);

    let curr = new Date(dateToDisplay);

    while (curr < dayEnd) {
        const slotButton = document.createElement("div");
        slotButton.className = "slotBtn";

        const hours = curr.getHours();
        const minutes = curr.getMinutes();

        slotButton.textContent = `${hours}:${minutes}`

        slotButton.dataset.date = curr.toISOString();

        curr.setMinutes(curr.getMinutes() + minuteInterval);

        schedule.appendChild(slotButton);
    }

    array.forEach(slot => {
        // block out occupied slots
    });
}
 //check this for bugs
function formatLocalDateTime(date) {
    const yyyy = date.getFullYear();
    const mm = String(date.getMonth() + 1).padStart(2, '0');
    const dd = String(date.getDate()).padStart(2, '0');
    const hh = String(date.getHours()).padStart(2, '0');
    const min = String(date.getMinutes()).padStart(2, '0');

    return `${yyyy}-${mm}-${dd}T${hh}:${min}:00`;
}


SetDefaultDate();
PopulateSlots();