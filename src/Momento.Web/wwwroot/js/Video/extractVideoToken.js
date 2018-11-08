function extractVideoToken(url) {
    if (url == null || url == undefined || url.length == 0) {
        return null;
    }

    let standartUrl = true;
    let videoToken = url.split('v=')[1];
    if (videoToken == undefined) {
        standartUrl = false;
    }
    if (standartUrl == true) {
        videoToken = videoToken.split('&')[0];
        return videoToken;
    } else {
        let regex = /\.be\/(.+?)(?:$|\?)/;
        var match = regex.exec(url);
        if (match == false) {
            return null;
        }
        videoToken = match[1];
        return videoToken;
    }
}