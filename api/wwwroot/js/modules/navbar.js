import { getCurrentUserFromToken, logout } from "./utils.js";

const navbar = document.getElementById("navbar");

navbar?.addEventListener("click", event => {
    if ((event.target)?.id === "logoutBtn") {
        event.preventDefault();
        logout();
        renderNavbar();
    }
});

async function renderNavbar() {
    if (!navbar) return;

    const user = getCurrentUserFromToken();

    let rightItems = "";

    if (user && user.role === "Admin") {
        rightItems = `
            <li class="nav-item"><a class="nav-link" href="admin-panel.html">Admin Panel</a></li>
            <li class="nav-item"><a class="nav-link" href="#" id="logoutBtn">Logout</a></li>
        `;
    } else if (user) {
        rightItems = `
            <li class="nav-item"><a class="nav-link" href="#" id="logoutBtn">Logout</a></li>
        `;
    } else {
        rightItems = `
            <li class="nav-item"><a class="nav-link" href="register.html">Register</a></li>
            <li class="nav-item"><a class="nav-link" href="login.html">Login</a></li>
        `;
    }

    const html = `
    <nav class="navbar navbar-expand-lg navbar-light bg-body-tertiary border-bottom">
      <div class="container">
        <a class="navbar-brand" href="index.html">Salon portal</a>
        <button class="navbar-toggler" type="button" data-bs-toggle="collapse" data-bs-target="#mainNavbar" aria-controls="mainNavbar" aria-expanded="false" aria-label="Toggle navigation">
          <span class="navbar-toggler-icon"></span>
        </button>
        <div class="collapse navbar-collapse" id="mainNavbar">
          <ul class="navbar-nav ms-auto mb-2 mb-lg-0">
            <li class="nav-item"><a class="nav-link" href="index.html">Home</a></li>
            ${rightItems}
          </ul>
        </div>
      </div>
    </nav>`;

    navbar.innerHTML = html;
}

renderNavbar();