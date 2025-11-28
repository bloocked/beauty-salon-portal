const registerForm = document.getElementById("registerForm");


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

        if (response.ok) window.alert("Registration success");
        else {
            const error = await response.text();
            window.alert(error);
        }
    } catch (e) {
        console.error(e);
    }
}

registerForm.addEventListener("submit", (event) => {
    event.preventDefault();
    register();
})