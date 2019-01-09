import React, { Component } from "react";
import { linkSSRSafe } from '../Helpers/HelperFuncs';
import * as c from '../Helpers/Constants';

class VideoNav extends Component {
    constructor(props) {
        super(props);
    }

    onClickDeleteVideo(e, id) {
        e.preventDefault;

        if (confirm("Are you sure you want to delete this video?") == false) {
            return;
        }

        fetch("/api/Video/Delete", {
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
                    let newState = this.props.parentState;
                    let newCurrentDir = newState.currentDir;
                    newCurrentDir.videos = newCurrentDir.videos.filter(x => x.id != id);

                    let newHistory = newState.history;
                    newHistory = newHistory.map(obj => {
                        return obj.id == newCurrentDir.id ? newCurrentDir : obj;
                    });

                    this.props.setStateFunc(newState)

                } else {
                    alert("Delete video did not work!");
                }
            });
    }

    render() {
        return (
            <div data-tip="Video Notes" className="card mb-2" style={{ border: c.videoNotesBorder }}>
                <div className="card-body">
                    <h6 className="card-title">{this.props.video.name}</h6>
                    <p className="card-text">{this.props.video.description}</p>
                    <h6>{"Notes count: "+this.props.video.notesCount}</h6>
                    {linkSSRSafe(c.rootDir + c.videoViewPath + "/" + this.props.video.id, "View", null, null)}
                    {linkSSRSafe(c.rootDir + c.videoNotesPath + "/" + this.props.video.id + "/" + this.props.parentState.currentDir.id, "Edit", null, "ml-1")}
                    <a className="ml-1" href="#" onClick={(e) => this.onClickDeleteVideo(e, this.props.video.id)} >Delete</a>
                </div>
            </div>
        );
    }
}

export default VideoNav;









//<a className="ml-1" href={'/Video/Edit?id=' + this.props.video.id}>Edit</a>