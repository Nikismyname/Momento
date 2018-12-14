import React, { Component, Fragment } from 'react';
import { NavLink } from "react-router-dom";
import Textarea from 'react-expanding-textarea';
import rootDir from "./Helpers/RootDir";

export default class Compare extends Component {
    constructor(props) {
        super(props);

        if (typeof this.props.initialComp != "undefined") {
            this.state = {
                initialState: JSON.parse(JSON.stringify(this.props.initialComp)),
                currentState: JSON.parse(JSON.stringify(this.props.initialComp)),
            }
        } else {
            this.state = {
                initialState: {
                    id: -1,
                    directoryId: null,
                    name: "",
                    description: "",
                    sourceLanguage: "",
                    targetLanguage: "", 
                    items: [],
                },
                currentState: {
                    id: -1,
                    directoryId: null,
                    name: "",
                    description: "",
                    sourceLanguage: "",
                    targetLanguage: "",
                    items: [],
                },
            };
        }

        this.onClickNewComparison = this.onClickNewComparison.bind(this);
        this.textAreaOnChanger = this.textAreaOnChange.bind(this);
        this.compTextAreaOnChange = this.compTextAreaOnChange.bind(this);
        this.onClickSave = this.onClickSave.bind(this);
    }

    componentWillMount() {
        //if we get the data from a prop, we do not downlad the data again
        if (typeof this.props.initialComp != "undefined") {
            return;
        }

        let id = this.props.match.params.id;
        let dirId = this.props.match.params.dirId;

        var data = {};
        data.comparisonId = id;

        if (this.props.id > 0)/*calling for existing props*/ {
            ///we do not care about this
            data.parentDirId = 0;
        } else {
            data.parentDirId = dirId;
        }

        fetch("/api/Comparison/Get", {
            method: "POST",
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(data)
        })
            .then((response) => {
                return response.json();
            })
            .then((data) => {
                console.log(data);
                this.setState({
                    initialState: JSON.parse(JSON.stringify(data)),
                    currentState: JSON.parse(JSON.stringify(data)),
                });
            });
    }

    compTextAreaOnChange(propName, event) {
        let newState = this.state.currentState;
        newState[propName] = event.target.value;
        this.setState({ currentState: newState });
    }

    renderComparisonData() {
        let fields = ["name", "description", "sourceLanguage", "targetLanguage"];

        return fields.map(x =>
            <div className="form-group row" key={x}>
                <label className="col-sm-2 col-form-label text-right">{x}</label>
                <div className="col-sm-6">
                    <input onChange={(e) => this.compTextAreaOnChange(x, e)} id="nameInput" value={this.state.currentState[x]} className="form-control-black" />
                </div>
            </div>);
    }

    textAreaOnChange(index, type, event) {
        let newCurrentState = this.state.currentState;
        newCurrentState.items[index][type] = event.target.value;
        this.setState({
            currentState: newCurrentState,
        });
    }

    renderCompItemsTextAreas() {
        return this.state.currentState.items.map((x, ind) =>
            <div className="comparison-div mb-4" key={x.inPageId}>
                <div className="d-flex">
                    <Textarea onChange={(e) => this.textAreaOnChange(ind, "source", e)} style={{ overflow: "hidden" }} type="text" value={this.state.currentState.items[ind].source} className="form-control-black" />
                    <Textarea onChange={(e) => this.textAreaOnChange(ind, "target", e)} style={{ overflow: "hidden" }} type="text" value={this.state.currentState.items[ind].target} className="form-control-black" />
                </div>
                <Textarea onChange={(e) => this.textAreaOnChange(ind, "comment", e)} style={{ overflow: "hidden" }} type="text" value={this.state.currentState.items[ind].comment} placeholder={x.id} className="form-control-black" />
            </div>)
    }

    onClickNewComparison() {
        let nextId;
        if (this.state.currentState.items.length == 0) {
            nextId = 0;
        } else {
            nextId = Math.max(...this.state.currentState.items.map(x => x.inPageId)) + 1;
        }

        var newState = this.state.currentState;
        newState.items.push({
            inPageId: nextId,
            dbId: 0,
            source: "",
            target: "",
            comment: "",
        })

        this.setState({
            currentState: newState,
        });
    }

    onClickSave() {
        ///This collects all the newly created items
        let newItemsCollection = this.state.currentState.items.filter(x => x.dbId == 0);

        let changes = [];
        let preExistingItems = this.state.initialState.items.sort((a, b) => a - b);
        let currentStateOfPEItems = this.state.currentState.items.filter(x => x.dbId > 0).sort((a, b) => a - b);

        if (preExistingItems.length != currentStateOfPEItems.length) {
            alert("The preexisting collection is not the same size!")
            return;
        }

        for (var i = 0; i < preExistingItems.length; i++) {
            initialState = preExistingItems[i];
            currentState = currentStateOfPEItems[i];
            for (let prop in initialState) {

                if (initialState[prop] != currentState[prop]) {
                    if (prop == "id") {
                        alert("Existring Item Id changed ERROR")
                        return;
                    }
                    changes.push({
                        id: currentState.id,
                        propertyChanged: prop,
                        newValue: currentState[prop],
                    });
                }
            }
        }

        var data =
        {
            id: this.state.initialState.id,
            newItems: newItemsCollection,
            alteredItems: changes,
        };

        var trackedProps = ["description", "name", "sourceLanguage", "targetLanguage"  ];

        for (var i = 0; i < trackedProps.length; i++) {
            var prop = trackedProps[i];
            var result = this.state.currentState[prop] != this.state.initialState[prop]
                ? this.state.currentState[prop] : null;
            data[prop] = result;
        }

        fetch("/api/Comparison/Save", {
            method: "POST",
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(data)
        })
            .then((response) => {
                return response.json();
            })
            .then((data) => {
                console.log(data);
            });
    }

    render() {
        return (
            <Fragment>
                <div className="text-center">
                    {this.state.currentState.id}
                </div >
                {this.renderComparisonData()}
                {this.renderCompItemsTextAreas()}
                <NavLink to={`${rootDir}/${this.state.currentState.directoryId}`} className="btn btn-primary">Back</NavLink>
                <button onClick={this.onClickNewComparison}>New Comparison</button>
                <button className="btn btn-success" onClick={this.onClickSave}>Save</button>
            </Fragment>)
    }
}
