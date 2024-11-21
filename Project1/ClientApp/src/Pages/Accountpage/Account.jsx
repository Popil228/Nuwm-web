import React from 'react';
import Header from "../../Components/Header/header";
import Footer from '../../Components/Footer/footer';
import UserCard from '../../Components/Usercard/Usercard';
import StudentList from '../../Components/Studentlist/Studentlist';
import PointCard from '../../Components/Pointcard/Pointcard';
import Teachersetpage from '../Teachersetpage/Teachersetpage';
import profileimg from './../../img_src/account frame.svg';
import './Account.css';

const AccountPage = () => {
  const user = {
    avatar: profileimg,
    firstName: "Андрій",
    lastName: "Галенюк",
    middleName: "Олександрович",
    group: "KI-21",
    institute: "КІТІ",
    course: 2,
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
          {/* <UserCard user={user} />
          <StudentList students={students} />
          <PointCard grades={grades} /> */}
          <Teachersetpage />  
        </div>
      </main>
      <Footer />
    </div>
  );
};

export default AccountPage;
