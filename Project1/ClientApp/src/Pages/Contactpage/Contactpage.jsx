import React from 'react';
import Header from "../../Components/Header/header";
import Footer from '../../Components/Footer/footer';
import './Contactpage.css';

const ContactsPage = () => {
    return (
        <div className="contact-page">
            <Header />
            <div className="content">
                <main>
                    <div className="contacts-page">
                        <div className="contacts-container">
                            <div className="contact-info">
                                <div className="block-1">
                                    <p className="location">
                                        <span><i className="fas fa-map-marker-alt"></i></span><br />
                                        <strong>Location:</strong><br /> Україна, 33028, м. Рівне, вул. Соборна 11
                                    </p>
                                </div>
                                <div className="block-2">
                                    <p className="email"><strong>Email:</strong><br /> support@email.com</p>
                                    <p className="phone"><strong>Phone:</strong><br /> +38 098 877 01<br /> +38 063 421 63</p>
                                </div>
                                <div className="block-3">
                                    <div className="social">
                                        <p><strong>Social:</strong></p>
                                        <ul>
                                            <li><a href="https://www.facebook.com/www.nuwm.edu.ua/?locale=uk_UA" target="_blank"><i className="fab fa-facebook-f"></i></a></li>
                                            <li><a href="https://www.youtube.com/@NUWEE" target="_blank"><i className="fab fa-youtube"></i></a></li>
                                            <li><a href="https://www.instagram.com/nuwee_official/" target="_blank"><i className="fab fa-instagram"></i></a></li>
                                        </ul>
                                    </div>
                                </div>
                            </div>
                            <div className="contact-form">
                                <p>Відправити повідомлення</p>
                                <form>
                                    <div className="name-surname">
                                        <input type="text" placeholder="Ім'я" required />
                                        <input type="text" placeholder="Прізвище" required />
                                    </div>
                                    <div className="email-phone">
                                        <input type="email" placeholder="Email" required />
                                        <input type="tel" placeholder="Телефон" required />
                                    </div>
                                    <textarea placeholder="Повідомлення" required></textarea>
                                    <button type="submit">Відправити</button>
                                </form>
                            </div>
                        </div>
                    </div>
                </main>
            </div>
            <Footer />
        </div>
    );
}

export default ContactsPage;
