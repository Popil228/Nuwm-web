import React, { useEffect, useState } from 'react';
import Header from "../../Components/Header/header";
import Footer from '../../Components/Footer/footer';
import UserCard from '../../Components/Usercard/Usercard';
import StudentList from '../../Components/Studentlist/Studentlist';
import PointCard from '../../Components/Pointcard/Pointcard';
import Teachersetpage from '../Teachersetpage/Teachersetpage';
import profileimg from './../../img_src/account frame.svg';
import './Account.css';

const AccountPage = () => {

    const [error, setError] = useState(null);
    const [userData, setUserData] = useState(null);
    const [markStudentData, setMarkStudentData] = useState(null);
    const [markGroupData, setMarkGroupData] = useState(null);

    useEffect(() => {
        // Отримуємо токен з localStorage
        const token = localStorage.getItem('authToken');

        if (!token) {
            setError('Authorization token is missing');
            return;
        }

        // Використовуємо fetch для запиту до API
        fetch('/api/account/data', {
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

        fetch('/api/account/student-total-scores', {
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
                setMarkStudentData(data);  // Встановлюємо отримані дані в стан
            })
            .catch(error => {
                setError(error.message);
            });

        fetch('/api/account/group-student-total-scores', {
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
                setMarkGroupData(data);  // Встановлюємо отримані дані в стан
            })
            .catch(error => {
                setError(error.message);
            });
    }, []);

    

    if (error) {
        return <div>Error: {error}</div>;
    }

    // Якщо дані ще не завантажені
    if (!userData || !markStudentData || !markGroupData) {
        return <div>Loading...</div>;
    }

  const user = {
    avatar: profileimg,
    firstName: userData.name,
    lastName: userData.surName,
    middleName: userData.thirdName,
    group: userData.group + " (" + userData.subgroup + ")",
    institute: userData.institute,
    course: userData.course,
  };

const students = markGroupData.map(item => ({
    initials: item.studentName,
    scores: item.totalScore
}));

  const grades = markStudentData.map(item => ({
     subject: item.subjectName,
     totalScore: item.value
  }));

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
