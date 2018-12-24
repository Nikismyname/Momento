import React, { Component, Fragment } from 'react';
import { Editor } from 'react-draft-wysiwyg';
import { EditorState, convertToRaw, convertFromRaw, ContentState } from 'draft-js';
import { stateToHTML } from 'draft-js-export-html';
import '../../../node_modules/react-draft-wysiwyg/dist/react-draft-wysiwyg.css';
import DraftExporter, { Draft } from 'draft-js-exporter';

const content = '{ "blocks": [{ "key": "6bnrm", "text": "1) Blue", "type": "unstyled", "depth": 0, "inlineStyleRanges": [{ "offset": 0, "length": 7, "style": "color-rgb(44,130,201)" }], "entityRanges": [], "data": {} }, { "key": "3ulkp", "text": "2) Green", "type": "unstyled", "depth": 0, "inlineStyleRanges": [{ "offset": 0, "length": 8, "style": "color-rgb(97,189,109)" }], "entityRanges": [], "data": {} }], "entityMap": {} }'

///To Make color work
const options1 = {
    customInlineFn: (element, { Style }) => {
        if (element.style && element.style.color) {
            return Style('color-' + element.style.color)
        }
    }
}

const options2 = {
    customInlineFn: (element, { Style, Entity }) => {
        if (element.style.color) {
            return Style('CUSTOM_COLOR_' + element.style.color); // this one 
        }
    }
};

export default class DraftTextEditor extends Component {
    constructor(props) {
        super(props);
        this.state = {
            editorState: EditorState.createEmpty(),
            html: "",
        };

        this.onEditorStateChange = this.onEditorStateChange.bind(this);
        this.onClickTestButton = this.onClickTestButton.bind(this);
        this.onClickConvertToRaw = this.onClickConvertToRaw.bind(this);
    }

    onEditorStateChange(editorState) {
        this.setState({
            editorState: editorState,
        });
    }

    onClickTestButton() {
        //let jsonContentState = JSON.stringify(convertToRaw(this.state.editorState.getCurrentContent()));
        //let stateObj = convertFromRaw(JSON.parse(jsonContentState));
        //let html = stateToHTML(stateObj, options1);

        let rawDraftContentBlock = Draft.convertToRaw(this.state.editorState);
        let exporter = new DraftExporter(rawDraftContentBlock);
        let html = exporter.export();

        console.log(html);
        this.setState({ html: html });
    }

    onClickConvertToRaw() {
        console.log(JSON.stringify(convertToRaw(this.state.editorState.getCurrentContent())));
    }

    convertContentFromJSONToHTML() {
        let element = convertFromRaw(JSON.parse(content));
        return stateToHTML(element, options1)
    }

    render() {
        return (
            <Fragment>
                <Editor
                    editorState={this.state.editorState}
                    onEditorStateChange={this.onEditorStateChange}
                />

                <div id="comment-div">
                    <div dangerouslySetInnerHTML={{ __html: this.state.html }}></div>
                </div>

                <button onClick={this.onClickTestButton}>Render</button>
                <button onClick={this.onClickConvertToRaw}>ConvertToRaw</button>
            </Fragment>)
    }
}
