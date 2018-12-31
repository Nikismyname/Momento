import { Component, Fragment } from 'react';
import { ContextMenu, MenuItem, ContextMenuTrigger } from "react-contextmenu";
import VideoNav from './PageSubComponents/VideoNav';
import ComparisonNav from './PageSubComponents/ComparisonNav';
import ListTodoNav from './PageSubComponents/ListTodoNav';
import SubDirNav from './PageSubComponents/SubDirNav';
import NoteNav from './PageSubComponents/NoteNav';
import LoadSvg from './Helpers/LoadSvg';
import { linkSSRSafe } from './Helpers/HelperFuncs';
import 'linqjs';
import * as c from './Helpers/Constants';
import ReactTooltip from 'react-tooltip';

const borderString = "3px solid rgba(0, 0, 0, 0.6)"

export default class NavigationPage extends Component {

    constructor(props) {
        super(props);

        if (typeof this.props.initialDir === "undefined") {
            this.state = {
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
            this.state = {
                history: [this.props.initialDir],
                currentDir: this.props.initialDir,
                itemsLoaded: true,
            };
        }

        if (typeof window === "undefined") {
            this.state.isSeverSide = true;
            console.log("serverSide=True");
        } else {
            console.log("serverSide=False");
            this.state.isSeverSide = false;
        }

        this.fetch = this.fetch.bind(this);
        this.navigateToDirectory = this.navigateToDirectory.bind(this);
        this.onClickCreateFolder = this.onClickCreateFolder.bind(this);
        this.onClickDeleteDir = this.onClickDeleteDir.bind(this);
        this.onClickNewCompareson = this.onClickNewCompareson.bind(this);
        this.setStateFunc = this.setStateFunc.bind(this);
        this.onClickContexMenuItem = this.onClickContexMenuItem.bind(this);
        this.renderListsToDo = this.renderListsToDo.bind(this);
    }

    //componentDidUpdate() {
    //    ReactTooltip.rebuild();
    //}

    componentDidMount() {
        if (this.state.itemsLoaded == false) {
            var id = this.props.match.params.id;
            this.fetch(id);
            console.log("NOT Prerendered");
        } else {
            console.log("Prerenderd")
        }
    }

    setStateFunc(obj) {
        this.setState(obj);
    }

    fetch(id) {
        console.log("Fetching: " + id);
        fetch('/api/Navigation/GetDirSingle/' + id)
            .then(data => data.json())
            .then(json => {
                history.pushState(null, null, c.rootDir + "/" + id);
                this.setState({
                    currentDir: json,
                    history: this.state.history.concat(json),
                    itemsLoaded: true
                })
            });
    };

    navigateToDirectory(id) {
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
                        <div data-tip="Creates new Video Notes in the current folder."><a href={"/Video/Create/" + this.state.currentDir.id} onClick={(e) => this.onClickStopPropagation(e)} >Create Video Notes</a></div>
                        <div data-tip="Creates new Comparison in the current folder.">{linkSSRSafe(`${c.rootDir + c.comparisonCreate}/${this.state.currentDir.id}`, "Create Comparison", null)}</div>
                        <div data-tip="Creates new ToDo list in the current folder."><a href={"/ListToDo/Create/" + this.state.currentDir.id} onClick={(e) => this.onClickStopPropagation(e)} >Create List ToDo</a></div>
                        <div data-tip="Test stuff.">{linkSSRSafe(c.rootDir + c.richTextNotePath, "Go To RTE", null)}</div>
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

    render() {
        const app = (
            <div className="row">
                <div className="col-sm-3">
                    <div>
                        {this.renderRoot(this.state.currentDir)}
                    </div>
                    {this.renderSubDirectories(this.state.currentDir.subdirectories)}
                </div>
                <div className="col-sm-3">
                    {this.renderVideos(this.state.currentDir.videos)}
                </div>
                <div className="col-sm-3">
                    {this.renderComparisons(this.state.currentDir.comparisons)}
                </div>
                <div className="col-sm-3">
                    {this.renderListsToDo(this.state.currentDir.listsToDo)}
                    {this.renderNotes(this.state.currentDir.notes)}
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
            </div>);

        if (!this.state.itemsLoaded) {
            return <LoadSvg />
        } else {
            return app;
        }
    }
}

