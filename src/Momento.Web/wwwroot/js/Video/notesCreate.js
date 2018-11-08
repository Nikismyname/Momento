function notesFunction(enumerators, notesCount, existingNotes, settings) {

    $(function () {
        let nextId = notesCount;
        let map = existingNotes;

        $('.add-note').on('click', function () {

            let isTop = $(this).attr('position') == 'top' ? true : false;
            subNoteButtonFunction(1, -1, true, isTop);
        });

        let subNoteButtonInfo = [
            {
                enabled: false,
                parentId: null
            },
            {
                enabled: false,
                parentId: null
            },
            {
                enabled: false,
                parentId: null
            },
        ]

        function subNoteButtonFunction(level, parentId, mainAdd, top) {
            $('.goBack').remove();

            //setting up the SSub buttons
            if (level < 4) {
                let zbLevel = level - 1;
                subNoteButtonInfo[zbLevel].enabled = true;
                subNoteButtonInfo[zbLevel].parentId = nextId;
                for (let i = zbLevel + 1; i < subNoteButtonInfo.length; i++) {
                    const element = subNoteButtonInfo[i];
                    element.enabled = false;
                    element.parentId = null;
                }
                RenderSubButtons();
            }

            map[nextId] = parentId;

            let noteDiv = $('<div id="mainNoteDiv' + nextId + '" class="note note-identifier"></div>');
            ///visible section
            let contentDiv = $('<div class="flex"></div>');
            let content = $('<textarea class="form-control-black size-auto" id="textarea' + nextId + '" rows="2" cols="50" name="ContentCreate.Notes[' + nextId + '].Content"></textarea>');
            let goBackBtn = $('<button id="goBack' + nextId + '" class="goBack btn btn-secondary" type="button">Back</button>');
            $(contentDiv).append(content);
            $(contentDiv).append(goBackBtn);
            let select = $('<select class="form-control-black size-auto" id="select' + nextId + '" name="ContentCreate.Notes[' + nextId + '].Formatting"></select>');
            let deleteBtn = $('<button class="btn btn-warning" id="delButton' + nextId + '" type="button" >Delete</button>');
            let noteControlsDiv = $('<div class="flex"></div>');
            $(noteControlsDiv).append(select);
            $(noteControlsDiv).append(deleteBtn);
            ///hidden section
                ///with ids
            let idInput = $('<input id="id'+nextId+'" type="number" name="ContentCreate.Notes['+nextId+'].Id" value="0" hidden="hidden">');
            let deleted = $('<input id="deleted' + nextId + '" type="text" name="ContentCreate.Notes[' + nextId + '].Deleted" value="false" hidden="hidden"/>');
            let seekTo = $('<input id="seekTo'+nextId+'" type="number" name="ContentCreate.Notes[' + nextId + '].SeekTo" value="' + Math.floor(player.getCurrentTime() ? player.getCurrentTime() : 0) + '" hidden="hidden"/>');
            let inPageParentId = $('<input id="inPageParentId'+nextId+'" type="number" name="ContentCreate.Notes[' + nextId + '].InPageParentId" value="' + parentId + '" hidden="hidden"/>');
            let noteTypeInput = $('<input id="type'+nextId+'" type="number" name="ContentCreate.Notes[' + nextId + '].Type" value="0" hidden="hidden" />');
            let levelInput = $('<input id="level'+nextId+'" type="number" name="ContentCreate.Notes[' + nextId + '].Level" value="' + level + '" hidden="hidden"/>');
                ///no ids
            let inPageId = $('<input type="number" name="ContentCreate.Notes[' + nextId + '].InPageId" value="' + nextId + '" hidden="hidden"/>');
            
            let subNoteDiv = $('<div class="note-offset" id = "div' + nextId + '"></div>');

            for (var i = 0; i < enumerators.length; i++) {
                enumerators[i] = enumerators[i].replace("_", " ");
                select.append('<option value="' + i + '">' + enumerators[i] + '</option>');
            }

            $(noteDiv).append(contentDiv);
            if (level < 4) {
                let subNoteBtn = $('<button class="btn btn-primary" id="subNoteButton' + nextId + '" type="button">Sub-note</button>');
                $(noteControlsDiv).prepend(subNoteBtn);
                $(noteDiv).append(noteControlsDiv);
            } else {
                $(noteDiv).append(noteControlsDiv);
            }
            $(noteDiv).append(levelInput);
            $(noteDiv).append(deleted);
            $(noteDiv).append(seekTo);
            $(noteDiv).append(inPageId);
            $(noteDiv).append(noteTypeInput);
            $(noteDiv).append(idInput);
            if (level > 1) {
                $(noteDiv).append(inPageParentId);
            }
            let parentDiv;
            if (parentId == -1) {
                parentDiv = "#NewNoteLocation";
            }
            else {
                parentDiv = '#div' + parentId;
            }
            $(parentDiv).append(noteDiv);
            $(parentDiv).append(subNoteDiv);

            ///settings 
            if (mainAdd && settings.GoDownOnNewNoteTop == true) {
                $(content).focus();
                $(noteDiv)[0].scrollIntoView(false);
            }
            if (mainAdd == false && settings.GoDownOnSubNoteAll == true) {
                $(content).focus();
                $(noteDiv)[0].scrollIntoView(false);
            }

            let currentId = nextId;
            if (level < 4) {
                $('#subNoteButton' + nextId).on('click', function () { subNoteButtonFunction(level + 1, currentId, false, false) });
            }

            $('#delButton' + nextId).on('click', function () {
                let conf = confirm("Are you sure you want to delete note and all subnotes?");
                if (conf == true) {
                    deleteNote(level, currentId)
                }
            });

            $('.goBack').on('click', function () {
                window.scrollTo(0, 0);
                player.playVideo();
                $('.goBack').remove();
            })

            if (mainAdd == true) {

                if (top == true && settings.PauseVideoOnTopNewNote == true) {
                    player.pauseVideo();
                }
                if (top == false && settings.PauseVideoOnBottomNewNote == true) {
                    player.pauseVideo();
                }
                ///SubNoteLogicHere 
            } else {
                if (top == true && settings.PauseVideoOnSubNoteTop == true) {
                    player.pauseVideo();
                }
                if (top == false && settings.PauseVideoOnSubNoteRegular == true) {
                    player.pauseVideo();
                }
            }

            nextId++;
        };

        function deleteNote(level, noteId) {
            $('#mainNoteDiv' + noteId).removeClass("note")
            $('#deleted' + noteId).val("true");
            $('#textarea' + noteId).hide();
            $('#goBackButton' + noteId).hide();
            $('#subNoteButton' + noteId).hide();
            $('#select' + noteId).hide();
            $('#delButton' + noteId).hide();
            $('#goBack' + noteId).remove();

            for (var i = 0; i < map.length; i++) {
                if (map[i] == noteId) {
                    deleteNote(level, i)
                }
            }
        }

        $('.subNoteButton').click(function () {
            subNoteButtonFunction(Number($(this).attr('level')) + 1,
                Number($(this).attr('parentId')),
                true)
        });

        $('.delButton').click(function () {
            let conf = confirm("Are you sure you want to delete note and all subnotes?");
            if (conf == true) {
                deleteNote(1, $(this).attr('noteId'));
            }
        });

        function createNonNote(type, top) {
            $('.goBack').remove();

            let noteDiv = $('<div id="mainNoteDiv' + nextId + '" class="note note-identifier"></div>');
            ///visible section
            let content = $('<textarea class="form-control-black size-auto" id="textarea' + nextId + '" rows="1" cols="50" name="ContentCreate.Notes[' + nextId + '].Content"></textarea>');
            let deleteBtn = $('<button class="btn btn-warning" id="delButton' + nextId + '" type="button" >Delete</button>');
            ///hidden section
            let idInput = $('<input id="id' + nextId + '" type="number" name="ContentCreate.Notes[' + nextId + '].Id" value="0" hidden="hidden">');
            let deleted = $('<input id="deleted' + nextId + '" type="text" name="ContentCreate.Notes[' + nextId + '].Deleted" value="false" hidden="hidden"/>');
            let seekTo = $('<input id="seekTo'+nextId+'" type="number" name="ContentCreate.Notes[' + nextId + '].SeekTo" value="' + Math.floor(player.getCurrentTime() ? player.getCurrentTime() : 0) + '" hidden="hidden"/>');
            let noteTypeInput = $('<input id="type'+nextId+'" type="number" name="ContentCreate.Notes[' + nextId + '].Type" value="' + type + '" hidden="hidden" />');
            let inPageId = $('<input type="number" name="ContentCreate.Notes[' + nextId + '].InPageId" value="' + nextId + '" hidden="hidden"/>');

            let contentDiv = $('<div class="flex"></div>');
            $(contentDiv).append(content);
            $(contentDiv).append(deleteBtn);

            if (type == 1) {
                $(content).addClass("time-stamp");
            } if (type == 2) {
                (content).addClass("new-topic");
            }

            $(noteDiv).append(contentDiv);
            $(noteDiv).append(deleted);
            $(noteDiv).append(seekTo);
            $(noteDiv).append(inPageId);
            $(noteDiv).append(noteTypeInput);
            $(noteDiv).append(idInput);

            $('#NewNoteLocation').append(noteDiv);

            let currentId = nextId;
            $('#delButton' + nextId).on('click', function () {
                let conf = confirm("Are you sure you want to delete note and all subnotes?");
                if (conf == true) {
                    deleteNote(1, currentId)
                }
            });

            if (settings.GoDownOnNewTimeStampTop == true && type == 1) {
                $(noteDiv).focus();
                $(noteDiv)[0].scrollIntoView(false);
            }
            if (settings.GoDownOnNewTopicTop == true && type == 2) {
                $(noteDiv).focus();
                $(noteDiv)[0].scrollIntoView(false);
            }

            if(settings.PauseVideoOnTopicTop == true && type ==2 && top == true){
                player.pauseVideo();
            }
            if(settings.PauseVideoOnTopicBottom == true && type ==2 && top == false){
                player.pauseVideo();
            }
            if(settings.PauseVideoOnTimeStampTop == true && type ==1 && top == true){
                player.pauseVideo();
            }
            if(settings.PauseVideoOnTimeStampBottom == true && type ==1 && top == false){
                player.pauseVideo();
            }

            nextId++;
        }

        function RenderSubButtons() {
            for (let i = 1; i < subNoteButtonInfo.length + 1; i++) {
                let button = $('#add' + i);
                let info = subNoteButtonInfo[i - 1];
                if (info.enabled == true) {
                    $(button).prop('disabled', false);
                    $(button).unbind('click');
                    button.click(function () {
                        subNoteButtonFunction(i + 1, info.parentId, false, true);
                    });
                } else {
                    $(button).unbind('click');
                    $(button).prop('disabled', true);
                }
            }
        }

        $('.time-stamp-button').click(function () {
            let isTop = $(this).attr('position') == 'top' ? true : false;
            createNonNote(1, isTop)
        });
        $('.new-topic-button').click(function () {
            let isTop = $(this).attr('position') == 'top' ? true : false;
            createNonNote(2, isTop)
        })
    });

    $('#go-back').click(function () {
        window.scrollTo(0, 0);
    });

    $('#submitBtn').click(function () {
        let currTime = Math.floor(player.getCurrentTime());
        $('#contentSeekTo').val(currTime);
    });

    //settings 
    let settingsDiv = $('#settings-div');
    $('#settings-btn').click(function () {
        if ($(this).attr('state') == '0') {

            $(settingsDiv).show();
            $(this).attr('state', '1')
        } else {
            $(settingsDiv).hide();
            $(this).attr('state', '0')
        }
    });
    let settingsMap = {};
    settingsMap.setting1 = 'PauseVideoOnTopNewNote';
    settingsMap.setting2 = 'PauseVideoOnBottomNewNote';
    settingsMap.setting3 = 'PauseVideoOnSubNoteTop';
    settingsMap.setting12 = 'PauseVideoOnSubNoteRegular';
    settingsMap.setting4 = 'PauseVideoOnTopicTop'; 
    settingsMap.setting5 = 'PauseVideoOnTopicBottom';
    settingsMap.setting6 = 'PauseVideoOnTimeStampTop';
    settingsMap.setting7 = 'PauseVideoOnTimeStampBottom';
    settingsMap.setting8 = 'GoDownOnNewNoteTop';
    settingsMap.setting9 = 'GoDownOnSubNoteAll';
    settingsMap.setting10 = 'GoDownOnNewTopicTop';
    settingsMap.setting11 = 'GoDownOnNewTimeStampTop';
    settingsMap.setting13 = 'AutoSaveProgress';

    for (let i in settingsMap) {
        $('#' + i).attr('checked', settings[settingsMap[i]]);
    }

    ///OnCange of setting we update the settings object 
    $('#settings-div div div input').change(function () {
        if (this.checked) {
            settings[settingsMap[$(this).attr('id')]] = true;
        } else {
            settings[settingsMap[$(this).attr('id')]] = false;
        }

        if (settingsMap[$(this).attr('id')] == "AutoSaveProgress") {

        }
    });
}

function onStateChange(e) {

    if (e.data == 1) {
        $('#start-pause').html('Pause');
        $('#start-pause').unbind('click');
        $('#start-pause').click(function () {
            player.pauseVideo();
        });
    }
    else if (e.data == -1 || e.data == 2) {
        $('#start-pause').html('Play');
        $('#start-pause').unbind('click');
        $('#start-pause').click(function () {
            player.playVideo();
        });
    }
}

