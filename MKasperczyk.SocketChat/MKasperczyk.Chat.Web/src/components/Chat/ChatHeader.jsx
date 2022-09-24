import React from 'react'
import "./ChatHeader.css"

export default function ChatHeader({currentChat}) {
    return (
        <div className="chat-header clearfix">
            <div className="row">
                <div className="col-lg-6">
                    <a href="#">
                        <img src="https://bootdey.com/img/Content/avatar/avatar2.png" alt="avatar" />
                    </a>
                    <div className="chat-header--about">
                        <h6 className="m-b-0">{currentChat.userName}</h6>
                        <small>Last seen: 2 hours ago</small>
                    </div>
                </div>
            </div>
        </div>
    )
}