import Counter from "./Components/Counter.jsx";

class App extends React.Component {
    constructor(props) {
        super(props);
    }

    render() {
        return (
            <div>
                <Counter /> <span>Change2</span>
                <Counter />
            </div>
        );
    }
}

ReactDOM.render(
    <App />,
    document.getElementById('react')
);