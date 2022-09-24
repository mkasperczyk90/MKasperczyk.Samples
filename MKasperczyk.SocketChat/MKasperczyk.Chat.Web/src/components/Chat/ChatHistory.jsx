import React, { useEffect, useState } from "react"
import "./ChatHistory.css"
import axios from "axios"
import { formatDistanceToNow } from "date-fns";

export default function ChatHistory({messages, currentChat, user}) {
    
    const historyElement = messages.map((message) => {
        return (                
            <li className="history--element clearfix" key={message.messageId}>
                <div className={message.type === "recieved" ? "history--element--details text-right" : "history--element--details" } >
                    <span className="history--element--time">{formatDistanceToNow(Date.parse(message.sendAt), "MMMM do, yyyy H:mma")}</span>
                </div>
                <div className={message.type === "recieved" ? "history--element--message other-message float-right" : "history--element--message my-message" } >{message.message}</div>
            </li>
        )
    })
    
    return(
         <div className="history">
            <ul className="m-b-0">
                 <li className="clearfix">
                    <div className="history--element--details text-right">
                        <span className="history--element--time">10:10 AM, Today</span>
                        <img src="https://bootdey.com/img/Content/avatar/avatar7.png" alt="avatar" />
                    </div>
                    <div className="history--element--message other-message float-right"> Hi Aiden, how are you? How is the project coming along? </div>
                </li>
                {/*<li className="clearfix">
                    <div className="message-data">
                        <span className="message-data-time">10:12 AM, Today</span>
                    </div>
                    <div className="message my-message">Are we meeting today?</div>                                    
                </li>                               
                <li className="clearfix">
                    <div className="message-data">
                        <span className="message-data-time">10:15 AM, Today</span>
                    </div>
                    <div className="message my-message">Project has been already finished and I have results to show you.</div>
                </li> */}
                {messages.length === 0 && <div></div>}
                {messages.length > 0 && historyElement}
            </ul>
        </div>
    )
}