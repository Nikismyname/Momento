let previousScanComplete = true;

$(document).ready(function () {

    let shouldAltoSave = settings.AutoSaveProgress;
    console.log('Should autoSave: ' + shouldAltoSave);
    ///setting13 is auto save functionallity.
    $("#setting13").change(function () {
        if (this.checked) {
            shouldAltoSave = true;
        } else {
            shouldAltoSave = false;
        }
        console.log('Should autoSave: ' + shouldAltoSave);
    });

    console.log(previousScanComplete);

    let oldState = scan(true);

    //$('#test').click(function () {

    //    console.log(previousScanComplete);
    //    if (previousScanComplete == false) {
    //        ///TODO: add error message;
    //        console.log('The previous scan is not complete yet!');
    //        return;
    //    }

    //    let newState = scan(false);
    //    if (CheckForDifferences(oldState, newState, false)) {
    //        oldState = newState;
    //    }
    //});

    $('#submitBtn').click(function (e) {

        if (previousScanComplete == false) {
            console.log("Coudn't save the data try again in a coulple of seconds!");
            e.preventDefault();
        }

        let newState = scan(false);
        CheckForDifferences(oldState, newState, true);
    });

    setInterval(function () {
        if (shouldAltoSave) {
            let newState = scan(false);
            CheckForDifferences(oldState, newState, false);
            oldState = newState; 
            console.log("SCHEDULED AUTOSAVE COMPLETED");
        }
    }, 1000 * 60 * 0.5);

});

///scans all note elements and extracts all relevent data about it, as
///well as the data about it's parent note dbId if it is present;
function scan(isFirstScan) {
    console.log('scanning');
    if (isFirstScan == false) {
        previousScanComplete = false;
    }

    let videoResults = {};
    videoResults.url = $("#txtUrl").val();
    videoResults.name = $("#nameInput").val();
    videoResults.description = $("#descriptionInput").val();
    let videoSeekTo;
    if (isFirstScan) {
        videoSeekTo = $("#contentSeekTo").val();
    } else {
        videoSeekTo = Math.floor(player.getCurrentTime());
    }
    videoResults.seekTo = videoSeekTo;


    let noteResults = [];
    $('.note-identifier').each(function (index, value) {
        let noteNumber = getIdNumberFromElementId('mainNoteDiv', value);
        let textArea = $('textarea', value);
        let textVal = $(textArea).val();
        let deleted = $('#deleted' + noteNumber, value).val();
        let seekTo = $('#seekTo' + noteNumber, value).val();
        let inPageParentId = $('#inPageParentId' + noteNumber, value).val();
        let type = $('#type' + noteNumber, value).val();
        let formatting = $('#select' + noteNumber, value).val();
        let dbId = $('#id' + noteNumber, value).val();
        let level = $('level' + noteNumber, value).val();

        let parentDbId;
        let parentInPageId;
        let parentDiv = $(value).parent();

        //if the note is root, parentDbId is -1
        if ($(parentDiv).attr('id') == 'NewNoteLocation') {
            parentDbId = -1;
            parentInPageId = -1;
        } else {
            parentInPageId = getIdNumberFromElementId('div', parentDiv);
            ///TODO: make it not search the whole document to find the id
            parentDbId = $('#id' + parentInPageId).val();
        }

        ///the nams of props are matched to the NoteCreate model 
        resultInst = {};
        resultInst.Id = dbId;
        resultInst.Content = textVal;
        resultInst.SeekTo = seekTo;
        resultInst.Formatting = formatting;
        resultInst.Type = type;
        resultInst.InPageId = noteNumber;
        resultInst.InPageParentId = inPageParentId;
        resultInst.InPageParentId = parentInPageId;
        resultInst.ParentDbId = parentDbId;
        resultInst.Level = level;
        resultInst.Deleted = deleted;
        noteResults[noteNumber] = resultInst;
    });

    let result = {};
    result.noteResults = noteResults;
    result.videoResults = videoResults;

    return result;
}

///extracts the noteNumber from id which ends with it
function getIdNumberFromElementId(restOfId, value) {
    let elementIdLength = restOfId.length;
    let fullId = $(value).attr('id');
    let noteNumber = fullId.substring(elementIdLength);
    return noteNumber;
}

function CheckForDifferences(oldStateFull, newStateFull, final) {

    let oldState = oldStateFull.noteResults;
    let newState = newStateFull.noteResults;
    ///changes to existing items with dbIds
    let changes = [];
    let counter = 0;
    for (let ind in oldState) {
        let oldEntry = oldState[ind];
        let newEntry = newState[ind];

        let noteNumber = Number[oldEntry.InPageId];

        for (let prop in oldEntry) {

            ///Beacuse of 1) we do not check for differences in Id
            if (prop == "Id") {
                continue;
            }
            
            let oldValue = oldEntry[prop];
            let newValue = newEntry[prop];

            if (oldValue != newValue) {
                changes[counter] = [];
                ///1)insead of updating the currentEntry with the new dbIds we 
                ///just update the elements witch is cought by the new scan 
                ///and use the Id from there
                changes[counter][0] = newEntry.Id;
                changes[counter][1] = prop;
                changes[counter][2] = newValue;
                counter++;
                console.log(prop + ' for note ' + noteNumber + ' changed from ' + oldValue + ' to ' + newValue);
            }
        }
    }

    ///New Items without dbIds 
    var existringNoteIds = oldState.map(x => x.InPageId);
    var currentMoteIds = newState.map(x => x.InPageId);
    let newItemsIds = [];
    for (var i = 0; i < currentMoteIds.length; i++) {
        let id = currentMoteIds[i];
        if (!existringNoteIds.includes(id)) {
            newItemsIds.push(id);
        }
    }
    let newItems = [];
    for (var i = 0; i < newState.length; i++) {
        let el = newState[i];
        if (newItemsIds.includes(el.InPageId)) {
            newItems.push(el);
        }
    }

    let videoFields = [];
    let seekTo = newStateFull.videoResults.seekTo == oldStateFull.videoResults.seekTo ?
        null : newStateFull.videoResults.seekTo;
    videoFields.push(seekTo);
    let name = newStateFull.videoResults.name == oldStateFull.videoResults.name ?
        null : newStateFull.videoResults.name;
    videoFields.push(name);
    let description = newStateFull.videoResults.description == oldStateFull.videoResults.description ?
        null : newStateFull.videoResults.description;
    videoFields.push(description);
    let url = newStateFull.videoResults.url == oldStateFull.videoResults.url ?
        null : newStateFull.videoResults.url;
    videoFields.push(url);

    let changesToTheVideoFields = true;
    if (videoFields.every(x => x == null)) {
        changesToTheVideoFields = false;
    }
    let newNotes = true;
    if (newItems.length == 0) {
        newNotes = false;
    }
    let changesToExistingNotes = true;
    if (changes.length == 0) {
        changesToExistingNotes = false;
    }

    let data = {
        seekTo: seekTo,
        name: name,
        description: description,
        url: url,
        videoId: $('#videoId').val(),

        changes: changes,
        newItems: newItems,
        finalSave: final
    };

    if (changesToTheVideoFields || newNotes || changesToExistingNotes || final) {

        $.ajax({
            type: "POST",
            url: "/Video/PartialSave",
            data: data,
            success: function (result) {
                AssignDbIds(result, final);
            },
            error: function () {
                console.log("AJAX ERROR");
            }
        });
    }
    /// in case there are no changes and it is the submit button 
    /// we redirect to parent dir
    else if (final) {

        $('#go-back-to-dir')[0].click();
    }

    function AssignDbIds(result, final) {

        console.log('AJAX SUCCESSS / Assigning new Ids!');

        if (result[0][0] != 0) {
            ///handle error;
            ///TODO: break it down to codes
            console.log("ERROR Sometging in the partial save went wrong");
            return;
        }

        if (!final) {
            ///update the html dbIds for future scans
            for (let i = 1; i < result.length; i++) {

                let inPageId = result[i][0];
                let dbId = result[i][1];
                $('#id' + inPageId).attr('value', dbId);
            }

            ///After reciving the new Ids we are ready for new scan
            previousScanComplete = true;
        } else {
            $('#go-back-to-dir')[0].click();
        }
    }

    return true;
}