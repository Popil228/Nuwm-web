import React, { useState } from 'react';
import { BrowserRouter as Router, Route, Routes, Navigate } from 'react-router-dom';
import LoginPage from './Pages/Loginpage/Loginpage.jsx';
import ContactsPage from './Pages/Contactpage/Contactpage.jsx';
import './styles/common.css';
import './styles/reset.css';
import LoginSuccess from './Pages/Loginsuccess/Loginsuccess.jsx';
import SwitchAccpage from './Pages/SwitchAccpage/SwitchAccpage.jsx';
import SwitchHomepage from './Pages/SwitchHomepage/SwitchHomepage.jsx';

// Функція для перевірки автентифікації (наприклад, перевірка localStorage чи контексту)
const isAuthenticated = () => {
    return localStorage.getItem('authToken'); // Приклад перевірки токена
};

// Компонент, що відображає лише, якщо користувач аутентифікований
const ProtectedRoute = ({ element }) => {
    return isAuthenticated() ? element : <Navigate to="/" />;
};

function App() {

    const [selectedDate, setSelectedDate] = useState(new Date());

    return (
        <Router>
            <div className="App">
                <Routes>
                    <Route path="/" element={<LoginPage />} />
                    <Route path="/home" element={<ProtectedRoute element={<SwitchHomepage selectedDate={selectedDate} setSelectedDate={setSelectedDate} />} />} />
                    <Route path="/main" element={<ProtectedRoute element={<SwitchHomepage selectedDate={selectedDate} setSelectedDate={setSelectedDate} />} />} />
                    <Route path="/account" element={<ProtectedRoute element={<SwitchAccpage />} />} />
                    <Route path="/contacts" element={<ProtectedRoute element={<ContactsPage />} />} />
                    <Route path="/login-success" element={<LoginSuccess />} />
                </Routes>
            </div>
        </Router>
    );
}

export default App;
