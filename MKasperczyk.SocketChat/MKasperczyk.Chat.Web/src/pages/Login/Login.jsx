import React from 'react'
import "./Login.css"
import axios from "axios"
import { ToastContainer, toast } from "react-toastify";
import { loginUrl, registerUrl } from '../../helpers/ApiRequests';
import { Link, useNavigate } from "react-router-dom"

export default function Login() {
    const navigate = useNavigate()
    const [formData, setFormData] = React.useState({
        userName: "",
        password: ""
    })

    const toastOption = {
        position: "top-right",
        pauseOnHover: true,
        draggable: true
    }

    const validateLoginRequest = () => {
        const { password, userName } = formData;
        if(userName === "" || password === "") {
            toast.error("Username and password should not be empty", toastOption)
            return false;
        } 
        return true;
    }

    const handleSubmit = async (event) => {
        event.preventDefault()
        if(validateLoginRequest()) {
            const { userName, password } = formData;
            const { data } = await axios.post(loginUrl, {
                userName,
                password
            })
            debugger
            if(!data.success) {
                toast.error(data.message, toastOption)
            }
            if(data.success) {
                localStorage.setItem("chat-user", JSON.stringify({
                    id: data.id,
                    userName: data.user,
                }))
                navigate("/");
            }
        }
    }

    const handleChange = (event) => {        
        const {name, value} = event.target
        setFormData(prevFormData => {
            return { 
                ...prevFormData,
                [name]: value
            }
        })
    }

    return (
        <div className="card">
            <div className="register container">
                <form onSubmit={handleSubmit}>
                    <fieldset>
                        <div id="legend">
                            <legend className="">Login</legend>
                        </div>
                        <div className="form-group">
                            <label htmlFor="userName">User Name</label>
                            <input 
                                id="userName"
                                type="text"
                                placeholder="UserName"
                                className="form-control"
                                name="userName"
                                onChange={handleChange} 
                                value={formData.userName}/>
                        </div>
                        
                        <div className="form-group">
                            <label htmlFor="password">Password</label>
                            <input 
                                id="password"
                                type="password"
                                placeholder="Password"
                                className="form-control"
                                name="password"
                                onChange={handleChange} 
                                value={formData.password}/>
                        </div>
                        
                        <div className="form-group">
                            <button type="submit" className="btn btn-primary">Login</button> <br /><br />
                            <span>If you do not have account, you can create here <Link to="/register">Register</Link></span>
                        </div>
                    </fieldset>
                </form>
            </div>
            <ToastContainer />
        </div>
    )
}