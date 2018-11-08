let tag = document.createElement('script');
tag.src = "https://www.youtube.com/iframe_api";
let firstScriptTag = document.getElementsByTagName('script')[0];
firstScriptTag.parentNode.insertBefore(tag, firstScriptTag);

var player;

window.onYouTubeIframeAPIReady = function () {

    let width = $('#iframeDiv').width();
    let hight = Math.round(9 * width / 16);

    $('#iframeDiv').height(hight);

    player = new YT.Player('video', {
        width: width,
        height: hight,
    });
}

$('body').on('click', '#btnPlay', function () {
    let url = $('#txtUrl').val();
    let videoToken = extractVideoToken(url);
    if(videoToken != null){
        player.loadVideoById(videoToken);
    }
});