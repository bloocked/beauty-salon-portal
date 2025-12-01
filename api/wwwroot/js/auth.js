const loginForm = document.getElementById("loginForm");

if (localStorage.getItem("jwt")) {
    window.location.href = "index.html";
}

loginForm.addEventListener("submit", event => {
    event.preventDefault();
    auth();
})

async function auth() {
    const formData = new FormData(loginForm);
    const user = Object.fromEntries(formData);

    try {
        const response = await fetch("/api/auth/login", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(user)
        });

        const data = await response.json();

        if (response.ok) {
            localStorage.setItem("jwt", data.token);

            window.alert("Login success");
            window.location.href = "index.html";
        }

        else console.log(data); // maybe make it cleaner later, ugly oneline
    } catch (e) {
        console.error(e);
    }

}