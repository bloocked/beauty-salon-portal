//refactor, look into adding helpers like an apiGet with error handling
export async function getResource(endpoint, BearerToken = null) {
    try {
    const response = await fetch(endpoint, {
        headers: { "Authorization": BearerToken ? `Bearer ${BearerToken}` : "" }
    });

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

export function getUserFromValidToken() {
    const token = localStorage.getItem("jwt");
    if (!token) return null;

    const decoded = jwt_decode(token);
    const currentTime = Date.now() / 1000;

    if (decoded.exp < currentTime) {
        localStorage.removeItem("jwt");
        logout();
        return null;
    }
    return decoded;
}

export function logout() {
    localStorage.removeItem("jwt");
    window.location.href = "index.html";
}