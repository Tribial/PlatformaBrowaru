import React  from 'react';
import {connect} from 'react-redux';
//import {bindActionCreators} from 'redux';
import {Navbar, NavItem, Nav, NavDropdown, MenuItem,} from 'react-bootstrap';
//import '../../node_modules/react-bootstrap/lib/';
//import {Link} from "react-router-dom";
import {history} from "../../_helpers/history";

class Header extends React.Component {
    constructor(props, context) {
        super(props, context);
        this.handleSelect=this.handleSelect.bind(this);

    }
    handleSelect(e) {
        if (e === 3.1) history.push("/user/updateProfile");
        else if (e === 3.2) history.push("/user/updatePassword");
        else if (e === 3.3) history.push("/user/updateEmail");
        else if (e === 3.4) history.push("/user/deleteUser");
        else if(e === 3.5) history.push("/login");
        else if(e === 1) history.push("/brand");
    }

    render() {
        const {user} = this.props;
        const AuthenticatedItems = (
            <Nav pullRight>
                <NavDropdown eventKey={3} title="Mój profil" id="basic-nav-dropdown" onSelect={this.handleSelect}>
                    <MenuItem eventKey={3.1}>Edytuj profil</MenuItem>
                    <MenuItem eventKey={3.2}>Zmień hasło</MenuItem>
                    <MenuItem eventKey={3.3}>Zmień email</MenuItem>
                    <MenuItem eventKey={3.4}>Usun konto</MenuItem>
                    <MenuItem divider />
                    <MenuItem eventKey={3.5}>Wyloguj się</MenuItem>
                </NavDropdown>
            </Nav>
        );

        return (
            <Navbar inverse collapseOnSelect style={{paddingTop: '20px', paddingBottom:'20px    '}}>
                <Navbar.Header>
                    <Navbar.Brand>
                        BrowaruPlatforma
                    </Navbar.Brand>
                    <Navbar.Toggle />
                </Navbar.Header>
                <Navbar.Collapse>
                    <Nav>
                        <NavItem eventKey={1} onSelect={this.handleSelect}>
                            Piwa
                        </NavItem>
                        <NavItem eventKey={2} onSelect={this.handleSelect}>
                            O nas
                        </NavItem>
                    </Nav>
                    {user ? AuthenticatedItems : null}
                </Navbar.Collapse>
            </Navbar>
        );
    }
}


function mapStateToProps(state) {
    const { authentication } = state;
    const { user } = authentication;
    return {
        user
    };
}

const connectedHeader = connect(mapStateToProps)(Header);
export { connectedHeader as Header };

