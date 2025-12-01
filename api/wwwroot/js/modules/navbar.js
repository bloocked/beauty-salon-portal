import { getCurrentUserFromToken, logout } from "./utils.js";

const navbar = document.getElementById("navbar");


navbar.addEventListener("click", event => {
    if (event.target.id === "logoutBtn") {
        event.preventDefault();
        logout();
        renderNavbar();
    }
});

async function renderNavbar() {
    if (!navbar) return;

    const user = getCurrentUserFromToken();

    let html = `
    <nav>
      <ul>
        <li><a href="index.html">Home</a></li>
    `;

    if (user && user.role === "Admin") {
        html += `
                <li><a href="admin.html">Admin Panel</a></li>
            </ul>
        </nav>
        `;
    }
    else if (user) {
        html += `
                <li><a href="#" id="logoutBtn">Logout</a></li>
            </ul>
        </nav>
        `;
    }
    else {
        html += `
                <li><a href="register.html">Register</a></li>
                <li><a href="login.html">Login</a></li>
            </ul>
        </nav>
        `;
    }


    navbar.innerHTML = html;
}



 renderNavbar();