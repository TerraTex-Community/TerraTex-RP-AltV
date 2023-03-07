import "./PasswordForgotten.scss"
import React, {useEffect} from "react";
import {Alert, Button, FloatingLabel, Form} from "react-bootstrap";
import {useNavigate} from "react-router";
import {AltV} from "../../services/alt.service";

function PasswordForgotten() {
    const [error, setError] = React.useState(null as null| string);
    const [password, setPassword] = React.useState("");
    const [passwordWdh, setPasswordWdh] = React.useState("");
    const [confirmCode, setConfirmCode] = React.useState("");
    const [email, setEmail] = React.useState("");
    const [isLocked, setIsLocked] = React.useState(false);
    const [codeWasSent, setCodeWasSent] = React.useState(0);
    const [timeLeft, setTimeLeft] = React.useState(0);
    const navigate = useNavigate();

    if (email === "") {
        AltV.on("login:email", (email: string) => setEmail(email));
        AltV.emit("login:getMail");

        AltV.on("login:passwordForgottenResult", (result: boolean) => {
            if (result) {
                navigate("/login", {state: {passwordChanged: true}});
            } else {
                setError("Der Bestätigungscode ist nicht gültig.");
            }
        });
    }

    function onSubmit(e: React.FormEvent<HTMLFormElement>) {
        e.preventDefault();
        e.stopPropagation();

        if (password !== passwordWdh) {
            setError("Die Passwörter stimmen nicht überein.");
            return;
        }

        if (password.length < 8 || !password.match(/[A-Z]/) || !password.match(/[a-z]/) || !password.match(/[0-9]/)) {
            setError("Das Passwort muss mindestens 8 Zeichen lang sein und mindestens einen Groß- und einen Kleinbuchstaben sowie eine Zahl enthalten.");
            return;
        }

        AltV.emit("login:changePassword", password, confirmCode);
    }

    useEffect(() => {
        const interval = setInterval(() => {
                if (isLocked && codeWasSent !== 0) {
                    const timeLeftCalc = Math.ceil((10000 - (new Date().getTime() - codeWasSent))/1000);
                    setTimeLeft(timeLeftCalc);

                    if (timeLeftCalc === 0) {
                        setIsLocked(false);
                        setCodeWasSent(0);
                    }
                }
            }, 1000);
        return () => clearInterval(interval);
    });

    function requestConfirmCode() {
        setCodeWasSent(new Date().getTime());
        setIsLocked(true);
        setTimeLeft(10);
        
        AltV.emit("login:sendConfirmCode");
    }

    return (
        <div className="PasswordForgottenForm p-3">
            <Form onSubmit={e => onSubmit(e)}>
                <h3>Du hast dein Passwort vergessen?</h3>
                <Alert variant="danger" hidden={!error}>
                    {error}
                </Alert>
                <FloatingLabel className="mb-3" controlId="floatingPassword" label="Neues Passwort">
                    <Form.Control
                        type="password"
                        placeholder="Neues Passwort"
                        required
                        onChange={(e) => setPassword(e.target.value)}
                    />
                </FloatingLabel>
                <FloatingLabel className="mb-3" controlId="floatingPasswordWdh" label="Neues Passwort Wiederholung">
                    <Form.Control
                        type="password"
                        placeholder="Neues Passwort Wiederholung"
                        required
                        onChange={(e) => setPasswordWdh(e.target.value)}
                    />
                </FloatingLabel>

                <Alert variant="info">
                    Um dein Passwort ohne Login ändern zu können, wird ein Bestätigungscode an deine E-Mailadresse {email} versendet.
                    Das kann ein paar Minuten dauern. Bitte prüfe auch deinen Spam Ordner.

                    <div className="mt-2">
                        <Button className="me-2"
                                variant="primary"
                                type="button"
                                disabled={isLocked}
                                onClick={() => requestConfirmCode()}>
                            {isLocked ? "Nächster Code kann erst in " + (timeLeft) + "s gesendet werden" : "Bestätigungscode senden"}
                        </Button>
                    </div>
                </Alert>


                <FloatingLabel className="mb-3" controlId="floatingConfirmCode" label="Bestätigungscode">
                    <Form.Control
                        type="text"
                        placeholder="Bestätigungscode"
                        required
                        onChange={(e) => setConfirmCode(e.target.value)}
                    />
                </FloatingLabel>

                <Button className="me-2" variant="danger" type="button" onClick={() => navigate("/Login")}>Abbrechen</Button>
                <Button variant="success" type="submit">
                    Passwort ändern
                </Button>

            </Form>
        </div>
    );
}
export default PasswordForgotten