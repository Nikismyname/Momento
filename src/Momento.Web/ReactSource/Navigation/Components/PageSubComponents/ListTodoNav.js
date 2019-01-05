import React, { Component } from "react";
import * as c from "../Helpers/Constants";

class ListTodoNav extends Component {
    constructor(props) {
        super(props);
    }

    onClickDeletelist(e, id) {
        e.preventDefault();

        if (confirm("Are you sure you want to delete this listarison") == false) {
            return;
        }

        fetch("/api/ListToDo", {
            method: "DELETE",
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
                    newCurrentDir.listsToDo = newCurrentDir.listsToDo.filter(x => x.id != id);
                    let newHistory = this.props.parentState.history;
                    newHistory = newHistory.map(obj => {
                        return obj.id == newCurrentDir.id ? newCurrentDir : obj;
                    });

                    this.props.setStateFunc({
                        currentDir: newCurrentDir,
                        history: newHistory,
                    });
                } else {
                    alert("Delete List did not work!")
                }
            });
    }

    render() {
        return (
            <div data-tip="ToDo list" className="card mb-2" style={{ border: c.listToDoBorder }}>
                <div className="card-body">
                    <h6 className="card-title">{this.props.list.name}</h6>
                    <p className="card-text">{this.props.list.description}</p>
                    <a href={"/ListToDo/Use/" + this.props.list.id}>Use</a>
                    <a href="#" className="ml-1" onClick={(e) => this.onClickDeletelist(e, this.props.list.id)}>Delete</a>
                </div>
            </div>
        );
    }
}

export default ListTodoNav;