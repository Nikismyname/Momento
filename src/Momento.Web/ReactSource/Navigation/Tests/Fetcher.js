import Rect, { Component } from 'react';
import Rect, { Component } from 'react';

export default class Fetcher extends Component {
    constructor(props) {
        super(props);

        this.state = {
            item: [],
            itemsLoaded: false
        };

        this.logItem = this.logItem.bind(this);
    }

    componentDidMount()
    {
        fetch('/api/Navigation')
            .then(data => data.json())
            .then(json => this.setState({
                item: [json],
                itemsLoaded: true
            }))
    }

    logItem() {
        console.log(this.state.item[0]);
    }

    render() {
        return (
            <div>
                <pre>{JSON.stringify(this.state.item[0],null,4)}</pre>
                <button onClick={this.logItem}>LogItem</button>
            </div>
        );
    }
}