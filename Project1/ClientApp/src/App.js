import React from 'react';
import { BrowserRouter as Router, Route, Routes, Navigate } from 'react-router-dom';
import HomePage from './Pages/HomePage/Homepage.jsx';
import LoginPage from './Pages/Loginpage/Loginpage.jsx';
import ContactsPage from './Pages/Contactpage/Contactpage.jsx';
import AccountPage from './Pages/Accountpage/Account.jsx';
import './styles/common.css';
import './styles/reset.css';
import LoginSuccess from './Pages/Loginsuccess/Loginsuccess.jsx';

// Функція для перевірки автентифікації (наприклад, перевірка localStorage чи контексту)
const isAuthenticated = () => {
    // Тобі слід реалізувати реальну перевірку статусу автентифікації
    return localStorage.getItem('authToken'); // Приклад перевірки токена
};

// Компонент, що відображає лише, якщо користувач аутентифікований
const ProtectedRoute = ({ element }) => {
    return isAuthenticated() ? element : <Navigate to="/" />;
};

function App() {
    return (
        <Router>
            <div className="App">
                <Routes>
                    <Route path="/" element={<LoginPage />} />
                    <Route path="/home" element={<ProtectedRoute element={<HomePage />} />} />
                    <Route path="/main" element={<ProtectedRoute element={<HomePage />} />} />
                    <Route path="/account" element={<ProtectedRoute element={<AccountPage />} />} />
                    <Route path="/contacts" element={<ProtectedRoute element={<ContactsPage />} />} />
                    <Route path="/login-success" element={<LoginSuccess />} />
                </Routes>
            </div>
        </Router>
    );
}

export default App;
