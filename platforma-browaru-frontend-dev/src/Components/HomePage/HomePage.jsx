import React from 'react';
import { Link } from 'react-router-dom';
import { connect } from 'react-redux';


class HomePage extends React.Component {

    render() {
        const { user, users} = this.props;

        return (
            <div className="col-md-6 col-md-offset-3">
                <h1>Hi !</h1>
                <p>You're logged in with React and ASP.NET Core 2.0!!</p>

                <p>
                    <Link to="/login">Logout</Link>
                </p>
            </div>
        );
    }
}

function mapStateToProps(state) {
    const { authentication, users } = state;
    const { user} = authentication;
    return {
        user,
        users
    };
}

const connectedHomePage = connect(mapStateToProps)(HomePage);
export { connectedHomePage as HomePage };