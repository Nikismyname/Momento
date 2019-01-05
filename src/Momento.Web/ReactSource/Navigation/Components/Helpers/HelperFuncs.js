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

export function handeleValidationErrors(errors, _this) {
    let newERRORS = _this.state.ERRORS;

    for (let [propName, errorMesages] of Object.entries(errors)) {

        let capitalPropName = propName.toUpperCase();
        let existingError = newERRORS.filter(x => x.fieldName == capitalPropName);
        if (existingError.length > 1) {
            alert("Duplicated Error Names, FIX");
        }
        if (existingError.length == 1) {
            existingError = existingError[0];
            console.log();
            for (var i = 0; i < errorMesages.length; i++) {
                existingError.errorMessages.push(errorMesages[i]);
            }
        } else /*no Existing Error*/ {
            newERRORS.push({
                fieldName: capitalPropName,
                errorMessages: errorMesages,
            });
        }
    }

    _this.setState({ ERRORS: newERRORS });
}

export function clientSideValidation(errorMessage, fieldName, _this) {

    fieldName = fieldName.toUpperCase();

    let newErrorState = _this.state.ERRORS; 

    let existingErrors = newErrorState.filter(x => x.fieldName == fieldName);

    if (existingErrors.length > 1) { alert("Duplicated field names - FIX"); return; }

    if (existingErrors.length == 1) {
        console.log("EXISTING Field Name");
        let existingError = existingErrors[0];
        if (existingError.errorMessages.includes(errorMessage)) {
            _this.setState({ ERRORS: newErrorState });
            return;
        } else {
            existingError.errorMessages.push(errorMessage);
            _this.setState({ ERRORS: newErrorState });
            return;
        }
    } else /*No existing Errors*/ {
        newErrorState.push({
            fieldName: fieldName,
            errorMessages: [errorMessage],
        });
        _this.setState({ ERRORS: newErrorState });
    }
}