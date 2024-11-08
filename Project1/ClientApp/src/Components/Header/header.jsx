import './header.css';
import { useState } from 'react';
import logoimg from './../../img_src/logo.svg';
import profileimg from './../../img_src/account frame.svg';

function Header() {
    const [isMenuOpen, setMenuOpen] = useState(false);

    const toggleMenu = () => {
        setMenuOpen(!isMenuOpen);
    };

    return (
        <header className="header">
            <div className='container'>
                <div className="header__row">
                    <div className="header__logo">
                        <img src={logoimg} alt="Logo" />
                    </div>

                    <nav className={`header__nav ${isMenuOpen ? 'open' : ''}`}>
                        <ul>
                            <li><a href="/main">Головна</a></li>
                            <li><a href="/account">Акаунт</a></li>
                            <li><a href="/contacts">Контакти</a></li>
                        </ul>
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
