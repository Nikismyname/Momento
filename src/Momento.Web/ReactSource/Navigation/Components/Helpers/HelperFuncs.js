import { NavLink } from "react-router-dom";

export function linkSSRSafe(path, name, onClickFunc = null, classNames = "") {
    if (typeof window !== "undefined" && typeof onClickFunc !== "undefined")/*Client*/ {
        if (onClickFunc != null) {
            return <NavLink className={classNames} onClick={onClickFunc} to={path}>{name}</NavLink>;
        } else {
            return <NavLink className={classNames} to={path}>{name}</NavLink>;
        }
    } else {
        return <a className={classNames} href={path}>{name}</a>
    }
}