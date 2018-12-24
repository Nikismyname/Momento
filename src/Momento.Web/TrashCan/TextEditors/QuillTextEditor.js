import React, { Component, Fragment } from 'react';
import 'react-quill/dist/quill.snow.css';
import 'react-quill/dist/quill.core.css';
import 'react-quill/dist/quill.bubble.css';


export default class QuillTextEditor extends Component {
    constructor(props) {
        super(props);

        if (document) {
            this.quill = require('react-quill')
        }

        this.state = {
            text: "<b>test</b>",
            html: "",
        }

        this.onChangeEditor = this.onChangeEditor.bind(this);
    }

    onChangeEditor(value) {
        this.setState({ text: value, html: value })
    }

    render() {
        const modules = {
            toolbar: [
                [{ 'header': [1, 2, 3, 4, 5, 6, false] }],
                ['bold', 'italic', 'underline', 'strike', 'blockquote'],
                [{ 'color': ['#FFFFFF', '#000000', '#FF0000', '#00FF00', '#0000FF'] }],
                [{ 'list': 'ordered' }, { 'list': 'bullet' }, { 'indent': '-1' }, { 'indent': '+1' }],
                ['link', 'image'],
                ['clean']
            ],
        };

        const formats = [
            'header',
            'bold', 'italic', 'underline', 'strike', 'blockquote',
            'list', 'bullet', 'indent',
            'link', 'image', 'color',
        ];

        const Quill = this.quill

        if (Quill) {
            return (
                <Fragment>
                    <Quill
                        modules={modules}
                        formats={formats}
                        onChange={this.onChangeEditor}
                        value={this.state.text}
                        theme={null}
                    />

                    <div id="comment-div">
                        <div dangerouslySetInnerHTML={{ __html: this.state.html }}></div>
                    </div>
                </Fragment>)
        } else {
            return null
        }
    }
}
