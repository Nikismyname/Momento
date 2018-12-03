function notesFunction(enumerators, notesCount, existingNotes) {

    $(function () {
        let nextId = notesCount;
        var map = existingNotes;

        $('.add-note').on('click', function () {
            subNoteButtonFunction(1, -1, true);
        });

        function subNoteButtonFunction(level, parentId, mainAdd) {
            $('.goBack').remove();

            map[nextId] = parentId;

            let noteDiv = $('<div class="note"></div>');

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
            let levelInput = $('<input type="number" name="ContentCreate.Notes[' + nextId + '].Level" value="' + level + '" hidden="hidden"/>');
            let deleted = $('<input id="deleted' + nextId + '" type="text" name="ContentCreate.Notes[' + nextId + '].Deleted" value="false" hidden="hidden"/>');
            let seekTo = $('<input type="number" name="ContentCreate.Notes[' + nextId + '].SeekTo" value="' + Math.floor(player.getCurrentTime() ? player.getCurrentTime() : 0) + '" hidden="hidden"/>');
            let inPageId = $('<input type="number" name="ContentCreate.Notes[' + nextId + '].InPageId" value="' + nextId + '" hidden="hidden"/>');
            let inPageParentId = $('<input type="number" name="ContentCreate.Notes[' + nextId + '].InPageParentId" value="' + parentId + '" hidden="hidden"/>');
            let noteTypeInput = $('<input type="number" name="ContentCreate.Notes[' + nextId + '].Type" value="0" hidden="hidden" />');

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

            $(content).focus();

            let currentId = nextId;
            if (level < 4) {
                $('#subNoteButton' + nextId).on('click', function () { subNoteButtonFunction(level + 1, currentId, false) });
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

            if (mainAdd) {
                window.scrollTo(0, document.body.scrollHeight);
            }

            player.pauseVideo();
            nextId++;
        };

        function deleteNote(level, noteId) {
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
                Number($(this).attr('parentId')))
        });

        $('.delButton').click(function () {
            let conf = confirm("Are you sure you want to delete note and all subnotes?");
            if (conf == true) {
                deleteNote(1, $(this).attr('noteId'));
            }
        });

        function createNonNote(type) {
            $('.goBack').remove();
            window.scrollTo(0, document.body.scrollHeight);

            let noteDiv = $('<div class="note"></div>');
            let content = $('<textarea class="form-control-black size-auto" id="textarea' + nextId + '" rows="1" cols="50" name="ContentCreate.Notes[' + nextId + '].Content"></textarea>');
            let deleteBtn = $('<button class="btn btn-warning" id="delButton' + nextId + '" type="button" >Delete</button>');
            let contentDiv = $('<div class="flex"></div>');
            $(contentDiv).append(content);
            $(contentDiv).append(deleteBtn);

            let deleted = $('<input id="deleted' + nextId + '" type="text" name="ContentCreate.Notes[' + nextId + '].Deleted" value="false" hidden="hidden"/>');
            let seekTo = $('<input type="number" name="ContentCreate.Notes[' + nextId + '].SeekTo" value="' + Math.floor(player.getCurrentTime() ? player.getCurrentTime() : 0) + '" hidden="hidden"/>');
            let inPageId = $('<input type="number" name="ContentCreate.Notes[' + nextId + '].InPageId" value="' + nextId + '" hidden="hidden"/>');
            let noteTypeInput = $('<input type="number" name="ContentCreate.Notes[' + nextId + '].Type" value="' + type + '" hidden="hidden" />');

            if (type == 1) {
                content.css('background-color','blue');
            }if(type ==2){
                content.css('background-color','green');
            }

            $(noteDiv).append(contentDiv);
            $(noteDiv).append(deleted);
            $(noteDiv).append(seekTo);
            $(noteDiv).append(inPageId);
            $(noteDiv).append(noteTypeInput);

            $('#NewNoteLocation').append(noteDiv);

            nextId++;
        }

        $('.time-stamp').click(function () { createNonNote(1) });
        $('.new-topic').click(function () { createNonNote(2) })
    });

    $('#go-back').click(function(){
        window.scrollTo(0, 0);
    });

    $('#submitBtn').click(function () {
        let currTime = Math.floor(player.getCurrentTime());
        $('#contentSeekTo').val(currTime);
    });
}