export default class Counter extends React.Component {
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
                <span>change4</span>
            </div>
        );
    }
}