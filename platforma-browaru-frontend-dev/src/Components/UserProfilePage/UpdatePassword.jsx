import React from 'react';
import {connect} from 'react-redux';
import {userActions} from "../../_actions";

import {bindActionCreators} from "redux";

class UpdatePassword extends React.Component {
    constructor(props, context) {
        super(props, context);
        this.state = {
            user: {
                password: '',
                newPassword: '',
                confirmNewPassword: '',
            },
            submitted: false,
            UserId: 10004,


        };

        this.handleChange = this.handleChange.bind(this);
        this.handleSubmit = this.handleSubmit.bind(this);
    }

    handleChange(event) {
        const {name, value} = event.target;
        const {user} = this.state;
        this.setState({
            user: {
                ...user,
                [name]: value
            }
        });
    }

    handleSubmit(event) {
        event.preventDefault();
        this.setState({submitted: true});
        const {user} = this.state;
        console.log("submit_UpdatePassword_user", user);
        console.log("submit_UpdatePassword_id", this.state.id);
        if (user.newPassword === user.confirmNewPassword && user.password && user.newPassword && user.confirmNewPassword) {
            this.props.actions.updatePassword(user);
        }
        else console.log("hasla nie pasuja l 47");
    }

    render() {
        const {user, submitted} = this.state;

        return (
            <div className="col-md-8" style={{padding: '0px 100px 100px 0px'}}>
                <h1>Zmień haslo</h1>
                <form name="form" onSubmit={this.handleSubmit}>
                    <div className={'form-group' + (submitted && !user.password ? ' has-error' : '')}>
                        <label htmlFor="password">Hasło</label>
                        <input type="password" className="form-control" name="password" value={user.password}
                               onChange={this.handleChange}/>
                        {submitted && !user.password &&
                        <div className="help-block">Password is required</div>
                        }
                    </div>
                    <div className={'form-group' + (submitted && !user.newPassword ? ' has-error' : '')}>
                        <label htmlFor="newPassword">Nowe hasło</label>
                        <input type="password" className="form-control" name="newPassword" value={user.newPassword}
                               onChange={this.handleChange}/>
                        {submitted && !user.newPassword &&
                        <div className="help-block">New password is required</div>
                        }
                    </div>
                    <div className={'form-group' + (submitted && !user.confirmNewPassword ? ' has-error' : '')}>
                        <label htmlFor="newPassword">Potwierdz nowe hasło</label>
                        <input type="password" className="form-control" name="confirmNewPassword"
                               value={user.confirmNewPassword}
                               onChange={this.handleChange}/>
                        {submitted && !user.confirmNewPassword &&
                        <div className="help-block">Confirming new password is required</div>
                        }
                    </div>

                    <div className="form-group">
                        <button className="btn btn-primary">Save</button>
                    </div>
                </form>

            </div>

        );
    }
}

function mapStateToProps(state) {
    const {authentication} = state;
    const {user} = authentication;
    return {
        user,
        users: state.users
    };
}

function mapDispatchToProps(dispatch) {
    return {
        actions: bindActionCreators(userActions, dispatch)
    };
}

const connectedUpdatePassword = connect(mapStateToProps, mapDispatchToProps)(UpdatePassword);
export {connectedUpdatePassword as UpdatePassword}


