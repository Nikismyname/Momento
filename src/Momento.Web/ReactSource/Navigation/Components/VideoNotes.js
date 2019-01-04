import React, { Component, Fragment } from "react";
import { ContextMenu, MenuItem, ContextMenuTrigger } from "react-contextmenu";
import ContentEditable from 'react-contenteditable'
import YouTube from 'react-youtube';
import { SketchPicker } from 'react-color';
import ReactTooltip from 'react-tooltip';

import * as c from "./Helpers/Constants";
import { extractVideoToken, clone, cnvRGBToStr, createBorder } from "./Helpers/HelperFuncs";
import LoadSvg from './Helpers/LoadSvg';

export default class VideoNotes extends Component {

    constructor(props) {
        super(props);

        this.types = { "note": 0, "time-stamp": 1, "topic": 2 }

        this.state = {
            fetchedData: false,
            settings: [],
            initialState: {}, /// {notes:[], videoData: {}}
            notes: [], ///{id: 1/null, inPageId: 0, inPageParentId: null/0, parentDbId: null/1
            ///content: "", level: 0, deleted: false, formatting: 0, type: 0, seekTo:0,
            ///backgroundColor: ""/null, textColor: ""/null,  borderColor: ""/null, borderThickness: 1}
            ///NonPersistant: selectingColor: false
            videoData: {}, ///{description: "", directoryId: 1, id: 1, 
            ///name: "", order: 1, seekTo: 0, url: ""}
            player: {},
            playerReady: false,
            showOptions: false,
            currentUrl: "",
            offset: 2,
            length: 6,

            color: {},
            selectingColor: false,
            itemLastSelectedFor: -1,

            userHasCreatedNote: false,

            youtubePlayerPlaying: false,

            subNoteInfo: [null, null, null],

            thicknessInput: "",
        }
        this.references = [];
        this.captureNewlyCreatedNoteRefs = this.captureNewlyCreatedNoteRefs.bind(this);

        this.onReadyYoutubePlayer = this.onReadyYoutubePlayer.bind(this);

        this.onFocusNote = this.onFocusNote.bind(this);
        this.onBlurNote = this.onBlurNote.bind(this);

        this.onChangeNote = this.onChangeNote.bind(this);
        this.onChangeInput = this.onChangeInput.bind(this);
        this.onChangeCompleteColorPicker = this.onChangeCompleteColorPicker.bind(this);
        this.onStateChangeYoutubePlayer = this.onStateChangeYoutubePlayer.bind(this);
        this.onChangeThiknessContextMenu = this.onChangeThiknessContextMenu.bind(this);

        this.renderNote = this.renderNote.bind(this);
        this.renderAllNotes = this.renderAllNotes.bind(this);
        this.renderVideoPlayerAndTopControlls = this.renderVideoPlayerAndTopControlls.bind(this);
        this.renderBottomControlls = this.renderBottomControlls.bind(this)
        this.renderPlayPauseButton = this.renderPlayPauseButton.bind(this);
        this.renderOptions = this.renderOptions.bind(this);
        this.renderVideoProperties = this.renderVideoProperties.bind(this);
        this.renderPlayButton = this.renderPlayButton.bind(this);
        this.renderColorPicker = this.renderColorPicker.bind(this);
        this.renderSaveAndDoNotSave = this.renderSaveAndDoNotSave.bind(this);

        this.onClickContexMenuNoteMakeNewTopic = this.onClickContexMenuNoteMakeNewTopic.bind(this);
        this.onClickContexMenuNoteMakeTimeStamp = this.onClickContexMenuNoteMakeTimeStamp.bind(this);
        this.onClickContexMenuNoteDelete = this.onClickContexMenuNoteDelete.bind(this);
        this.onClickContexMenuNoteSeekTo = this.onClickContexMenuNoteSeekTo.bind(this);
        this.onClickContexMenuChangeColor = this.onClickContexMenuChangeColor.bind(this);
        this.onClickContexMenuChangeBorderWithInputs = this.onClickContexMenuChangeBorderWithInputs.bind(this);

        this.onClickAddNote = this.onClickAddNote.bind(this);
        this.onClickColorPickerCancel = this.onClickColorPickerCancel.bind(this);
        this.onClickColorPickerDone = this.onClickColorPickerDone.bind(this);
        this.onClickOptions = this.onClickOptions.bind(this);
        this.onClickPlay = this.onClickPlay.bind(this);
        this.onClickSave = this.onClickSave.bind(this);
        this.onClickBoBackToVideo = this.onClickBoBackToVideo.bind(this);
        this.onClickPausePlayVideo = this.onClickPausePlayVideo.bind(this);
    }

    captureNewlyCreatedNoteRefs(element) {
        if (element == null) {
            console.log("null element:");
            console.log(element);
            return;
        }
        if (this.state.userHasCreatedNote) {
            element.el.current.focus();
        }
        this.references.push(element);
        console.log(this.references);
    }

    componentWillMount() {
        let id = this.props.match.params.id;
        let videoId = id;

        fetch("/api/Video/GetForEdit", {
            method: "POST",
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(videoId)
        })
            .then((response) => {
                return response.json();
            })
            .then((data) => {
                if (data != null) {
                    let notes = data.contentCreate.notes;
                    let videoData = data.contentCreate;
                    delete videoData.notes;

                    let initialState = {
                        notes: clone(notes),
                        videoData: clone(videoData),
                    };

                    ///Data modification that should not be persisted
                    for (var i = 0; i < notes.length; i++) {
                        notes[i].shouldExpand = false;
                        notes[i].selectingColor = false;
                        if (notes[i].borderColor == null) {
                            switch (notes[i].type) {
                                case 0: notes[i].borderColor = c.noteBorderColor; break;
                                case 1: notes[i].borderColor = c.timeStampBorderColor; break;
                                case 2: notes[i].borderColor = c.topicBorderColor; break;
                            }
                        }

                        if (notes[i].borderThickness == 0) {
                            notes[i].borderThickness = 1;
                        }
                    }
                    ///...
                    console.log("Notes Here:");
                    console.log(notes);

                    this.setState({
                        initialState: initialState,
                        settings: data.settings,
                        notes: notes,
                        videoData: videoData,
                        fetchedData: true,
                        currentUrl: videoData.url,
                    });

                } else {
                    alert("Loading video notes data failed!");
                }
            });

    }

    componentDidMount() {
        $("#iframeDiv").height($("#iframeDiv").width() * 9 / 16);
        window.addEventListener("resize", this.updateDimensions);
    }

    updateDimensions() {
        $("#iframeDiv").height($("#iframeDiv").width() * 9 / 16);
    }

    onChangeNote(e, inPageId) {
        let newNotes = this.state.notes;
        newNotes.filter(x => x.inPageId == inPageId)[0].content = e.target.value;
        this.setState({ notes: newNotes });
    }

    onChangeInput(e, propName) {
        var newVideoData = this.state.videoData;
        newVideoData[propName] = e.target.value;
        this.setState({
            videoData: newVideoData,
        });
    }

    onChangeCompleteColorPicker(color) {
        console.log(color);
        this.setState({ color: color });
    }

    onStateChangeYoutubePlayer(event) {
        if (event.data == 2) {//paused
            this.setState({ youtubePlayerPlaying: false })
        } else {
            this.setState({ youtubePlayerPlaying: true })
        }
    }

    onChangeThiknessContextMenu(e) {
        this.setState({
            thicknessInput: e.target.value,
        });
    }

    onFocusNote(inPageId) {
        let newNotes = this.state.notes;
        newNotes.filter(x => x.inPageId == inPageId)[0].shouldExpand = true;
        this.setState({ notes: newNotes });
    }
    onBlurNote(inPageId) {
        let newNotes = this.state.notes;
        newNotes.filter(x => x.inPageId == inPageId)[0].shouldExpand = false;
        this.setState({ notes: newNotes });
    }

    renderAllNotes(notes) {
        notes = notes.filter(x => x.deleted == false);

        var rootNotes = notes
            .filter(x => x.inPageParentId == null)
            .sort((a, b) => a.inPageId - b.inPageId);

        return rootNotes.map(x => this.renderNestedNote(x, notes))
    }

    renderNestedNote(note, allNotes) {
        this.renderNote(note);
        var childNotes = allNotes
            .filter(x => x.inPageParentId == note.inPageId)
            .sort((a, b) => a.inPageId - b.inPageId);

        return (
            <Fragment key={"nestedNote" + note.inPageId}>
                {this.renderNote(note)}
                <div className="ml-3">
                    {this.renderCildNotes(childNotes, allNotes)}
                </div>
            </Fragment>
        )
    }
    renderCildNotes(childNotes, allNotes) {
        return childNotes.map(x => this.renderNestedNote(x, allNotes))
    }

    ///style={{ borderColor: note.selectingColor ? color : note.borderColor }}

    renderNote(note) {
        let className = "";
        switch (note.type) {
            case 0: className = "note-video-view"; break;
            case 1: className = "timestamp-video-view"; break;
            case 2: className = "newtopic-video-view"; break;
        }

        let color = cnvRGBToStr(this.state.color, "black");

        return (
            <div
                style={{ border: note.selectingColor ? createBorder(color, note.borderThickness) : createBorder(note.borderColor, note.borderThickness) }}
                className={className + " outer-text-box"}>

                <div className="d-flex">
                    <ContentEditable
                        inpageid={note.inPageId}
                        ref={this.captureNewlyCreatedNoteRefs}
                        style={{ outline: "0px solid transparent", border: "0", height: note.shouldExpand ? "100%" : "3em", width: "100%" }}
                        className="inner-text-box"
                        html={this.state.notes.filter(x => x.inPageId == note.inPageId)[0].content}
                        onChange={(e) => this.onChangeNote(e, note.inPageId)}
                        onFocus={() => this.onFocusNote(note.inPageId)}
                        onBlur={() => this.onBlurNote(note.inPageId)}
                    />
                    <ContextMenuTrigger id="noteControlls" attributes={{ id: note.inPageId }}>
                        <button
                            style={{
                                height: "5em", width: "2em", backgroundColor: "black",
                            }}
                            onClick={note.type == 0 ? () => this.onClickAddNote(note.inPageId, note.level, "note", true) : null}
                        >
                        </button>
                    </ContextMenuTrigger>
                </div>
            </div>
        )
    }

    renderVideoPlayerAndTopControlls() {
        return (
            <div className="row mb-4">
                <div id="iframeDiv" className="col-sm-10">
                    <YouTube
                        videoId={extractVideoToken(this.state.currentUrl) == null ? "" : extractVideoToken(this.state.currentUrl)}
                        onReady={this.onReadyYoutubePlayer}
                        onStateChange={this.onStateChangeYoutubePlayer}
                        opts={{
                            width: "100%",
                            height: "100%",
                        }}
                    />
                </div>
                <div className="col-sm-2 align-self-end">
                    <button
                        data-tip={c.createNewNoteButtonDataTip}
                        onClick={() => this.onClickAddNote(null, 0, "note", true)}
                        className="btn btn-primary btn-block add-note" position="top">
                        Add Note
                    </button>

                    <button
                        data-tip={c.createSubNoteButtnoDataTip}
                        onClick={() => this.onClickAddNote(this.state.subNoteInfo[0], 1, "note", true)}
                        className="btn btn-primary btn-block"
                        disabled={this.state.subNoteInfo[0] == null ? true : false}>
                        Sub Note
                    </button>

                    <button
                        data-tip={c.createSSubNoteButtnoDataTip}
                        onClick={() => this.onClickAddNote(this.state.subNoteInfo[1], 2, "note", true)}
                        className="btn btn-primary btn-block"
                        disabled={this.state.subNoteInfo[1] == null ? true : false}>
                        SSub Note
                    </button>

                    <button
                        data-tip={c.createSSSubNoteButtnoDataTip}
                        onClick={() => this.onClickAddNote(this.state.subNoteInfo[2], 3, "note", true)}
                        className="btn btn-primary btn-block"
                        disabled={this.state.subNoteInfo[2] == null ? true : false}>
                        SSSub Note
                     </button>

                    <button
                        data-tip={c.createTimeStampButtonDataTip}
                        onClick={() => this.onClickAddNote(null, 0, "time-stamp")}
                        className="btn btn-primary btn-block time-stamp-button">
                        Time Stamp
                    </button>

                    <button
                        data-tip={c.createTopicButtonDataTip}
                        onClick={() => this.onClickAddNote(null, 0, "topic")}
                        className="btn btn-primary btn-block new-topic-button">
                        New Topic
                    </button>
                </div>
            </div >
        )
    }

    renderBottomControlls() {
        return (
            <Fragment>
                <div className="container-fluid mt-5 mb-4">
                    <div className="row">

                        <div data-tip={c.createNewNoteButtonDataTip} className="col no-padding">
                            <button
                                onClick={() => this.onClickAddNote(null, 0, "note", false)} c
                                lassName="col btn btn-primary btn-block add-note">
                                Add Note
                             </button>
                        </div>

                        {this.renderPlayPauseButton()}

                        <div data-tip={c.goBackToVideoButtonDataTip} className="col no-padding">
                            <button
                                onClick={this.onClickBoBackToVideo}
                                className="col btn btn-primary btn-block" id="go-back">
                                Go Back
                            </button>
                        </div>

                        <div data-tip={c.createTimeStampButtonDataTip} className="col no-padding">
                            <button
                                onClick={() => this.onClickAddNote(null, 0, "time-stamp", false)}
                                className="col btn btn-primary btn-block time-stamp-button">
                                Time Stamp
                            </button>
                        </div>

                        <div data-tip={c.createTopicButtonDataTip} className="col no-padding">
                            <button
                                onClick={() => this.onClickAddNote(null, 0, "topic", false)}
                                className="col btn btn-primary btn-block new-topic-button">
                                New Topic
                            </button>
                        </div>

                        {/*So we get some spacing*/}
                        <div className="col"></div>
                        <div className="col"></div>

                    </div>
                </div>
            </Fragment>
        )
    }

    renderPlayPauseButton() {
        return (
            <div data-tip={c.pausePlayButtonDataTip} className="col no-padding">
                <button onClick={this.onClickPausePlayVideo} className="col btn btn-primary btn-block" id="start-pause">
                    {this.state.youtubePlayerPlaying ? "Pause" : "Play"}
                </button>
            </div>
        )
    }

    onReadyYoutubePlayer(event) {
        let player = event.target;
        this.setState({
            player: player,
            playerReady: true,
        });
    }

    getAllChildrenOfNote(note, allNotes) {
        let allChilder = [];
        this.recursivelyGetAllChilder(note, allNotes, allChilder);
        console.log(allChilder);
        return allChilder;
    }

    recursivelyGetAllChilder(note, allNotes, collection) {
        let noteChildren = allNotes.filter(x => x.inPageParentId == note.inPageId);
        for (var i = 0; i < noteChildren.length; i++) {
            collection.push(noteChildren[i]);
        }
        for (var i = 0; i < noteChildren.length; i++) {
            this.recursivelyGetAllChilder(noteChildren[i], allNotes, collection);
        }
    }

    onClickContexMenuNoteDelete(e, data, target) {
        let result = confirm("Are you sure you want to delete this note and all sub-notes?");
        if (result == false) {
            return;
        }
        const inPageId = parseInt(target.getAttribute('id'), 10);

        let newNotes = this.state.notes;

        let note = newNotes.filter(x => x.inPageId == inPageId);

        if (note.length <= 0) { alert("Cound note find ntoe with inPageId: " + inPageId); return; }
        if (note.length > 1) { alert("There are more then one notes with inPageId: " + inPageId); return; }

        note = note[0];
        note.deleted = true;

        let allChildren = this.getAllChildrenOfNote(note, newNotes);

        //console.log("The number of children: " + allChildren.length);
        //console.log("The childer:");
        //console.log(allChildren);

        for (var i = 0; i < allChildren.length; i++) {
            allChildren[i].deleted = true;
        }

        this.setState({ notes: newNotes });
    }

    onClickContexMenuChangeBorderWithInputs(e, data, target) {
        const inPageId = parseInt(target.getAttribute('id'), 10);
        let value = this.state.thicknessInput;

        if (isNaN(value) == true) {
            return;
        }

        value = Number(value);

        if (value < 0) {
            value = 0;
        }

        if (value > 15) {
            value = 15;
        }

        let newNotes = this.state.notes;
        let note = newNotes.filter(x => x.inPageId == inPageId)[0];
        note.borderThickness = value;

        this.setState({ notes: newNotes });
    }

    onClickContexMenuNoteSeekTo(e, data, target) {
        const inPageId = parseInt(target.getAttribute('id'), 10);
        let seekTo = this.state.notes.filter(x => x.inPageId == inPageId)[0].seekTo;
        this.state.player.seekTo(seekTo);
    }

    onClickContexMenuNoteMakeTimeStamp(e, data, target) {
        const inPageId = parseInt(target.getAttribute('id'), 10);
        let newNotes = this.state.notes;
        newNotes.filter(x => x.inPageId == inPageId)[0].deleted = true;
        this.setState({ notes: newNotes });
    }

    onClickContexMenuNoteMakeNewTopic(e, data, target) {
        const inPageId = parseInt(target.getAttribute('id'), 10);
        let newNotes = this.state.notes;
        newNotes.filter(x => x.inPageId == inPageId)[0].deleted = true;
        this.setState({ notes: newNotes });
    }

    onClickContexMenuChangeColor(e, data, target) {
        const inPageId = parseInt(target.getAttribute('id'), 10);
        let newNotes = this.state.notes;
        newNotes.filter(x => x.inPageId == inPageId)[0].selectingColor = true;
        //console.log(newNotes.filter(x => x.inPageId == inPageId)[0].selectingColor + " " + inPageId);
        this.setState({ selectingColor: true, notes: newNotes, itemLastSelectedFor: inPageId });
    }

    onClickColorPickerDone() {
        this.setState({ selectingColor: false });
        let newNotes = this.state.notes;
        let itemLastSelectedFor = newNotes.filter(x => x.inPageId == this.state.itemLastSelectedFor);
        if (itemLastSelectedFor.length !== 1) {
            alert("Error with the item last selected for!");
            return;
        }
        itemLastSelectedFor[0].selectingColor = false;
        itemLastSelectedFor[0].borderColor = cnvRGBToStr(this.state.color, "pink");
        this.setState({ notes: newNotes });
    }

    /*
            string[][] changes, VideoNoteCreate[] newNotes)
    */
    onClickSave() {
        ///Preparing new items - adding the parentDbId value id applicable
        let newElements = this.state.notes.filter(x => x.id == 0);
        for (var i = 0; i < newElements.length; i++) {
            let element = newElements[i];

            delete element.selectingColor;
            delete element.shouldExpand;
        }
        console.log(newElements);
        ///...

        ///Gathering the changes to pre-existing items
        let changes = [];
        let exstNotesNewState = this.state.notes.filter(x => x.id > 0 && x.id != null).sort((a, b) => a.id - b.id);
        let exstNotesOldState = this.state.initialState.notes.sort((a, b) => a.id - b.id);
        console.log(exstNotesNewState);
        console.log(exstNotesOldState);

        if (exstNotesNewState.length != exstNotesOldState.length) { alert("The old and new state of preexisting notes does note match!"); }

        let counter = 0;
        let monitoredProperties = ["deleted", "content", "formatting", "seekTo", "type", "borderColor", "borderThickness"];
        for (var i = 0; i < exstNotesNewState.length; i++) {
            var newStateNote = exstNotesNewState[i];
            var oldStateNote = exstNotesOldState[i];

            if (newStateNote.id != oldStateNote.id) { alert("New and old state of preexisting notes do not match"); }

            for (let prop of monitoredProperties) {
                let oldValue = oldStateNote[prop];
                let newValue = newStateNote[prop];

                if (oldValue != newValue) {
                    changes[counter] = [];
                    changes[counter][0] = newStateNote.id;
                    changes[counter][1] = prop;
                    changes[counter][2] = newValue;

                    console.log("CHANGED VALUE " + prop + " " + oldValue + " " + newValue);
                    console.log(changes[counter]);

                    counter++;
                }
            }
        }
        ///...

        let data = {};
        data.videoId = this.props.match.params.id;
        data.seekTo = Math.floor(this.state.player.getCurrentTime());
        data.name = this.state.videoData.name;
        data.desctiption = this.state.videoData.desctiption;
        data.url = this.state.videoData.url;
        data.finalSave = true;
        data.newNotes = newElements;
        data.changes = changes;

        fetch("/api/Video/Save", {
            method: "POST",
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(data)
        })
            .then((response) => {
                console.log(response);
                return response.json();
            })
            .then((data) => {
                if (data == true) {
                    window.history.back();
                } else {
                    alert("Save video did not work!");
                }
            });
    }

    onClickPausePlayVideo() {
        if (this.state.youtubePlayerPlaying) {
            this.state.player.pauseVideo();
        } else {
            this.state.player.playVideo();
        }
    }

    onClickBoBackToVideo() {
        document.documentElement.scrollTop = 0;
    }

    onClickColorPickerCancel() {
        this.setState({ selectingColor: false });
        let newNotes = this.state.notes;
        let itemLastSelectedFor = newNotes.filter(x => x.inPageId == this.state.itemLastSelectedFor);
        if (itemLastSelectedFor.length !== 1) {
            alert("Error with the item last selected for!");
            return;
        }
        itemLastSelectedFor[0].selectingColor = false;
        this.setState({ notes: newNotes });
    }

    onClickOptions() {
        this.setState({
            showOptions: !this.state.showOptions,
        });
    }

    onClickPlay() {
        this.setState({
            currentUrl: this.state.videoData.url,
        });
    }

    onClickAddNote(parentId = null, parentLevel = 0, type = "note", top = true) {

        let subnote = parentId == null ? false : true;

        if (this.state.userHasCreatedNote == false) {
            this.setState({ userHasCreatedNote: true });
        }

        if (parentLevel >= c.MaxNoteNestingLevel) {
            alert("you can not nest notes further than " + c.MaxNoteNestingLevel + " levels!")
            return;
        }

        let level = parentLevel + 1;
        let newNotes = this.state.notes;
        let inPageId = newNotes.length; ///asuming notes are always inPageIded in order starting from 0
        let typeNumber = this.types[type];

        ///Calculating the parentDbId
        ///{ -1 }: root, { 0 }: not root but no id on parent, { >0 }: parent's id 
        let parentDbId = null;
        if (parentId != null) {
            let parentElement = this.state.notes.filter(x => x.inPageId == parentId);
            if (parentElement.length != 1) { alert("Did not find the parent element!"); return; }
            parentElement = parentElement[0];
            if (parentElement.id != 0) {
                parentDbId = parentElement.id;
            } else {
                parentDbId = 0;
            }
        } else {
            parentDbId = -1;
        }
        if (parentDbId == null) { alert("Failed to set items parentDbId"); return; }
        ///...

        ///Setting up the subButtons with the data they need ///Only works for the last root note you have put and branches of it
        let zeroBasedLevel = level - 1;
        if (zeroBasedLevel == 0) {
            setSubNInfo(0, inPageId, this);
            clearSubNoteInfoAfter(0, this);
        } else {
            if (parentId != this.state.subNoteInfo[zeroBasedLevel - 1]) {
                ///Do nothing the user us adding sub-notes to another note
            } else {
                setSubNInfo(zeroBasedLevel, inPageId, this);
                clearSubNoteInfoAfter(zeroBasedLevel, this);
            }
        }

        function clearSubNoteInfoAfter(ind, _this) {
            let newSubNoteInfo = _this.state.subNoteInfo;
            for (var i = ind + 1; i < 3; i++) {
                newSubNoteInfo[i] = null;
            }
            _this.setState({ subNoteInfo: newSubNoteInfo });
        }

        function setSubNInfo(ind, value, _this) {
            let newSubNoteInfo = _this.state.subNoteInfo;
            newSubNoteInfo[ind] = value;
            _this.setState({ subNoteInfo: newSubNoteInfo });
        }
        ///...

        ///Figuring out the dafault border color 
        let defaultBorderColor = "";
        switch (typeNumber) {
            case 0: defaultBorderColor = c.noteBorderColor; break;
            case 1: defaultBorderColor = c.timeStampBorderColor; break;
            case 2: defaultBorderColor = c.topicBorderColor; break;
        }
        ///...

        newNotes.push({
            id: 0, ///no db id 
            inPageId: inPageId,
            inPageParentId: parentId, ///null if no parent, parentId Otherwise 
            parentDbId: parentDbId, /// I do not think I will be using this/// I will be using this!
            content: "",
            level: level, /// not sure what I am using this for anymore
            deleted: false,
            formatting: 0,
            type: typeNumber,
            seekTo: Math.floor(this.state.player.getCurrentTime()), ///Getting the current time from the video is whole seconds
            borderColor: defaultBorderColor,
            borderThickness: 1,
        });

        this.setState({ notes: newNotes });
    }

    renderVideoProperties() {
        var props = ["url", "name", "description"];
        return props.map(x =>
            <div className="row" key={"videoprop" + x}>
                <div className={"col-sm-" + this.state.offset + " text-right"}>
                    <label>{x}</label>
                </div>
                <div className={"col-sm-" + this.state.length}>
                    <input
                        className="form-control-black mb-3"
                        value={this.state.videoData[x]}
                        onChange={(e) => this.onChangeInput(e, x)}
                    >
                    </input>
                </div>
                {this.renderPlayButton(x)}
            </div>
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

    renderColorPicker() {
        if (this.state.selectingColor) {
            return (
                <div className="centered-ontop">
                    <SketchPicker
                        color={this.state.color}
                        onChangeComplete={this.onChangeCompleteColorPicker}
                    />
                    <div className="d-flex">
                        <button onClick={this.onClickColorPickerDone} className="btn btn-success">Done</button>
                        <button onClick={this.onClickColorPickerCancel} className="btn btn-warning">Cancel</button>
                    </div>
                </div>
            )
        }
    }

    renderSaveAndDoNotSave() {
        return (
            <Fragment>
                <button onClick={this.onClickSave} className="btn btn-success">Save Notes</button>
                <button onClick={() => window.history.back()} className="float-right btn btn-danger">Don't Save</button>
            </Fragment>
        )
    }

    renderOptions() {
        return (
            <div className="row">
                <div className={"offset-sm-" + this.state.offset + " col-sm-" + this.state.length}>
                    <button
                        className="btn btn-primary btn-block"
                        onClick={this.onClickOptions}
                    >
                        Options
                    </button>
                </div>
            </div>
        )
    }

    render() {
        const app = (
            <Fragment>
                {/*this.props.match.params.id*/}
                {this.renderColorPicker()}
                {this.renderVideoPlayerAndTopControlls()}
                {this.renderVideoProperties()}
                {this.renderOptions()}
                {this.renderAllNotes(this.state.notes)}
                {this.renderBottomControlls()}
                {this.renderSaveAndDoNotSave()}

                <ContextMenu id="noteControlls">
                    <MenuItem data={{ action: 'change' }} onClick={this.onClickContexMenuNoteSeekTo}>
                        Go to time
                    </MenuItem>
                    <MenuItem data={{ action: 'change' }} onClick={this.onClickContexMenuChangeColor}>
                        Change Color
                    </MenuItem>
                    <MenuItem data={{ action: 'change' }} onClick={this.onClickContexMenuNoteMakeTimeStamp}>
                        Make it TimeStamp
                    </MenuItem>
                    <MenuItem data={{ action: 'change' }} onClick={this.onClickContexMenuNoteMakeNewTopic}>
                        Make it Topic
                    </MenuItem>
                    <MenuItem data={{ action: 'delete' }} onClick={this.onClickContexMenuNoteDelete}>
                        Delete
                    </MenuItem>

                    <MenuItem data={{ action: 'decrease' }} onClick={this.onClickContexMenuChangeBorderWithInputs}>
                        <div>
                            Set Border Thickness
                        </div>
                        <input
                            onClick={(e) => { e.stopPropagation(); }}
                            value={this.state.thicknessInput}
                            onChange={this.onChangeThiknessContextMenu}
                        />
                    </MenuItem>

                </ContextMenu>

                <ReactTooltip
                    place="bottom"
                    effect="float" />

            </Fragment>)

        if (this.state.fetchedData == false) {
            return <LoadSvg />
        } else {
            return app;
        }
    }
}