$(document).ready(function () {

    let notes = [];

    $(function(){

    });

    $('#parseSource').click(function () {
        
        let source = $('#source').val();
        let parsedSource = parseSource(source);
        $('#source').val(parsedSource);

        let sourceForPrettifying = renderForPrettifying(parsedSource);
        let prettyContent = PR.prettyPrintOne(sourceForPrettifying);
        $('#prettyCode').html(prettyContent);
        $('#prettyCode').show();
    });

    function parseSource(source){
        let regex = /([a-zA-Z]{1,})(\(\*[0-9]*\*\))*/g
        let match = regex.exec(source);
        let indecies = [];
        let numbers = [];
        while (match != null){
            if(match[2] == null || match[2] == ''){
                indecies.push(match.index + match[1].length);
            }else{
                numbers.push(Number(match[2]));
            }
            match = regex.exec(source);
        }
        let nextIndex = Math.max(numbers) + 1;

        let adjustment = 0;
        for (let i = 0; i < indecies.length; i++) {
            let index = indecies[i];
            index += adjustment;

            let insert = '(*'+ nextIndex +'*)';
            nextIndex++;
            source = [source.slice(0, index) , insert , source.slice(index)].join(''); 
            adjustment += insert.length;
        }
        return source;
    }

    function renderForPrettifying(text) {
        let inds = [];
        let regex = /([a-zA-Z]{1,})(\(\*([0-9]{0,})\*\))/g;
        let match = regex.exec(text);
        while (match != null) {
            inds.push([match.index, match[1].length, match[2].length, match[3]]);
            match = regex.exec(text);
        }

        let adjustment = 0;
        for (let index = 0; index < inds.length; index++) {

            const element = inds[index];
            element[0] += adjustment;

            let posRemoveStart = element[0] + element[1];
            let posTemoveEnd = posRemoveStart + element[2];
            text = [text.slice(0, posRemoveStart) , text.slice(posTemoveEnd)].join(''); 
            adjustment -=element[2];

            let posAfter = element[0] + element[1];
            let spanTail = '</span>';
            text = [text.slice(0, posAfter), spanTail , text.slice(posAfter)].join('');
            adjustment+= spanTail.length;

            let spanHead = '<span class="clickable" val='+element[3]+'>';
            let posBefore = element[0];
            text = [text.slice(0, posBefore), spanHead , text.slice(posBefore)].join('');
            adjustment+= spanHead.length;
        }

        return text;
    }

    $(function(){
        var noteDivs = $('#notes div');
        var nextId = noteDivs.length;

        $('body').on('click','.clickable', function (){
            createNote(nextId);
            nextId++;
        });
    })

    function createNote(nextId, wordId){
        let noteDiv = $('<div noteId="'+nextId+'" class="note"></div>');    
        let content = $('<textarea class="form-control-black  size-height-auto" id="textarea' + nextId + '" rows="2" cols="50" name="Notes[' + nextId + '].Content"></textarea>');
        let hashtags = $('<input type="text" class="form-control-black" id="hashtags' + nextId + '" name="Notes[' + nextId + '].Hashtags"></input>');
        let wordIdObject = $('<input type="number" name="Notes[' + nextId + '].WordId value="'+wordId+'" hidden="hidden"/>');
        $(noteDiv).append(content);
        $(noteDiv).append(hashtags);
        $(noteDiv).append(wordIdObject);

        $('#notes').append(noteDiv);
    }
});