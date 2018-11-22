function ToDoList(tabCounts, itemsCount) {

    let nextId = itemsCount;
    let alternatingRightClick = 0;

    ///Create new ITEM
    $('#btn-create').on('click', function () {

        let tabName = getActiveTabName();
        let tab = $('#' + tabName);
        let appendTarget = $('ul', tab);

        let box = $('<div id="box' + nextId + '" ident="' + nextId + '" class="todo-item content-none box" style="display:inline-block"><p id="text' + nextId + '"></p></div>')
        sizeBox(box);

        let newLi = $('<li class="ui-state-default non-li"></li>');

        let contentDiv = $('<div></div>');
        let commentDiv = $('<div></div>');
        let contentTextArea = $('<textarea id="text-area' + nextId + '" name="Items[' + nextId + '].Content" class="text-area form-control-black" ident="' + nextId + '" rows="1" cols="100" style="display:none"></textArea>');
        let commentTextArea = $('<textarea id="comment' + nextId + '" name="Items[' + nextId + '].Comment"  class="comment form-control-black" ident="' + nextId + '" rows="3" cols="100" style="display:none;height:100%"></textarea>');
        let statusInput = $('<input id="status' + nextId + '" class="status-input" name="Items[' + nextId + '].Status" value="' + tabName + '" hidden="hidden" />');
        let deletedInput = $('<input id="deleted' + nextId + '" class="delete-input" name="Items[' + nextId + '].Deleted" value="false" hidden ="hidden"/>');
        let orderInput = $('<input id="order' + nextId + '" class="order-input" name="Items[' + nextId + '].Order" value="' + tabCounts[tabName] + '" hidden="hidden"/>');
        let idInput = $('<input id="id' + nextId + '" class="id-input" name="Items[' + nextId + '].Id" value="0" hidden="hidden" />');
        let changedInput = $('<input id="changed' + nextId + '" class="change-input" name="Items[' + nextId + '].Changed" value="false" hidden="hidden" />');
        $(contentDiv).append(contentTextArea);
        $(commentDiv).append(commentTextArea);

        $(newLi).append(box);
        $(newLi).append(contentDiv);
        $(newLi).append(commentDiv);
        $(newLi).append(statusInput);
        $(newLi).append(deletedInput);
        $(newLi).append(orderInput);
        $(newLi).append(idInput);
        $(newLi).append(changedInput);
        $(newLi).addClass("margin-top-1-percent");
        $(appendTarget).append(newLi);

        $(commentTextArea).mousedown(function () { textAreaMouseDown(commentTextArea, false) });
        $(commentTextArea).blur(function () { textAreaBlur(commentTextArea, false) });
        $(contentTextArea).mousedown(function () { textAreaMouseDown(contentTextArea, true) });
        $(contentTextArea).blur(function () { textAreaBlur(contentTextArea, true) });
        $(box).click(boxClick);

        tabCounts[tabName]++;
        nextId++;

        ///binding right click to new boxes
        $(box).bind("contextmenu", function (e) {

            e.preventDefault();
            $('#cntnr').css("left", e.pageX);
            $('#cntnr').css("top", e.pageY - dw_getScrollOffsets().y);
            $('#cntnr').fadeIn(200, startFocusOut());

            rightClickedElement = this.parentElement;
        });
    });

    ///New TAB 
    $('#btn-new-tab').click(function () {

        let newTabName = prompt("Select name for the new Tab!", "NameHere");
        let regex = /^[a-zA-Z0-9]{1,}$/;
        if (!regex.test(newTabName)) {
            alert('Tab names may only contain letters and digits!');
            return;
        }
        let existingTabNames = [];
        for (let key in tabCounts) {
            existingTabNames.push(key);
        }
        if (existingTabNames.includes(newTabName)) {
            alert('You can not have tabs with the same name');
            return;
        }

        let tabUl = $('#tab-ul');
        let contentParent = $('#newTabContentHere')

        let headerLi = $('<li class="nav-item"></li>');
        let headerA = $('<a id="heading-' + newTabName + '" class="nav-link nav-link-tab" data-toggle="tab" href="#' + newTabName + '">' + newTabName + '</a>');
        $(headerLi).append(headerA);
        $(tabUl).append(headerLi);

        let contentDiv = $('<div id="' + newTabName + '" class="container tab-pane fade"></div>');
        let contentUl = $(' <ul class="sortableList non-list" style="display:inline-block" type="' + newTabName + '"></ul>');
        $(contentDiv).append(contentUl);
        $(contentDiv).insertBefore(contentParent);

        tabCounts[newTabName] = 0;

        ///updating the categories to include the new one for saving
        let currCategories = $('#todo-list-categories').attr('value');
        let newCategories = currCategories + ';' + newTabName;
        $('#todo-list-categories').attr('value', newCategories);

        ///TODO: check if items with that name exist in unassigned and assign them to the new tab
    });

    ///Delete Tab Button
    $('#btn-delete-tab').click(function () {

        let tabName = getActiveTabName();
        if (tabName == 'unassigned') {
            alert("You can not delete the unassigned categorie!");
            return;
        }

        if (!confirm('Are you sure you want to delete ' + tabName + ' Tab?')) {
            return
        }

        ///Removing the deleted name from object so you can create new item with the same name
        delete tabCounts[tabName];

        let headingToDelete = $('#heading-' + tabName);
        $(headingToDelete).hide();
        $(headingToDelete).attr('id', 'deleted')

        let categories = $('#todo-list-categories').attr("value");
        categories = categories.replace(';' + tabName, '');
        $('#todo-list-categories').attr("value", categories);

        ///Move all the deleted items to unassigned 
        let bodyToDelete = $('#' + tabName);
        let lisInBodyToDel = $('li', bodyToDelete);
        let unassignedUl = $('#unassigned ul');
        $(unassignedUl).append(lisInBodyToDel);

        ///switch focus to unassigned 
        let unassigned = $('#heading-unassigned');
        $(unassigned)[0].click();
    });

    ///Rename Button
    $('#btn-rename').click(function () {
        let newTabName = prompt("Select name for the new Tab!", "NameHere");

        if (newTabName == null) {
            return;
        }

        let regex = /^[a-zA-Z0-9]{1,}$/;
        if (!regex.test(newTabName)) {
            alert('Tab names may only contain letters and digits!');
            return;
        }

        let existingTabNames = [];
        for (let key in tabCounts) {
            existingTabNames.push(key);
        }

        if (existingTabNames.includes(newTabName)) {
            alert('You can not have tabs with the same name!');
            return;
        }

        ///Rename the heading.
        let oldTabName = getActiveTabName();
        $('#heading-' + oldTabName).text(newTabName);

        ///Rename the id of the the heading
        $('#heading-' + oldTabName).attr('id', 'heading-' + newTabName);

        ///Change the name in the categories field WORKS
        let categories = $('#todo-list-categories').attr("value");
        categories = categories.replace(oldTabName, newTabName);
        $('#todo-list-categories').attr("value", categories);

        ///Change the name in the  tabCounts WORKS
        tabCounts[newTabName] = tabCounts[oldTabName];
        delete tabCounts[oldTabName];

        ///Change the categories of the items which heading was renamed
        let bodyThatWasRenamed = $('#' + oldTabName);
        let lisInBodyRenamed = $('li', bodyThatWasRenamed);
        $(lisInBodyRenamed).each(function (ind, li) {
            $('input', li).attr('value', newTabName);
        });

        ///Cnage the id of the body so it could be found after the first rename
        $(bodyThatWasRenamed).attr('id', newTabName);
    });

    ///Delete All Items
    $('#btn-delete-all-items').click(function () {
        let confirmResult = confirm("Are you sure you want to delete all Items in this Tab?");
        if (!confirmResult) {
            return;
        }

        let tabName = getActiveTabName();
        let bodyWhosLisToDel = $('#' + tabName);
        let lis = $('li', bodyWhosLisToDel);
        $(lis).each(function (id, li) {
            deleteItem(li);
        });
        tabCounts[tabName] = 0;
    });

    ///DeleteIitem 
    function deleteItem(item) {
        $('.delete-input', item).attr('value', 'true');
        $(item).hide();
    }

    let initialContent = null;
    let initialComment = null; 
    ///item change states from textbox to dragable div
    function boxClick() {
        let parentUl = $(this.parentElement);
        let textArea = $('.text-area', parentUl);
        let comment = $('.comment', parentUl);
        $(this).hide();
        $(textArea).show();
        $(comment).show();
        $(textArea).focus();
        let text = $('p', this)[0].innerHTML;
        textArea.val(text);
        $(textArea).focus();

        initialContent = textArea.val();
        initialComment = comment.val();
    };

    ///Change state back to div with text
    function textAreaBlur(item, isContent) {

        let identity = $(item).attr('ident');
        if (isContent) {
            identity = 'comment' + identity;
        } else {
            identity = 'content' + identity;
        }

        if (identity == commentMouseDownId) {
            commentMouseDownId = -1;
            return;
        }

        let parentLi = $(item)[0].parentElement.parentElement;
        let box = $('.box', parentLi);
        let comment = $('.comment', parentLi);
        let content = $('.text-area', parentLi);
        $(box).show();
        $(content).hide();
        $(comment).hide();
        let text = $(content).val();
        $('p', box)[0].innerHTML = text;

        let currentContentVal = content.val();
        let currentCommentVal = comment.val();

        //console.log(currentContentVal);
        //console.log(currentCommentVal);
        //console.log(initialContent);
        //console.log(initialComment);

        if (currentContentVal != initialContent || currentCommentVal != initialComment) {
            let changed = $('.change-input', parentLi);
            console.log(changed);
            $(changed).attr('value', 'true');
        }

        initialContent = null;
        initialComment = null; 
    };

    let commentMouseDownId = -1;

    function textAreaMouseDown(item, isContent) {
        let identity = $(item).attr('ident');
        if (isContent) {
            commentMouseDownId = 'content' + identity;
        } else {
            commentMouseDownId = 'comment' + identity;
        }
    }

    function dw_getScrollOffsets() {
        var doc = document, w = window;
        var x, y, docEl;

        if (typeof w.pageYOffset === 'number') {
            x = w.pageXOffset;
            y = w.pageYOffset;
        } else {
            docEl = (doc.compatMode && doc.compatMode === 'CSS1Compat') ?
                doc.documentElement : doc.body;
            x = docEl.scrollLeft;
            y = docEl.scrollTop;
        }
        return { x: x, y: y };
    }

    let rightClickedElement = {};

    ///binding right click to existing boxes
    $('.box').bind("contextmenu", function (e) {

        if (alternatingRightClick == 0) {

            e.preventDefault();
            $('#cntnr').css("left", e.pageX);
            $('#cntnr').css("top", e.pageY - dw_getScrollOffsets().y);
            $('#cntnr').fadeIn(200, startFocusOut());
            rightClickedElement = this.parentElement;
            alternatingRightClick = 1;
        } else {
            alternatingRightClick = 0;
            $("#cntnr").hide();
        }
    });
    function startFocusOut() {
        $(document).on("click", function () {
            $("#cntnr").hide();
        });
    }
    //this is the after dropDown click events
    $("#items > li").click(function () {

        switch ($(this).text()) {
            case 'Delete':
                deleteItem(rightClickedElement);
                break;
            case 'MoveTo':
                createRelocationPrompt();
                break;
        }
    });

    ///Creates the window to select the new location of an element
    function createRelocationPrompt() {
        let relocationDiv = $('<div id="relocation-selection-div"></div>');
        $(relocationDiv).addClass('list-item-reloaction-div');
        let categories = $('#todo-list-categories').attr('value');
        let categorieNames = categories.split(';');

        let activeTabName = getActiveTabName();

        categorieNames.forEach(function (name) {

            if (name != activeTabName) {
                let buttonDiv = $('<div></div>');
                let button = $('<button val="' + name + '" type="button">' + name + '</button>');
                $(button).addClass("btn");
                $(button).addClass("btn-default");
                $(button).addClass("btn-block");

                $(buttonDiv).append(button);
                $(relocationDiv).append(buttonDiv);

                $(button).click(function () {
                    moveListItemTo(relocationDiv, name, activeTabName)
                });
            }
        });

        let cancelButtonDiv = $('<div></div>');
        let cancelButton = $('<button val="Cancel" type="button">Cancel</button>');
        $(cancelButton).addClass("btn");
        $(cancelButton).addClass("btn-default");
        $(cancelButton).addClass("btn-block");
        $(cancelButtonDiv).append(cancelButton);
        $(relocationDiv).append(cancelButtonDiv);

        $(cancelButton).click(function () {
            $(relocationDiv).hide();
        });

        $(document.body).append(relocationDiv);
    }

    function moveListItemTo(relocationDiv, newLocation, oldTabName) {
        let bodyUl = $('#' + newLocation + ' ul');

        $(bodyUl).append(rightClickedElement);
        ///changing the status so the transfer is registered by the ASP
        $('.status-input', rightClickedElement).attr('value', newLocation);
        $(relocationDiv).hide();
        tabCounts[oldTabName] -= 1;
        tabCounts[newLocation] += 1;
    }

    $('#btn-test').click(function () {
        //console.log("test stuff here");
    });

    ///Fixing and adding functuonality to pre-existing items
    $('.box').click(boxClick);
    $('.box').each(function () {
        sizeBox($(this));
    });

    $('.comment').each(function (ind, element) {
        $(element).mousedown(function () { textAreaMouseDown($(element), false) });
        $(element).blur(function () { textAreaBlur($(element), false) });
    });
    $('.text-area').each(function (ind, element) {
        $(element).mousedown(function () { textAreaMouseDown($(element), true) });
        $(element).blur(function () { textAreaBlur($(element), true) });
    });
}

function sizeBox(box) {
    let scale = $('#for-scale');
    let width = $(scale).width();
    let height = $(scale).height();
    box.width(width);
    box.height(height);
    $(box).css("padding", $(scale).css("padding"));
    $(box).css("margin", $(scale).css("margin"));
}

function getActiveTabName() {
    let result = 'No active nav-link-tabs found!';
    let tabs = $('a.nav-link-tab');
    $(tabs).each(function (ind, tab) {
        let hasClass = $(tab).hasClass('active');
        if (hasClass) {
            result = $(tab).text();
        }
    });
    return result;
}

$('.box').mousedown(function () {
    liBeingDragged = $(this.parentElement);
});

let dragInProgress = false;
let liBeingDragged = null;

$(".sortableList").sortable({
    ///Updating the ordering
    update: function (event, ui) {
        let parentUl = $(ui.item)[0].parentElement;
        var lis = $('li', parentUl);

        let counter = 0;

        lis.each(function (ind, li) {
            let pText = $('p', li).text();
            let order = $('.order-input', li);
            $(order).attr('value', counter);
            counter++;
        });
    },

    start: function (event, ui) {
        dragInProgress = true;
    },

    helper: function (event, ui) {
        let toReturn = $(ui).clone().css("pointer-events", "none").appendTo("body").show();
        let div = $(".todo-item", toReturn);
        $(div).css({ 'opacity': 0.5 });
        return toReturn;
    },

    stop: function (event, ui) {
        if (shouldRelocate) {
            dragListItemTo(newTab, oldTab, ui.item);
            shouldRelocate = false;
        }
    },
});

$(".sortableListTabs").sortable({
    update: function (event, ui) {
        let parentUl = $('#tab-ul');
        let allAs = $('a', parentUl);
        let names = [];
        $(allAs).each(function (index, item) {
            names.push(item.innerHTML);
        });
        let formattedNames = names.join(';');
        $('#todo-list-categories').attr('value', formattedNames);
    }
});

let shouldRelocate = false;
let oldTab = null;
let newTab = null;

$(document).mouseup(function () {
    if (dragInProgress == true) {
        dragInProgress = false;
        let newLocation = tabLiMouseIsOver
        if (newLocation == null) return;
        oldTab = getActiveTabName();
        newTab = newLocation;
        shouldRelocate = true;
    }
});

function dragListItemTo(newLocation, oldLocation, element) {

    let query = '#' + newLocation + ' ul';
    let bodyUl = $(query);

    $(bodyUl).append(element);
    ///changing the status so the transfer is registered by the ASP
    $('.status-input', element).attr('value', newLocation);
    tabCounts[oldLocation] -= 1;
    tabCounts[newLocation] += 1;

    let changed = $('.change-input', element);
    $(changed).attr('value', 'true');
    console.log($(changed).attr('value'));
}

let tabLiMouseIsOver = null;
let backgroundColor = null;
let isBackgroundChanged = false;

$('.nav-item-tab').mouseover(function () {
    if (dragInProgress) {
        let element = $(this);
        let name = $('a', element)[0].innerHTML;
        tabLiMouseIsOver = name;
        backgroundColor = $(element).css("background-color");
        $(element).css("background-color", "blue");
        isBackgroundChanged = true;
    }
});

$('.nav-item-tab').mouseleave(function () {
    if (dragInProgress || isBackgroundChanged) {
        let element = $(this);
        tabLiMouseIsOver = null;
        $(element).css("background-color", backgroundColor);
        isBackgroundChanged = false;
    }
});


///Making boxes responsive
$(document).ready(function () {
    let forScale = $('#for-scale');
    $(window).resize(function () {
        $('.box').each(function () {
            sizeBox($(this));
        });
    });
});