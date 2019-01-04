import React, { Component, Fragment } from 'react';
import * as c from './Helpers/Constants';
import { createBorder } from './Helpers/HelperFuncs';
//import { linkSSRSafe } from './Helpers/HelperFuncs';
import YouTube from 'react-youtube';
import LoadSvg from './Helpers/LoadSvg';
import { Tab, Tabs, TabList, TabPanel } from 'react-tabs';
import "react-tabs/style/react-tabs.css";
import { ContextMenu, MenuItem, ContextMenuTrigger } from "react-contextmenu";

///seekTo() pauseVideo() 

export default class VideoView extends Component {
    constructor(props) {
        super(props);

        this.state = {
            fetchedData: false,
            playerReady: false,
            videoId: "",
            player: {},
            data: {///{description: "", name: "" url: "" notes:[]}
                notes: [],///{ childNotes: [], content: "", formatting: 0, id: 1, level: 0, seekTo: 0, type: 0}}
                spreadNotes: []///same as notes but no nestring + shouldExpand field initially set to false;
            }, 
        };

        this.onReadyYoutubePlayer = this.onReadyYoutubePlayer.bind(this);
        this.onClickContexMenuItem = this.onClickContexMenuItem.bind(this);

        this.onClickTestButton = this.onClickTestButton.bind(this);
        this.onClickNote = this.onClickNote.bind(this);
        this.onClickContexMenuTopic = this.onClickContexMenuTopic.bind(this);
        this.onClickContexMenuItemTopicGoTo = this.onClickContexMenuItemTopicGoTo.bind(this);

        this.renderNote = this.renderNote.bind(this);
        this.renderYouTubePlayer = this.renderYouTubePlayer.bind(this);
        this.renderTabs = this.renderTabs.bind(this);
        this.renderTabContentForAllNotes = this.renderTabContentForAllNotes.bind(this);
    }

    componentWillMount() {

        let id = this.props.match.params.id;

        fetch("/api/Video/GetView", {
            method: "POST",
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(id)
        })
            .then((x) => {
                return x.json();
            })
            .then((data) => {
                if (data != null) {
                    ///Ordering them through their order time
                    this.orderNotesBasedOrder(data.notes);
                    data.spreadNotes = this.spreadNotes(data.notes);
                    this.setState({
                        data: data,
                        fetchedData: true,
                        videoId: this.extractVideoToken(data.url),
                    });
                    $("#iframeDiv").height($("#iframeDiv").width() * 9 / 16);
                    console.log("HERE");
                    console.log(data);
                } else {
                    alert("View Data was not fetched!");
                }
            });
    }

    spreadNotes(notes) {
        let finalNotes = [];
        this.spreadNotesRecursively(notes, finalNotes)
        return finalNotes;
    }
    spreadNotesRecursively(notes, collection) {
        for (var i = 0; i < notes.length; i++) {
            notes[i].shouldExpand = false;
            if (notes[i].type == 2) {
                notes[i].showContent = false;
            }
            collection.push(notes[i]);
        }
        for (var i = 0; i < notes.length; i++) {
            this.spreadNotesRecursively(notes[i].childNotes, collection);
        }
    }

    extractVideoToken(url) {
        if (url == null || url == undefined || url.length == 0) {
            return null;
        }
        let standartUrl = true;
        let videoToken = url.split('v=')[1];
        if (videoToken == undefined) {
            standartUrl = false;
        }
        if (standartUrl == true) {
            videoToken = videoToken.split('&')[0];
            return videoToken;
        } else {
            let regex = /\.be\/(.+?)(?:$|\?)/;
            var match = regex.exec(url);
            if (match == false) {
                return null;
            }
            videoToken = match[1];
            return videoToken;
        }
    }

    orderNotesBasedOnSeekTo(notes) {
        notes = notes.sort((a, b) => a.seekTo - b.seekTo);
        for (var i = 0; i < notes.length; i++) {
            var note = notes[i];
            this.orderNotesBasedOnSeekTo(note.childNotes);
        }
    }

    orderNotesBasedOrder(notes) {
        notes = notes.sort((a, b) => a.inPageId - b.inPageId);
        for (var i = 0; i < notes.length; i++) {
            var note = notes[i];
            this.orderNotesBasedOrder(note.childNotes);
        }
    }

    onReadyYoutubePlayer(event) {
        let player = event.target;
        this.setState({
            player: player,
            playerReady: true,
        });
    }

    onClickTestButton() {
        if (this.state.playerReady == false) {
            return;
        }

        this.state.player.seekTo(12);
    }

    renderYouTubePlayer() {
        return (
            <div className="row mb-5">
                <div id="iframeDiv" className="col-sm-9 no-padding">
                    <YouTube
                        videoId={this.state.videoId}
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

    renderTabs() {
        return (
            <Tabs>
                <TabList>
                    <Tab>All</Tab>
                    <Tab>Time Stamps</Tab>
                    <Tab>Topics</Tab>
                </TabList>

                <TabPanel>
                    {this.renderTabContentForAllNotes(this.state.data.notes)}
                </TabPanel>
                <TabPanel>
                    {this.renderTabsTimeStamps()}
                </TabPanel>
                <TabPanel>
                    {this.renderTabsTopics()}
                </TabPanel>
            </Tabs>
        )
    }

    renderTabContentForAllNotes(notes) {
        return notes.map(x => this.renderAnyNote(x));
    }

    renderTabContentForAllNotesTopic(notes, shouldRender) {
        if (shouldRender) {
            return notes.map(x => this.renderAnyNote(x));
        }
    }

    renderTabsTimeStamps() {
        let timeStamps = this.state.data.spreadNotes
            .filter(x => x.type == 1)
            .sort((a, b) => a.seekTo - b.seekTo);

        return timeStamps.map(x => this.renderNote(x));
    }

    renderTabsTopics() {
        let topics = this.state.data.spreadNotes
            .filter(x => x.type == 2)
            .sort((a, b) => a.seekTo - b.seekTo);

        let dataToVisualise = [];

        for (var i = 0; i < topics.length; i++) {

            let currentTopic = topics[i];
            let currentTopicData = {};
            currentTopicData.topic = currentTopic;

            ///TODO: make a different way to order, original order or time in the video
            if (i == topics.length-1) {
                currentTopicData.notes = this.state.data.notes.
                    filter(x => x.seekTo > currentTopic.seekTo)
            } else {
                currentTopicData.notes = this.state.data.notes.
                    filter(x => x.seekTo > currentTopic.seekTo && x.seekTo < topics[i + 1].seekTo)
            }

            dataToVisualise.push(currentTopicData);
        }

        return dataToVisualise.map(x => (
            <Fragment>
                {this.renderTopicForTopicView(x.topic)}
                <div className="ml-2">
                    {this.renderTabContentForAllNotesTopic(x.notes, x.topic.showContent)}
                </div>
            </Fragment>
        ))
    }

    renderAnyNote(note) {
            return this.renderNoteWithChildren(note);
    }

    renderNoteWithChildren(note) {
        return (
            <Fragment>
                {this.renderNote(note)}
                <div className="childern-div ml-2">
                    {this.renderNoteChilder(note.childNotes)}
                </div>
            </Fragment>
        )
    }
    renderNoteChilder(notes) {
        return notes.map(x => this.renderNoteWithChildren(x));
    }

    renderNote(note) {
        let shouldExpand = this.state.data.spreadNotes.filter(x => x.id == note.id)[0].shouldExpand;
        return (
            <ContextMenuTrigger id="default" attributes={{ id: note.id }}>
                <div
                    className="outer-text-box note-video-view"
                    style={{ border: createBorder(note.borderColor, note.borderThickness) }}
                >
                    <div className="inner-text-box"
                        style={{height: shouldExpand ? "100%" : "3em",}}
                        onClick={() => this.onClickNote(note.id)} >
                        {note.content}
                    </div>
                </div>
            </ContextMenuTrigger>
        )
    }

    //renderTimeStamp(timeStamp) {
    //    let shouldExpand = this.state.data.spreadNotes.filter(x => x.id == timeStamp.id)[0].shouldExpand;
    //    return (
    //        <ContextMenuTrigger id="default" attributes={{ id: timeStamp.id }}>
    //            <div className="outer-text-box timestamp-video-view">
    //                <div className="inner-text-box"
    //                    style={{ height: shouldExpand ? "100%" : "3em" }}
    //                    onClick={() => this.onClickNote(timeStamp.id)}>
    //                    {timeStamp.content}
    //                </div>
    //            </div>
    //        </ContextMenuTrigger>
    //    )
    //}

    //renderTopic(topic) {
    //    let shouldExpand = this.state.data.spreadNotes.filter(x => x.id == topic.id)[0].shouldExpand;
    //    return (
    //        <ContextMenuTrigger id="default" attributes={{ id: topic.id }}>
    //            <div className="outer-text-box newtopic-video-view">
    //                <div className="inner-text-box"
    //                    style={{ height: shouldExpand ? "100%" : "3em" }}
    //                    onClick={() => this.onClickNote(topic.id)}>
    //                    {topic.content}
    //                </div>
    //            </div>
    //        </ContextMenuTrigger>
    //    )
    //}

    renderTopicForTopicView(topic) {
        let shouldExpand = this.state.data.spreadNotes.filter(x => x.id == topic.id)[0].shouldExpand;
        return (
            <ContextMenuTrigger id="topic" attributes={{ id: topic.id }}>
                <div className="outer-text-box newtopic-video-view">
                    <div className="inner-text-box"
                        style={{ height: shouldExpand ? "100%" : "3em" }}
                        onClick={() => this.onClickNote(topic.id)}>
                        {topic.content}
                    </div>
                </div>
            </ContextMenuTrigger>
        )
    }

    onClickContexMenuItem(e, data, target) {
        const id = parseInt(target.getAttribute('id'), 10);

        let newData = this.state.data;
        let currentNote = newData.spreadNotes.filter(x => x.id == id)[0];
        currentNote.shouldExpand = !currentNote.shouldExpand;

        this.setState({
            data: newData,
        });
    }

    onClickContexMenuItemTopicGoTo(e, data, target) {
        const id = parseInt(target.getAttribute('id'), 10);
        let seekTo = this.state.data.spreadNotes.filter(x => x.id == id)[0].seekTo;
        this.state.player.seekTo(seekTo);
    }

    onClickContexMenuTopic(e, data, target) {
        const id = parseInt(target.getAttribute('id'), 10);

        let newData = this.state.data;
        let currentNote = newData.spreadNotes.filter(x => x.id == id)[0];
        currentNote.showContent = !currentNote.showContent;
        this.setState({
            data: newData,
        });
    }

    onClickNote(id) {
        var clickedNote = this.state.data.spreadNotes.filter(x => x.id == id);
        if (clickedNote.length != 1) {
            alert("More than one note with the same id");
        }
        clickedNote = clickedNote[0];
        var seekToValue = clickedNote.seekTo;
        this.state.player.seekTo(seekToValue);
    }

    render() {
        const app =
            (<Fragment>
                {this.renderYouTubePlayer()}
                {this.renderTabs()}
                <div>
                    <button className="btn btn-success mt-3" onClick={()=>window.history.back()}>Back</button>
                </div>

                <ContextMenu id="default">
                    <div style={{ color: "white", backgroundColor: "black" }}>
                        <MenuItem data={{ action: 'Expand' }} onClick={this.onClickContexMenuItem}>
                            Expand
                        </MenuItem>
                    </div>
                </ContextMenu>

                <ContextMenu id="topic">
                    <div style={{ color: "white", backgroundColor: "black" }}>
                        <MenuItem data={{ action: 'Expand' }} onClick={this.onClickContexMenuTopic}>
                            Show Items
                        </MenuItem>
                        <MenuItem data={{ action: 'Expand' }} onClick={this.onClickContexMenuItem}>
                            Expand
                        </MenuItem>
                        <MenuItem data={{ action: 'Expand' }} onClick={this.onClickContexMenuItemTopicGoTo}>
                            GoTo
                        </MenuItem>
                    </div>
                </ContextMenu>

            </Fragment>)

        if (this.state.fetchedData == false) {
            return <LoadSvg />
        } else {
            return app;
        }
    }
}