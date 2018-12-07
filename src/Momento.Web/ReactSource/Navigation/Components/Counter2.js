export default class Counter2 extends React.Component {
    constructor(props) {
        super(props);
        this.state = { counter: 0 };
        this.increaseCounter = this.increaseCounter.bind(this);
    }

    increaseCounter() {
        this.setState({ counter: this.state.counter + 2 });
        let a = 5;
        let b = 6;
        let c = a + b;
        let d = 7;
    }

    render() {
        return (
            <div>
                <label>{this.state.counter}</label>
                <button onClick={this.increaseCounter}>Increase Counter by 2</button>
                <span>change8</span>
            </div>
        );
    }
}