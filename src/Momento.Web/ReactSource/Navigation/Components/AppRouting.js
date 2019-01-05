import React, { Component, Fragment } from 'react';
import { Route, BrowserRouter, Switch } from "react-router-dom";
import * as c from './Helpers/Constants';
import NavigationPage from './NavigationPage';
import Compare from './Compare';
import Note from './Note';
import NoteCreate from './NoteCreate';
import ComparisonCreate from './ComparisonCreate';
import VideoView from "./VideoView";
import VideoNotes from './VideoNotes';
import VideoNoteCreate from './VideoNoteCreate';

export default class AppRouting extends Component {
    constructor(props) {
        super(props);
        this.state = {
            comparisonId: 0,
            comparisonDirId: 0,
        };

        this.setComparisonId = this.setComparisonId.bind(this);
    }

    setComparisonId(compId, compDirId) {
        this.setState({
            comparisonId: compId,
            comparisonDirId: compDirId,
        });
    }

    render() {
        if (typeof window === 'undefined') {
            if (this.props.comp == "index") {
                return (
                    <div className="pageContent">
                        <NavigationPage
                            setComparisonId={this.setComparisonId}
                            initialDir={this.props.initialDir} />
                    </div>)
            } else if (this.props.comp == "compare") {
                return (
                    <div className="pageContent">
                        <Compare
                            initialComp={this.props.initialComp}
                            id={this.state.comparisonId}
                            dirId={this.state.comparisonDirId} />
                    </div>)
            }
        } else {
            if (typeof this.props.comp !== "udefined") {
                console.log("We got server data gov!");
            }
            return (<Fragment>
                <BrowserRouter>
                    <div className="pageContent">
                        <Switch>
                            <Route path={c.rootDir + c.noteCreatePath + "/:id"} component={NoteCreate} />
                            <Route path={c.rootDir + c.comparisonCreatePath + "/:id"} component={ComparisonCreate} />
                            <Route path={c.rootDir + c.videoViewPath + "/:id"} component={VideoView} />
                            <Route path={c.rootDir + c.videoNotesPath + "/:id/:dirId"} component={VideoNotes} />
                            <Route path={c.rootDir + c.videoNotesCreatePath + "/:id"} component={VideoNoteCreate} />

                            <Route path={c.rootDir + c.richTextNotePath + "/:id/:dirId"} component={Note} />
                            <Route path={c.rootDir + c.comparePath + "/:id/:dirId"} component={Compare} />
                            <Route path={c.rootDir + "/:id"} component={NavigationPage} />
                            <Route path={c.rootDir + c.comparePath} render={() => <Compare initialComp={this.props.initialComp} />} />
                            <Route path={c.rootDir} render={() => <NavigationPage initialDir={this.props.initialDir} />} />
                        </Switch>
                    </div>
                </BrowserRouter>
            </Fragment>)
        }
    }
}