import ReactLoading from 'react-loading';

let LoadSvg = (props) => {
    return (
        <div style={{ display: "block", margin: "auto", width: "100px", height: "100px" }} className="h4 items-center justify-center flex flex-column flex-wrap">
            <ReactLoading className="m-0 p-0 b-0 text-center" type="bars" color="#fff" height={"100px"} width={'100px'} />
        </div>)
}

export default LoadSvg;