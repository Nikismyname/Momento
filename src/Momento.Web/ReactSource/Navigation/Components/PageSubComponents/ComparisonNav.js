import React, { Component } from "react";
import { linkSSRSafe } from '../Helpers/HelperFuncs';
import * as c from '../Helpers/Constants';

class ComparisonNav extends Component {
    constructor(props) {
        super(props);
    }

    onClickDeleteComp(e, id) {
        e.preventDefault();

        if (confirm("Are you sure you want to delete this Comparison") == false) {
            return;
        }

        fetch("/api/Comparison/Delete", {
            method: "POST",
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(id)
        })
            .then((response) => {
                return response.json();
            })
            .then((data) => {
                if (data == true) {
                    let newCurrentDir = this.props.parentState.currentDir;
                    newCurrentDir.comparisons = newCurrentDir.comparisons.filter(x => x.id != id);
                    let newHistory = this.props.parentState.history;
                    newHistory = newHistory.map(obj => {
                        return obj.id == newCurrentDir.id ? newCurrentDir : obj;
                    });

                    this.props.setStateFunc({
                        currentDir: newCurrentDir,
                        history: newHistory,
                    });
                } else {
                    alert("Delete did not work!")
                }
            });
    }

    render() {
        return (
            <div data-tip="Comparison" className="card mb-2" style={{ border: c.comparisonBorder }}>
                <div className="card-body">
                    <h6 className="card-title">{this.props.comp.name}</h6>
                    <p className="card-text">{this.props.comp.description}</p>
                    <p className="card-text">Number of items: {this.props.comp.itemsCount}</p>
                    {linkSSRSafe(c.rootDir + c.comparePath +"/"+ this.props.comp.id + "/0", "Edit", null)}
                    <a href="#" className="ml-1" onClick={(e) => this.onClickDeleteComp(e, this.props.comp.id)}>Delete</a>
                </div>
            </div>
        )
    }
}

export default ComparisonNav;