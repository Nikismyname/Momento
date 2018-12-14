import React, { Component } from 'react';

export default class Counter1 extends Component {
    constructor(props) {
        super(props);
        this.state = { counter: 0 };
        this.increaseCounter = this.increaseCounter.bind(this);
    }

    increaseCounter() {
        this.setState({ counter: this.state.counter + 1 });
    }

    render() {
        return (
            <div>
                <label>{this.state.counter}</label>
                <button onClick={this.increaseCounter}>Increase Counter</button>
                <button className="btn" onClick={()=> this.props.func(2)}></button>
            </div>
        );
    }
}