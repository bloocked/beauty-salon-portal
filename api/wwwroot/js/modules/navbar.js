async function loadNavbar() {
    const placeholder = document.getElementById("navbar");

    if (!placeholder) return;

    try {
        const response = await fetch ("navbar.html");
        const navbarHtml = await response.text();
        placeholder.innerHTML = navbarHtml;
    } catch (e) {
        console.error(e);
    }
}

loadNavbar();