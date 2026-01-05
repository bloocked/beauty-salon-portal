const registerForm = document.getElementById("registerForm");

registerForm.addEventListener("submit", event => {
    event.preventDefault();
    register();
})

async function register() {
    const formData = new FormData(registerForm);
    const user = Object.fromEntries(formData);
    console.log("User object to send:", user);

    try {
        const response = await fetch("/api/users", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(user)
        });

        if (response.ok) {
            window.alert("Registration success, please log in");
            window.location.href = "login.html";
        }
        else {
            const error = await response.text();
            window.alert(error);
        }
    } catch (e) {
        console.error(e);
    }
}

