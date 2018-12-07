import React, { Component } from 'react';

export default class Counter3 extends Component {
    constructor(props) {
        super(props);
        this.state = { data: this.props.initialData };
        this.increaseCounter = this.increaseCounter.bind(this);
    }

    increaseCounter() {
        this.setState({ counter: this.state.data + 2 });
    }

    //componentDidMount() {
    //    this.setState({ counter: this.props.number});
    //}

    render() {
        return (
            <div>
                <label>{this.state.data}</label>
                <button onClick={this.increaseCounter}>Increase Counter by 2</button>
                <span>change8</span>
            </div>
        );
    }
}