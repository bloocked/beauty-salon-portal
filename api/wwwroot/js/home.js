const cityDropDown = document.getElementById("cities");

const Cities = {
    Vilnius: "Vilnius",
    Kaunas: "Kaunas"
};

Object.values(Cities).forEach(city => {
    const option = document.createElement("option");
    option.value = city;
    option.textContent = city;

    cityDropDown.appendChild(option);
});
