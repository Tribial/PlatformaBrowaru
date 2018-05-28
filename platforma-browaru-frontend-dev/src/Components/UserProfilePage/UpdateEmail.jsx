import React from 'react';
import {connect} from 'react-redux';
import {userActions} from "../../_actions";
import {bindActionCreators} from "redux";

class UpdateEmail extends React.Component {
    constructor(props, context) {
        super(props, context);
        this.state = {
            user: {
                newEmail: '',
                confirmNewEmail: '',
                password: '',
            },
            submitted: false,
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
        //const {dispatch} = this.props;
        console.log("submit_UpdateEmail_user", user);
        if (user.newEmail  && user.confirmNewEmail && user.password) {
            this.props.actions.updateEmail(user);
        }
    }

    render() {
        const {user, submitted} = this.state;

        return (
            <div className="col-md-8" style={{padding: '0px 100px 100px 0px'}}>
                <h1>Zmień email</h1>
                <form name="form" onSubmit={this.handleSubmit}>
                    <div className={'form-group' + (submitted && !user.newEmail ? ' has-error' : '')}>
                        <label htmlFor="newEmail">Nowy email</label>
                        <input type="text" className="form-control" name="newEmail" value={user.newEmail}
                               onChange={this.handleChange}/>
                        {submitted && !user.newEmail &&
                        <div className="help-block">New email is required</div>
                        }
                    </div>
                    <div className={'form-group' + (submitted && !user.confirmNewEmail ? ' has-error' : '')}>
                        <label htmlFor="email"> Potwierdz nowy email</label>
                        <input type="text" className="form-control" name="confirmNewEmail" value={user.confirmNewEmail}
                               onChange={this.handleChange}/>
                        {submitted && !user.confirmNewEmail &&
                        <div className="help-block">Confirming new email is required</div>
                        }
                    </div>
                    <div className={'form-group' + (submitted && !user.password ? ' has-error' : '')}>
                        <label htmlFor="password">Hasło</label>
                        <input type="password" className="form-control" name="password" value={user.password}
                               onChange={this.handleChange}/>
                        {submitted && !user.password &&
                        <div className="help-block">Password is required</div>
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

const connectedUpdateEmail = connect(mapStateToProps, mapDispatchToProps)(UpdateEmail);
export {connectedUpdateEmail as UpdateEmail}


