import React, { Component, Fragment } from 'react';
import * as c from './Helpers/Constants';
import { linkSSRSafe } from './Helpers/HelperFuncs';

export default class NoteCreate extends Component {
    constructor(props) {
        super(props);
        this.state = {
            name: "",
            description: "",
        };

        this.onClickButtonCreate = this.onClickButtonCreate.bind(this);
    }

    onClickButtonCreate() {
        let data = {};
        data.description = this.state.description;
        data.name = this.state.name;
        data.directoryId = this.props.match.params.id;
        console.log(data);
        fetch("/api/Note/Create", {
            method: "POST",
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(data)
        })
            .then(x=> x.json())
            .then((data) => {
                if (data == true) {
                    this.props.history.push(c.rootDir + "/" + this.props.match.params.id);
                } else {
                    alert("Note was not created!");
                }
            });
    }

    onChangeInput(target, event) {
        let newState = this.state;
        newState[target] = event.target.value;
        this.setState(newState);
    }

    render() {
        return (
            <Fragment>
                <div className="form-group row">
                    <label className="col-sm-3 col-form-label text-right">Name</label>
                    <div className="col-sm-6">
                        <input onChange={(e) => this.onChangeInput("name", e)} className="form-control-black" type="text" name="Name" value={this.state.name} />
                    </div>
                </div>
                <div className="form-group row">
                    <label className="col-sm-3 col-form-label text-right">Description</label>
                    <div className="col-sm-6">
                        <input onChange={(e) => this.onChangeInput("description", e)} className="form-control-black" type="text" name="Desctiption" value={this.state.description} />
                    </div>
                </div>
                <div className="text-center">
                    <button onClick={this.onClickButtonCreate} className="btn btn-success">Create</button>
                    {linkSSRSafe(`${c.rootDir}/${this.props.match.params.id}`, "Back", null, "btn btn-warning")}
                </div>
            </Fragment>)
    }
}