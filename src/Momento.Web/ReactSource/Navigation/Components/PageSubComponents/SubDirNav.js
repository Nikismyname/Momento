import React, { Component, Fragment } from "react";

const borderString = "3px solid rgba(100, 100, 100, 0.6)"

class SubDirNav extends Component {
    constructor(props) {
        super(props);
    }

    render() {

        return (
            <Fragment>
                <div data-tip="Folder" className="card mb-2"
                    style={{ border: borderString }}
                    onClick={() => { console.log("HERE"); this.props.navigateToDirectory(this.props.dir.id); }}>
                    <div className="card-body">
                        <h6 className="card-title">{this.props.dir.name}</h6>
                    </div>
                </div>
            </Fragment>
        );
    }
}

export default SubDirNav;