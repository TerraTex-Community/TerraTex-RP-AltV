import React from "react";
import "./LoadingScreen.scss";
import backj_screen from "../../assets/backj_screen.jpg";

function LoadingScreen() {  

    return (
        <div className="container-fluid" style={{padding:0, width: "100%", height: "100%", background: `no-repeat 0 0 url(${backj_screen})`, backgroundSize: "100% 100%"}}>
            {/*<img style={{width: "100%", height: "100%"}} src={backj_screen} />*/}
            <div style={{position: "absolute", top: "10%", left: "10%", fontSize: "5em", textShadow: "3px 3px 11px black"}}>
                Bitte Logge dich via Discord ein.
                
            </div>
        </div>
    )
}

export default LoadingScreen