import { userConstants } from '../_constants';
import { brandService } from '../_services';
import { alertActions } from './';
import { history } from '../_helpers';
import {brandConstants} from "../_constants/brand.constants";

export const brandActions = {
    addBrand
};

function addBrand(brand) {
    return dispatch => {
        dispatch(request(brand));

        brandService.addBrand(brand)
            .then(
                () => {
                    dispatch(success());
                    dispatch(alertActions.success('Addition successful'));
                },
                error => {
                    dispatch(failure(error));
                    dispatch(alertActions.error(error));
                }

            );

    };


    function request(brand) { return { type: brandConstants.ADD_BRAND_REQUEST, brand } }
    function success(brand) { return { type: brandConstants.ADD_BRAND_SUCCESS, brand } }
    function failure(error) { return { type: brandConstants.ADD_BRAND_ERROR, error } }

}

