import React, { Component } from "react";
import { linkSSRSafe } from '../Helpers/HelperFuncs';
import * as c from '../Helpers/Constants';

class VideoNav extends Component {
    constructor(props) {
        super(props);
    }

    onClickDeleteNote(e, id) {
        e.preventDefault;

        if (confirm("Are you sure you want to delete this note?") == false) {
            return;
        }

        fetch("/api/Note/" + id, {
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
                    let newState = this.props.parentState;
                    let newCurrentDir = newState.currentDir;
                    newCurrentDir.notes = newCurrentDir.notes.filter(x => x.id != id);

                    let newHistory = newState.history;
                    newHistory = newHistory.map(obj => {
                        return obj.id == newCurrentDir.id ? newCurrentDir : obj;
                    });

                    this.props.setStateFunc(newState)

                } else {
                    alert("Delete note did not work!");
                }
            });
    }

    render() {
        return (
            <div data-tip="Note" className="card mb-2" style={{ border: c.noteBorder }}>
                <div className="card-body">
                    <h6 className="card-title">{this.props.note.name}</h6>
                    <p className="card-text">{this.props.note.description}</p>
                    {linkSSRSafe(c.rootDir + c.richTextNotePath + "/" + this.props.note.id + "/" + this.props.parentState.currentDir.id, "Edit", null)}
                    <a className="ml-1" href="#" onClick={(e) => this.onClickDeleteNote(e, this.props.note.id)} >Delete</a>
                </div>
            </div>
        );
    }
}

export default VideoNav;