import React, { Component, Fragment } from "react";
import * as c from "./Helpers/Constants";
import { extractVideoToken, handeleValidationErrors, clientSideValidation } from "./Helpers/HelperFuncs";
import YouTube from "react-youtube";
import ShowError from "./Helpers/ShowError"

export default class VideoNoteCreate extends Component {
    constructor(props) {
        super(props);
        this.state = {
            description: "",
            name: "",
            url: "",
            playerUrl: "",

            player: {},
            playerReady: false,

            offset: 3,
            length: 6,

            showErrors: true,
            ERRORS: [/*{ fieldName: "", errorMessages: ["",""]}*/],
        };
        this.onClickPlay = this.onClickPlay.bind(this);
        this.onChangeInput = this.onChangeInput.bind(this);

        this.onClickCreate = this.onClickCreate.bind(this);

        this.renderVideoProperties = this.renderVideoProperties.bind(this);
        this.renderPlayButton = this.renderPlayButton.bind(this);

        this.onReadyYoutubePlayer = this.onReadyYoutubePlayer.bind(this);
    }

    componentDidMount() {
        $("#iframeDiv").height($("#iframeDiv").width() * 9 / 16);
        window.addEventListener("resize", this.updateDimensions);
    }

    updateDimensions() {
        $("#iframeDiv").height($("#iframeDiv").width() * 9 / 16);
    }

    onClickCreate() {
        ///Reset The error messages
        ///ToDo a bug where setState({ERRORS: []}); does not clear all the errors 
        this.setState({ ERRORS: [] });
        console.log("After Cleaning");
        console.log(this.state.ERRORS);

        if (extractVideoToken(this.state.url).length == 0) {
            clientSideValidation("Not a valid YouTube URL!","URL", this);
            return;
        }

        let videoCreate = {};
        videoCreate.description = this.state.description;
        videoCreate.name = this.state.name;
        videoCreate.url = this.state.url;

        videoCreate.directoryId = this.props.match.params.id;

        fetch("/api/Video/Create", {
            method: "POST",
            headers: {
                'Accept': "application/json",
                'Content-Type': "application/json"
            },
            body: JSON.stringify(videoCreate)
        })
            .then(x => x.json())
            .then((data) => {
                console.log(data);

                if (data.hasOwnProperty("errors")) {
                    handeleValidationErrors(data.errors, this);
                    return;
                }

                if (data)
                    if (data == true) {
                        this.props.history.push(c.rootDir + "/" + this.props.match.params.id);
                    } else if (data == false) {
                        alert("VideoNotes were not created!");
                    }
            });
    }

    onChangeInput(event, target) {
        let newState = this.state;
        newState[target] = event.target.value;
        this.setState(newState);
    }

    onReadyYoutubePlayer(event) {
        let player = event.target;
        this.setState({
            player: player,
            playerReady: true,
        });
    }

    onClickPlay() {
        this.setState({
            playerUrl: this.state.url,
        });
    }

    renderVideoProperties() {
        var props = ["url", "name", "description"];
        return props.map(x =>
            <Fragment>
                <ShowError prop={x.toUpperCase()} ERRORS={this.state.ERRORS} showErrors={this.state.showErrors} />
                <div className="row" key={"videoprop" + x}>
                    <div className={"col-sm-" + this.state.offset + " text-right"}>
                        <label>{x}</label>
                    </div>
                    <div className={"col-sm-" + this.state.length}>
                        <input
                            className="form-control-black mb-3"
                            value={this.state[x]}
                            onChange={(e) => this.onChangeInput(e, x)}
                        >
                        </input>
                    </div>
                    {this.renderPlayButton(x)}
                </div>
            </Fragment>
        )
    }

    renderPlayButton(prop) {
        if (prop == "url") {
            return (
                <div className="col-sm-2">
                    <button
                        onClick={this.onClickPlay}
                        className="btn btn-primary">
                        Play
                    </button>
                </div>
            )
        }
    }

    renderPlayer() {
        return (
            <div className="row mb-4">
                <div id="iframeDiv" className="col-sm-10">
                    <YouTube
                        videoId={extractVideoToken(this.state.playerUrl)}
                        onReady={this.onReadyYoutubePlayer}
                        opts={{
                            width: "100%",
                            height: "100%",
                        }}
                    />
                </div>
            </div>
        )
    }

    render() {
        return (
            <Fragment>
                {this.renderPlayer()}
                {this.renderVideoProperties()}
                <Fragment>
                    <button onClick={this.onClickCreate} className="btn btn-success">Create</button>
                    <button onClick={() => window.history.back()} className="float-right btn btn-warning">Back</button>
                </Fragment>
            </Fragment>
        )
    }
}