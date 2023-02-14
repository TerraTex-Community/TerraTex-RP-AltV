import React, {useState} from "react";
import "./Login.scss";
import logo from "../assets/logo.png";
import {Alert, Button, FloatingLabel, Form} from "react-bootstrap";
import {AltV} from "../services/alt.service";

function Login() {
    const [isDevServer, setIsDevServer] = useState(false);
    const [nickname, setNickname] = useState("placeholder");
    const [password, setPassword] = useState("");
    const [error, setError] = useState(false);

    if (nickname === "placeholder") {
        AltV.on("login:nickname", (nickname: string) => setNickname(nickname));
        AltV.emit("login:ready");
    }

    AltV.on("login:devServer", () => setIsDevServer(true));
    AltV.on("login:error", () => setError(true));

    function onSubmit(e: React.FormEvent<HTMLFormElement>) {
        e.preventDefault();
        e.stopPropagation();

        AltV.emit("login:submit", password);
    }

    return (
        <div className="LoginForm p-3">
            <Form onSubmit={e => onSubmit(e)}>
                <div>
                    <Alert variant="warning">
                        Das Script ist derzeit noch in einem Early Access Alpha Status.
                        Viele Features fehlen noch oder können fehlerhaft sein. Weitere Infos dazu gibt
                        es auf unserem Discord: <a
                        href="https://discord.gg/sEUary3aKK">https://discord.gg/sEUary3aKK</a>
                    </Alert>

                    <Alert variant="warning" hidden={!isDevServer}>
                        Du befindest dich auf dem <span className="text-decoration-underline">Development-Server</span>.
                        Einige Features sind unvollständig oder können fehlerhaft sein. <span className="fw-bold">
                        Deine Daten auf DevServer werden NICHT mit dem LiveServer abgeglichen.
                        Dieser Server ist nur zum Testen gedacht.</span>
                    </Alert>

                    <h3>Willkommen auf dem TerraTex Reallife Roleplay Server</h3>
                    <div className="d-flex align-items-center mb-2">
                        <img src={logo} className="logo" alt=""/>
                        <span className="text-justify">Willkommen auf dem Server.
                        Unter deinem Pseudonym <span className="fw-bold">"{nickname}"</span> wurde
                        ein Account gefunden. Bitte Authentifiziere dich mit deinem Passwort.
                        </span>
                    </div>

                    <Alert variant="danger" hidden={!error}>
                        Dein Passwort ist falsch. Bitte versuche es erneut.
                    </Alert>

                    <FloatingLabel className="mb-3" controlId="floatingPassword" label="Password">
                        <Form.Control
                            type="password"
                            placeholder="Passwort"
                            required
                            onChange={(e) => setPassword(e.target.value)}
                        />
                    </FloatingLabel>

                    <Button className="me-2" variant="success" type="submit">Login</Button>
                    <Button variant="success" type="submit" hidden={true}>
                        Login & Aktiviere Autologin auf diesem PC
                    </Button>
                </div>

            </Form>
        </div>
    )
}

export default Login