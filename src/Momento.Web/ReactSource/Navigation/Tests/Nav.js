import React, { Component } from "react";
import { Route, NavLink, BrowserRouter } from "react-router-dom";
import c1 from "./Counter1";
import c2 from "./Counter2";

class Nav extends Component {
    render() {
        if (typeof window === 'undefined') {
            return(
            <div>
                <h1>React Router Simple Starter</h1>
                <ul className="header">
                    <li><a href="/c1">c1</a></li>
                    <li><a href="/c2">c2</a></li>
                </ul>
                <div className="content">

                </div>
            </div>)
            // return <h1>Server</h1>
        }

        return (
            <BrowserRouter>
                <div>
                    <h1>React Router Simple Starter</h1>
                    <ul className="header">
                        <li><NavLink to="/c1">c1</NavLink></li>
                        <li><NavLink to="/c2">c2</NavLink></li>
                    </ul>
                    <div className="content">
                        <Route path="/c1" component={c1} />
                        <Route path="/c2" component={c2} />
                    </div>
                </div>
            </BrowserRouter>
        );
    }
}

export default Nav;