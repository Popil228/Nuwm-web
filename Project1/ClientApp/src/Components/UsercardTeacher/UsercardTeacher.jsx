import React from 'react';
import './UsercardTeacher.css';


const UserCardTeacher = ({ user }) => {
  return (
    <div className="user-info-t">
      <div className="user-avatar-t">
        <img src={user.avatar} alt="Avatar-t" />
      </div>
      <div className="user-details-t">
        <p>Прізвище: {user.lastName}</p>
        <p>Ім'я: {user.firstName}</p>
        <p>По батькові: {user.middleName}</p>
      </div>
    </div>
  );
};

export default UserCardTeacher;


