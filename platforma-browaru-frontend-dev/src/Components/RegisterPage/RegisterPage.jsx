import React from 'react';
import {Link} from 'react-router-dom';
import {connect} from 'react-redux';

import ReactLoading from 'react-loading';
import DatePicker from 'react-date-picker';
import {userActions} from '../../_actions/index';

class RegisterPage extends React.Component {
    constructor(props) {
        super(props);
        //this.state.user.dateOfBirth.format('dd-mm-yy');
        this.state = {
            user: {
                firstName: '',
                lastName: '',
                username: '',
                email: '',
                password: '',
                confirmPassword: '',
                gender: '',
                dateOfBirth: new Date()
            },
            submitted: false
        };

        this.handleChange = this.handleChange.bind(this);
        this.onChangeDate = this.onChangeDate.bind(this);
        this.handleSubmit = this.handleSubmit.bind(this);
    }

    handleChange(event) {
        const {name, value} = event.target;
        const {user} = this.state;
        console.log("tutaj zmiany");
        this.setState({
            user: {
                ...user,
                [name]: value
            }
        });
    }

    onChangeDate(dateOfBirth) {
        console.log("tutaj zmiany");
        const {user} = this.state;
        this.setState({
            user:
                { ...user, dateOfBirth }
        });

    }

    handleSubmit(event) {
        event.preventDefault();
        this.setState({submitted: true});
        const {user} = this.state;
        const {dispatch} = this.props;
        if (user.firstName && user.lastName && user.email && user.password && user.dateOfBirth) {
            dispatch(userActions.register(user));
        }
    }

    render() {
        const {registering} = this.props;
        const {user, submitted} = this.state;
        return (
            <div className="col-md-6 col-md-offset-3">
                <h2>Rejestracja</h2>
                <form name="form" onSubmit={this.handleSubmit}>
                    <div className={'form-group' + (submitted && !user.email ? ' has-error' : '')}>
                        <label htmlFor="email">Email</label>
                        <input type="text" className="form-control" name="email" value={user.email}
                               onChange={this.handleChange}/>
                        {submitted && !user.email &&
                        <div className="help-block">Email is required</div>
                        }
                    </div>
                    <div className={'form-group' + (submitted && !user.username ? ' has-error' : '')}>
                        <label htmlFor="email">Nick</label>
                        <input type="text" className="form-control" name="username" value={user.username}
                               onChange={this.handleChange}/>
                        {submitted && !user.username &&
                        <div className="help-block">Username is required</div>
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
                    <div
                        className={'form-group' + (submitted && !user.confirmPassword && user.password !== user.confirmPassword ? ' has-error' : '')}>
                        <label htmlFor="password">Potwierdź hasło</label>
                        <input type="password" className="form-control" name="confirmPassword"
                               value={user.confirmPassword} onChange={this.handleChange}/>
                        {submitted && !user.password && user.password !== user.confirmPassword && !
                            <div className="help-block">Password is required</div>
                        }
                    </div>

                    <div className={'form-group' + (submitted && !user.firstName ? ' has-error' : '')}>
                        <label htmlFor="firstName">Imię</label>
                        <input type="text" className="form-control" name="firstName" value={user.firstName}
                               onChange={this.handleChange}/>
                        {submitted && !user.firstName &&
                        <div className="help-block">First Name is required</div>
                        }
                    </div>
                    <div className={'form-group' + (submitted && !user.lastName ? ' has-error' : '')}>
                        <label htmlFor="lastName">Nazwisko</label>
                        <input type="text" className="form-control" name="lastName" value={user.lastName}
                               onChange={this.handleChange}/>
                        {submitted && !user.lastName &&
                        <div className="help-block">Last Name is required</div>
                        }
                    </div>
                    <div className="form-group">
                        <label htmlFor="dateOfBirth">Data urodzenia</label>
                        <div className="form-group">
                            <DatePicker
                                name="dateOfBirth"
                                value={user.dateOfBirth}
                                onChange={this.onChangeDate}
                            />
                        </div>
                    </div>
                    <div className="form-group">
                        <label htmlFor="gender">Płeć</label>
                        <select className="form-control" name="gender"
                                value={user.gender} onChange={this.handleChange}>
                            <option value="Male">Mężczyzna</option>
                            <option value="Female">Kobieta</option>
                            <option value="Other">Nie podaję</option>
                        </select>
                    </div>
                    <div className="form-group">
                        <label>Kilka słów o sobie</label>
                        <textarea className="form-control" rows="3" name="description" value={user.description}
                        onChange={this.handleChange}/>
                    </div>

                    <div className="form-group">
                        <button className="btn btn-primary">Register</button>
                        <Link to="/login" className="btn btn-link">Cancel</Link>
                        {registering &&
                        <ReactLoading type="spin" color="#fff" height={40} width={40}/>
                        }
                    </div>
                </form>
            </div>
        );
    }
}

function mapStateToProps(state) {
    const {registering} = state.registration;
    return {
        registering
    };
}

const connectedRegisterPage = connect(mapStateToProps)(RegisterPage);
export {connectedRegisterPage as RegisterPage};

