import React, {ReactElement, Ref} from "react";
import "./Chat.scss";
import {Form} from "react-bootstrap";
import {AltV} from "../../services/alt.service";

class Chat extends React.Component {
    state = {
        isInputActive: false,
        messages: [] as ReactElement[],
        inputValue: "",
        inputHistory: [] as string[],
        inputReference: null as HTMLInputElement | null
    }

    componentDidMount() {
        AltV.emit("chat:loaded");

        AltV.on("chat:open", () => {
            this.setState({isInputActive: true, inputValue: ""}, () => {
                this.state.inputReference?.focus();
            });
        });
        AltV.on("chat:close", () => {
            this.setState({isInputActive: false});
        });
        AltV.on("chat:addString", (message: string) => {
            this.state.messages.push(<p>{message}</p>);
            this.setState({messages: this.state.messages},
                () => this.scrollToBottom());
        });
        AltV.on("chat:addMessage", (name: string, message: string) => {
            this.state.messages.push(<p><span className="messageName">{name}: </span><span className="messageContent">{message}</span></p>);
            this.setState({messages: this.state.messages},
                () => this.scrollToBottom());
        });
        AltV.on("chat:addHtml", (message: string) => {
            this.state.messages.push(<p dangerouslySetInnerHTML={{__html: message}}></p>);
            this.setState({messages: this.state.messages},
                () => this.scrollToBottom());
        });

        // @todo:
        // - Add Key Down and Up Listener
        // - Add Scroll Listener (Picture Up and Down)
    }

    private scrollToBottom() {
        // @todo: check that scroll was on bottom before and scrolling is not disabled
        // @todo: maybe only allow scrolling disabled if input is active
        const element = document.querySelector(".msglist");
        if (element) {
            element.scrollTo({
                left: 0,
                top: element!.scrollHeight,
                behavior: "smooth"
            })
        }

        this.checkOverflow();
    }

    private checkOverflow() {
        const element = document.querySelector(".msglist");
        if (element) {
            if (element.scrollHeight > element.clientHeight) {
                element.classList.add("overflow");
            }
        }
        return false;
    }

    private sendMessage(e: React.FormEvent<HTMLFormElement>) {
        e.preventDefault();
        AltV.emit("chat:message", this.state.inputValue);
        this.state.inputHistory.push(this.state.inputValue);
        this.setState({isInputActive: false, inputValue: "", inputHistory: this.state.inputHistory});
    }

    render() {
        return <div className="ChatComponent">
            <div className="msglist ps-2">
                <div className="messages">
                    {this.state.messages}
                </div>
            </div>
            <div className="msginput" hidden={!this.state.isInputActive}>
                <Form onSubmit={(event) => this.sendMessage(event)}>
                    <Form.Control
                        type="text"
                        autoFocus={true}
                        ref={(input: HTMLInputElement | null) => { this.state.inputReference = input; }}
                        tabIndex={1}
                        value={this.state.inputValue}
                        onChange={(event) => {
                            this.setState({inputValue: event.target.value})
                        }}
                    />
                </Form>
            </div>
        </div>;
    }
}

export default Chat;