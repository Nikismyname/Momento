import React, { Component, Fragment } from 'react';

export default class ShowError extends Component {
    constructor(props) {
        super(props);

        this.state = {
            property: this.props.prop.toUpperCase(),
            enabled: true,
            currentlyDisplaying: false,
            timeout: 1,
        };

        this.renderErrors = this.renderErrors.bind(this);
        this.onClickErrorMessage = this.onClickErrorMessage.bind(this);
    }

    onClickErrorMessage() {
        this.setState({ enabled: false });
    }

    ///Addumed that errors will be cleared before every request that can return validation errors
    renderErrors() {
        if (this.state.enabled == false && this.props.ERRORS.length == 0) {
            this.setState({ enabled: true, /*currentlyDisplaying: false */});
            //clearTimeout(this.state.timeout);
        }

        if (this.state.enabled == false) {
            return <Fragment></Fragment>
        }

        if (typeof this.props.ERRORS === "undefined") {
            return <Fragment></Fragment>
        }

        let errorObjectsMatchingProperty = this.props.ERRORS.filter(x => x.fieldName == this.state.property);
        if (errorObjectsMatchingProperty > 1) { alert("Duplicate Error prop name in the collection FIX"); }

        if (errorObjectsMatchingProperty.length == 1) {

            let errorMessagesForMatchingProp = errorObjectsMatchingProperty[0].errorMessages;

            //if (this.state.currentlyDisplaying == false) {
            //    this.setState({ currentlyDisplaying: true });
            //    if (typeof this.props.timer !== "undefined") {
            //        let timeout = setTimeout(() => { this.setState({ enabled: false }) }, this.props.timer);
            //        this.setState({ timeout: timeout });
            //    }
            //}

            return errorMessagesForMatchingProp.map((x, i) => 
                <p
                    onClick={this.onClickErrorMessage}
                    style={{
                        color: "white", padding: "0.5em", backgroundColor: "red",
                        borderRadius: "10px"
                    }}
                    key={"errorMessage" + i}>
                    {x}
                </p>
            );
        } else {
            <Fragment></Fragment>
        }
    }

    render() {
        return (
            <Fragment>
                {this.renderErrors()}
            </Fragment>
        )
    }
}