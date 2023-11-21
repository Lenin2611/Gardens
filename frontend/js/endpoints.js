let update = {
    id: 2,
    nombre: "Colombia"
};

async function post(json, endpoint) {
    const url = `http://localhost:5280/api${endpoint}`;
    const options = {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify(json)
    };
    try {
        const response = await fetch(url, options);
        const result = await response.json();
        return result;
    } catch (error) {
        console.error(error);
        return null;
    }
}

async function put(json, endpoint, id) {
    const url = `http://localhost:5280/api${endpoint}/${id}`;
    const options = {
        method: 'PUT',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify(json)
    };
    try {
        const response = await fetch(url, options);
        const result = await response.json();
        return result;
    } catch (error) {
        console.error(error);
        return null;
    }
}

let get = async (endpoint) => {
    const url = `http://localhost:5280/api${endpoint}`;
    const options = {
        method: 'GET',
        headers: {}
    };

    try {
        const response = await fetch(url, options);
        const result = await response.json();
        return (result);
    } catch (error) {
        return (error);
    }
}

let remove = async (endpoint, id) => {
    const url = `http://localhost:5280/api${endpoint}/${id}`;
    const options = {
        method: 'DELETE',
        headers: {}
    };

    try {
        const response = await fetch(url, options);
        const result = await response.json();
        return (result);
    } catch (error) {
        return (error);
    }
}

async function performEndpoint(arg1 = null, arg2 = null, arg3 = null) {
    const result = await get(arg1);
    console.log(result);
    return (result);
}

// Call performLogin somewhere in your code where you want the login process to happen

