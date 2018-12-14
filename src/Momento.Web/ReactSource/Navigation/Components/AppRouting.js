import React, { Component, Fragment } from 'react';
import { Route, BrowserRouter, Switch } from "react-router-dom";
import NavigationPage from './NavigationPage';
import Compare from './Compare';

export default class AppRouting extends Component {
    constructor(props) {
        super(props);
        this.state = {
            comparisonId: 0,
            comparisonDirId: 0,
        };

        this.setComparisonId = this.setComparisonId.bind(this);
    }

    setComparisonId(compId, compDirId) {
        this.setState({
            comparisonId: compId,
            comparisonDirId: compDirId,
        });
    }

    render() {
        if (typeof window === 'undefined') {
            if (this.props.comp == "index") {
                return (
                    <div className="pageContent">
                        <NavigationPage
                            setComparisonId={this.setComparisonId}
                            initialDir={this.props.initialDir} />
                    </div>)
            } else if (this.prop.comp == "compare") {
                return (
                    <div className="pageContent">
                    <Compare
                        initialComp={this.props.initialComp}
                        id={this.state.comparisonId}
                            dirId={this.state.comparisonDirId} />
                    </div>)
            }
        } else {
            //if (typeof this.props.comp == "undefined") {
            //    return (<Fragment>
            //        <BrowserRouter>
            //            <Fragment>
            //                <div className="pageContent">
            //                    <Switch>
            //                        <Route path="/compare/:id" component={Compare} />
            //                        <Route path="/:id" component={NavigationPage} />
            //                    </Switch>
            //                </div>
            //            </Fragment>
            //        </BrowserRouter>
            //    </Fragment>)
            //} else {
            return (<Fragment>
                <BrowserRouter>
                    <div className="pageContent">
                        <Switch>
                            <Route path="/Directory/IndexReact/compare/:id/:dirId" component={Compare} />
                            <Route path="/Directory/IndexReact/:id" component={NavigationPage} />
                            <Route path="/Directory/IndexReact/compare" render={() => <Compare id={this.state.comparisonId} dirId={this.state.comparisonDirId} />} />
                            <Route path="/Directory/IndexReact" render={() => <NavigationPage setComparisonId={this.setComparisonId} initialDir={this.props.initialDir} />} />
                        </Switch>
                    </div>
                </BrowserRouter>
            </Fragment>)
        }
    }
}