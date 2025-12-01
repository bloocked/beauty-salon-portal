//refactor, look into adding helpers like an apiGet with error handling
export async function getResource(endpoint, urlParams = "") {
    try {
    const response = await fetch(endpoint);

    if(!response.ok) {
        const error = await response.text();
        window.alert(error);
        return [];
    }
    
    const resource = await response.json();
    console.log(resource); //ommit after debugging

    return resource;

    } catch (e) {
        console.error(e);
        return [];
    }
}

export function getCurrentUserFromToken() {
    const token = localStorage.getItem("jwt");
    if (!token) return null;

    return jwt_decode(token);
}

export function logout() {
    localStorage.removeItem("jwt");
    window.location.href = "#";
}