import React, { Component } from "react";

const borderString = "3px solid rgba(100, 100, 100, 0.6)"

class SubDirNav extends Component {
    constructor(props) {
        super(props);
    }

    render2() {
        return (
            <div className="directory-react" onClick={() => this.props.navigateToDirectory(this.props.dir.id)}>
                <label>{this.props.dir.name}</label>
            </div>
        );
    }

    render() {
        return (
            <div data-tip="Folder" className="card mb-2"
                 style={{ border: borderString }}
                 onClick={() => this.props.navigateToDirectory(this.props.dir.id)}>
                <div className="card-body">
                    <h6 className="card-title">{this.props.dir.name}</h6>
                </div>
            </div>
        );
    }
}

export default SubDirNav;