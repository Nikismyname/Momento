import { Component, Fragment } from "react";
import { ContextMenu, MenuItem, ContextMenuTrigger } from "react-contextmenu";
import { SortableContainer, SortableElement, arrayMove } from "react-sortable-hoc";
import ReactTooltip from "react-tooltip";

import VideoNav from "./PageSubComponents/VideoNav";
import ComparisonNav from "./PageSubComponents/ComparisonNav";
import ListTodoNav from "./PageSubComponents/ListTodoNav";
import SubDirNav from "./PageSubComponents/SubDirNav";
import NoteNav from "./PageSubComponents/NoteNav";
import LoadSvg from "./Helpers/LoadSvg";
import { linkSSRSafe, handeleValidationErrors, clientSideValidation } from "./Helpers/HelperFuncs";
import * as c from "./Helpers/Constants";
import ShowError from "./Helpers/ShowError"

const borderString = "3px solid rgba(0, 0, 0, 0.6)"

export default class NavigationPage extends Component {

    constructor(props) {
        super(props);

        if (typeof this.props.initialDir === "undefined") {
            console.log("Did not get initial Dir");
            this.state = {

                showErrors: true,
                ERRORS: [],

                history: [],
                currentDir: {
                    subdirectories: [],
                    videos: [],
                    comparisons: [],
                    listsToDo: [],
                    notes: [],
                },
                itemsLoaded: false,
            };
        } else {

            let data = this.setOrder(this.props.initialDir);

            this.state = {
                history: [data],
                currentDir: data,
                itemsLoaded: true
            };
        }

        if (typeof window === "undefined") {
            this.state.isSeverSide = true;
            console.log("serverSide=True");
        } else {
            console.log("serverSide=False");
            this.state.isSeverSide = false;
        }

        ///Orders all the collections in dorectory acording to their order property.
        this.setOrder = this.setOrder.bind(this);

        this.fetch = this.fetch.bind(this);
        this.navigateToDirectory = this.navigateToDirectory.bind(this);
        this.onClickCreateFolder = this.onClickCreateFolder.bind(this);
        this.onClickDeleteDir = this.onClickDeleteDir.bind(this);
        this.onClickNewCompareson = this.onClickNewCompareson.bind(this);
        this.setStateFunc = this.setStateFunc.bind(this);
        this.onClickContexMenuItem = this.onClickContexMenuItem.bind(this);
        this.renderListsToDo = this.renderListsToDo.bind(this);

        this.onSortEndDir = this.onSortEndDir.bind(this);
        this.onSortEndComp = this.onSortEndComp.bind(this);
        this.onSortEndListTD = this.onSortEndListTD.bind(this);
        this.onSortEndNote = this.onSortEndNote.bind(this);
        this.onSortEndVideo = this.onSortEndVideo.bind(this);

        this.onReorderUpdateDb = this.onReorderUpdateDb.bind(this);
    }

    componentDidUpdate() {
        ReactTooltip.rebuild();
    }

    componentDidMount() {
        if (this.state.itemsLoaded == false) {
            var id = this.props.match.params.id;
            this.fetch(id);
            console.log("NOT Prerendered");
        } else {
            console.log("Prerendered HERE")
        }
    }

    setStateFunc(obj) {
        this.setState(obj);
    }

    fetch(id) {
        console.log("Fetching: " + id);
        fetch('/api/Navigation/GetDirSingle/' + id)
            .then(x => x.json())
            .then(data => {
                history.pushState(null, null, c.rootDir + "/" + id);

                this.setOrder(data);

                this.setState({
                    currentDir: data,
                    history: this.state.history.concat(data),
                    itemsLoaded: true
                })
            });
    };

    setOrder(data) {
        data.subdirectories = data.subdirectories.sort((a, b) => a.order - b.order);
        data.videos = data.videos.sort((a, b) => a.order - b.order);
        data.comparisons = data.comparisons.sort((a, b) => a.order - b.order);
        data.listsToDo = data.listsToDo.sort((a, b) => a.order - b.order);
        data.notes = data.notes.sort((a, b) => a.order - b.order);

        return data;
    }

    navigateToDirectory(id) {
        console.log(id);
        if (id == null) {
            return;
        }

        if (this.state.history.some(x => x.id == id)) {
            var dir = this.state.history.filter(x => x.id == id);
            if (dir.length != 1) {
                alert("Error With Taking Item Out Of History!")
                return;
            }
            this.setState({
                currentDir: dir[0],
            });
            console.log("UsedHistory");
            history.pushState(null, null, c.rootDir + "/" + id);
        } else {
            this.fetch(id);
            console.log("FetchedNewData");
        }
    }

    onClickStopPropagation(e) {
        e.stopPropagation();
    }

    renderRoot(data) {
        return (
            <Fragment >
                <div className="card mb-2" style={{ border: borderString }} onClick={() => this.navigateToDirectory(data.parentDirectoryId)}>
                    <div data-tip="Current folder and all things you can create in it." className="card-body">
                        <div data-tip="The name of the current folder."><h6 className="card-title">{data.name}</h6></div>
                        <div data-tip="Creates a Subfolder in the current folder."><a href="#" onClick={e => this.onClickCreateFolder(e)}>Create Folder</a></div>
                        <div data-tip="Creates new Video Notes in the current folder.">{linkSSRSafe(`${c.rootDir + c.videoNotesCreatePath}/${this.state.currentDir.id}`, "Create Video Notes", null)}</div>
                        <div data-tip="Creates new Comparison in the current folder.">{linkSSRSafe(`${c.rootDir + c.comparisonCreatePath}/${this.state.currentDir.id}`, "Create Comparison", null)}</div>
                        <div data-tip="Creates new ToDo list in the current folder."><a href={"/ListToDo/Create/" + this.state.currentDir.id} onClick={(e) => this.onClickStopPropagation(e)} >Create List ToDo</a></div>
                        <div data-tip="Creates new Note in the current folder.">{linkSSRSafe(`${c.rootDir + c.noteCreatePath}/${this.state.currentDir.id}`, "Create Note", null)}</div>
                    </div>
                </div>
            </Fragment>
        )
    }

    renderSubDirectories(data) {
        return data.map(subDir =>
            <div key={"subDir" + subDir.id}>
                <ContextMenuTrigger id="subDirectory" attributes={{ id: subDir.id }}>
                    <SubDirNav dir={subDir} navigateToDirectory={this.navigateToDirectory} />
                </ContextMenuTrigger>
            </div>
        );
    }

    renderNotes(data) {
        return data.map(note =>
            <Fragment key={"noteNav" + note.id}>
                <NoteNav note={note} setStateFunc={this.setStateFunc} parentState={this.state} />
            </Fragment>
        );
    }

    renderVideos(data) {
        if (data.length > 0) {
            return (
                <div>
                    {data.map(x => <VideoNav key={"video" + x.id} setStateFunc={this.setStateFunc} parentState={this.state} video={x} />)}
                </div>
            );
        }
    }

    renderComparisons(data) {
        if (data.length > 0) {
            return (
                <Fragment>
                    {data.map(comp =>
                        <div key={"compNav" + comp.id}>
                            <ComparisonNav comp={comp} setStateFunc={this.setStateFunc} parentState={this.state} />
                        </div>
                    )}
                </Fragment>)
        }
    }

    renderListsToDo(data) {
        return data.map(list => <ListTodoNav list={list} setStateFunc={this.setStateFunc} parentState={this.state} />)
    }

    onClickCreateFolder(e) {
        e.preventDefault();
        e.stopPropagation();

        ///Reset The error messages
        this.setState({ ERRORS: [] });

        let directoryName = prompt('Select directory name:');
        if (directoryName == null || directoryName == '' || directoryName.length == 0 || directoryName.toLowerCase() == 'root') {
            alert('You must enter name, can not be Root');
            return;
        }

        var data = {
            "directoryName": directoryName,
            "parentDirId": this.state.currentDir.id,
        }

        fetch("/api/Navigation/CreateDirectory", {
            method: "POST",
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(data)
        })
            .then((response) => {
                return response.json();
            })
            .then((data) => {
                if (data.hasOwnProperty("errors")) {
                    handeleValidationErrors(data.errors, this);
                    return;
                }

                let newSubDirArray = this.state.currentDir.subdirectories.slice();
                newSubDirArray.push({
                    id: data,
                    name: directoryName,
                });

                const newCurrentDir = { ...this.state.currentDir, subdirectories: newSubDirArray } // create a new object by spreading in the this.state.car and overriding features with our new array 
                this.setState({ currentDir: newCurrentDir }) // set this.state.car to our new object

                const updatedHistory = this.state.history.map((obj) => {
                    return obj.id == this.state.currentDir.id ? this.state.currentDir : obj;
                });

                this.setState({ history: updatedHistory });
            });
    }

    onClickDeleteDir(e, ind) {
        if (e != null) {
            e.preventDefault();
        }
        let confirmation = confirm("Are you sure you want to delete this and all child directories?");
        if (confirmation == false) { return; }

        let id = ind;

        fetch("/api/Navigation/Delete", {
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
                    let newSubDirArray = this.state.currentDir.subdirectories.filter(x => x.id !== ind);

                    const newCurrentDir = { ...this.state.currentDir, subdirectories: newSubDirArray }
                    this.setState({ currentDir: newCurrentDir })

                    const updatedHistory = this.state.history.map((obj) => {
                        return obj.id == this.state.currentDir.id ? this.state.currentDir : obj;
                    });

                    this.setState({ history: updatedHistory });
                } else {
                    console.log("Did Not Delete");
                }
            });
    }

    onClickNewCompareson() {
        this.props.setComparisonId(-1, this.state.currentDir.id);
    }

    onClickContexMenuItem(e, data, target) {
        const id = parseInt(target.getAttribute('id'), 10);
        this.onClickDeleteDir(null, id);
    }

    ///ON SORT END
    onSortEndDir({ oldIndex, newIndex }) {
        if (oldIndex == newIndex) {
            return;
        }
        let newCurrentDir = this.state.currentDir;
        newCurrentDir.subdirectories = arrayMove(newCurrentDir.subdirectories, oldIndex, newIndex);

        let newHistory = this.state.history;
        let archivedCurrentDir = newHistory.filter(x => x.id == newCurrentDir.id)[0];
        archivedCurrentDir = newCurrentDir;

        this.setState({
            history: newHistory,
            currentDir: newCurrentDir,
        });

        var data = [];

        for (var i = 0; i < this.state.currentDir.subdirectories.length; i++) {
            let dir = this.state.currentDir.subdirectories[i];
            console.log([dir.id, i]);
            data.push([dir.id,i]);
        }

        this.onReorderUpdateDb(c.DirectoryType, data);
    }

    onSortEndComp({ oldIndex, newIndex }) {
        if (oldIndex == newIndex) {
            return;
        }
        let newCurrentDir = this.state.currentDir;
        newCurrentDir.comparisons = arrayMove(newCurrentDir.comparisons, oldIndex, newIndex);

        let newHistory = this.state.history;
        let archivedCurrentDir = newHistory.filter(x => x.id == newCurrentDir.id)[0];
        archivedCurrentDir = newCurrentDir;

        this.setState({
            history: newHistory,
            currentDir: newCurrentDir,
        });

        var data = [];

        for (var i = 0; i < this.state.currentDir.comparisons.length; i++) {
            let dir = this.state.currentDir.comparisons[i];
            console.log([dir.id, i]);
            data.push([dir.id, i]);
        }

        this.onReorderUpdateDb(c.ComparisonType, data);
    }

    onSortEndListTD({ oldIndex, newIndex }) {
        if (oldIndex == newIndex) {
            return;
        }
        let newCurrentDir = this.state.currentDir;
        newCurrentDir.listsToDo = arrayMove(newCurrentDir.listsToDo, oldIndex, newIndex);

        let newHistory = this.state.history;
        let archivedCurrentDir = newHistory.filter(x => x.id == newCurrentDir.id)[0];
        archivedCurrentDir = newCurrentDir;

        this.setState({
            history: newHistory,
            currentDir: newCurrentDir,
        });

        var data = [];

        for (var i = 0; i < this.state.currentDir.listsToDo.length; i++) {
            let dir = this.state.currentDir.listsToDo[i];
            console.log([dir.id, i]);
            data.push([dir.id, i]);
        }

        this.onReorderUpdateDb(c.ListToDoType, data);
    }

    onSortEndNote({ oldIndex, newIndex }) {
        if (oldIndex == newIndex) {
            return;
        }
        let newCurrentDir = this.state.currentDir;
        newCurrentDir.notes = arrayMove(newCurrentDir.notes, oldIndex, newIndex);

        let newHistory = this.state.history;
        let archivedCurrentDir = newHistory.filter(x => x.id == newCurrentDir.id)[0];
        archivedCurrentDir = newCurrentDir;

        this.setState({
            history: newHistory,
            currentDir: newCurrentDir,
        });

        var data = [];

        for (var i = 0; i < this.state.currentDir.notes.length; i++) {
            let dir = this.state.currentDir.notes[i];
            console.log([dir.id, i]);
            data.push([dir.id, i]);
        }
        console.log(data);

        this.onReorderUpdateDb(c.NoteType, data);
    }

    onSortEndVideo({ oldIndex, newIndex }) {
        if (oldIndex == newIndex) {
            return;
        }
        let newCurrentDir = this.state.currentDir;
        newCurrentDir.videos = arrayMove(newCurrentDir.videos, oldIndex, newIndex);

        let newHistory = this.state.history;
        let archivedCurrentDir = newHistory.filter(x => x.id == newCurrentDir.id)[0];
        archivedCurrentDir = newCurrentDir;

        this.setState({
            history: newHistory,
            currentDir: newCurrentDir,
        });

        var data = [];

        for (var i = 0; i < this.state.currentDir.videos.length; i++) {
            let dir = this.state.currentDir.videos[i];
            console.log([dir.id, i]);
            data.push([dir.id, i]);
        }

        this.onReorderUpdateDb(c.VideoType, data);
    }
    ///...

    onReorderUpdateDb(type, orders) {

        let data = {
            type: type,
            directory: this.state.currentDir.id,
            newOrders: orders,
        }

        fetch("/api/Reorder/Reorder", {
            method: "POST",
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(data)
        });
    }

    render() {
        const app = (
            <Fragment>
                <ShowError prop={"directoryName"} ERRORS={this.state.ERRORS} showErrors={this.state.showErrors} />
                <div className="row">
                    <div className="col-md-3 col-sm-6 col-xs-12">
                        <div>
                            {this.renderRoot(this.state.currentDir)}
                        </div>
                        <SortableDirectories onSortEnd={this.onSortEndDir} items={this.state.currentDir.subdirectories} _this={this} distance={10} />
                    </div>
                    <div className="col-md-3 col-sm-6 col-xs-12">
                        <SortableVideos onSortEnd={this.onSortEndVideo} items={this.state.currentDir.videos} _this={this} distance={10}/>
                    </div>
                    <div className="col-md-3 col-sm-6 col-xs-12">
                        <SortableNotes onSortEnd={this.onSortEndNote} items={this.state.currentDir.notes} _this={this} distance={10}/>
                    </div>
                    <div className="col-md-3 col-sm-6 col-xs-12">
                        <SortableListsToDo onSortEnd={this.onSortEndListTD} items={this.state.currentDir.listsToDo} _this={this} distance={10}/>
                        <SortableComparisons onSortEnd={this.onSortEndComp} items={this.state.currentDir.comparisons} _this={this} distance={10}/>
                    </div>

                    <ContextMenu id="subDirectory">
                        <div style={{ color: "white", backgroundColor: "black" }}>
                            <MenuItem data={{ action: 'delete' }} onClick={this.onClickContexMenuItem}>
                                Delete
                            </MenuItem>
                        </div>
                    </ContextMenu>

                    <ReactTooltip
                        place="bottom"
                        effect="float" />

                </div>
            </Fragment >
        );

        if (!this.state.itemsLoaded) {
            return <LoadSvg />
        } else {
            return app;
        }
    }
}

///SORTABLE ELEMENTS
const SortableVideo = SortableElement(({ value, _this }) => {
    return <VideoNav setStateFunc={_this.setStateFunc} parentState={_this.state} video={value} />
});

const SortableComparison = SortableElement(({ value, _this }) => {
    return <ComparisonNav comp={value} setStateFunc={_this.setStateFunc} parentState={_this.state} />
});

const SortableNote = SortableElement(({ value, _this }) => {
    return <NoteNav note={value} setStateFunc={_this.setStateFunc} parentState={_this.state} />
});

const SortableListToDo = SortableElement(({ value, _this }) => {
    return <ListTodoNav list={value} setStateFunc={_this.setStateFunc} parentState={_this.state} />
});

const SortableDirectory = SortableElement(({ value, _this }) => {
    //console.log("this coming through the element");
    //console.log(_this);
    return (
        <div>
            <ContextMenuTrigger id="subDirectory" attributes={{ id: value.id }}>
                <SubDirNav dir={value} _this={_this} navigateToDirectory={_this.navigateToDirectory} />
            </ContextMenuTrigger>
        </div>
    )
});
///...

///SORTABLE COLLECTIONS
const SortableVideos = SortableContainer(({ items, _this }) => {
    return (
        <ul className="pl-0">
            {items.map((value, index) => {
                return (<SortableVideo key={`vid-${index}`} index={index} ind={index} value={value} _this={_this} />)
            }
            )}
        </ul>
    );
});

const SortableComparisons = SortableContainer(({ items, _this }) => {
    return (
        <ul className="pl-0">
            {items.map((value, index) => {
                return (<SortableComparison key={`comp-${index}`} index={index} ind={index} value={value} _this={_this} />)
            }
            )}
        </ul>
    );
});

const SortableNotes = SortableContainer(({ items, _this }) => {
    return (
        <ul className="pl-0">
            {items.map((value, index) => {
                return (<SortableNote key={`note-${index}`} index={index} ind={index} value={value} _this={_this} />)
            }
            )}
        </ul>
    );
});

const SortableListsToDo = SortableContainer(({ items, _this }) => {
    return (
        <ul className="pl-0 mb-0">
            {items.map((value, index) => {
                return (<SortableListToDo key={`listToDo-${index}`} index={index} ind={index} value={value} _this={_this} />)
            }
            )}
        </ul>
    );
});

const SortableDirectories = SortableContainer(({ items, _this }) => {
    //console.log("this coming throught the contained");
    //console.log(_this);
    return (
        <ul className="pl-0">
            {items.map((value, index) => {
                return (<SortableDirectory key={`dir-${index}`} index={index} ind={index} value={value} _this={_this} />)
            }
            )}
        </ul>
    );
});
///......

