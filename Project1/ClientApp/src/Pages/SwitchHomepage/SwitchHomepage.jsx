import React, { useState, useEffect } from 'react';
import HomePage from '../HomePage/Homepage';
import TeacherPage from '../Teacherpage/Teacherpage';

const SwitchHomepage = ({ selectedDate, setSelectedDate }) => {

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
            return <TeacherPage />;
        } else if (roles.includes('Student') || roles.includes('Teacher')) {
            return <HomePage selectedDate={selectedDate} setSelectedDate={setSelectedDate} />;
        } else {
            return <div>Access denied: You don't have the necessary role.</div>;
        }
    };

    return (
       
        <div>
            {error && <p style={{ color: 'red' }}>{error}</p>}
            {renderPage()}
        </div>
    )
}

export default SwitchHomepage;