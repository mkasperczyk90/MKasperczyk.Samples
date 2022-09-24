import React, { useEffect, useState, useRef } from 'react'
import { useNavigate } from 'react-router-dom';
import { io } from 'socket.io-client'
import './Chat.css'
import axios from 'axios'
import { usersUrl, sendMessageUrl, messagesUrl, host } from '../../helpers/ApiRequests'
import ChatMessageBox from '../../components/Chat/ChatMessageBox'
import ChatHistory from '../../components/Chat/ChatHistory'
import ChatHeader from '../../components/Chat/ChatHeader'
import ChatContact from '../../components/Chat/ChatContact';

export default function Chat() {
    const navigate = useNavigate()
    const socket = useRef(); 
    const [contacts, setContacts] = React.useState([])
    const [user, setUser] = React.useState(undefined)
    const [currentChat, setCurrentChat] = React.useState(undefined)
    const [messages, setMessages] = useState([])
    const [arrivalMessage, setArrivalMessage] = useState(undefined)

    console.log("TEST")

    useEffect(() => {
        const getMessagesFromApi = async () => {
            const response = await axios.get(messagesUrl, {
                params: {
                    senderId: user.id,
                    receiverId: currentChat.id
                }
            })
            setMessages(response.data)
        }
        getMessagesFromApi()

    }, [currentChat]);

    useEffect(() => {
        if(user) {
            console.log("JESTEM TU" + user.id)
            socket.current = io("wss://localhost:7172/", {
                path: "/ws",
                transports: ["websocket"]
            })
            socket.current.emit("add-user", user.id)
        }
    }, [user])

    useEffect(() => {
        const setData = async () => {
            
            if(!localStorage.getItem("chat-user")) {
                navigate("/login")
            } else {
                const userData = JSON.parse(localStorage.getItem("chat-user"))
                setUser(userData)

                const data = await axios.get(`${usersUrl}/${userData.id}`);
                setContacts(data.data)
            }
        }
        
        setData()
    }, [])

    const changeChat = async (chat) => {
        setCurrentChat(chat)
    }

    const handleSendMessage = async (msg) => {
        await axios.post(sendMessageUrl, {
            Sender: user.id,
            Message: msg,
            Recipients: [currentChat.id]
        });
        socket.current.emit("send-msg", {
            from: user.id,
            to: currentChat.id,
            message: msg
        })

        const currentMessages = [...messages]
        currentMessages.push(msg)
        setMessages(prvMessages => [msg, ...prvMessages])
    }

    useEffect(() => {
        if(socket.current){
            socket.current.on("msg-receive", (msg) => {
                setArrivalMessage(msg)
            })
        }
    }, [])

    useEffect(() => {
        arrivalMessage && setMessages((prev) => [...prev, arrivalMessage])
    }, [arrivalMessage])

    return (
        <div className="card chat-app">
            <ChatContact contacts={contacts} changeChat={changeChat}/>
            {
                currentChat !== undefined ?
                <div className="chat">
                    <ChatHeader currentChat={currentChat} />
                    <ChatHistory currentChat={currentChat} 
                                user={user}
                                messages={messages}/>
                    <ChatMessageBox handleSendMessage={handleSendMessage} 
                                currentChat={currentChat} 
                                user={user}
                                socket={socket}/>
                </div> : 
                <div className="chat chat--empty">
                    <span>Select user to chat with</span>
                </div>
            }
        </div>
    )
}