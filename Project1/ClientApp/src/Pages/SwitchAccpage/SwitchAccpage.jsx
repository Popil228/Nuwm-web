import React, { useState, useEffect } from 'react';
import Teachersetpage from '../Teachersetpage/Teachersetpage';
import Accontpage from '../Accountpage/Account';

const SwitchAccpage = () => {

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
        if (roles.includes('Teacher')) {
            return <Teachersetpage />;
        } else if (roles.includes('Student')) {
            return <Accontpage />;
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

export default SwitchAccpage;