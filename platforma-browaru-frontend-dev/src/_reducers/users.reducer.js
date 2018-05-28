import { userConstants } from '../_constants';

export function users(state = {}, action) {
  switch (action.type) {

      case userConstants.UPDATE_EMAIL:
        console.log("user.reducer");
        return {
            ...state,
            newEmail: action.user.newEmail,
            confirmNewEmail: action.user.confirmNewEmail,
            password: action.user.password
        };
      case userConstants.UPDATE_PASSWORD:
          console.log("user.reducer");
        return {
            ...state,
            password: action.user.password,
            newPassword: action.user.newPassword,
            confirmNewPassword: action.user.confirmNewPassword
        };
      case userConstants.GET_USER:
          return {
              ...state,
              firstName: action.users.firstName,
              lastName:action.users.lastName,
              username: action.users.username,
              email: action.users.email,
              description: action.users.description,
              status: action.users.status,
              gender: action.users.gender,
              createdAt: action.users.createdAt,
              dateOfBirth: action.users.dateOfBirth
          };
      case userConstants.UPDATE_USER:
          return {
              ...state,
              firstName: action.users.firstName,
              lastName:action.users.lastName,
              username: action.users.username

          };
    case userConstants.GETALL_REQUEST:
      return {
        loading: true
      };
    case userConstants.GETALL_SUCCESS:
      return {
        items: action.users
      };
    case userConstants.GETALL_FAILURE:
      return {
        error: action.error
      };
    case userConstants.DELETE_REQUEST:
      // add 'deleting:true' property to user being deleted
      return {
        ...state,
        items: state.items.map(user =>
          user.id === action.id
            ? { ...user, deleting: true }
            : user
        )
      };
    case userConstants.DELETE_SUCCESS:
      // remove deleted user from state
      return {
        items: {}
      };
    case userConstants.DELETE_FAILURE:
      // remove 'deleting:true' property and add 'deleteError:[error]' property to user
      return {
        ...state,
        items: state.items.map(user => {
          if (user.id === action.id) {
            // make copy of user without 'deleting:true' property
            const { deleting, ...userCopy } = user;
            // return copy of user with 'deleteError:[error]' property
            return { ...userCopy, deleteError: action.error };
          }

          return user;
        })
      };
    default:
      return state
  }
}