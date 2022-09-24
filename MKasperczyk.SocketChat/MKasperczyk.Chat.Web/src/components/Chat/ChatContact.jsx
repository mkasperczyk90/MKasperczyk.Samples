import React from 'react'
import "./ChatContact.css"
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome'

export default function ChatContact({contacts, user, changeChat}) {
    const [selectedUser, setSelectedUser] = React.useState(undefined)

    const changeCurrentChat = (index, contact) => {
        setSelectedUser(index)
        changeChat(contact)
    }

    const contactsElement = contacts.map((contact) => {
        return (                
            <li key={contact.id} className="clearfix chat-contact--element" onClick={() => changeCurrentChat(contact.id, contact)}>
                <img className="chat-contact--image" src="https://bootdey.com/img/Content/avatar/avatar1.png" alt="avatar" />
                <div className="chat-contact--about">
                    <div className="chat-contact--name">{contact.userName}</div>
                    <div className="chat-contact--status online"> 
                        <FontAwesomeIcon icon="fa-solid fa-circle"/> left 7 mins ago 
                    </div>                                            
                </div>
            </li>
        )
    })

    return (
        <div id="plist" className="chat-contact">
            <div className="input-group">
                <div className="input-group-prepend">
                    <span className="chat-contact--icon input-group-text">
                        <FontAwesomeIcon icon="fa-solid fa-search" size="lg"/>
                    </span>
                </div>
                <input type="text" className="chat-contact--input form-control" placeholder="Search..." />
            </div>
            <ul className="chat-contact--list list-unstyled mt-2">
                {contactsElement}
            </ul>
        </div>
    )
}