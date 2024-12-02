import './header.css';
import React, { useState, useEffect } from 'react';
import logoimg from './../../img_src/logo.svg';
import profileimg from './../../img_src/account frame.svg';

function Header() {
    const [isMenuOpen, setMenuOpen] = useState(false);

    const toggleMenu = () => {
        setMenuOpen(!isMenuOpen);
    };

    const [roles, setRoles] = useState([]);
    const [error, setError] = useState('');

    useEffect(() => {
        const fetchRoles = async () => {
            try {
                // Отримуємо токен із локального сховища або кукі
                const token = localStorage.getItem('authToken');
                if (!token) {
                    setError('No token found');
                    return;
                }

                // Виконуємо запит до API для отримання ролей
                const response = await fetch('/api/role/role', {
                    method: 'GET',
                    headers: {
                        'Authorization': `Bearer ${token}`,
                        'Content-Type': 'application/json'
                    }
                });

                if (!response.ok) {
                    throw new Error('Failed to fetch roles');
                }

                const data = await response.json();
                setRoles(data); // зберігаємо ролі в стані
            } catch (error) {
                setError(error.message); // якщо виникли помилки, виводимо їх
            }
        };

        fetchRoles();
    }, []);

    const renderPage = () => {
        if (roles.includes('Admin')) {
            return(
                <ul>
                    <li><a href="/main">Головна</a></li>
                </ul>
            )
        } else if (roles.includes('Student') || roles.includes('Teacher')) {
            return(
                <ul>
                    <li><a href="/main">Головна</a></li>
                    <li><a href="/account">Акаунт</a></li>
                    <li><a href="/contacts">Контакти</a></li>
                </ul>
            )
        } else {
            return <div></div>;
        }
    };

    return (
        <header className="header">
            <div className='container'>
                <div className="header__row">
                    <div className="header__logo">
                        <img src={logoimg} alt="Logo" />
                    </div>

                    <nav className={`header__nav ${isMenuOpen ? 'open' : ''}`}>
                        {error && <p style={{ color: 'red' }}>{error}</p>}
                        {renderPage()}
                    </nav>

                    <div className='burger-menu' onClick={toggleMenu}>
                        <div></div>
                        <div></div>
                        <div></div>
                    </div>

                    <div className='profile-section'>
                        <img src={profileimg} alt="Profile" />
                    </div>
                </div>
            </div>
        </header>
    );
}

export default Header;
