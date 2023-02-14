import React, {useState} from "react";
import "./Register.scss";
import logo from "../assets/logo.png";
import {Button, Col, Form, Row} from "react-bootstrap";
import {AltV} from "../services/alt.service";

function Register() {
    const [nickname, setNickname] = useState("placeholder");
    const [password, setPassword] = useState("");
    const [passwordRepeat, setPasswordRepeat] = useState("");
    const [email, setEmail] = useState("");
    const [forename, setForename] = useState("");
    const [lastname, setLastname] = useState("");
    const [birthday, setBirthday] = useState("");
    const [errors, setErrors] = useState({password: false, passwordRepeat: false});
    const [gender, setGender] = useState("");

    if (nickname === "placeholder") {
        AltV.on("register:nickname", (nickname: string) => setNickname(nickname));
        AltV.emit("register:ready");
    }

    function onSubmit(e: React.FormEvent<HTMLFormElement>) {
        e.preventDefault();
        e.stopPropagation();

        const errors = {
            password: false,
            passwordRepeat: false
        }

        if (password.length < 8 || !password.match(/[A-Z]/) || !password.match(/[a-z]/) || !password.match(/[0-9]/)) {
            errors.password = true;
        }

        if (password !== passwordRepeat) {
            errors.passwordRepeat = true;
        }

        if (errors.password || errors.passwordRepeat) {
            setErrors(errors);
            return;
        }

        AltV.emit("register:submit", JSON.stringify({
            Password: password,
            Email: email,
            Forename: forename,
            Lastname: lastname,
            Birthday: birthday,
            Gender: gender
        }));
    }

    return (
        <div className="RegisterForm p-3">
            <Form onSubmit={e => onSubmit(e)}>
                <div>
                    <h3>Willkommen auf dem TerraTex Reallife Roleplay Server</h3>
                    <div className="d-flex align-items-center">
                        <img src={logo} className="logo" alt=""/>
                        <span className="text-justify">Willkommen auf dem
                        Server. Unter deinem Pseudonym <b>"{nickname}"</b> wurde noch
                        kein Account gefunden. Mit diesem Formular kannst du ein neuen Account und einen neuen
                        Character erstellen. Da wir im wesentlichen ein RPG Server sind, solltest du dir einen Character
                        ausdenken, den es im realen wirklich geben könnte und mit
                        dem du dich indentifizieren kannst. Im Abschnitt "Character Daten" werden auch nur Daten zum
                        Character abgefragt. Diese müssen nicht mit deinen realen Daten übereinstimmen.
                        </span>
                    </div>
                </div>
                <hr/>
                <div className="mt-3">
                    <h5>Account Daten</h5>
                    <Form.Group className="mb-4" controlId="email">
                        <Form.Label>
                            <span className="fw-bold">Email</span>
                        </Form.Label>
                            <Form.Control
                                type="email"
                                placeholder="E-Mail"
                                required
                                onChange={(e) => setEmail(e.target.value)}
                            />
                            <Form.Text className="text-muted">
                                <span className="text-justify">
                                    Bitte gebe hier eine real existierende E-Mail Adresse ein. Unter dieser solltest du auch in
                                    Zukunft erreichbar sein. Sie wird zur Identifikation
                                    bei moderativen Hilfen benötigt und du wirst sie ggf. auch in Zukunft benötigen, solltest du
                                    beispielsweise mal dein Passwort vergessen. Wir werden die E-Mail
                                    nicht für andere Zwecke (wie Werbung) nutzen. Sie wird nur zu deinen Gunsten gespeichert.
                                </span>
                            </Form.Text>
                    </Form.Group>

                    <Form.Group className="mb-2" controlId="password">
                        <Form.Label>
                            <span className="fw-bold">Passwort</span>
                        </Form.Label>
                        <Form.Control
                            type="password"
                            placeholder="Passwort"
                            required
                            isInvalid={errors.password}
                            onChange={(e) => setPassword(e.target.value)}
                        />
                        <Form.Control.Feedback type="invalid">
                            Passwort muss mindestens 8 Zeichen lang sein.
                            Es muss mindestens eine Zahl, ein Großbuchstabe und ein Kleinbuchstabe beinhalten.
                        </Form.Control.Feedback>
                    </Form.Group>

                    <Form.Group className="mb-4" controlId="passwordWdh">
                        <Form.Label>
                            <span className="fw-bold">Passwort wiederholen</span>
                        </Form.Label>
                        <Form.Control
                            type="password"
                            placeholder="Passwort wiederholen"
                            required
                            isInvalid={errors.passwordRepeat}
                            onChange={(e) => setPasswordRepeat(e.target.value)}
                        />
                        <Form.Control.Feedback type="invalid">
                            Passwort stimmt nicht überein.
                        </Form.Control.Feedback>
                    </Form.Group>
                </div>

                <hr/>
                <div className="mt-3">
                    <h5>(Roleplay-) Character Daten</h5>

                    <Form.Group className="mb-4" controlId="forename">
                        <Form.Label>
                            <span className="fw-bold">Vorname</span>
                        </Form.Label>
                        <Form.Control
                            type="text"
                            placeholder="Vorname"
                            required
                            onChange={(e) => setForename(e.target.value)}
                        />
                    </Form.Group>

                    <Form.Group className="mb-4" controlId="lastname">
                        <Form.Label>
                            <span className="fw-bold">Nachname</span>
                        </Form.Label>
                        <Form.Control
                            type="text"
                            placeholder="Nachname"
                            required
                            onChange={(e) => setLastname(e.target.value)}
                        />
                    </Form.Group>

                    <Form.Group className="mb-4" controlId="birthday">
                        <Form.Label>
                            <span className="fw-bold">Geburtstag</span>
                        </Form.Label>
                        <Form.Control
                            type="date"
                            required
                            onChange={(e) => setBirthday(e.target.value)}
                        />
                    </Form.Group>

                    <Form.Group className="mb-4" controlId="gender" aria-required>
                        <Form.Label>
                            <span className="fw-bold">Geschlecht</span>
                        </Form.Label>
                        <br/>
                        <Form.Check
                            inline
                            label="männlich"
                            name="gender"
                            type="radio"
                            id="male"
                            onChange={(e) => setGender("male")}
                            required
                        />
                        <Form.Check
                            inline
                            label="weiblich"
                            name="gender"
                            type="radio"
                            id="female"
                            onChange={(e) => setGender("female")}
                            required
                        />
                        <Form.Check
                            inline
                            label="diverse"
                            type="radio"
                            name="gender"
                            id="diverse"
                            onChange={(e) => setGender("diverse")}
                            required
                        />
                        <br/>
                        <Form.Text className="text-muted">
                            <span className="text-justify">
                                Das Geschlecht deines Characters wird in deinen virtuellen Ausweis stehen. Außerdem kann es die Auswahl von
                                Kauf- / Auswahlmöglichkeiten beeinflussen. (z.B. Kleidung oder Skin)
                            </span>
                        </Form.Text>
                    </Form.Group>

                    <Button variant="success" type="submit">Registrieren</Button>
                </div>

            </Form>
        </div>
    )
}

export default Register