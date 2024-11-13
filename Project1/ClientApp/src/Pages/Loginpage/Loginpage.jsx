import React from 'react';
//import { useNavigate } from 'react-router-dom';
import './Loginpage.css'
import logoico from './../../img_src/logoico.svg'

const Loginpage = () => {
    
    useEffect(() => {
        async function fetchLogin() {
            const token = await authService.getAccessToken();
            const response = await fetch('Login/Login', {
                headers: !token ? {} : { 'Authorization': `Bearer ${token}` }
            });
            //const data = await response.json();
            //setLogin(data);
        }

        fetchLogin();
    }, []);

       

    return (
        <div className="login-box">
            <div className="icon">
                <img src={logoico} alt="Icon" />
            </div>
            <div className='text'>
                <p>Увійдіть в акаунт</p>
            </div>
            <button className="login-button" onClick={fetchData} >Увійти</button>
        </div>
    );
};

export default Loginpage;