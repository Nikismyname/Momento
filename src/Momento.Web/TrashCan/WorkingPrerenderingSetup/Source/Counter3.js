import React, { Component } from 'react';

class Counter3 extends Component {
    constructor(props) {
        super(props);
        this.state = { data: this.props.initialData };
        this.increaseCounter = this.increaseCounter.bind(this);
    }

    increaseCounter() {
        this.setState({ data: this.state.data + 2 });
    }

    render() {
        return (
            <div>
                <label>{this.state.data}</label>
                <button onClick={this.increaseCounter}>Increase Counter by 2</button>
            </div>
        );
    }
}

export default Counter3;