import { NavLink } from "react-router-dom";

export function linkSSRSafe(path, name, onClickFunc = null, classNames = "") {
    if (typeof window !== "undefined")/*Client*/ {
        if (onClickFunc != null) {
            return <NavLink className={classNames} onClick={onClickFunc} to={path}>{name}</NavLink>;
        } else {
            return <NavLink className={classNames} to={path}>{name}</NavLink>;
        }
    } else {
        return <a className={classNames} href={path}>{name}</a>
    }
}

export function extractVideoToken(url) {
    if (url == null || url == undefined || url.length == 0) {
        return "";
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
            return "";
        }
        videoToken = match[1];
        return videoToken;
    }
}

export function clone(obj) {
    var copy;

    // Handle the 3 simple types, and null or undefined
    if (null == obj || "object" != typeof obj) return obj;

    // Handle Date
    if (obj instanceof Date) {
        copy = new Date();
        copy.setTime(obj.getTime());
        return copy;
    }

    // Handle Array
    if (obj instanceof Array) {
        copy = [];
        for (var i = 0, len = obj.length; i < len; i++) {
            copy[i] = clone(obj[i]);
        }
        return copy;
    }

    // Handle Object
    if (obj instanceof Object) {
        copy = {};
        for (var attr in obj) {
            if (obj.hasOwnProperty(attr)) copy[attr] = clone(obj[attr]);
        }
        return copy;
    }

    throw new Error("Unable to copy obj! Its type isn't supported.");
}

export function cnvRGBToStr(color, def = "black") {
    if (typeof color.rgb === "undefined") {
        return def;
    }

    let rgb = color.rgb;
    let colorResult = `rgba(${rgb.r}, ${rgb.g}, ${rgb.b}, ${rgb.a})`

    return colorResult;
}

export function createBorder(color, thickness) {
    if (typeof color === "undefined" || color == null || color.length == 0) {
        color = "black";
    }
    if (typeof thickness === "undefined" || thickness == null || thickness.length == 0) {
        thickness = 1;
    }
    return `${thickness}px solid ${color}`;
}