import React, { useState, useEffect } from 'react';
import Header from "../../Components/Header/header";
import Footer from '../../Components/Footer/footer';
import './Contactpage.css';

const ContactsPage = () => {

    const [formData, setFormData] = useState({
        firstName: "",
        lastName: "",
        email: "",
        phone: "",
        message: "",
    });

    const handleChange = (e) => {
        const { name, value } = e.target;
        setFormData({ ...formData, [name]: value });
    };

    const handleSubmit = async (e) => {
        e.preventDefault();

        try {
            const response = await fetch("/api/contact/contact", {
                method: "POST",
                headers: {
                    "Content-Type": "application/json",
                },
                body: JSON.stringify(formData),
            });

            if (response.ok) {
                alert("Повідомлення успішно відправлено!");
                setFormData({
                    firstName: "",
                    lastName: "",
                    email: "",
                    phone: "",
                    message: "",
                });
            } else {
                alert("Сталася помилка при надсиланні повідомлення.");
            }
        } catch (error) {
            console.error("Error:", error);
            alert("Сталася помилка при надсиланні повідомлення.");
        }
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
            return <div>Access denied: You don't have the necessary role.</div>;
        } else if (roles.includes('Student') || roles.includes('Teacher')) {
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
                                        <p className="phone"><strong>Phone:</strong><br /> +38 063 619 7705<br /> +38 063 619 8805</p>
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
                                    <form onSubmit={handleSubmit}>
                                        <div className="name-surname">
                                            {/*Ім'я*/}
                                            <input type="text" name="firstName" placeholder="Ім'я" value={formData.firstName} onChange={handleChange} required />
                                            {/*Прізвище*/}
                                            <input type="text" name="lastName" placeholder="Прізвище" value={formData.lastName} onChange={handleChange} required />
                                        </div>
                                        <div className="email-phone">
                                            {/*Пошта*/}
                                            <input type="email" name="email" placeholder="Email" value={formData.email} onChange={handleChange} required />
                                            {/*Телефон*/}
                                            <input type="text" name="phone" placeholder="Телефон" value={formData.phone} onChange={handleChange} />
                                        </div>
                                        {/*Повідомлення*/}
                                        <textarea name="message" placeholder="Повідомлення" value={formData.message} onChange={handleChange} required></textarea>
                                        {/*Відправити*/}
                                        <button type="submit">Відправити</button>
                                    </form>
                                </div>
                            </div>
                        </div>
                    </main>
                </div>
                <Footer />
            </div>);
        } else {
            return <div>Access denied: You don't have the necessary role.</div>;
        }
    };

    return (
        <div>
            {error && <p style={{ color: 'red' }}>{error}</p>}
            {renderPage()}
        </div>
    );
}

export default ContactsPage;
