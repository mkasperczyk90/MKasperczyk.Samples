import React, { useEffect, useState } from "react"
import "./ChatMessageBox.css"
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome'
import axios from "axios"

export default function ChatMessageBox({handleSendMessage, socket}) {
    const [message, setMessage] = useState("")

    const sendMessage = (event) => {
        event.preventDefault()
        if(message.length > 0) {
            handleSendMessage(message)
            setMessage("")
        }
    }

    return (
            <div className="chat-message clearfix">
                <form>
                    <div className="input-group mb-0">
                        <span className="chat-contact--icon input-group-text" onClick={sendMessage}>
                            <FontAwesomeIcon icon="fa-solid fa-paper-plane"/>
                        </span>
                        <input  type="text" 
                                className="chat-message--input form-control" 
                                placeholder="Enter message ..." 
                                value={message} 
                                onChange={(event) => setMessage(event.target.value)} />                                
                    </div>  
                </form>  
            </div>

    )
}