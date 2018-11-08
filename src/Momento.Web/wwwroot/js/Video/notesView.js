$(document).ready(function () {
    var tag = document.createElement('script');
    tag.src = "https://www.youtube.com/iframe_api";
    var firstScriptTag = document.getElementsByTagName('script')[0];
    firstScriptTag.parentNode.insertBefore(tag, firstScriptTag);

    var player;

    window.onYouTubeIframeAPIReady = function () {

        let width = $('#iframeDiv').width();
        let hight = Math.round(9 * width / 16);

        $('#iframeDiv').height(hight);
        player = new YT.Player('video', {
            width: width,
            height: hight,
            playerVars: { 'autoplay': 0 },
            events: {
                'onReady': onPlayerReady,
            }
        });
    };

    function onPlayerReady() {

        $('.seek').click(function () {
            player.seekTo($(this).val())
            player.playVideo();
        });

        var url = $('#txtUrl').val();

        if (url != '') {
            let standartUrl = true;
            let url = $('#txtUrl').val();
            let videoToken = url.split('v=')[1];
            if (videoToken == undefined) {
                standartUrl = false;
            }
            if (standartUrl == true) {
                videoToken = videoToken.split('&')[0];
            } else {
                let regex = /\.be\/(.+?)(?:$|\?)/;
                var match = regex.exec(url);
                videoToken = match[1];
            }

            let startData = {};
            startData.videoId = videoToken;
            startData.startSeconds = 0;

            player.cueVideoById(startData);
            player.playVideo();
            player.pauseVideo();
        }
    }

    $('.note-view-div').click(function () {

        var selection = window.getSelection();
        if (selection.toString().length === 0) {
            if ($(this).attr('state') == '0') {
                $(this).attr('state', '1');
                $(this).css('height', 'auto');
            }
            else {
                $(this).attr('state', '0');
                $(this).css('height', '2em');
            }
        }
    });

    $('.btn-view').click(function () {
        window.open('/directory/index', 'DirIndex', 'width=200,height=400');
    });
});
