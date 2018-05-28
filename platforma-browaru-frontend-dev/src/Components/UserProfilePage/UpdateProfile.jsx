import React from 'react';
import {connect} from 'react-redux';
import {userActions} from "../../_actions";
import {Link} from "react-router-dom";
import {bindActionCreators} from "redux";

class UpdateProfile extends React.Component {
    constructor(props, context) {
        super(props, context);
        this.state = {
            draftUser: {
                firstName: this.props.users.firstName,
                lastName: this.props.users.lastName,
                username: this.props.users.username,
            },
            submitted: false,
            //test:false,


        };

        this.handleChange = this.handleChange.bind(this);
        this.handleSubmit = this.handleSubmit.bind(this);
        //this.handleClick = this.handleClick.bind(this);
    }
    handleChange(event) {
        const {name, value} = event.target;
        const {draftUser} = this.state;
        this.setState({
            draftUser: {
                ...draftUser,
                [name]: value
            }
        });
    }

    /*handleClick(e){
        this.setState({test:true});
    }*/

    handleSubmit(event) {
        event.preventDefault();
        this.setState({submitted: true});
        const {draftUser} = this.state;
        console.log("submit_UpdateProfile_user", draftUser);
        let user2 = JSON.parse(localStorage.getItem('user'));
        this.props.actions.updateUser(draftUser,user2.id);


    }

    /*
    { this.state.test && <div className={'form-group' + (submitted && !user.username ? ' has-error' : '')}>
                        <label htmlFor="email">Nick</label>
                        <input type="text" className="form-control" name="username" value={user.username}
                               onChange={this.handleChange}/>
                        {submitted && !user.username &&
                        <div className="help-block">Username is required</div>
                        }
                    </div>
                    }
                    { !this.state.test && <div className={'form-group' + (submitted && !user.username ? ' has-error' : '')}>
                        <label htmlFor="email">Nick</label>
                        <button onClick={this.handleClick}>user.username</button>
                    </div>
                    }
     */

    render() {
        const {draftUser, submitted} = this.state;
        console.log("UpdateProfile.jsx", this.props.users);
        const {users} = this.props;
        return (
            <div className="col-md-8" style={{padding: '0px 100px 100px 0px'}}>
                <h2>Edytuj Profil</h2>
                <form name="form" onSubmit={this.handleSubmit}>
                    <div className={'form-group' + (submitted && !draftUser.username ? ' has-error' : '')}>
                        <label htmlFor="email">Nick</label>
                        <input type="text" className="form-control" name="username" value={draftUser.username}
                               onChange={this.handleChange}/>
                        {submitted && !draftUser.username &&
                        <div className="help-block">Username is required</div>
                        }
                    </div>
                    <div className={'form-group' + (submitted && !draftUser.firstName ? ' has-error' : '')}>
                        <label htmlFor="firstName">Imię</label>
                        <input type="text" className="form-control" name="firstName" value={draftUser.firstName}
                               onChange={this.handleChange}/>
                        {submitted && !draftUser.firstName &&
                        <div className="help-block">First Name is required</div>
                        }
                    </div>
                    <div className={'form-group' + (submitted && !draftUser.lastName ? ' has-error' : '')}>
                        <label htmlFor="lastName">Nazwisko</label>
                        <input type="text" className="form-control" name="lastName" value={draftUser.lastName}
                               onChange={this.handleChange}/>
                        {submitted && !draftUser.lastName &&
                        <div className="help-block">Last Name is required</div>
                        }
                    </div>
                    <div className="form-group">
                        <button className="btn btn-primary">Save</button>
                        <Link to="/login" className="btn btn-link">Cancel</Link>
                    </div>
                </form>
                <h2>Informacje</h2>
                <div className="row " style={{padding: '0'}}>
                    <div className="col-md-4">
                        <div className="panel panel-default">
                            <div className="panel-heading" style={{fontWeight: '700'}}>Data urodzenia</div>
                            <div className="well">{!!users.dateOfBirth ? users.dateOfBirth.substring(0,10) : " " }</div>
                        </div>
                    </div>
                    <div className="col-md-4">
                        <div className="panel panel-default">
                            <div className="panel-heading" style={{fontWeight: '700'}}>Data dolaczenia</div>
                            <div className="well">{!!users.createdAt ? users.createdAt.substring(0,10) : "Niepodano"}</div>
                        </div>
                    </div>
                    <div className="col-md-4">
                        <div className="panel panel-default">
                            <div className="panel-heading" style={{fontWeight: '700'}}>Plec</div>
                            <div className="well">{!!users.gender ? users.gender : "Niepodano"}</div>
                        </div>
                    </div>
                    <div className="col-md-8">
                        <div className="panel panel-default">
                            <div className="panel-heading" style={{fontWeight: '700'}}>Opis</div>
                            <div className="well">{users.description}</div>
                        </div>
                    </div>
                        <div className="col-md-4">
                        <div className="panel panel-default">
                            <div className="panel-heading" style={{fontWeight: '700'}}>Status</div>
                            <div className="well">{users.status}</div>
                        </div>
                    </div>

                </div>
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

const connectedUpdateProfile = connect(mapStateToProps, mapDispatchToProps)(UpdateProfile);

export {connectedUpdateProfile as UpdateProfile}



        