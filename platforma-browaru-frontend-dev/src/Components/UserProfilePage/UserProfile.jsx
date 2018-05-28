import React from 'react';
//import {connect} from 'react-redux';
//import {bindActionCreators} from 'redux';
import {NavItem, Nav} from 'react-bootstrap';
import {history} from "../../_helpers/history";


class UserProfile extends React.Component {
    constructor(props, context) {
        super(props, context);
        this.handleSelect = this.handleSelect.bind(this);
    }

    handleSelect(selectedKey) {
        if (selectedKey === 1) history.push('/user/updateProfile');
        else if (selectedKey === 2) history.push('/user/updatePassword');
        else if (selectedKey === 3) history.push('/user/updateEmail');
        else if (selectedKey === 4) history.push('/user/deleteUser');
        else if (selectedKey === 5) history.push('/login');
    };

    render() {
        return (
                <div className="col-md-2" style={divStyle}>
                    <Nav bsStyle="pills" stacked onSelect={this.handleSelect}>
                        <NavItem eventKey={1} title="Item">
                            Edytuj Profil
                        </NavItem>
                        <NavItem eventKey={2} title="Item">
                            Zmień haslo
                        </NavItem>
                        <NavItem eventKey={3} title="Item">
                            Zmień email
                        </NavItem>
                        <NavItem eventKey={4} title="Item">
                            Usuń konto
                        </NavItem>
                        <NavItem eventKey={5} title="Logout">
                            Wyloguj sie
                        </NavItem>
                    </Nav>
                </div>
        );
    }
}

/*UserProfile.propTypes = {
    //myProp: PropTypes.string.isRequired
};

function mapStateToProps(state) {
    return {
        state: state
    };
}

const connectedUserProfile = connect(mapStateToProps)(UserProfile);
export {connectedUserProfile as UserProfile};
*/
export {UserProfile}

const divStyle = {
    backgroundColor: 'lightgray',
    padding: '5px',
    borderRadius: '5px',
    marginRight: '20px',
    marginLeft: '20px',
    marginTop: '80px'
};