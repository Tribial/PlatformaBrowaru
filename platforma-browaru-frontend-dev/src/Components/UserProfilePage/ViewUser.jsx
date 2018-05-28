import React from 'react';
import {connect} from 'react-redux';
import {userActions} from "../../_actions";
import {Link} from "react-router-dom";
import ReactLoading from 'react-loading';

class ViewUser extends React.Component {
    constructor(props, context) {
        super(props, context);
        this.state = {
            user: {
                firstName: '',
                lastName: '',
                username: '',
            },
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

    render() {
        const {user} = this.state;

        return (
            <div className="col-md-8" >
                <h1>Profil</h1>
                <div className="col-md-6">

                </div>
                <div className="col-md-2">

                </div>
            </div>

        );
    }
}



export {ViewUser}


