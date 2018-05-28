import { brandConstants } from "../_constants/";
export function brand(state = {}, action) {
    switch (action.type) {

        case brandConstants.ADD_BRAND_REQUEST:
            return { adding: true };
        case brandConstants.ADD_BRAND_SUCCESS:
            return {};
        case brandConstants.ADD_BRAND_ERROR:
            return {};

        default:
            return state
    }
}