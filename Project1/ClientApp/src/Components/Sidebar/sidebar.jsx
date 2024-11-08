import React, { useState } from 'react';
import './sidebar.css';

const generateWeekDates = (startDate) => {
  const week = [];
  for (let i = 0; i < 7; i++) {
    const currentDate = new Date(startDate);
    currentDate.setDate(startDate.getDate() + i);
    week.push(currentDate);
  }
  return week;
};

const Sidebar = () => {
  const getMondayOfCurrentWeek = (date) => {
    const day = date.getDay();
    const diff = date.getDate() - day + (day === 0 ? -6 : 1);
    return new Date(date.setDate(diff));
  };

  const [isOpen, setIsOpen] = useState(false);
  const [currentWeekStart, setCurrentWeekStart] = useState(getMondayOfCurrentWeek(new Date()));
  const [selectedDate, setSelectedDate] = useState(null);
  const today = new Date();

  const toggleSidebar = () => {
    setIsOpen(!isOpen);
  };

  const handleUpClick = () => {
    const newStartDate = new Date(currentWeekStart);
    newStartDate.setDate(currentWeekStart.getDate() - 7);
    setCurrentWeekStart(newStartDate);
  };

  const handleDownClick = () => {
    const newStartDate = new Date(currentWeekStart);
    newStartDate.setDate(currentWeekStart.getDate() + 7);
    setCurrentWeekStart(newStartDate);
  };

  const weekDates = generateWeekDates(currentWeekStart);

  const handleDayClick = (date) => {
    setSelectedDate(date);
  };

  const isToday = (date) => {
    return (
      date.getDate() === today.getDate() &&
      date.getMonth() === today.getMonth() &&
      date.getFullYear() === today.getFullYear()
    );
  };

  const isSelectedDate = (date) => {
    return (
      selectedDate &&
      date.getDate() === selectedDate.getDate() &&
      date.getMonth() === selectedDate.getMonth() &&
      date.getFullYear() === selectedDate.getFullYear()
    );
  };

  return (
    <>
      <button className="menu-button" onClick={toggleSidebar}>
        ➤
      </button>
      <div className={`sidebar ${isOpen ? 'open' : ''}`}>
        <div className="arrow-up">
          <button className="arrow-btn" onClick={handleUpClick}>
            ▲
          </button>
        </div>
        <div className="week-calendar">
          {weekDates.map((date, index) => (
            <button
              key={index}
              className={`nav-btn ${isToday(date) ? 'highlight' : ''} ${isSelectedDate(date) ? 'selected' : ''}`}
              onClick={() => handleDayClick(date)}
            >
              {date.toLocaleDateString('uk-UA', { weekday: 'short' })}
              <br />
              {date.toLocaleDateString('uk-UA')}
            </button>
          ))}
        </div>
        <div className="arrow-down">
          <button className="arrow-btn" onClick={handleDownClick}>
            ▼
          </button>
        </div>
      </div>
    </>
  );
};

export default Sidebar;
