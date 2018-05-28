import { authHeader, config } from '../_helpers';

export const brandService = {
    addBrand
};

function addBrand(brand) {
    const requestOptions = {
        method: 'POST',
        headers: { ...authHeader(), 'Content-Type': 'application/json' },
        body: JSON.stringify(brand)
    };

    return fetch(config.apiUrl + '/Brands', requestOptions).then(handleResponse, handleError);
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