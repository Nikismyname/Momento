import React, { Component, Fragment } from "react";
import { NavLink } from "react-router-dom";
import Textarea from "react-expanding-textarea";
import * as c from "./Helpers/Constants";
import { SortableContainer, SortableElement, arrayMove } from "react-sortable-hoc";
import LoadSvg from './Helpers/LoadSvg';
import { ContextMenu, MenuItem, ContextMenuTrigger } from "react-contextmenu";

import { library } from '@fortawesome/fontawesome-svg-core'
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome'
import { faArrowsAlt } from '@fortawesome/free-solid-svg-icons'
library.add(faArrowsAlt)

const compItemBorderString = "2px solid rgba(50, 50, 50, 0.50)";
const searchBarBorderString = "2px solid rgba(0, 0, 0, 0.5)";

const SortableItem = SortableElement(({ value, ind, index, _this }) => {
    if (_this.state.currentState.hiddenComps.includes(value.inPageId) || typeof value.deleted !== "undefined") {
        return <div></div>
    } else {
        return (
            <div>
                <div className="comparison-div mb-4">
                    <div className="d-flex">
                        <Textarea onChange={(e) => _this.onChangeTextArea(ind, "source", e)} style={{ overflow: "hidden", backgroundColor: c.secondaryColor, border: compItemBorderString }} type="text" value={_this.state.currentState.items[ind].source} className="form-control-black" />
                        <Textarea onChange={(e) => _this.onChangeTextArea(ind, "target", e)} style={{ overflow: "hidden", backgroundColor: c.secondaryColor, border: compItemBorderString }} type="text" value={_this.state.currentState.items[ind].target} className="form-control-black" />
                    </div>
                    <div className="d-flex">
                        <Textarea onChange={(e) => _this.onChangeTextArea(ind, "comment", e)} style={{ overflow: "hidden", backgroundColor: c.secondaryColor, border: compItemBorderString }} type="text" value={_this.state.currentState.items[ind].comment} className="form-control-black" />
                        <div className="anchor d-flex" style={{ backgroundColor: "black", alignItems: "center", justifyItems: "center" }}>
                            <ContextMenuTrigger id="comparisonItems" attributes={{ id: value.inPageId }}>
                                <div className="container text-center">
                                    <FontAwesomeIcon icon={faArrowsAlt} />
                                </div>
                            </ContextMenuTrigger>
                        </div>
                    </div>
                </div>
            </div>)
    }
});

const SortableList = SortableContainer(({ items, _this }) => {
    return (
        <ul className="pl-0">
            {items.map((value, index) => {
                return (<SortableItem key={`item-${index}`} index={index} ind={index} value={value} _this={_this} />)
            }
            )}
        </ul>
    );
});

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
                testItems: [1, 2, 3, 4, 5, 6],
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
                    loaded: false,

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

        this.renderSearchBar = this.renderSearchBar.bind(this);
        this.renderComparisonData = this.renderComparisonData.bind(this);

        this.fiterSerchResults = this.fiterSerchResults.bind(this);
        this.onSortEnd = this.onSortEnd.bind(this);
        this.onClickContexMenuItem = this.onClickContexMenuItem.bind(this);
    }

    componentDidUpdate() {
        let anchors = $(".anchor");
        console.log(anchors);
        for (var i = 0; i < anchors.length; i++) {
            console.log(anchors[i].clientWidth);
            console.log(anchors[i].clientHeight);
            $(anchors[i]).width($(anchors[i]).height());
        }
    }

    componentWillMount() {
        ///if we get the data from a prop, we do not downlad the data again
        //if (typeof this.props.initialComp != "undefined") {
        //    return;
        //}

        let id = this.props.match.params.id;
        let dirId = this.props.match.params.dirId;

        let comparisonId = id;

        fetch("/api/Comparison/Get", {
            method: "POST",
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(comparisonId)
        })
            .then((response) => {
                return response.json();
            })
            .then((data) => {

                ///Fix up the null values of properties from null to "" so the text fields 
                ///that connect to them are managed from the beginning and to avoid warnings
                let fieldsTocheckForNull = ["name", "description", "sourceLanguage", "targetLanguage"]
                let newCurrentState = JSON.parse(JSON.stringify(data));
                for (var i = 0; i < fieldsTocheckForNull.length; i++) {
                    if (newCurrentState[fieldsTocheckForNull[i]] === null) {
                        newCurrentState[fieldsTocheckForNull[i]] = "";
                    }
                }

                ///Only have it in the currentStat state because it is not persisted
                newCurrentState.hiddenComps = [];
                newCurrentState.searchComments = true;
                newCurrentState.searchSource = true;
                newCurrentState.searchTarget = true;
                newCurrentState.searchQuery = "";

                newCurrentState.loaded = true;

                ///Adding inPageIds to the preexisting Items
                for (var i = 0; i < newCurrentState.items.length; i++) {
                    newCurrentState.items[i].inPageId = i;
                }

                ///Reordering The preexisting items so they are in the same order the user left them
                newCurrentState.items = newCurrentState.items.sort((a, b) => a.order - b.order)

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
                    <input onChange={(e) => this.onChangeCompTextArea(x, e)} id="nameInput" value={this.state.currentState[x]} className="form-control-black" style={{ backgroundColor: c.secondaryColor }} />
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
                if (x.comment.toUpperCase().includes(value.toUpperCase())) {
                    matchesComment = true;
                }
            }

            if (this.state.currentState.searchTarget) {
                if (x.target.toUpperCase().includes(value.toUpperCase())) {
                    matchesTarger = true;
                }
            }

            if (this.state.currentState.searchSource) {
                if (x.source.toUpperCase().includes(value.toUpperCase())) {
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

    renderSearchBar() {
        return (
            <div className="row mb-4 mt-4">
                <div className=""></div>
                <div className="d-flex col-sm-6">
                    <label htmlFor="source">Source</label>
                    <input
                        id="source"
                        className="form-control-black"
                        type="checkbox"
                        checked={this.state.currentState.searchSource}
                        onChange={(e) => this.onChangeCheckBox(e, "searchSource")} />

                    <label htmlFor="target">Target</label>
                    <input
                        id="target"
                        className="form-control-black"
                        type="checkbox"
                        checked={this.state.currentState.searchTarget}
                        onChange={(e) => this.onChangeCheckBox(e, "searchTarget")} />

                    <label htmlFor="comment">Comment</label>
                    <input
                        id="comment"
                        className="form-control-black"
                        type="checkbox"
                        checked={this.state.currentState.searchComments}
                        onChange={(e) => this.onChangeCheckBox(e, "searchComments")} />
                </div>

                <div className="col-sm-6">
                    <input className="form-control-black" value={this.state.currentState.searchQuery} onChange={this.onChangeSearchQuery} style={{ backgroundColor: c.secondaryColor, border: searchBarBorderString }} />
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
        ///This saves the final order of the elements
        let newCurrentState = this.state.currentState;
        for (var i = 0; i < newCurrentState.items.length; i++) {
            newCurrentState.items[i].order = i;
        }
        this.setState({ currentState: newCurrentState });

        ///This collects all the newly created items
        let newItemsCollection = this.state.currentState.items.filter(x => x.id == 0 && typeof x.deleted === "undefined");

        ///This orders the preexisting items and their new versions by Id so I can
        ///Compare then to get the differences
        let changes = [];
        let preExistingItems = this.state.initialState.items.sort((a, b) => a.id - b.id);
        let currentStateOfPEItems = this.state.currentState.items.filter(x => x.id > 0).sort((a, b) => a.id - b.id);

        ///Check to make sure we didn't give nonzero id to a new item
        if (preExistingItems.length != currentStateOfPEItems.length) {
            alert("The preexisting collection is not the same size!")
            return;
        }

        ///Collect all the differences
        for (var i = 0; i < preExistingItems.length; i++) {
            let initialState = preExistingItems[i];
            let currentState = currentStateOfPEItems[i];
            for (let prop in currentState) {

                if (prop === "deleted") {
                    changes.push({
                        id: currentState.id,
                        propertyChanged: "deleted",
                        newValue: true,
                    });
                    continue;
                }

                ///InPageId is only for the front end
                if (prop == "inPageId") {
                    continue;
                }

                if (initialState[prop] != currentState[prop]) {
                    ///Check to make sure Ids are aligned
                    if (prop == "id") {
                        alert("Ids are not aligned ERROR")
                        return;
                    }

                    ///Push the difference in a collection
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
                if (data == true) {
                    this.props.history.push(c.rootDir + "/" + this.props.match.params.dirId);
                } else {
                    alert("The save did not work!");
                }
            });
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

    onClickContexMenuItem(e, data, target) {
        const id = parseInt(target.getAttribute('id'), 10);
        let newCurrentState = this.state.currentState;
        let itemToBeDeleted = newCurrentState.items.filter(x => x.inPageId == id);
        if (itemToBeDeleted.length != 1) {
            alert("item to be deleted id multiple items with the same id");
        }
        itemToBeDeleted = itemToBeDeleted[0];
        itemToBeDeleted.deleted = true;

        this.setState({ currentState: newCurrentState });
    }

    render() {
        const app = (
            <Fragment>
                {this.renderComparisonData()}
                {this.renderSearchBar()}
                <SortableList items={this.state.currentState.items} onSortEnd={this.onSortEnd} _this={this} />
                <div className="d-flex justify-content-around">
                    <button className="btn btn-success" onClick={this.onClickSave}>Save</button>
                    <button className="btn btn-primary" onClick={this.onClickNewComparison}>New Comparison</button>
                    <NavLink to={`${c.rootDir}/${this.state.currentState.directoryId}`} className="btn btn-warning">Back</NavLink>
                </div>
                <ContextMenu id="comparisonItems">
                    <div style={{ color: "white", backgroundColor: "black" }}>
                        <MenuItem data={{ action: 'delete' }} onClick={this.onClickContexMenuItem}>
                            Delete
                            </MenuItem>
                    </div>
                </ContextMenu>
            </Fragment>)

        if (this.state.currentState.loaded == true) {
            return app;
        } else {
            return <LoadSvg />
        }
    }
}