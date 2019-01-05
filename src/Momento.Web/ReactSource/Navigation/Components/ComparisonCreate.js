import React, { Component, Fragment } from 'react';
import * as c from './Helpers/Constants';
import { linkSSRSafe, handeleValidationErrors, clientSideValidation } from './Helpers/HelperFuncs';
import ShowError from "./Helpers/ShowError"

export default class ComparisonCreate extends Component {
    constructor(props) {
        super(props);
        this.state = {
            name: "",
            description: "",
            sourceLanguage: "",
            targetLanguage: "",

            showErrors: true,
            ERRORS: [],
        };

        this.renderComparisonData = this.renderComparisonData.bind(this);
        this.onClickButtonCreate = this.onClickButtonCreate.bind(this);
        this.onChangeInput = this.onChangeInput.bind(this);
    }

    onClickButtonCreate() {
        this.setState({ ERRORS: [] });

        let data = {};
        data.description = this.state.description;
        data.name = this.state.name;
        data.sourceLanguage = this.state.sourceLanguage;
        data.targetLanguage = this.state.targetLanguage;
        data.ParentDirId = this.props.match.params.id;

        console.log(data);
        fetch("/api/Comparison/Create", {
            method: "POST",
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(data)
        })
            .then(x => x.json())
            .then((data) => {

                if (data.hasOwnProperty("errors")) {
                    handeleValidationErrors(data.errors, this);
                    return;
                }

                if (data == true) {
                    this.props.history.push(c.rootDir + "/" + this.props.match.params.id);
                } else if (data == false) {
                    alert("Note was not created!");
                }
            });
    }

    onChangeInput(target, event) {
        let newState = this.state;
        newState[target] = event.target.value;
        this.setState(newState);
    }

    renderComparisonData() {
        let fields = ["name", "description", "sourceLanguage", "targetLanguage"];

        return fields.map(x =>
            <Fragment>
                <ShowError prop={x.toUpperCase()} ERRORS={this.state.ERRORS} showErrors={this.state.showErrors} />
                <div className="form-group row" key={x}>
                    <label className="col-sm-2 col-form-label text-right">{x}</label>
                    <div className="col-sm-6">
                        <input onChange={(e) => this.onChangeInput(x, e)} id="nameInput" value={this.state[x]} className="form-control-black" style={{ backgroundColor: c.secondaryColor }} />
                    </div>
                </div>
            </Fragment>
        );
    }

    render() {
        return (
            <Fragment>
                {this.renderComparisonData()}
                <div className="text-center">
                    <button onClick={this.onClickButtonCreate} className="btn btn-success">Create</button>
                    {linkSSRSafe(`${c.rootDir}/${this.props.match.params.id}`, "Back", null, "btn btn-warning")}
                </div>
            </Fragment>)
    }
}