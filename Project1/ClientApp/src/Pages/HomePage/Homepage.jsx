import React, { useState, useEffect } from 'react';
import Header from "../../Components/Header/header";
import Sidebar from "../../Components/Sidebar/sidebar";
import Footer from '../../Components/Footer/footer';
import ScheduleCard from "../../Components/Schedulecard/Schedule";
import TeacherPage from '../Teacherpage/Teacherpage'
import "./Homepage.css";

const HomePage = ({ selectedDate, setSelectedDate }) => {

    const [schedules, setSchedules] = useState([]);

    const Data = selectedDate.toISOString().split('T')[0]

    const fetchSchedules = async () => {
        try {

            const token = localStorage.getItem('authToken'); // Отримуємо токен із локального сховища (або звідки ви його зберігаєте)

            if (!token) {
                throw new Error('Token is missing');
            }

            const response = await fetch('/api/getschedule/date', {
                method: 'POST', // використовуємо POST метод
                headers: {
                    'Content-Type': 'application/json', // вказуємо тип вмісту
                    'Authorization': `Bearer ${token}`
                },
                body: JSON.stringify(Data), // передаємо дані в тілі запиту
            });

            if (!response.ok) {
                throw new Error(`HTTP error! Status: ${response.status}`);
            }

            const data = await response.json();
            setSchedules(data); // Зберігаємо отримані дані в стан
        } catch (error) {
            console.error("Error fetching schedules:", error);
        }
    };

    // Виконується щоразу, коли змінюється selectedDate
    useEffect(() => {
        if (selectedDate) {
            fetchSchedules(selectedDate);
            setSchedules([]);
        }
    }, [selectedDate]);

    return (
        <div className="home-page">
            <Header />
            <Sidebar selectedDate={selectedDate} setSelectedDate={setSelectedDate} />
            <div className="content">
                <main>
                    <div className="blocks">
                        {schedules.length === 0 ? (
                            <p>На сьогодні пар немає</p>
                        ) : (
                            schedules.map((schedule, index) => (
                                <ScheduleCard
                                    key={index}
                                    time={schedule.numPara}
                                    room={`каб. ${schedule.cabinet}`}
                                    subject={schedule.subject}
                                    type={schedule.typePara}
                                    groups={schedule.groups}
                                    teacher={schedule.teacherName}
                                />
                            ))
                        )}
                    </div>
                </main>
            </div>  
            <Footer />
        </div>
    );
}

export default HomePage;
