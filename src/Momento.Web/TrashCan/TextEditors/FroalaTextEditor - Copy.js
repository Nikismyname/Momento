import React, { Component, Fragment } from 'react';
import Textarea from 'react-expanding-textarea';

import 'froala-editor/css/froala_style.min.css';
import 'froala-editor/css/froala_editor.pkgd.darkTheme.css';
import '../../../../wwwroot/css/google-prettify/desert.css';
///Have to change the dark theme too.
//import 'froala-editor/css/themes/dark.min.css';
//import '../../../../node_modules/font-awesome/css/font-awesome.css';
//import '../../../../node_modules/font-awesome/fonts/fontawesome-webfont.woff2';
const initialNoteContent = '<span style = "color: rgb(255, 255, 255);" > Double Click To Edit!</span>'
const indexPrefixRgx = /^\(\*([0-9]+)\*\)/;
const ESCAPE_KEY = 27;
const O_KEY = 79;

export default class FroalaTextEditor extends Component {
    constructor(props) {
        super(props);

        if (document) {
            this.FroalaEditor = require('react-froala-wysiwyg').default;
            this.FroalaEditorView = require('react-froala-wysiwyg/FroalaEditorView').default;
            //this.froalaEditorView = this.froalaAll.FroalaEditorView;
            require('froala-editor/js/froala_editor.pkgd.min.js');
        }

        this.state = {
            mainNote: { content: initialNoteContent, editorMode: false, visible: true },
            notes: [{ content: "", editorMode: false, visible: true }],///{content: "", editorMode: false, visible: false}
            code: { source: "", lines: []/*{content: "", id: 1, note: {} }*/, showSourceEditor: true },
        }

        this.onChangeMainNote = this.onChangeMainNote.bind(this);
        this.onClickChageMode = this.onClickChageMode.bind(this);
        this.renderSourceEditor = this.renderSourceEditor.bind(this);
        this.renderChangeModeButton = this.renderChangeModeButton.bind(this);
        this.onClickParseSource = this.onClickParseSource.bind(this);
        this.onDClickCodeLine = this.onDClickCodeLine.bind(this);
        this.onSourceEditorChange = this.onSourceEditorChange.bind(this);
        this.onClickTest = this.onClickTest.bind(this);
        this.renderCodeLineNoteContent = this.renderCodeLineNoteContent.bind(this);
        this.onDClickHtmlContent = this.onDClickHtmlContent.bind(this);
        this.handleKeyDown = this.handleKeyDown.bind(this);
        this.openUpNotes = this.openUpNotes.bind(this);
        this.closeDownNotes = this.closeDownNotes.bind(this);
        this.onDClickMainNote = this.onDClickMainNote.bind(this);
        this.renderMainNote = this.renderMainNote.bind(this);
        this.onCodeLineChange = this.onCodeLineChange.bind(this);
    }

    componentDidMount() {
        this.runCodePrettify();
        document.addEventListener("keydown", this.handleKeyDown);
    }

    handleKeyDown(event) {
        switch (event.keyCode) {
            case ESCAPE_KEY:
                this.closeDownNotes();
                break;
            case O_KEY:
                this.openUpNotes();
                break;
            default:
                break;
        }
        if (event.keyCode === ESCAPE_KEY) {
        }
    }

    openUpNotes() {
        var newNotes = this.state.notes;

        if (newNotes.some(x => x.visible == false)) {
            newNotes = newNotes.map(x => {
                if (x.visible == false) {
                    x.visible = true;
                    x.editorMode = false;
                }
                return x;
            });
        } else if (newNotes.some(x => x.editorMode == false)) {
            newNotes = newNotes.map(x => {
                if (x.editorMode == false) {
                    x.editorMode = true;
                }
                return x;
            })
        }

        this.setState({ notes: newNotes });
    }

    closeDownNotes() {
        if (this.state.mainNote.editorMode == true) {
            let note = this.state.mainNote;
            note.editorMode = false;
            this.setState({ mainNote: note });
        }

        var newNotes = this.state.notes;

        if (newNotes.some(x => x.visible == true)) {
            if (newNotes.some(x => x.editorMode == true)) {
                newNotes = newNotes.map(x => {
                    if (x.editorMode == true) {
                        x.editorMode = false;
                    }
                    return x;
                })
            } else {
                newNotes = newNotes.map(x => {
                    if (x.visible == true) {
                        x.visible = false;
                    }
                    return x;
                })
            }
        }

        this.setState({ notes: newNotes });
    }

    runCodePrettify() {
        let script = document.createElement('script');
        script.type = 'text/javascript';
        script.async = true;
        script.src = 'https://cdn.rawgit.com/google/code-prettify/master/loader/run_prettify.js';
        (document.getElementsByTagName('head')[0] || document.getElementsByTagName('body')[0]).appendChild(script);
    }

    onClickParseSource() {
        let lines = this.state.code.source.split("\n");

        let startNumber = 0;

        let existingInds = lines
            .filter(x => indexPrefixRgx.exec(x) != null)
            .map(x => Number(indexPrefixRgx.exec(x)[1]));


        for (var i = 0; i < lines.length; i++) {
            let match = indexPrefixRgx.exec(lines[i]);
            if (match == null) {
                let newInd = findFirstOpening();
                let notes = this.state.notes;
                notes[newInd] = { content: initialNoteContent, editorMode: false, visible: false };
                lines[i] = { content: lines[i], id: newInd };
                continue;
            }
            let existingIndex = match[1];
            lines[i] = { content: lines[i].replace(indexPrefixRgx, ""), id: existingIndex };
        }

        this.state

        function findFirstOpening() {
            if (existingInds.includes(startNumber)) {
                startNumber++;
                return findFirstOpening();
            } else {
                existingInds.push(startNumber);
                let result = startNumber;
                startNumber++;
                return result;
            }
        }

        var newCode = this.state.code;
        newCode.lines = lines;
        newCode.source = lines.map(x => "(*" + x.id + "*)" + x.content).join("\n");
        this.setState({ code: newCode });
        PR.prettyPrint();
    }

    onChangeMainNote(value) {
        let note = this.state.mainNote;
        note.content = value;
        this.setState({ mainNote : note });
    }

    range(start, end) {
        var ans = [];
        for (let i = start; i <= end; i++) {
            ans.push(i);
        }
        return ans;
    }

    renderCodeLines() {
        return this.state.code.lines.map(x =>
            <Fragment key={"codeLine" + x.id}>
                {this.renderCodeLineNoteContent(x.id)}
                <pre className="prettyprint"
                    dangerouslySetInnerHTML={{ __html: PR.prettyPrintOne(x.content) }} />
            </Fragment>)
    }

    onCodeLineChange(e) {
        console.log("here");
    }

    renderChangeModeButton(index) {
        return <button onClick={() => this.onClickChageMode(index)}>Done</button>
    }

    renderCodeLineNoteContent(ind) {
        const FroalaEditor = this.FroalaEditor;
        const FroalaEditorView = this.FroalaEditorView;

        if (this.state.notes[ind].visible == false) {
            return null;
        }

        if (this.state.notes[ind].editorMode == true) {
            return (
                <FroalaEditor
                    config={{
                        theme: "dark"
                    }}
                    theme="dark"
                    tag="textarea"
                    model={this.state.notes[ind].content}
                    onModelChange={(val) => this.onChangeMainNote(val, ind)}
                />)
        }
        else {
            return (
                <div onDoubleClick={() => this.onDClickHtmlContent(ind)}>
                    <FroalaEditorView model={this.state.notes[ind].content} />
                </div>)
        }
    }

    renderSourceEditor() {
        if (this.state.code.showSourceEditor) {
            return <Textarea onChange={this.onSourceEditorChange} style={{ overflow: "hidden" }} type="text" value={this.state.code.source} className="form-control-black" />
        } else {
            return null;
        }
    }

    renderMainNote() {
        const FroalaEditor = this.FroalaEditor;
        const FroalaEditorView = this.FroalaEditorView;

        if (this.state.mainNote.editorMode == true) {
            return (<FroalaEditor
                tag="textarea"
                model={this.state.mainNote.content}
                onModelChange={this.onChangeMainNote}/>)
        } else {
            return (
                <div onDoubleClick={this.onDClickMainNote}>
                    <FroalaEditorView model={this.state.mainNote.content} />
                </div>
            )
        }
    }

    onSourceEditorChange(e) {
        let value = e.target.value;
        let newCode = this.state.code;
        newCode.source = value;
        this.setState({ code: newCode });
    }

    onClickChageMode(ind) {
        let newEditorModes = this.state.editorModes;
        newEditorModes[ind] = !newEditorModes[ind];
        this.setState({ editorModes: newEditorModes })
    }

    onDClickCodeLine(ind) {
        let newNotes = this.state.notes;
        newNotes[ind].visible = !newNotes[ind].visible;
        this.setState({ notes: newNotes });
    }

    onDClickHtmlContent(ind) {
        let newNotes = this.state.notes;
        newNotes[ind].editorMode = !newNotes[ind].editorMode;
        this.setState({ notes: newNotes });
    }

    onDClickMainNote() {
        let mNote = this.state.mainNote;
        mNote.editorMode = !mNote.editorMode;
        this.setState({ mainNote: mNote });
    }

    onClickTest() {
        PR.prettyPrint();
    }

    render() {
        if (this.FroalaEditor) {
            return (
                <Fragment>
                    {this.renderMainNote()}
                    {this.renderSourceEditor()}
                    <button onClick={this.onClickParseSource}>Render</button>
                    {this.renderCodeLines()}
                    <button onClick={this.onClickTest}>Test</button>
                </Fragment>)
        } else {
            return null;
        }
    }
}


























//renderCodeLines2() {
//    return this.state.code.lines.map(x =>
//        <Fragment key={"codeLine" + x.id}>
//            {this.renderCodeLineNoteContent(x.id)}
//            <pre key={"codeLine" + x.id}
//                className="prettyprint"
//                onDoubleClick={() => this.onDClickCodeLine(x.id)}>
//                {x.content}
//            </pre>
//        </Fragment>)
//}