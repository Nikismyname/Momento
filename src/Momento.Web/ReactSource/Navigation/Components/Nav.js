import React, { Component } from "react";
import { Route, NavLink, BrowserRouter } from "react-router-dom";
import c1 from "./Counter";
import c2 from "./Counter2";
import c3 from "./Counter";

class Nav extends Component {
    render() {
        return (
            <BrowserRouter>
                <div>
                    <h1>React Router Simple Starter</h1>
                    <ul className="header">
                        <li><NavLink to="/c1">c1</NavLink></li>
                        <li><NavLink to="/c2">c2</NavLink></li>
                        <li><NavLink to="/c3">c3</NavLink></li>
                    </ul>
                    <div className="content">
                        <Route path="/c1" component={c1} />
                        <Route path="/c2" component={c2} />
                        <Route path="/c3" component={c3} />
                    </div>
                </div>
            </BrowserRouter>
        );
    }
}

export default Nav;