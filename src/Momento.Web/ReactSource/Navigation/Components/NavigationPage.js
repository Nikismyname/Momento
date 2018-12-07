import Rect, { Component } from 'react';

export default class NavigationPage extends Component {
    constructor(props) {
        super(props);

        this.state = {
            history: [this.props.initialDir],
            currentDir: this.props.initialDir,
            itemsLoaded: true
        };

        this.fetch = this.fetch.bind(this);
        this.navigateToDirectory = this.navigateToDirectory.bind(this);
    }

    fetch(id) {
        console.log("Fetching: " + id);
        fetch('/api/Navigation/' + id)
            .then(data => data.json())
            .then(json => this.setState({
                currentDir: json,
                history: this.state.history.concat(json),
                itemsLoaded: true
            }));
    };

    navigateToDirectory(id) {
        console.log(id);

        if (id == null) {
            console.log("You are root there is no going back now is there!");
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
            return;
        } else {
            console.log("FetchedNewData");
            this.fetch(id);
        }
    }

    generateRoot(data) {
        return (
            <div className="text-center" onClick={() => this.navigateToDirectory(data.parentDirectoryId)}>
                <label>{data.name}</label>
            </div>
        )
    }

    generateSubFolder(data) {
        return data.map(x =>
            <div className="text-center" key={x.id} onClick={() => this.navigateToDirectory(x.id)}>
                <label>{x.name}</label>
            </div>);
    }

    generateVideos(data) {
        if (data.length == 0) { 
            return <div></div> 
        }
        else {
            return (
                <div>
                    <label style={{ color: "red" }}>Videos</label>
                    {data.map(x =>
                        <div className='text-center' key={x.id}>
                            <label>{x.name}</label>
                            <a href={'/Video/Edit?id=' + x.id}>Edit</a>
                        </div>
                    )}
                </div>
            );
        }
    }

    render() {
        return (
            <div>
                {this.generateRoot(this.state.currentDir)}
                {this.generateSubFolder(this.state.currentDir.subdirectories)}
                {this.generateVideos(this.state.currentDir.videos)}
            </div>
        );
    }
}