import { userConstants } from '../_constants';
import { userService } from '../_services';
import { alertActions } from './';
import { history } from '../_helpers';

export const userActions = {
    updateEmail,
    login,
    updateUser,
    updatePassword,
    logout,
    register,
    getAll,
    deleteUser
};

function updateEmail(user) {
    return function (dispatch) {
        return userService.updateEmail(user)
            .then(
                () => {
                    dispatch(updateEmail(user));
                }
            );
    };

    function updateEmail(user) { return { type: userConstants.UPDATE_EMAIL, user: user }}
}

function updatePassword(user) {
    return function (dispatch) {
        return userService.updatePassword(user)
            .then(
                () => {
                    dispatch(updatePassword(user));
                }
            );
    };

    function updatePassword(user) {return {type: userConstants.UPDATE_PASSWORD, user: user}}
}


function updateUser(users, id){
    //return userService.updateUser(user,id);
    /*id = 10004;
    return dispatch =>{
        userService.updateUser(users, id)
            .then(
                (users, id) =>{
                dispatch(updateUser(users,id));
                }
                );
    };*/

    return function (dispatch) {
      return userService.updateUser(users, id)
          .then(
              () => {
                  dispatch(updateUser(users, id));
              }
          );

    };

    function updateUser(users,id) {return {type: userConstants.UPDATE_USER, users: users, id: id } }

    /*
    * case userConstants.UPDATE_USER:
            return {
                ...state,
                user: {
                    ...state.user,
                    first_name: action.updateUser.first_name,
                    last_name:action.updateUser.last_name,
                    username: action.updateUser.username
                }
            };
    * */
}
function login(email, password) {
    return dispatch => {
        dispatch(request({ email: email }));
        userService.login(email, password)
            .then(
                user => {
                    dispatch(success(user));
                    history.push('/');
                    userService.getById(user.object.id)
                        .then(
                            response =>  {dispatch(getUserById(response.object, user.object.id))}
                        );
                },
                error => {
                    dispatch(failure(error));
                    dispatch(alertActions.error(error));
                }
            );
    };

    function getUserById(users,id) {return {type: userConstants.GET_USER, users: users, id: id } }
    function request(user) { return { type: userConstants.LOGIN_REQUEST, user } }
    function success(user) { return { type: userConstants.LOGIN_SUCCESS, user } }
    function failure(error) { return { type: userConstants.LOGIN_FAILURE, error } }
}

function logout() {
    userService.logout();
    return { type: userConstants.LOGOUT };
}

function register(user) {
    return dispatch => {
        dispatch(request(user));

        userService.register(user)
            .then(
                () => {
                    dispatch(success());
                    history.push('/login');
                    dispatch(alertActions.success('Registration successful'));
                },
                error => {
                    dispatch(failure(error));
                    dispatch(alertActions.error(error));
                }
            );
    };

    function request(user) { return { type: userConstants.REGISTER_REQUEST, user } }
    function success(user) { return { type: userConstants.REGISTER_SUCCESS, user } }
    function failure(error) { return { type: userConstants.REGISTER_FAILURE, error } }
}

function getAll() {
    return dispatch => {
        dispatch(request());

        userService.getAll()
            .then(
                users => dispatch(success(users)),
                error => dispatch(failure(error))
            );
    };

    function request() { return { type: userConstants.GETALL_REQUEST } }
    function success(users) { return { type: userConstants.GETALL_SUCCESS, users } }
    function failure(error) { return { type: userConstants.GETALL_FAILURE, error } }
}

// prefixed function name with underscore because delete is a reserved word in javascript
function deleteUser(password) {
    return dispatch => {
        userService.deleteUser(password).then(
            () => {
                dispatch(success(password));
                history.push("/user/deletedUser");
            }
        );

    };
    function success(password) { return { type: userConstants.DELETE_SUCCESS, password  } }
        /*dispatch(request(password));
    userService.deleteUser(password).then(

        history.push("/user/deletedUser"),
        history.push("/login")*/
    //)
    /*return dispatch => {
        dispatch(request(password));

        userService.delete(password)
            /*.then(
                () => {
                    dispatch(success(password));
                },
                error => {
                    dispatch(failure(password, error));
                }
            );
    };

    function request(password) { return { type: userConstants.DELETE_REQUEST, password } }
    function success(password) { return { type: userConstants.DELETE_SUCCESS, password  } }
    function failure(password, error) { return { type: userConstants.DELETE_FAILURE, password , error } }*/
}