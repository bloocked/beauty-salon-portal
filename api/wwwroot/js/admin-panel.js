import { getCurrentUserFromToken, getResource } from "./modules/utils.js";

const user = getCurrentUserFromToken();

// prevent unauthorized access
if (!user || user.role !== "Admin") {
    console.log(user);
    window.alert("Access denied. Admins only.");
    window.location.href = "index.html";
}

//mock for now
const usersContainer = document.getElementById("usersContainer");
const users = await getResource("/api/users", localStorage.getItem("jwt"));

function populateUsers(container, array) {
    let html = `
        <table>
            <thead>
                <tr>
                    <th>ID</th>
                    <th>Name</th>
                    <th>Email</th>
                </tr>
            </thead>
            <tbody>
    `;
    array.forEach(user => {
        html += `
            <tr>
                <td>${user.id}</td>
                <td>${user.username}</td>
                <td>${user.email}</td>
            </tr>
        `;
    });
    html += `
            </tbody>
        </table>
    `;
    container.innerHTML = html;
}

populateUsers(usersContainer, users);