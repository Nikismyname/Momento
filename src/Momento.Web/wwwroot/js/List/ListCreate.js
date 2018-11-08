function noteCreate(listItemCount)
{
    $(function () {
        let nextId = listItemCount;

        $('#newItem').click(function ()
        {
            let textArea = $('<textarea rows="2" cols="50" name="ListItems[' + nextId + '].Content"></textarea>');

            $('#listItems').append(textArea);

            nextId++;
        });
    });
}