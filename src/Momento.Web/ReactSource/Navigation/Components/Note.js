import React, { Component, Fragment } from 'react';
import Textarea from 'react-expanding-textarea';
import * as c from './Helpers/Constants';
import LoadSvg from './Helpers/LoadSvg';

import 'froala-editor/css/froala_style.min.css';
import 'froala-editor/css/froala_editor.pkgd.darkTheme.css';
import './../../../wwwroot/css/google-prettify/desert.css';
import '../../../wwwroot/css/froala.css';

///Have to change the dark theme too.
//import 'froala-editor/css/themes/dark.min.css';
//import '../../../../node_modules/font-awesome/css/font-awesome.css';
//import '../../../../node_modules/font-awesome/fonts/fontawesome-webfont.woff2';
const initialNoteContent = '<span style = "color: rgb(255, 255, 255);" >Double Click To Edit!</span>'
const indexPrefixRgx = /^\(\*([0-9]+)\*\)/;
const ESCAPE_KEY = 27;

export default class Note extends Component {
    constructor(props) {
        super(props);

        if (document) {
            this.FroalaEditor = require('react-froala-wysiwyg').default;
            this.FroalaEditorView = require('react-froala-wysiwyg/FroalaEditorView').default;
            //this.froalaEditorView = this.froalaAll.FroalaEditorView;
            require('froala-editor/js/froala_editor.pkgd.min.js');
        }

        this.state = {
            PRLoaded: false,
            codePresent: false,
            currentlyTyping: false,
            mainNote: { content: initialNoteContent, editorMode: false, visible: true },
            code: { source: "", showSourceEditor: false, lines: []/*{content: "", id: 1, dbId=0, note: {content: "", editorMode: false, visible: false} }*/ },
        }

        this.onChangeMainNote = this.onChangeMainNote.bind(this);
        this.onChangeCodeLineNote = this.onChangeCodeLineNote.bind(this);
        this.onChangeSourceEditor = this.onChangeSourceEditor.bind(this);

        this.onClickChageMode = this.onClickChageMode.bind(this);
        this.onClickParseSource = this.onClickParseSource.bind(this);
        this.onClickSave = this.onClickSave.bind(this);
        this.onClickShowSource = this.onClickShowSource.bind(this);
        this.onCLickAddCode = this.onCLickAddCode.bind(this);
        this.onCLickRemoveCode = this.onCLickRemoveCode.bind(this);

        this.onDClickCodeLine = this.onDClickCodeLine.bind(this);
        this.onDClickMainNote = this.onDClickMainNote.bind(this);
        this.onDClickHtmlContent = this.onDClickHtmlContent.bind(this);

        this.onFocusFroalaEditor = this.onFocusFroalaEditor.bind(this);
        this.onBlurFroalaEditor = this.onBlurFroalaEditor.bind(this);

        this.renderSourceEditor = this.renderSourceEditor.bind(this);
        this.renderChangeModeButton = this.renderChangeModeButton.bind(this);
        this.renderCodeLineNoteContent = this.renderCodeLineNoteContent.bind(this);
        this.renderMainNote = this.renderMainNote.bind(this);
        this.renderCodeLines = this.renderCodeLines.bind(this);
        this.renderAddRemoveCodeButton = this.renderAddRemoveCodeButton.bind(this);

        this.handleKeyDown = this.handleKeyDown.bind(this);
        this.openCloseCodeLineNotes = this.openCloseCodeLineNotes.bind(this);
    }

    componentWillMount() {
        this.runCodePrettify();
        document.addEventListener("keydown", this.handleKeyDown);

        let interval = setInterval(() => {
            if (typeof PR !== "undefined") {
                this.setState({ PRLoaded: true })
                clearInterval(interval);
            }
        }, 20);

        let noteId = this.props.match.params.id;
        console.log(noteId);
        fetch("/api/Note/" + noteId)
            .then(x => x.json())
            .then(note => {
                let mNote = {
                    content: note.mainNoteContent,
                    editorMode: note.editorMode,

                    visible: true,
                }
                let newCode = {
                    source: note.source,
                    showSourceEditor: false,

                    lines: note.lines
                        .sort((a, b) => a.order - b.order)
                        .map(line => {
                            return {
                                content: line.sourceContent,
                                id: line.inPageId,
                                dbId: line.id,
                                note: {
                                    content: line.noteContent,
                                    editorMode: line.editorMode,
                                    visible: line.visible,
                                }
                            }
                        })
                }

                this.setState({
                    mainNote: mNote,
                    code: newCode,
                });
            })
    }

    onClickSave() {
        let s = this.state;

        let dbLines = [];
        for (var i = 0; i < s.code.lines.length; i++) {
            let pl = s.code.lines[i];
            dbLines.push({
                order: i,
                id: pl.dbId,
                sourceContent: pl.content,
                inPageId: pl.id,
                noteContent: pl.note.content,
                editorMode: pl.note.editorMode,
                visible: pl.note.visible,
            });
        }

        let data = {
            id: this.props.match.params.id,
            mainNoteContent: s.mainNote.content,
            editorMode: s.mainNote.editorMode,
            source: s.code.source,
            showSourceEditor: s.code.showSourceEditor,
            lines: dbLines,
        }

        fetch("/api/Note/", {
            method: "POST",
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(data)
        })
            .then(x => x.json())
            .then((data) => {
                if (data == true) {
                    this.props.history.push(c.rootDir + "/" + this.props.match.params.dirId);
                } else {
                    alert("Save Note did not work!");
                }
            });
    }

    runCodePrettify() {
        let script = document.createElement('script');
        script.type = 'text/javascript';
        script.async = true;
        script.src = 'https://cdn.rawgit.com/google/code-prettify/master/loader/run_prettify.js';
        (document.getElementsByTagName('head')[0] || document.getElementsByTagName('body')[0]).appendChild(script);
    }

    handleKeyDown(event) {
        switch (event.keyCode) {
            case ESCAPE_KEY:
                this.openCloseCodeLineNotes();
                break;
            default:
                break;
        }
    }

    openCloseCodeLineNotes() {
        ///Closing main note.
        if (this.state.mainNote.editorMode == true) {
            let note = this.state.mainNote;
            note.editorMode = false;
            this.setState({ mainNote: note });
            return;
        }

        var newLines = this.state.code.lines;
        if (newLines.some(x => x.note.editorMode == true)) {
            newLines = newLines.map(x => {
                x.note.editorMode = false;
                return x;
            });
        } else if (newLines.some(x => x.note.visible == true)) {
            newLines = newLines.map(x => {
                x.note.visible = false;
                return x;
            });
        } else {
            newLines = newLines.map(x => {
                x.note.visible = true;
                return x;
            });
        }

        let newCode = this.state.code;
        newCode.lines = newLines;
        this.setState({ code: newCode });
    }

    onClickParseSource() {
        let sourceLines = this.state.code.source.split("\n");

        if (sourceLines.length == 1 && sourceLines[0].length == 0) {
            let newCode = this.state.code;
            newCode.lines = [],
                newCode.source = "";
            this.setState({ code: newCode });

            return;
        }

        let startNumber = 0;

        let existingInds = sourceLines
            .filter(x => indexPrefixRgx.exec(x) != null)
            .map(x => Number(indexPrefixRgx.exec(x)[1]));

        console.log(this.state.code.lines);

        for (var i = 0; i < sourceLines.length; i++) {
            let match = indexPrefixRgx.exec(sourceLines[i]);
            ///New line.
            if (match == null) {
                let newInd = findFirstOpening();
                let newItemNote = { content: initialNoteContent, editorMode: false, visible: false };
                sourceLines[i] = { content: sourceLines[i], id: newInd, dbId: 0, note: newItemNote };
                continue;
            }
            ///indexed line
            let existingIndex = match[1];
            console.log(existingIndex);
            let existingLine = this.state.code.lines.filter(x => x.id == existingIndex)[0];
            existingLine.content = sourceLines[i].replace(indexPrefixRgx, "");
            sourceLines[i] = existingLine;
        }

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

        let newCode = this.state.code;
        newCode.lines = sourceLines;
        newCode.source = sourceLines.map(x => "(*" + x.id + "*)" + x.content).join("\n");
        this.setState({ code: newCode });
    }

    onChangeMainNote(value) {
        let note = this.state.mainNote;
        note.content = value;
        this.setState({ mainNote: note });
    }

    onChangeCodeLineNote(value, line) {
        let newCode = this.state.code;
        newCode.lines = newCode.lines.map(x => {
            if (x.id == line.id) {
                x.note.content = value;
            }
            return x;
        });

        this.setState({ code: newCode });
    }

    range(start, end) {
        var ans = [];
        for (let i = start; i <= end; i++) {
            ans.push(i);
        }
        return ans;
    }

    renderCodeLines() {
        if (this.state.PRLoaded == false || this.state.codePresent == false) {
            return <div className="mt-5"></div>;
        } else {
            return (
                <Fragment>
                    {this.state.code.lines.map(x =>
                        <Fragment key={"codeLine" + x.id}>
                            {this.renderCodeLineNoteContent(x)}
                            <pre className="prettyprint"
                                onDoubleClick={() => this.onDClickCodeLine(x)}
                                dangerouslySetInnerHTML={{ __html: PR.prettyPrintOne(x.content) }} />
                        </Fragment>)}
                    <div className="mb-2 mt-5 row">
                        <div className="col-sm-2">
                            <button className="btn btn-primary btn-block" onClick={this.onClickParseSource}>Render</button>
                        </div>
                        <div className="col-sm-2">
                            <button className="btn btn-primary btn-block" onClick={this.onClickShowSource}>Show Source</button>
                        </div>
                    </div>
                </Fragment>)
        }
    }

    renderChangeModeButton(index) {
        return <button onClick={() => this.onClickChageMode(index)}>Done</button>
    }

    renderCodeLineNoteContent(line) {
        const FroalaEditor = this.FroalaEditor;
        const FroalaEditorView = this.FroalaEditorView;

        const options = {
            events: {
                'froalaEditor.focus': () => {
                    this.onFocusFroalaEditor();
                },
                'froalaEditor.blur': () => {
                    this.onBlurFroalaEditor();
                },
            }
        };

        if (line.note.visible == false) {
            return null;
        }

        if (line.note.editorMode == true) {
            return (
                <FroalaEditor
                    tag="textarea"
                    model={line.note.content}
                    onModelChange={(val) => this.onChangeCodeLineNote(val, line)}
                    config={options}
                />)
        }
        else {
            return (
                <div onDoubleClick={() => this.onDClickHtmlContent(line)}>
                    <FroalaEditorView model={line.note.content} />
                </div>)
        }
    }

    renderSourceEditor() {
        if (this.state.code.showSourceEditor) {
            return <Textarea onChange={this.onChangeSourceEditor} style={{ overflow: "hidden" }} type="text" value={this.state.code.source} className="form-control-black mb-2 mt-2" />
        } else {
            return null;
        }
    }

    renderMainNote() {
        const FroalaEditor = this.FroalaEditor;
        const FroalaEditorView = this.FroalaEditorView;

        const options = {
            events: {
                'froalaEditor.focus': () => {
                    this.onFocusFroalaEditor();
                },
                'froalaEditor.blur': () => {
                    this.onBlurFroalaEditor();
                },
            }
        };

        if (this.state.mainNote.editorMode == true) {
            return (
                <div className="mb-5">
                    <FroalaEditor
                        tag="textarea"
                        model={this.state.mainNote.content}
                        onModelChange={this.onChangeMainNote}
                        config={options} />
                </div>
            )
        } else {
            return (
                <div onDoubleClick={this.onDClickMainNote} className="mb-5">
                    <FroalaEditorView model={this.state.mainNote.content} />
                </div>
            )
        }
    }

    renderAddRemoveCodeButton() {
        if (this.state.codePresent) {
            return <button className="btn btn-primary btn-block" onClick={this.onCLickRemoveCode}>Hide Code</button>
        } else {
            return <button className="btn btn-primary btn-block" onClick={this.onCLickAddCode}>Show Code</button>
        }
    }

    onChangeSourceEditor(e) {
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

    onDClickCodeLine(line) {

        var newCode = this.state.code;
        newCode.lines = newCode.lines.map(x => {
            if (x.id == line.id) {
                x.note.visible = !x.note.visible;
            }
            return x;
        });

        this.setState({ code: newCode });
    }

    onDClickHtmlContent(line) {
        let newCode = this.state.code;
        newCode.lines = newCode.lines.map(x => {
            if (x.id == line.id) {
                x.note.editorMode = !x.note.editorMode;
            }
            return x;
        })
        this.setState({ code: newCode });
    }

    onDClickMainNote() {
        let mNote = this.state.mainNote;
        mNote.editorMode = !mNote.editorMode;
        this.setState({ mainNote: mNote });
    }

    onClickShowSource() {
        let newCode = this.state.code;
        newCode.showSourceEditor = !newCode.showSourceEditor;
        this.setState({ code: newCode });
    }

    onCLickAddCode() {
        let newCode = this.state.code;
        if (this.state.code.lines.length > 0) {
            newCode.showSourceEditor = false;
        } else {
            newCode.showSourceEditor = true;
        }

        this.setState({ codePresent: true, code: newCode });
    }
    onCLickRemoveCode() {
        let newCode = this.state.code;
        newCode.showSourceEditor = false;
        this.setState({ codePresent: false, code: newCode });
    }

    onBlurFroalaEditor() {
        this.setState({ currentlyTyping: false });
    }

    onFocusFroalaEditor() {
        this.setState({ currentlyTyping: true });
    }

    render() {
        const app = (<Fragment>
            {this.renderMainNote()}
            {this.renderSourceEditor()}
            {this.renderCodeLines()}
            <div className="mb-2 mt-2 row">
                <div className="col-sm-2">
                    <button className="btn btn-success btn-block" onClick={this.onClickSave}>Save</button>
                </div>
                <div className="col-sm-2">
                    {this.renderAddRemoveCodeButton()}
                </div>
            </div>
        </Fragment>);

        if (this.FroalaEditor) {
            if (this.state.PRLoaded) {
                return app;
            } else {
                return <LoadSvg />
            }
        } else {
            return null;
        }
    }
}