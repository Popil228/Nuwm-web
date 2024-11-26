import React from 'react';
import './footer.css';

const Footer = () => {
    return (
        <footer className="footer">
            <div className="container">
                <div className="footer-content">
                    <p>©2024 Національний університет водного господарства та природокористування</p>
                </div>
                <div className="footer-contact">
                    <div className="contact-item1">
                        <p>Телефон: <br />+38 063 619 7705</p>
                    </div>
                    <div className="contact-item2">
                        <p>Тех. підтримка:<br /> support@email.com</p>
                    </div>
                </div>
            </div>
        </footer>
    );
}

export default Footer;
