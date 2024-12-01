import React from 'react';
import HomePage from '../HomePage/Homepage';
import TeacherPage from '../Teacherpage/Teacherpage';

const SwitchHomepage = ({ selectedDate, setSelectedDate }) => {
    return (
        <HomePage selectedDate={selectedDate} setSelectedDate={setSelectedDate} />
        //<TeacherPage />
    )
}

export default SwitchHomepage;