import { Component, Fragment } from "react";
import LoadSvg from "./Helpers/LoadSvg";
import * as c from "./Helpers/Constants";

export default class AdminView extends Component {

    constructor(props) {
        super(props);

        this.state = {
            usersLoaded: false,
            users: [] //{id, rootDirectoryId: 1, isAdmin: false, userName: "",
            //directoriesCount: 1, videosCount: 0, listsToDoCount: 0,
            //notesCount: 0, comparisonsCount: 0, isCurrentUser: false,}
        }

        this.renderUsers = this.renderUsers.bind(this);
        this.onClickDemoteUser = this.onClickDemoteUser.bind(this);
        this.onClickPromoteUser = this.onClickPromoteUser.bind(this);
        this.onClickInspectButton = this.onClickInspectButton.bind(this);
    }

    componentDidMount() {
        console.log("ADMIN STUFF");
        fetch("/api/Admin/Get")
            .then(x => x.json())
            .then(data => {
                this.setState({
                    users: data,
                    usersLoaded: true
                });
            });
    }

    onClickPromoteUser(userId) {
        console.log(userId);
        fetch("/api/Admin/Promote", {
            method: "POST",
            headers: {
                'Accept': "application/json",
                'Content-Type': "application/json"
            },
            body: JSON.stringify(userId)
        })
            .then((response) => {
                return response.json();
            })
            .then((data) => {
                if (data == true) {
                    let newUsers = this.state.users;
                    let user = newUsers.filter(x => x.id == userId)[0];
                    user.isAdmin = true;
                    this.setState({ users: newUsers });
                    console.log("Promoted");
                } else {
                    alert("Promotion did not work!");
                }
            });
    }

    onClickDemoteUser(userId) {
        fetch("/api/Admin/Demote", {
            method: "POST",
            headers: {
                'Accept': "application/json",
                'Content-Type': "application/json"
            },
            body: JSON.stringify(userId)
        })
            .then((response) => {
                return response.json();
            })
            .then((data) => {
                if (data == true) {
                    let newUsers = this.state.users;
                    let user = newUsers.filter(x => x.id == userId)[0];
                    user.isAdmin = false;
                    this.setState({ users: newUsers });
                    console.log("Demoted");
                } else {
                    alert("Demotion did not work!");
                }
            });
    }

    onClickInspectButton(rootDirId) {
        this.props.history.push(c.rootDir + "/" + rootDirId);
    }

    renderUsers() {
        return (
            <table className="table table-striped" style={{ width: "100%" }}>
                <thead>
                    <tr className="row">
                        <th className="col-sm-1">Dirs</th>
                        <th className="col-sm-1">Videos</th>
                        <th className="col-sm-1">Comps</th>
                        <th className="col-sm-1">Lists</th>
                        <th className="col-sm-1">Notes</th>
                        <th className="col-sm-2">Is Admin</th>
                        <th className="col-sm-2">Username</th>
                        <th className="col-sm-3">Actions</th>
                    </tr>
                </thead>
                <tbody>
                    {this.renderAllTableLines(this.state.users)}
                </tbody>
            </table>
        )
    }

    renderAllTableLines(users) {
        return users.map(x => this.renderUserTableLine(x));
    }

    renderUserTableLine(user) {
        console.log(user);
        return (
            <tr key={user.id} className="row">
                <td className="col-sm-1">{user.directoriesCount}</td>
                <td className="col-sm-1">{user.videosCount}</td>
                <td className="col-sm-1">{user.comparisonsCount}</td>
                <td className="col-sm-1">{user.listsToDoCount}</td>
                <td className="col-sm-1">{user.notesCount}</td>
                <td className="col-sm-2">{user.isAdmin ? "True" : "False"}</td>
                <td className="col-sm-2">{user.userName}</td>
                <td className="col-sm-3">
                    <div className="d-flex">
                        <button style={{ backgroundColor: "black", color: "white" }} onClick={() => this.onClickInspectButton(user.rootDirectoryId)}>Inspect</button>
                        {this.renderDemotePromoteButton(user)}
                    </div>
                </td>
            </tr>
        )
    }

    renderDemotePromoteButton(user) {
        if (user.isCurrentUser) {
            return <Fragment></Fragment>
        } else if (user.isAdmin) {
            return <button style={{backgroundColor: "black", color: "white"}} onClick={() => this.onClickDemoteUser(user.id)}>Demote</button>
        } else {
            return <button style={{ backgroundColor: "black", color: "white" }} onClick={() => this.onClickPromoteUser(user.id)}>Promote</button>
        }
    }

    render() {
        const app = (
            <Fragment>
                {this.renderUsers()}
            </Fragment >
        );

        if (!this.state.usersLoaded) {
            return <LoadSvg />;
        } else {
            return app;
        }
    }
}
