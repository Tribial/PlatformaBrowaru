import React from 'react';
import {history} from "../../_helpers/history";
import {Button} from "react-bootstrap";


class Brand extends React.Component {
    constructor(props, context) {
        super(props, context);
    }

    handleClick(){
        history.push('/addBrand')
    }

    render() {
        return (
            <div className="col-md-12">
                <h1>Beers</h1>
                <Button bsStyle="primary" bsSize="large" onClick={this.handleClick}>Dodaj dzika</Button>
            </div>
        );
    }
}

export {Brand}