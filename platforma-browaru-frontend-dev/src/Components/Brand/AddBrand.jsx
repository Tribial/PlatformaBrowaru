import React from 'react';
import {ControlLabel, FormControl, FormGroup} from "react-bootstrap";
import {brandActions} from "../../_actions";
import {Link} from "react-router-dom";
import DatePicker from 'react-date-picker';
import {Checkbox} from 'antd';
import {connect} from 'react-redux';
import ReactLoading from 'react-loading';

const seasons = [
    {id: 1, seasonIds: 1, text: 'wiosna'},
    {id: 2, seasonIds: 2, text: 'lato'},
    {id: 3, seasonIds: 3, text: 'jesien'},
    {id: 4, seasonIds: 4, text: 'zima'},
];

const wrappings = [
    {id: 1, wrappingIds: 1, text: 'butelka'},
    {id: 2, wrappingIds: 2, text: 'puszka'},
    {id: 3, wrappingIds: 3, text: 'beczka'},
];


class AddBrand extends React.Component {
    constructor(props, context) {
        super(props, context);
        this.state = {
            brand: {
                name: '',
                description: '',
                ingredients: '',
                color: '',
                alcoholAmountPercent: '',
                extractPercent: '',
                hopIntensity: '',
                tasteFullness: '',
                sweetness: '',
                kindId: '1',
                isPasteurized: true,
                isFiltered: true,
                seasonIds: [],
                fermentationTypeIds: [],
                brewingMethodIds: [],
                wrappingIds: [],
                creationDate: ''
            },
            allChecked: false,
            allCheckedWrappings: false
        };

        this.handleChange = this.handleChange.bind(this);
        this.onChangeDate = this.onChangeDate.bind(this);
        this.handleSubmit = this.handleSubmit.bind(this);
        this.onChangeCheckboxSeason = this.onChangeCheckboxSeason.bind(this);
        this.onChangeCheckboxWrapping = this.onChangeCheckboxWrapping.bind(this);
        this.handleCheckAll = this.handleCheckAll.bind(this);
        this.handleCheckAllWrappings = this.handleCheckAllWrappings.bind(this);
        this.handleChangeList = this.handleChangeList.bind(this);
    }
    handleChangeList(event){
        const {name, value} = event.target;
        const {brand} = this.state;
        if (name===brand.fermentationTypeIds) {
            this.setState({
                brand: {
                    ...brand,
                    fermentationTypeIds: [
                        ...brand.fermentationTypeIds,
                        value
                    ]
                }
            });
            console.log("fermentationTypeIds", this.state);
        }
            else {//if(event.target.name===brand.brewingMethodIds){
            this.setState({
                brand: {
                    ...brand,
                    brewingMethodIds: [
                        ...brand.brewingMethodIds,
                        value
                    ]
                }
            });
            console.log("brewingMethodIds", this.state);
            }
    }

    onChangeCheckboxSeason(seasonIds) {
        const {brand} = this.state;
        this.setState({
                 seasonIds, allChecked: seasonIds.length === seasons.length

        });
        this.setState({
            brand:{
                ...brand,
                seasonIds
            },
            allChecked: seasonIds.length === seasons.length
        });
        console.log("state", this.state)
    }

    onChangeCheckboxWrapping(wrappingIds) {
        const {brand} = this.state;
        this.setState({
            wrappingIds, allCheckedWrappings: wrappingIds.length === wrappings.length

        });
        this.setState({
            brand:{
                ...brand,
                wrappingIds
            },
            allCheckedWrappings: wrappingIds.length === wrappings.length
        });
        console.log("state", this.state)
    }

    handleCheckAll(e) {
        const {brand} = this.state;
        const checked = e.target.checked;
        this.setState({allChecked: checked});
        if (checked) {
            this.setState({brand: {...brand, seasonIds: seasons.map(item => item.seasonIds)}});
        } else {
            this.setState({brand: {...brand, seasonIds: []}});
        }
    }

    handleCheckAllWrappings(e) {
        const {brand} = this.state;
        const checked = e.target.checked;
        this.setState({allCheckedWrappings: checked});
        if (checked) {
            this.setState({brand: {...brand, wrappingIds: wrappings.map(item => item.wrappingIds)}});
        } else {
            this.setState({brand: {...brand, wrappingIds: []}});
        }
    }

    /*getValidationState() {
        const length = this.state.value.length;
        if (length > 10) return 'success';
        else if (length > 5) return 'warning';
        else if (length > 0) return 'error';
        return null;
    }*/
    onChangeDate(creationDate) {
        const {brand} = this.state;
        this.setState({
            brand: {
                ...brand, creationDate
            }
        });

    }

    handleChange(event) {
        const {name, value} = event.target;
        const {brand} = this.state;

        this.setState({
            brand: {
                ...brand,
                [name]: value
            }
        });
        console.log("state", this.state);
    }

    handleSubmit(event) {
        event.preventDefault();
        const {brand} = this.state;
        const {dispatch} = this.props;
        console.log("brand", brand);
        dispatch(brandActions.addBrand(brand));
    }

    render() {
        const {brand} = this.state;
        const {adding} = this.props;
        return (
            <div className="col-md-offset-2 col-md-8"
                 style={{backgroundColor: "#bcaaa4", padding: "20px", borderRadius: "5px"}}>
                <form name="form" onSubmit={this.handleSubmit}>
                    <div className="col-md-3">
                        <FormGroup
                            controlId="formBasicText"
                            //validationState={this.getValidationState()}
                        >
                            <ControlLabel>Nazwa</ControlLabel>
                            <FormControl
                                type="text"
                                name="name"
                                value={brand.name}
                                placeholder="nazwa"
                                onChange={this.handleChange}
                            />
                        </FormGroup>
                    </div>

                    <div className="col-md-6">
                        <FormGroup
                            controlId="formBasicText"
                            //validationState={this.getValidationState()}
                        >
                            <ControlLabel>Składniki</ControlLabel>
                            <FormControl
                                type="text"
                                name="ingredients"
                                value={brand.ingredients}
                                placeholder="Skladniki"
                                onChange={this.handleChange}
                            />
                        </FormGroup>
                    </div>
                    <div className="col-md-3">
                        <FormGroup
                            controlId="formBasicText"
                            //validationState={this.getValidationState()}
                        >
                            <ControlLabel>Kolor</ControlLabel>
                            <FormControl
                                type="text"
                                name="color"
                                value={brand.color}
                                placeholder="Kolor"
                                onChange={this.handleChange}
                            />
                        </FormGroup>
                    </div>
                    <div className="col-md-3">
                        <FormGroup
                            controlId="formBasicText"
                            //validationState={this.getValidationState()}
                        >
                            <ControlLabel>Zawartość alkoholu</ControlLabel>
                            <FormControl
                                type="number"
                                name="alcoholAmountPercent"
                                value={brand.alcoholAmountPercent}
                                placeholder="0-30%"
                                onChange={this.handleChange}
                            />
                        </FormGroup>
                    </div>
                    <div className="col-md-3">
                        <FormGroup
                            controlId="formBasicText"
                            //validationState={this.getValidationState()}
                        >
                            <ControlLabel>Ekstrakt</ControlLabel>
                            <FormControl
                                type="number"
                                name="extractPercent"
                                value={brand.extractPercent}
                                placeholder="0-30%"
                                onChange={this.handleChange}
                            />
                        </FormGroup>
                    </div>
                    <div className="col-md-3">
                        <FormGroup
                            controlId="formBasicText"
                            //validationState={this.getValidationState()}
                        >
                            <ControlLabel>Chmielowość</ControlLabel>
                            <FormControl
                                type="number"
                                name="hopIntensity"
                                value={brand.hopIntensity}
                                placeholder="W skali 0-5"
                                onChange={this.handleChange}
                            />
                        </FormGroup>
                    </div>
                    <div className="col-md-3">
                        <FormGroup
                            controlId="formBasicText"
                            //validationState={this.getValidationState()}
                        >
                            <ControlLabel>Pełnia</ControlLabel>
                            <FormControl
                                type="number"
                                name="tasteFullness"
                                value={brand.tasteFullness}
                                placeholder="W skali 0-5"
                                onChange={this.handleChange}
                            />
                        </FormGroup>
                    </div>
                    <div className="col-md-3">
                        <FormGroup
                            controlId="formBasicText"
                            //validationState={this.getValidationState()}
                        >
                            <ControlLabel>Słodycz</ControlLabel>
                            <FormControl
                                type="number"
                                name="sweetness"
                                value={brand.sweetness}
                                placeholder="W skali 0-5"
                                onChange={this.handleChange}
                            />
                        </FormGroup>
                    </div>
                    <div className="col-md-3">
                        <FormGroup
                            controlId="formBasicText"
                            //validationState={this.getValidationState()}
                        >
                            <ControlLabel>Gatunek</ControlLabel>
                            <select className="form-control" name="kindId"
                                    value={brand.kindId} onChange={this.handleChange}>
                                <option value={1}>Jasny lager</option>
                                <option value={2}>Ciemny</option>
                            </select>
                        </FormGroup>
                    </div>
                    <div className="col-md-3">
                        <FormGroup
                            controlId="formBasicText"
                            //validationState={this.getValidationState()}
                        >
                            <ControlLabel>Czy pasteryzowane?</ControlLabel>
                            <select className="form-control" name="isPasteurized"
                                    value={brand.isPasteurized} onChange={this.handleChange}>
                                <option value={true}>Tak</option>
                                <option value={false}>Nie</option>
                            </select>
                        </FormGroup>
                    </div>
                    <div className="col-md-3">
                        <FormGroup
                            controlId="formBasicText"
                            //validationState={this.getValidationState()}
                        >
                            <ControlLabel>Czy filtrowane?</ControlLabel>
                            <select className="form-control" name="isFiltered"
                                    value={brand.isFiltered} onChange={this.handleChange}>
                                <option value={true}>Tak</option>
                                <option value={false}>Nie</option>
                            </select>
                        </FormGroup>
                    </div>
                    <div className="col-md-3">
                        <Checkbox onChange={this.handleCheckAll} checked={this.state.allChecked}> Całoroczne</Checkbox>
                        <Checkbox.Group onChange={this.onChangeCheckboxSeason} value={this.state.seasonIds}>
                            {
                                seasons.map(item =>
                                    <div key={item.id}>
                                        <Checkbox value={item.seasonIds}>{}</Checkbox>
                                        <span>{item.text}</span>
                                </div>
                                )
                            }
                        </Checkbox.Group>
                    </div>
                    <div className="col-md-3">
                        <Checkbox onChange={this.handleCheckAllWrappings} checked={this.state.allCheckedWrappings}> Wszystkie</Checkbox>
                        <Checkbox.Group onChange={this.onChangeCheckboxWrapping} value={this.state.wrappingIds}>
                            {
                                wrappings.map(item =>
                                    <div key={item.id}>
                                        <Checkbox value={item.wrappingIds}>{}</Checkbox>
                                        <span>{item.text}</span>
                                    </div>
                                )
                            }
                        </Checkbox.Group>
                    </div>
                    <div className="col-md-3">
                        <FormGroup
                            controlId="formBasicText"
                            //validationState={this.getValidationState()}
                        >
                            <ControlLabel>Metoda fermentacji</ControlLabel>
                            <select className="form-control" name="fermentationTypeIds"
                                    value={this.state.fermentationTypeIds} onChange={this.handleChangeList} >
                                <option value="1">dolna</option>
                                <option value="2">górna</option>
                                <option value="3">spontatniczna</option>
                            </select>
                        </FormGroup>
                    </div>
                    <div className="col-md-3 ">
                        <FormGroup
                            controlId="formBasicText"
                            //validationState={this.getValidationState()}
                        >
                            <ControlLabel>Metoda warzenia</ControlLabel>
                            <select className="form-control" name="brewingMethodIds"
                                    value={this.state.brewingMethodIds} onChange={this.handleChangeList}>
                                <option value="1">tradycyjna</option>
                                <option value="2">nowoczesna(HGB)</option>
                            </select>
                        </FormGroup>
                    </div>
                    <div className="col-md-3">
                        <FormGroup
                            controlId="formBasicText"
                            //validationState={this.getValidationState()}
                        >
                            <ControlLabel>Data powstania</ControlLabel>
                            <div className="form-group">
                                <DatePicker
                                    name="creationDate"
                                    value={this.state.creationDate}
                                    onChange={this.onChangeDate}
                                />
                            </div>
                        </FormGroup>
                    </div>
                    <div className="col-md-12 " style={{padding: "0"}}>
                        <div className="col-md-6 ">
                            <FormGroup
                                controlId="formBasicText"
                                //validationState={this.getValidationState()}
                            >
                                <ControlLabel>Opis</ControlLabel>
                                <FormControl
                                    componentClass="textarea"
                                    type="text"
                                    name="description"
                                    value={this.state.description}
                                    placeholder="Maksymalnie 255 znaków"
                                    onChange={this.handleChange}
                                />
                            </FormGroup>
                        </div>
                        <div className="col-md-6" style={{marginTop: "10px"}}>
                            <br/>
                            <FormGroup
                                controlId="formBasicText"
                                //validationState={this.getValidationState()}
                            >
                                <button className="btn btn-lg"
                                        style={{backgroundColor: '#c5e1a5', color: 'black', marginRight: '5px'}}>Dodaj
                                </button>
                                <Link to="/brand" className="btn btn-lg"
                                      style={{backgroundColor: '#ffab91', color: 'black'}}>Anuluj</Link>
                                {adding &&
                                <ReactLoading type="spin" color="#fff" height={40} width={40}/>
                                }
                            </FormGroup>
                        </div>
                    </div>

                </form>
            </div>
        );
    }
}

function mapStateToProps(state) {
    const {adding} = state.brand;
    return {
        adding
    };
}
const connectedAddBrand = connect(mapStateToProps)(AddBrand);
export {connectedAddBrand as AddBrand}