import { NavLink } from "react-router-dom";

export function linkSSRSafe(path, name, onClickFunc) {
    if (typeof window !== "undefined" && typeof onClickFunc !== "undefined")/*Client*/ {
        if (onClickFunc != null) {
            return <NavLink onClick={onClickFunc} to={path}>{name}</NavLink>;
        } else {
            return <NavLink to={path}>{name}</NavLink>;
        }
    } else {
        return <a href={path}>{name}</a>
    }
}