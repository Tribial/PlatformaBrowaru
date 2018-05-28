import { authHeader, config } from '../_helpers';

export const userService = {
    updateEmail,
    updatePassword,
    updateUser,
    login,
    logout,
    register,
    getAll,
    getById,
    update,
    deleteUser
};

function updateEmail(user) {
    const requestOptions = {
        method: 'PATCH',
        headers: {...authHeader(), 'Content-Type': 'application/json'},
        body: JSON.stringify(user)
    };
    return fetch(config.apiUrl+'/Users/ChangeEmail',requestOptions)
        .then(handleResponse,handleError);
}

function updatePassword(user) {
    const requestOptions = {
        method: 'PATCH',
        headers: {...authHeader(), 'Content-Type': 'application/json'},
        body: JSON.stringify(user)
    };
    return fetch(config.apiUrl+'/Users/ChangePassword',requestOptions)
        .then(handleResponse,handleError);
}

function updateUser(user, id){
    const requestOptions = {
        method: 'PATCH',
        headers: { ...authHeader(),'Content-Type': 'application/json'},
        body: JSON.stringify(user)
    };
    return fetch(config.apiUrl+'/Users/'+id, requestOptions)
        .then(handleResponse, handleError);
}

function login(email, password) {

    const requestOptions = {
        method: 'POST',
        headers: { 'Content-Type': 'application/json'},
        body: JSON.stringify({ email, password })
    };

    return fetch(config.apiUrl + '/Users/Login', requestOptions)
        .then(handleResponse, handleError)
        .then(user => {
            // login successful if there's a jwt token in the response
            if (user && user.object) {
                // store user details and jwt token in local storage to keep user logged in between page refreshes
                localStorage.setItem('user', JSON.stringify(user.object));
            }

            return user;
        });
}

function logout() {
    // remove user from local storage to log user out
    localStorage.removeItem('user');
}

function getAll() {
    const requestOptions = {
        method: 'GET',
        headers: authHeader()
    };

    return fetch(config.apiUrl + '/Users', requestOptions).then(handleResponse, handleError);
}

function getById(id) {
    const requestOptions = {
        method: 'GET',
        headers: { ...authHeader(),'Content-Type': 'application/json'},
    };
    return fetch(config.apiUrl + '/Users/' + id, requestOptions).then(handleResponse, handleError);
}

function register(user) {
    const requestOptions = {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(user)
    };

    return fetch(config.apiUrl + '/Users/Register', requestOptions).then(handleResponse, handleError);
}

function update(user) {
    const requestOptions = {
        method: 'PUT',
        headers: { ...authHeader(), 'Content-Type': 'application/json' },
        body: JSON.stringify(user)
    };

    return fetch(config.apiUrl + '/users/' + user.id, requestOptions).then(handleResponse, handleError);
}

// prefixed function name with underscore because delete is a reserved word in javascript
function deleteUser(password) {
    const requestOptions = {
        method: 'DELETE',
        headers: { ...authHeader(), 'Content-Type': 'application/json' },
        body: JSON.stringify(password)
    };
    console.log("user serwis 116", requestOptions);
    return fetch(config.apiUrl + '/Users/Delete', requestOptions).then(handleResponse, handleError);
}

function handleResponse(response) {
    return new Promise((resolve, reject) => {
        if (response.ok) {
            // return json if it was returned in the response
            var contentType = response.headers.get("content-type");
            if (contentType && contentType.includes("application/json")) {
                response.json().then(json => resolve(json));
            } else {
                resolve();
            }
        } else {
            response.json().then(error=>reject(error.errors));
        }
    });
}

function handleError(errors) {
    return Promise.reject(errors && errors.message);
}