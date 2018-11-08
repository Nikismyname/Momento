$(window).resize(function () {
    let width = $('#iframeDiv').width();
    let hight = Math.round(9 * width / 16);

    $('#iframeDiv').height(hight);

    player.width = width;
    player.hight = hight;
});