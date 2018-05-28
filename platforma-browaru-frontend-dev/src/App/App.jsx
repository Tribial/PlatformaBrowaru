import React from 'react';
import { Router, Route } from 'react-router-dom';
import { connect } from 'react-redux';

import { history } from '../_helpers';
import { alertActions } from '../_actions';
import { PrivateRoute } from '../_components';
import { HomePage } from '../Components/HomePage';
import { LoginPage } from '../Components/LoginPage';
import { RegisterPage } from '../Components/RegisterPage';
import { Header }from "../Components/Common";
import {UserProfile, UpdateProfile, UpdateEmail, UpdatePassword, DeleteUser, DeletedUserInfo} from "../Components/UserProfilePage";
import {Brand, AddBrand, BrandDetails} from "../Components/Brand";

class App extends React.Component {
    constructor(props) {
        super(props);

        const { dispatch } = this.props;
        history.listen((location, action) => {
            // clear alert on location change
            dispatch(alertActions.clear());
        });
    }

    render() {
        const { alert } = this.props;
        return (
            <div className="container-fluid">
                <Header />
                <div className="row">
                    <div className="col-sm-6 col-sm-offset-4 ">
                        {alert.message &&
                        <div className={`alert ${alert.type}`}>{alert.message}</div>
                        }
                    </div>
                        <Router history={history}>
                            <div className="row">
                                <PrivateRoute exact path="/" component={HomePage} />
                                <Route path="/login" component={LoginPage} />
                                <Route path="/register" component={RegisterPage} />
                                <PrivateRoute path="/user" component={UserProfile}/>
                                <PrivateRoute path="/user/updateProfile" component={UpdateProfile}/>
                                <PrivateRoute path="/user/updatePassword" component={UpdatePassword}/>
                                <PrivateRoute path="/user/updateEmail" component={UpdateEmail}/>
                                <PrivateRoute path="/user/deleteUser" component={DeleteUser}/>
                                <PrivateRoute path="/user/deletedUser" component={DeletedUserInfo}/>
                                <PrivateRoute path="/brand" component={Brand}/>
                                <PrivateRoute path="/addBrand" component={AddBrand}/>
                                <PrivateRoute path="/brandDetails" component={BrandDetails}/>
                            </div>
                        </Router>

                </div>
            </div>
        );
    }
}

function mapStateToProps(state) {
    const { alert } = state;
    return {
        alert
    };
}

const connectedApp = connect(mapStateToProps)(App);
export { connectedApp as App }; 