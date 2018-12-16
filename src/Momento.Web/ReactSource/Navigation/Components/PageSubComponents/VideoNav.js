import React, { Component } from "react";

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
            <div className="video">
                <label>{this.props.video.name}</label>
                <div className="d-flex">
                    <a href={'/Video/View?id=' + this.props.video.id}>View</a>
                    <a className="ml-1" href={'/Video/Edit?id=' + this.props.video.id}>Edit</a>
                    <a className="ml-1" href="#" onClick={(e) => this.onClickDeleteVideo(e, this.props.video.id)} >Delete</a>
                </div>
            </div>
        );
    }
}

export default VideoNav;