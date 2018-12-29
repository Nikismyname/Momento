import React, { Component, Fragment } from 'react';
import { NavLink } from "react-router-dom";
import Textarea from 'react-expanding-textarea';
import * as c from "./Helpers/Constants";
import { SortableContainer, SortableElement, arrayMove } from 'react-sortable-hoc';

export default class Compare extends Component {

    constructor(props) {
        super(props);

        if (typeof this.props.initialComp != "undefined" && this.props.initialComp != null) {
            console.log("server side");
            this.state = {
                initialState: JSON.parse(JSON.stringify(this.props.initialComp)),
                currentState: JSON.parse(JSON.stringify(this.props.initialComp)),
            }
        } else {
            console.log("no server side")
            this.state = {
                testItems: [1,2,3,4,5,6],
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
                    hiddenComps: [],
                    searchComments: true,
                    searchSource: true,
                    searchTarget: true,
                    searchQuery: "",

                    id: -1,
                    directoryId: null,
                    name: "",
                    description: "",
                    sourceLanguage: "",
                    targetLanguage: "",
                    items: [], /// {inPageId: 0, id: 0, comment: "", source: "", targer: "", order: 0,  }
                },
            };
        }

        this.onChangeTextArea = this.onChangeTextArea.bind(this);
        this.onChangeCompTextArea = this.onChangeCompTextArea.bind(this);
        this.onChangeSearchQuery = this.onChangeSearchQuery.bind(this);
        this.onChangeCheckBox = this.onChangeCheckBox.bind(this);

        this.onClickSave = this.onClickSave.bind(this);
        this.onClickNewComparison = this.onClickNewComparison.bind(this);

        this.renderCompItems = this.renderCompItems.bind(this);
        this.renderSearchBar = this.renderSearchBar.bind(this);
        this.renderComparisonData = this.renderComparisonData.bind(this);

        this.fiterSerchResults = this.fiterSerchResults.bind(this);
        this.onSortEnd = this.onSortEnd.bind(this);
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

                let fieldsTocheckForNull = ["name", "description", "sourceLanguage", "targetLanguage"]
                let newCurrentState = JSON.parse(JSON.stringify(data));
                for (var i = 0; i < fieldsTocheckForNull.length; i++) {
                    if (newCurrentState[fieldsTocheckForNull[i]] === null) {
                        newCurrentState[fieldsTocheckForNull[i]] = "";
                    }
                }

                newCurrentState.hiddenComps = [];

                for (var i = 0; i < newCurrentState.items.length; i++) {
                    newCurrentState.items[i].inPageId = i;
                }

                newCurrentState.searchComments = true;
                newCurrentState.searchSource = true;
                newCurrentState.searchTarget = true;
                newCurrentState.searchQuery = "";

                this.setState({
                    initialState: JSON.parse(JSON.stringify(data)),
                    currentState: newCurrentState,
                });
            });
    }

    onChangeCompTextArea(propName, event) {
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
                    <input onChange={(e) => this.onChangeCompTextArea(x, e)} id="nameInput" value={this.state.currentState[x]} className="form-control-black" />
                </div>
            </div>);
    }

    onChangeTextArea(index, type, event) {
        let newCurrentState = this.state.currentState;
        newCurrentState.items[index][type] = event.target.value;
        this.setState({
            currentState: newCurrentState,
        });
    }

    onChangeSearchQuery(e) {
        let value = e.target.value
        let newCurrentState = this.state.currentState;
        newCurrentState.searchQuery = value;
        this.setState({
            currentState: newCurrentState,
        });

        if (this.state.currentState.searchComments == false &&
            this.state.currentState.searchSource == false &&
            this.state.currentState.searchTarget == false) {
            let newCurrentState3 = this.state.currentState;
            newCurrentState3.hiddenComps = [];
            this.setState({ currentState: newCurrentState3 });
            return;
        }

        this.fiterSerchResults(value);
    }

    fiterSerchResults(value) {

        if (value.length == 0) {
            let newCurrentState3 = this.state.currentState;
            newCurrentState3.hiddenComps = [];
            this.setState({ currentState: newCurrentState3 });
            return;
        }

        let compsToHide = this.state.currentState.items.filter(x => {
            let matchesComment = false;
            let matchesTarger = false;
            let matchesSource = false;

            if (this.state.currentState.searchComments) {
                if (x.comment.includes(value)) {
                    matchesComment = true;
                }
            }

            if (this.state.currentState.searchTarget) {
                if (x.target.includes(value)) {
                    matchesTarger = true;
                }
            }

            if (this.state.currentState.searchSource) {
                if (x.source.includes(value)) {
                    matchesSource = true;
                }
            }

            if (matchesComment || matchesSource || matchesTarger) {
                return false;
            } else {
                return true;
            }
        }).map(x => x.inPageId);

        let newCurrentState = this.state.currentState;
        newCurrentState.hiddenComps = compsToHide;

        this.setState({
            currentState: newCurrentState,
        });
    }

    onChangeCheckBox(e, propName) {
        let newCurrentState = this.state.currentState;
        newCurrentState[propName] = e.target.checked;

        this.setState({
            currentState: newCurrentState,
        });

        if (this.state.currentState.searchComments == false &&
            this.state.currentState.searchSource == false &&
            this.state.currentState.searchTarget == false) {
            let newCurrentState3 = this.state.currentState;
            newCurrentState3.hiddenComps = [];
            this.setState({ currentState: newCurrentState3 });
            return;
        }

        this.fiterSerchResults(this.state.currentState.searchQuery);
    }

    renderCompItems() {
        console.log(this.state.currentState.hiddenComps);
        return this.state.currentState.items
            .filter(x => !this.state.currentState.hiddenComps.includes(x.inPageId))
            .map((x, ind) =>
                <div className="comparison-div mb-4" key={"CompItem" + x.inPageId}>
                    <div className="d-flex">
                        <Textarea onChange={(e) => this.onChangeTextArea(ind, "source", e)} style={{ overflow: "hidden" }} type="text" value={this.state.currentState.items.filter(y => y.inPageId == x.inPageId)[0].source} className="form-control-black" />
                        <Textarea onChange={(e) => this.onChangeTextArea(ind, "target", e)} style={{ overflow: "hidden" }} type="text" value={this.state.currentState.items.filter(y => y.inPageId == x.inPageId)[0].target} className="form-control-black" />
                    </div>
                    <Textarea onChange={(e) => this.onChangeTextArea(ind, "comment", e)} style={{ overflow: "hidden" }} type="text" value={this.state.currentState.items.filter(y => y.inPageId == x.inPageId)[0].comment} placeholder={x.inPageId} className="form-control-black" />
                    <span>{x.inPageId}</span>
                </div>
            )
    }

    renderSearchBar() {
        return (
            <div className="row mb-4 mt-4">
                <div className=""></div>
                <div className="d-flex col-sm-6">
                    <input
                        id="source"
                        className="form-control-black"
                        type="checkbox"
                        checked={this.state.currentState.searchSource}
                        onChange={(e) => this.onChangeCheckBox(e, "searchSource")} />
                    <label htmlFor="source">Source</label>
                    <input
                        id="target"
                        className="form-control-black"
                        type="checkbox"
                        checked={this.state.currentState.searchTarget}
                        onChange={(e) => this.onChangeCheckBox(e, "searchTarget")} />
                    <label htmlFor="target">Target</label>

                    <input
                        id="comment"
                        className="form-control-black"
                        type="checkbox"
                        checked={this.state.currentState.searchComments}
                        onChange={(e) => this.onChangeCheckBox(e, "searchComments")} />
                    <label htmlFor="comment">Comment</label>
                </div>

                <div className="col-sm-6">
                    <input className="form-control-black" value={this.state.currentState.searchQuery} onChange={this.onChangeSearchQuery} />
                </div>
            </div>)
    }

    onClickNewComparison() {
        let nextId;
        if (this.state.currentState.items.length == 0) {
            nextId = 0;
        } else {
            nextId = Math.max(...this.state.currentState.items.map(x => x.inPageId)) + 1;
        }

        let nextOrder;
        if (this.state.currentState.items.length == 0) {
            nextOrder = 0;
        } else {
            nextOrder = Math.max(...this.state.currentState.items.map(x => x.order)) + 1;
        }

        var newState = this.state.currentState;
        newState.items.push({
            id: 0,
            inPageId: nextId,
            order: nextOrder,
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
        let newItemsCollection = this.state.currentState.items.filter(x => x.id == 0);
        console.log(newItemsCollection);

        let changes = [];
        let preExistingItems = this.state.initialState.items.sort((a, b) => a - b);
        let currentStateOfPEItems = this.state.currentState.items.filter(x => x.id > 0).sort((a, b) => a - b);

        if (preExistingItems.length != currentStateOfPEItems.length) {
            alert("The preexisting collection is not the same size!")
            return;
        }

        ///TODO: Deleteion is not covered yet
        for (var i = 0; i < preExistingItems.length; i++) {
            let initialState = preExistingItems[i];
            let currentState = currentStateOfPEItems[i];
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

        var trackedProps = ["description", "name", "sourceLanguage", "targetLanguage"];
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
        const SortableItem = SortableElement(({ value, index }) =>
            <div className="comparison-div mb-4" key={"CompItem" + value.inPageId}>
                <div className="d-flex">
                    <Textarea onChange={(e) => this.onChangeTextArea(index, "source", e)} style={{ overflow: "hidden" }} type="text" value={this.state.currentState.items.filter(y => y.inPageId == value.inPageId)[0].source} className="form-control-black" />
                    <Textarea onChange={(e) => this.onChangeTextArea(index, "target", e)} style={{ overflow: "hidden" }} type="text" value={this.state.currentState.items.filter(y => y.inPageId == value.inPageId)[0].target} className="form-control-black" />
                </div>
                <Textarea onChange={(e) => this.onChangeTextArea(index, "comment", e)} style={{ overflow: "hidden" }} type="text" value={this.state.currentState.items.filter(y => y.inPageId == value.inPageId)[0].comment} className="form-control-black" />
                <span>{value.inPageId}</span>
            </div>
        );

        const SortableList = SortableContainer(({ items }) => {
            return (
                <ul>
                    {items.map((value, index) => (
                        <SortableItem key={`item-${index}`} index={index} value={value} />
                    ))}
                </ul>
            );
        });

       let itemsTorender = this.state.currentState.items
                .filter(x => !this.state.currentState.hiddenComps.includes(x.inPageId))

        return (
            <Fragment>
                <div className="text-center">
                    {this.state.currentState.id}
                </div >
                {this.renderComparisonData()}
                {this.renderSearchBar()}
                {/*this.renderCompItems()*/}
                <SortableList items={itemsTorender} onSortEnd={this.onSortEnd} />
                <NavLink to={`${c.rootDir}/${this.state.currentState.directoryId}`} className="btn btn-primary">Back</NavLink>
                <button onClick={this.onClickNewComparison}>New Comparison</button>
                <button className="btn btn-success" onClick={this.onClickSave}>Save</button>
                <h1>V01</h1>
            </Fragment>)
    }

    onSortEnd({ oldIndex, newIndex }) {
        ///Stop sorting when searching since it makes no sense to sort there
        if (this.state.currentState.hiddenComps.length > 0) {
            return;
        }

        let newCurrentState = this.state.currentState;
        newCurrentState.items = arrayMove(newCurrentState.items, oldIndex, newIndex),
            this.setState({
                currentState: newCurrentState,
        });
    };
}

