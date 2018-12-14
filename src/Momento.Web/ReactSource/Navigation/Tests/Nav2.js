import React, { Component, Fragment} from "react";
import Counter1 from "./Counter1";
import Counter2 from "./Counter2";

class Nav2 extends Component {

    constructor(props) {
        super(props);
        this.state = { currentComponent: 1 };

        this.changeState = this.changeState.bind(this);
    }

    changeState(num) {
        this.setState({ currentComponent: num });
    }

    render() {
        if(this.state.currentComponent == 1){
            return (<Counter1 func={this.changeState}/>);
        }else{
            return (
            <Fragment>
                <Counter2 func={this.changeState}/>
            </Fragment>)
        }
    }
}

export default Nav2;