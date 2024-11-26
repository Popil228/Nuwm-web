import React from "react";
import './Loginpage.css';
import logoico from './../../img_src/logoico.svg';

const LoginPage = () => {
    const handleLogin = () => {
        window.location.href = "https://localhost:7230/api/auth/login";
    };

    return (
        <div className="login-box">
            <div className="icon">
                <img src={logoico} alt="Icon" />
            </div>
            <div className='text'>
                <p>Увійдіть в акаунт</p>
            </div>
            <button className="login-button" onClick={handleLogin} >
            Увійти
            </button>
        </div>
    );
};

export default LoginPage;