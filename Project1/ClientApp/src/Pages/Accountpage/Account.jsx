import React, { useEffect, useState } from 'react';
import Header from "../../Components/Header/header";
import Footer from '../../Components/Footer/footer';
import UserCard from '../../Components/Usercard/Usercard';
import StudentList from '../../Components/Studentlist/Studentlist';
import PointCard from '../../Components/Pointcard/Pointcard';
//import Teachersetpage from '../Teachersetpage/Teachersetpage';
import profileimg from './../../img_src/account frame.svg';
import './Account.css';

const AccountPage = () => {

    const [error, setError] = useState(null);
    const [userData, setUserData] = useState(null);

    useEffect(() => {
        // Отримуємо токен з localStorage
        const token = localStorage.getItem('authToken');

        if (!token) {
            setError('Authorization token is missing');
            return;
        }

        // Використовуємо fetch для запиту до API
        fetch('/api/usercard/data', {
            method: 'GET',
            headers: {
                'Authorization': `Bearer ${token}`,  // Додаємо токен в заголовок
                'Content-Type': 'application/json'
            }
        })
            .then(response => {
                if (!response.ok) {
                    throw new Error('Failed to fetch user data');
                }
                return response.json();
            })
            .then(data => {
                setUserData(data);  // Встановлюємо отримані дані в стан
            })
            .catch(error => {
                setError(error.message);
            });
    }, []);

    if (error) {
        return <div>Error: {error}</div>;
    }

    // Якщо дані ще не завантажені
    if (!userData) {
        return <div>Loading...</div>;
    }

  const user = {
    avatar: profileimg,
    firstName: userData.Name,
    lastName: userData.SurName,
    middleName: userData.ThirdName,
    group: userData.Student.Group.Name,
    institute: userData.Student.Institute.Name,
    course: userData.Student.Group.Course,
  };

  const students = [
    { initials: "Бачманюк Д.О.", scores: 0 },
    { initials: "Галенюк М.П.", scores: 0 },
    { initials: "Парипа А.О.", scores: 0 },
  ];

  const grades = [
    { subject: 'Укр. мова', totalScore: 70 },
    { subject: 'Математика', totalScore: 85 },
    { subject: 'Програмування', totalScore: 95 },
    { subject: 'Програмування', totalScore: 95 },
    { subject: 'Програмування', totalScore: 95 },
    { subject: 'Програмування', totalScore: 95 },
    { subject: 'Програмування', totalScore: 95 },
    { subject: 'Програмування', totalScore: 95 },
    { subject: 'Програмування', totalScore: 95 },
    { subject: 'Програмування', totalScore: 95 },
    
  ];

  return (
    <div className="account-page">
      <Header />
      <main className="account-content">
        <div className="account-container">
          <UserCard user={user} />
          <StudentList students={students} />
          <PointCard grades={grades} /> 
          {/*<Teachersetpage />  */}
        </div>
      </main>
      <Footer />
    </div>
  );
};

export default AccountPage;
