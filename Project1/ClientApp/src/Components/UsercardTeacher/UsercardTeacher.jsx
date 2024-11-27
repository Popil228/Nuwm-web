import React from 'react';
import './UsercardTeacher.css'

const UserCard = ({ user }) => {
  return (
    <div className="user-info">
      <div className="user-avatar">
        <img src={user.avatar} alt="Avatar" />
      </div>
      <div className="user-details">
        <p>Прізвище: {user.lastName}</p>
        <p>Ім'я: {user.firstName}</p>
        <p>По батькові: {user.middleName}</p>
      </div>
    </div>
  );
};

export default UserCard;


