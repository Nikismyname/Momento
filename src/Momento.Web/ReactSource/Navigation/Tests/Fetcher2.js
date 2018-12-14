import Rect, { Component } from 'react';

export default class Fetcher2 extends Component {
    constructor(props) {
        super(props);

        this.state = {
            item: [],
            currentId:0,
            itemsLoaded: false
        };

        this.logItem = this.logItem.bind(this);
        this.fetch = this.fetch.bind(this);
        this.onInputChange = this.onInputChange.bind(this);
    }

    logItem() {
        console.log(this.state.item[0]);
    }

    fetch(id) {
        console.log("Fetching: " + id);
        fetch('/api/Navigation/' + id)
            .then(data => data.json())
            .then(json => this.setState({
                item: this.state.item.concat(json),
                itemsLoaded: true
            }));
    }

    onInputChange(evt) {
        console.log(evt.target.value);
        this.setState({
            currentId: evt.target.value,
        });
    }

    render() {
        return (
            <div>
                <input type="number" id="number" onChange={evt => this.onInputChange(evt)} />
                <button onClick={() => this.fetch(this.state.currentId)}>Fetch</button>
                <pre>{JSON.stringify(this.state.item[this.state.item.length-1], null, 2)}</pre>
            </div>
        );
    }
}