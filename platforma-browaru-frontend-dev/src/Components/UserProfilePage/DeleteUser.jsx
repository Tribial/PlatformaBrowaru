import React from 'react';
import {connect} from 'react-redux';
import {userActions} from "../../_actions";
import {bindActionCreators} from "redux";

class DeleteUser extends React.Component {
    constructor(props, context) {
        super(props, context);
        this.state = {
            user: {
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
        if (user.password) {
            this.props.actions.deleteUser(user);
        }
    }

    render() {
        const {user, submitted} = this.state;

        return (
            <div className="col-md-8" style={{padding: '0px 100px 100px 0px'}}>
                <h2>Usun konto</h2>
                <form name="form" onSubmit={this.handleSubmit}>
                    <div className={'form-group' + (submitted && !user.password ? ' has-error' : '')}>
                        <label htmlFor="password">Hasło</label>
                        <input type="password" className="form-control" name="password" value={user.password}
                               onChange={this.handleChange}/>
                        {submitted && !user.password &&
                        <div className="help-block">Password is required</div>
                        }
                    </div>
                    <div className="form-group">
                        <button className="btn btn-danger">Delete</button>
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

const connectedDeleteUser = connect(mapStateToProps, mapDispatchToProps)(DeleteUser);
export {connectedDeleteUser as DeleteUser}



