import React from 'react';
import {FormGroup} from "react-bootstrap";
import {Link} from "react-router-dom";


class BrandDetails extends React.Component {
    constructor(props, context) {
        super(props, context);
    }

    render() {
        return (
            <div className="col-md-offset-2 col-md-8" style={{backgroundColor: "#bcaaa4", border:"1px solid #000"}}>
                <h1>Beer Details</h1>
                <div className="col-md-6">
                    <div className="col-md-12" style={styleLeftSide}>
                        <p style={labelP}>Nazwa marki</p>
                        <p style={valueP}>Heineken</p>
                    </div>
                    <div className="col-md-12" style={styleLeftSide}>
                        <p style={labelP}>Opis</p>
                        <p style={valueP}>Heineken jest najsłynniejszą marką holenderskiej grupy. Piwo ma bladożółtą
                            barwę (7 EBC) oraz "lekki, nieskomplikowany smak", cechuje je „duża stabilność i swoista
                            szlachetna subtelność.</p>
                    </div>
                    <div className="col-md-12" style={styleLeftSide}>
                        <p style={labelP}>Składniki</p>
                        <p style={valueP}>woda, słód jęczmienny, chmiel</p>
                    </div>
                    <div className="col-md-12" style={styleLeftSide}>
                        <p style={labelP}>Gatunek</p>
                        <p style={valueP}>jasny lager</p>
                    </div>
                    <div className="col-md-12" style={styleLeftSide}>
                        <p style={labelP}>Barwa</p>
                        <p style={valueP}>Złocisty</p>
                    </div>
                    <div className="col-md-12">
                        <div className="col-sm-6" style={styleLeftSideDivided} >
                            <p style={labelP}>Zawartość alkoholu</p>
                            <p style={valueP}>5%</p>
                        </div>
                        <div className="col-sm-6" style={styleLeftSide}>
                            <p style={labelP}>Ekstrakt</p>
                            <p style={valueP}>11.4%</p>
                        </div>
                    </div>
                </div>
                <div className="col-md-6">
                    <div className="col-md-12" style={styleRightSide}>
                        <p style={labelP}>Ocena ogólna</p>
                        <p style={valueP}>4/5</p>
                    </div>
                    <div className="col-md-12" style={styleRightSide}>
                        <p style={labelP}>Ocena własna</p>
                        <p style={valueP}>4.5/5</p>
                    </div>
                    <div className="col-md-12">
                        <div className="col-md-6" style={styleRightSide}>
                            <p style={labelP}>Dostępne opakowania</p>
                            <p style={valueP}>butelka, puszka, beczka</p>
                        </div>
                        <div className="col-md-6" style={styleRightSide}>
                            <p style={labelP}>Rok powstania</p>
                            <p style={valueP}>2000</p>
                        </div>
                    </div>
                    <div className="col-md-12">
                        <div className="col-md-6" style={styleRightSide}>
                            <p style={labelP}>Chmielowość</p>
                            <p style={valueP}>4</p>
                        </div>
                        <div className="col-md-6" style={styleRightSide}>
                            <p style={labelP}>Metoda warzenia</p>
                            <p style={valueP}>tradycyjna</p>
                        </div>
                    </div>
                    <div className="col-md-12">
                        <div className="col-md-6" style={styleRightSide}>
                            <p style={labelP}>Pelnia</p>
                            <p style={valueP}>5%</p>
                        </div>
                        <div className="col-md-6" style={styleRightSide}>
                            <p style={labelP}>fermentacja dolna</p>
                            <p style={valueP}>11.4%</p>
                        </div>
                    </div>
                    <div className="col-md-12">
                        <div className="col-md-6" style={styleRightSide}>
                            <p style={labelP}>Slodycz</p>
                            <p style={valueP}>4</p>
                        </div>
                        <div className="col-md-6" style={styleRightSide}>
                            <p style={labelP}>Pasteryzacja</p>
                            <p style={valueP}>Pasteryzowane</p>
                        </div>
                    </div>
                    <div className="col-md-12">
                        <div className="col-md-6" style={styleRightSide}>
                            <p style={labelP}>Dostepnosc sezonowa</p>
                            <p style={valueP}>caloroczna</p>
                        </div>
                        <div className="col-md-6" style={styleRightSide}>
                            <p style={labelP}>Filtracja</p>
                            <p style={valueP}>Filtrowane</p>
                        </div>
                    </div>
                    <div className="col-md-12">
                        <FormGroup
                            controlId="formBasicText"
                            //validationState={this.getValidationState()}
                        >
                            <button className="btn btn-lg"
                                    style={{backgroundColor: '#c5e1a5', color: 'black', marginRight: '5px'}}>Dodaj
                            </button>
                            <Link to="/brand" className="btn btn-lg"
                                  style={{backgroundColor: '#ffab91', color: 'black'}}>Anuluj</Link>
                        </FormGroup>
                    </div>
                </div>
            </div>
        );
    }
}

export {BrandDetails};

const styleLeftSide = {
    backgroundColor: "#d3b2ad",
    padding: "10px",
    border: '1px solid black',
    margin: '5px 0 5px '
};
const styleLeftSideDivided = {
    backgroundColor: "#d3b2ad",
    padding: "10px",
    border: '1px solid black',
    margin: '5px 0 5px ',
    borderRight:'0px'
};
const styleRightSide = {
    backgroundColor: "#d3bcad",
    padding: "10px",
    border: '1px solid black',
    margin: '5px 0 5px '
};

const labelP = {
    fontWeight: '700',
    fontSize: '10px'
};
const valueP = {
    fontWeight: '700',
    fontSize: '18px'
};