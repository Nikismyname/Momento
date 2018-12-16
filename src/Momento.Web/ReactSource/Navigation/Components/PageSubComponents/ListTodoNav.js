import React, { Component } from "react";
//import { linkSSRSafe } from '../Helpers/HelperFuncs';
//import rootDir from '../Helpers/RootDir';

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
            <div className="video" key={"listToDo" + this.props.list.id}>
                <label>{this.props.list.name}</label>
                <div className="d-flex">
                    <a href={"/ListToDo/Use/" + this.props.list.id}>Use</a>
                    <a href="#" className="ml-1" onClick={(e) => this.onClickDeletelist(e, this.props.list.id)}>Delete</a>
                </div>
            </div>);
    }
}

export default ListTodoNav;