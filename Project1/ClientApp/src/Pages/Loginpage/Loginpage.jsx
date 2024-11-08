import React from 'react';
import { useNavigate } from 'react-router-dom';
import './Loginpage.css'
import logoico from './../../img_src/logoico.svg'

const Loginpage = () => {
    const navigate = useNavigate();
    const handleLogin = () => {
        console.log('Користувач натиснув кнопку "Увійти"');
        navigate('/home');
    };

    return (
        <div className="login-box">
            <div className="icon">
                <img src={logoico} alt="Icon" />
            </div>
            <div className='text'>
                <p>Увійдіть в акаунт</p>
            </div>
            <button className="login-button" onClick={handleLogin}>Увійти</button>
        </div>
    );
};

export default Loginpage;