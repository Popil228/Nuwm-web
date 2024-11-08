import React from 'react';
import { BrowserRouter as Router, Route, Routes } from 'react-router-dom';
import HomePage from './Pages/HomePage/Homepage.jsx';
import LoginPage from './Pages/Loginpage/Loginpage.jsx';
import ContactsPage from './Pages/Contactpage/Contactpage.jsx';
import AccountPage from './Pages/Accountpage/Account.jsx';
import './styles/common.css';
import './styles/reset.css';


function App() {
  return (
    <Router>
      <div className="App">
        <Routes>
          <Route path="/" element={<LoginPage />} /> 
          <Route path="/home" element={<HomePage />} /> 
          <Route path="/main" element={<HomePage />} /> 
          <Route path="/account" element={<AccountPage />} />
          <Route path="/contacts" element={<ContactsPage />} />
        </Routes>
      </div>
    </Router>
  );
}

export default App;
