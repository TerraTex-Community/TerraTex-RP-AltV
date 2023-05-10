import React, {ReactElement, Ref} from "react";
import "./Chat.scss";
import {Form} from "react-bootstrap";
import {AltV} from "../../services/alt.service";
import { animateScroll as scroll, scroller } from 'react-scroll';

class Chat extends React.Component {
    state = {
        isInputActive: false,
        messages: [] as ReactElement[],
        inputValue: "",
        inputHistoryId: 0,
        inputHistoryLastNotSendInputMessage: "",
        inputHistory: [] as string[],
        inputReference: null as HTMLInputElement | null,
        isScrollingEnabled: true,
        currentScrollPosition: 0
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

        window.addEventListener("scroll", this.checkOverflow);
        window.addEventListener("scrollend", this.checkOverflow);
    }

    private onKeyDown(event: React.KeyboardEvent<HTMLInputElement | HTMLTextAreaElement>) {

        switch(event.key) {
            case "Tab":
                event.preventDefault();
                event.stopPropagation();
                break;
            case "ArrowUp":
                if (this.state.inputHistoryId === 0) {
                    this.setState( {inputHistoryLastNotSendInputMessage: this.state.inputValue});
                }
                this.state.inputHistoryId++;
                if (this.state.inputHistoryId > this.state.inputHistory.length) {
                    this.state.inputHistoryId = this.state.inputHistory.length;
                }
                if (this.state.inputHistoryId !== 0) {
                    this.setState({
                        inputHistoryId: this.state.inputHistoryId,
                        inputValue: this.state.inputHistory[this.state.inputHistory.length - this.state.inputHistoryId]
                    });
                }
                break;
            case "ArrowDown":
                if (event.ctrlKey) {
                    if (this.state.inputHistoryId !== 0) {
                        this.setState({
                            inputValue: this.state.inputHistoryLastNotSendInputMessage,
                            inputHistoryId: 0
                        });
                    }
                } else {
                    if (this.state.inputHistoryId !== 0) {
                        this.state.inputHistoryId--;

                        if (this.state.inputHistoryId > 0) {
                            this.setState({
                                inputHistoryId: this.state.inputHistoryId,
                                inputValue: this.state.inputHistory[this.state.inputHistory.length - this.state.inputHistoryId]
                            });
                        } else {
                            this.setState({
                                inputValue: this.state.inputHistoryLastNotSendInputMessage,
                                inputHistoryId: 0
                            });
                        }
                    }
                }
                break;
            case "PageUp":
                if (this.state.isScrollingEnabled) {
                    const element = document.querySelector(".msglist");
                    this.setState({
                        isScrollingEnabled: false,
                        // currentScrollPosition: element!.scrollHeight
                    }, this.scrollUp.bind(this))
                } else {
                    this.scrollUp();
                }
                break;
            case "PageDown":
                this.scrollDown();
                break;
            case "End":
                this.setState({
                    isScrollingEnabled: true
                }, this.scrollToBottom.bind(this));
                break;
        }
    }

    scrollUp() {
        const element = document.querySelector(".msglist");

        if (element) {
            element.scrollTo({
                left: 0,
                // top: element.scrollTop - 10,
                top: element.scrollTop - 100,
                behavior: "smooth"
            });
            setTimeout(this.checkOverflow.bind(this), 100);
        }
    }

    scrollDown() {
        const element = document.querySelector(".msglist");

        if (element) {
            element.scrollTo({
                left: 0,
                top: element.scrollTop + 100,
                behavior: "smooth"
            });
            setTimeout(this.checkOverflow.bind(this), 100);
        }
    }

    private scrollToBottom() {
        if (this.state.isScrollingEnabled) {
            const element = document.querySelector(".msglist");
            if (element) {
                element.scrollTo({
                    left: 0,
                    top: element!.scrollHeight,
                    behavior: "smooth"
                })
            }

            setTimeout(this.checkOverflow.bind(this), 100);
        }
    }

    private checkOverflow() {
        const element = document.querySelector(".msglist");
        if (element) {
            if (element.scrollHeight > element.clientHeight) {
                if (element.scrollTop > 10 || this.state.isScrollingEnabled) {
                    element.classList.add("overflow-top");
                } else {
                    element.classList.remove("overflow-top");
                }

                const checkPos = element.clientHeight + element.scrollTop + 50;
                if (checkPos < element.scrollHeight && !this.state.isScrollingEnabled) {
                    element.classList.add("overflow-bottom");
                } else {
                    this.setState({isScrollingEnabled : true});
                    element.classList.remove("overflow-bottom");
                }
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
            <div className="msglist ps-2" id="MessageList">
                <div className="messages">
                    {this.state.messages}
                </div>
            </div>
            <div className="msginput" hidden={!this.state.isInputActive}>
                <Form onSubmit={(event) => this.sendMessage(event)}>
                    <Form.Control
                        type="text"
                        onKeyDown={event => this.onKeyDown(event)}
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