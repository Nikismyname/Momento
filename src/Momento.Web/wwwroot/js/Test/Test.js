$(document).ready(function () {

    $('#doneBtn').click(function () {
        var source = $('#source').val();
        console.log(source);
        var processesSource = initialize(source);
        console.log(processesSource);
        var prettyContent = PR.prettyPrintOne(processesSource);
        console.log(prettyContent);
        $('#prettyCode').html(prettyContent);
    });

    function initialize(text) {
        let inds = [];
        var regex = /([^\s(){}*:]{1,})(\(\*([0-9]*)*:*((?:[0-9]*,*)*)\*\))/g;
        var match = regex.exec(text);
        while (match != null) {
            console.log(match[0] + ' ' + match.index + ' ' + match[0].length + ' ' + match[1].length + ' ' + match[2].length);
            inds.push([match.index, match[1].length, match[2].length]);
            match = regex.exec(text);
        }

        for (let index = 0; index < inds.length; index++) {

            const element = inds[index];

            console.log(element[0] + ' ' +element[1] + ' ' + element[2]);

            let adjustment = 0;
            
            let posRemoveStart = element[0] + element[1];
            let posTemoveEnd = posRemoveStart + element[2];
            text = [text.slice(0, posRemoveStart) , text.slice(posTemoveEnd)].join(''); 
            adjustment -=element[2];

            let posAfter = element[0] + element[1];
            let spanTail = '</span>';
            text = [text.slice(0, posAfter), spanTail , text.slice(posAfter)].join('');
            adjustment+= spanTail.length;

            let spanHead = '<span class="clickable">';
            let posBefore = element[0];
            text = [text.slice(0, posBefore), spanHead , text.slice(posBefore)].join('');
            adjustment+= spanHead.length;

            for(let i = index+1; i< inds.length; i++){
                inds[i][0]+= adjustment;
            }
        }
        return text;
    }

    $('body').on('click', '.clickable', function () {
        alert('it is working');
    })
});