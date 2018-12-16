import React, { Component } from "react";
import { linkSSRSafe } from '../Helpers/HelperFuncs';
import rootDir from '../Helpers/RootDir';

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
            <div className="video" key={"comparison" + this.props.comp.id}>
                {linkSSRSafe(rootDir + "/compare/" + this.props.comp.id + "/0", this.props.comp.name + " " + this.props.comp.itemsCount, null)}
                <a href="#" className="ml-1" onClick={(e) => this.onClickDeleteComp(e, this.props.comp.id)}>Delete</a>
            </div>);
    }
}

export default ComparisonNav;