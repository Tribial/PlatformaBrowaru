import React from 'react';
import {connect} from 'react-redux';
import {userActions} from "../../_actions";
import {Link} from "react-router-dom";
import ReactLoading from 'react-loading';
import {history} from '../../_helpers/history'

class DeletedUserInfo extends React.Component {
    constructor(props, context) {
        super(props, context);

        this.handleSubmit = this.handleSubmit.bind(this);
    }
    handleSubmit(){
        history.push("/login")
    }

    render() {
        return (
            <div className="col-md-offset-2 col-md-6" style={{padding: '0px 100px 100px 0px',}}>
                <h3>Twoje konto zostalo usuniete</h3>
                <div className="col-md-6">
                    <div className="form-group">
                        <button className="btn btn-danger" onClick={this.handleSubmit}>Wyloguj</button>
                    </div>
                </div>
            </div>


        );
    }
}


export {DeletedUserInfo}


