import React from 'react'
import { Link, useNavigate } from "react-router-dom"
import "./Register.css"
import axios from "axios"
import { ToastContainer, toast } from "react-toastify";
import { registerUrl } from '../../helpers/ApiRequests';

export default function Register() {
    const navigate = useNavigate()
    const [formData, setFormData] = React.useState({
        userName: "",
        password: "",
        confirmPassword: "",
    })

    const toastOption = {
        position: "bottom-right",
        pauseOnHover: true,
        draggable: true
    }

    const validateRegisterRequest = () => {
        const { password, confirmPassword, userName } = formData;
        if(password !== confirmPassword) {
            toast.error("Password and Confirmation password should be the same.", toastOption)
            return false;
        } else if(userName.length < 3) {
            toast.error("Username should be greater then 3 characters", toastOption)
            return false;
        } else if(password.length < 7) {
            toast.error("Password should be equal or greate then 7 characters", toastOption)
            return false;
        }
        return true;
    }

    const handleSubmit = async (event) => {
        event.preventDefault()
        if(validateRegisterRequest()) {
            const { userName, password } = formData;
            await axios.post(registerUrl, {
                userName,
                password
            }).then((data) => {
                debugger;
                if(!data.success) {
                    toast.error(data.message, toastOption)
                } else {
                    localStorage.setItem("chat-user", JSON.stringify(data.userName))
                    navigate("/");
                }
            })
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
                            <legend className="">Register</legend>
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
                            <label htmlFor="confirmPassword">Confirm Password</label>
                            <input 
                                id="confirmPassword"
                                type="password"
                                placeholder="Confirm Password"
                                className="form-control"
                                name="confirmPassword"
                                onChange={handleChange} 
                                value={formData.confirmPassword}/>
                        </div>
                        
                        <div className="form-group">
                            <button type="submit" className="btn btn-primary">Create User</button> <br />
                            <span>Already have an account? <Link to="/login">Login</Link></span>
                        </div>
                    </fieldset>
                </form>
            </div>
            <ToastContainer />
        </div>
    )
}