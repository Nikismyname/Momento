$(document).ready(function () {

    $('.deleteContent').click(function (e) {
        let res = confirm('Are you sure you want to delete the video.');
        if (res == false) {
            e.preventDefault();
        }
    });

    console.log('#dir' + $('#currDir').val());
    $('#dir' + $('#currDir').val()).show();

    $('.createDir').click(function (e) {
        let directoryName = prompt('Select directory name:');
        if (directoryName == null || directoryName == '' || directoryName.length == 0 || directoryName == 'Root') {
            e.preventDefault();
            alert('You must enter name');
            return;
        }

        $('#directoryName' + $(this).attr('pid')).val(directoryName);
    });

    $('.dirDel').click(function (e) {
        let res = confirm('Are you sure you want to delete the folder');
        if (res == false) {
            e.preventDefault();
        }
    });

    $('.directory').click(function () {
        let id = $(this).attr('id');
        $('#dir' + id).show();
        let pid = $(this).attr('pid');
        $('#dir' + pid).hide();
        history.pushState(null, null, '/Directory/Index/' + id );
    });

    $('.backLink').click(function () {
        let id = $(this).attr('id');
        $('#dir' + id).hide();
        let pid = $(this).attr('pid');
        $('#dir' + pid).show();
        history.pushState(null, null, '/Directory/Index/' + pid);
    })

    //mouse over 
    $('[data-toggle="tooltip"]').bootstrapTooltip();

    $('.directory').bind("contextmenu", function (e) {
        e.preventDefault();
        $('#cntnr').css("left", e.pageX);
        $('#cntnr').css("top", e.pageY - dw_getScrollOffsets().y);
        $('#cntnr').fadeIn(200, startFocusOut());

        $('#dirId').attr('val', $(this).attr('id'));
    });


    //determins how much is scrolled
    function dw_getScrollOffsets() {
        var doc = document, w = window;
        var x, y, docEl;
        
        if ( typeof w.pageYOffset === 'number' ) {
            x = w.pageXOffset;
            y = w.pageYOffset;
        } else {
            docEl = (doc.compatMode && doc.compatMode === 'CSS1Compat')?
                    doc.documentElement: doc.body;
            x = docEl.scrollLeft;
            y = docEl.scrollTop;
        }
        return {x:x, y:y};
    }

    ///Hiding the drop down menu
    function startFocusOut() {
        $(document).on("click", function () {
            $("#cntnr").hide();
            $(document).off("click");
        });
    }

    //This is the dropDown menu action on click.
    $("#items > li").click(function () {

        switch ($(this).text()) {
            case 'Delete':
                $('#delLink' + $('#dirId').attr('val'))[0].click();
                break;
            case 'Create Folder':
                $('#createFolder' + $('#dirId').attr('val'))[0].click();
                break;
            case 'Create Content':
                $('#createContent' + $('#dirId').attr('val'))[0].click();
                break;
            case 'Download Data':
                let val = $('#dirId').attr('val');
                $('#dl-id-input').val(val);
                $('#dl-submit-button')[0].click();
                break;
            case 'Upload Data':
                let val2 = $('#dirId').attr('val');
                $('#ul-id-input').val(val2);
                $('#ul-submit-button')[0].click();
                break;
        }
    });

    ///Sortable
    $(".sortableList").sortable({
        update: function (event, ui) {

            var type = $(this).attr('type');

            var parentId = $(this).attr('parentId');

            var divs = $('li div', this)
            let vals = [];
            for (var i = 0; i < divs.length; i++) {
                vals.push(divs[i].getAttribute('id'));
            }

            var data = {
                type: type,
                parentDir: parentId,
                values: vals,
            }

            $.ajax({
                type: 'POST',
                url: '/Directory/Reorder',
                data: data,
            });
        }
    });
});

