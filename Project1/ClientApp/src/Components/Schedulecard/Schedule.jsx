import React from 'react';
import './Schedule.css'; 

const ScheduleCard = ({ time, subject, room, type, groups, teacher }) => {
  return (
      <div className="schedule-card"> 
        <div className="left-bar"></div>
        <div className="schedule-card-content">
          <div className="schedule-card-header">
            <div className="time">{time || "Не вказано"}</div> 
            <div className="room">{room || "Не вказано"}</div> 
          </div>
          <div className="subject">{subject || "Не вказано"}</div> 
          <div className="type">{type || "Не вказано"}</div> 
          <div className="groups-teacher-container">
            <div className="groups">{groups || "Не вказано"}</div> 
            <div className="teacher">{teacher || "Не вказано"}</div> 
          </div>
        </div>
      </div>
  );
};

export default ScheduleCard;
