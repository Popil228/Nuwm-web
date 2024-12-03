import React, { useState, useEffect } from 'react';
import moment from 'moment';
import Select from 'react-select';
import DatePicker from 'react-datepicker';
import 'react-datepicker/dist/react-datepicker.css';
import './Teacherpage.css';
import Header from '../../Components/Header/header';
import Footer from '../../Components/Footer/footer';

const TeacherPage = () => {
    const [group, setGroup] = useState(null);
    const [subgroup, setSubgroup] = useState(null);
    const [pairNumber, setPairNumber] = useState(null);
    const [type, setType] = useState(null);
    const [subject, setSubject] = useState(null);
    const [teacher, setTeacher] = useState(null);
    const [classroom, setClassroom] = useState('');
    const [date, setDate] = useState(null);
    const [logs, setLogs] = useState([]);

    const [groups, setGroups] = useState([]);
    const [subgroups, setSubgroups] = useState([]);
    const [pairNumbers, setPairNumbers] = useState([]);
    const [types, setTypes] = useState([]);
    const [subjects, setSubjects] = useState([]);
    const [teachers, setTeachers] = useState([]);

    // Завантаження даних для форми
    useEffect(() => {
        const fetchData = async () => {
            try {
                const response = await fetch('/api/setschedule/data', {
                    method: 'POST'
                });

                if (!response.ok) {
                    throw new Error(`Помилка сервера: ${response.statusText}`);
                }

                const data = await response.json();
                setGroups(data.groups);
                setPairNumbers(data.pairNumbers);
                setTypes(data.types);
            } catch (error) {
                console.error('Помилка завантаження даних:', error);
            }
        };

        fetchData();
    }, []);

    // Завантаження підгруп та викладачів
    useEffect(() => {
        if (group) {
            setSubgroup(null)
            setTeacher(null)
            const fetchSubgroupsAndTeachers = async () => {
                try {
                    const response = await fetch('/api/setschedule/subgroups_teachers', {
                        method: 'POST',
                        headers: {
                            'Content-Type': 'application/json',
                        },
                        body: JSON.stringify({ groupId: group.value }),
                    });

                    if (!response.ok) {
                        throw new Error(`Помилка сервера: ${response.statusText}`);
                    }

                    const data = await response.json();
                    setSubgroups(data.subgroups);
                    setTeachers(data.teachers);
                } catch (error) {
                    console.error('Помилка завантаження підгруп та викладачів:', error);
                }
            };

            fetchSubgroupsAndTeachers();
        } else {
            setSubgroups([]);
            setTeachers([]);
        }
    }, [group]);

    // Завантаження дисциплін
    useEffect(() => {
        if (teacher) {
            setSubject(null)
            const fetchSubjects = async () => {
                try {
                    const response = await fetch('/api/setschedule/subjects', {
                        method: 'POST',
                        headers: {
                            'Content-Type': 'application/json',
                        },
                        body: JSON.stringify({ teacherId: teacher.value }),
                    });

                    if (!response.ok) {
                        throw new Error(`Помилка сервера: ${response.statusText}`);
                    }

                    const data = await response.json();
                    setSubjects(data);
                } catch (error) {
                    console.error('Помилка завантаження дисциплін:', error);
                }
            };

            fetchSubjects();
        } else {
            setSubjects([]);
        }
    }, [teacher]);

    const handleSubmit = async (e) => {
        e.preventDefault();

        if (!group || !subgroup || !pairNumber || !type || !subject || !teacher || !classroom || !date) {
            alert('Будь ласка, заповніть усі поля форми.');
            return;
        }

        const formattedDate = moment(date).format('YYYY-MM-DD');

        const scheduleData = {
            SubgroupId: subgroup?.value || null,
            NumParaId: pairNumber.value,
            TypeParaId: type.value,
            SubjectId: subject.value,
            Cabinet: classroom,
            Data: formattedDate
        };

        try {
            const response = await fetch('/api/setschedule/schedule', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify(scheduleData),
            });

            if (!response.ok) {
                throw new Error('Помилка при додаванні розкладу');
            }

            const result = await response.json();
            alert(result);
        } catch (error) {
            console.error('Помилка:', error);
            alert('Сталася помилка при додаванні розкладу');
        }

        const logEntry = {
            group: group?.label || 'Не вказано',
            subgroup: subgroup?.label || 'Не вказано',
            pairNumber: pairNumber?.label || 'Не вказано',
            type: type?.label || 'Не вказано',
            subject: subject?.label || 'Не вказано',
            classroom: classroom || 'Не вказано',
            teacher: teacher?.label || 'Не вказано',
            date: date ? date.toLocaleDateString() : 'Не вказано',
            timestamp: new Date().toLocaleString(),
        };

        setLogs((prevLogs) => [...prevLogs, logEntry]);

        // Скидання форми
        setGroup(null);
        setSubgroup(null);
        setPairNumber(null);
        setType(null);
        setSubject(null);
        setTeacher(null);
        setClassroom('');
        setDate(null);
    };

    const deleteLastLog = async () => {

        try {
            const response = await fetch('/api/setschedule/delete', {
                method: 'DELETE', // Використовуємо метод DELETE
            });

            if (!response.ok) {
                throw new Error('Помилка при видаленні останнього розкладу');
            }

            const result = await response.json();
            alert(result); // Виводимо повідомлення про успішне видалення
        } catch (error) {
            console.error('Помилка:', error);
            alert('Сталася помилка при видаленні останнього розкладу');
        }

        setLogs((prevLogs) => prevLogs.slice(0, prevLogs.length - 1));
    };

    return (
        <div>
            <Header />
            <div className="teacher-page-container">
                <form onSubmit={handleSubmit} className="inputs">
                    <div className="form-group">
                        <label>Група</label>
                        <Select
                            options={groups}
                            value={group}
                            onChange={setGroup}
                            placeholder="Оберіть групу"
                        />
                    </div>
                    <div className="form-group">
                        <label>Підгрупа</label>
                        <Select
                            options={subgroups}
                            value={subgroup}
                            onChange={setSubgroup}
                            placeholder="Оберіть підгрупу"
                            isDisabled={!group} // Вимкнути, якщо група не вибрана
                        />
                    </div>
                    <div className="form-group">
                        <label>Ініціали викладача</label>
                        <Select
                            options={teachers}
                            value={teacher}
                            onChange={setTeacher}
                            placeholder="Оберіть викладача"
                            isDisabled={!group} // Вимкнути, якщо група не вибрана
                        />
                    </div>
                    <div className="form-group">
                        <label>Дисципліна</label>
                        <Select
                            options={subjects}
                            value={subject}
                            onChange={setSubject}
                            placeholder="Оберіть дисцепліну"
                            isDisabled={!teacher} // Вимкнути, якщо викладач не вибраний
                        />
                    </div>
                    <div className="form-group">
                        <label>Номер пари</label>
                        <Select
                            options={pairNumbers}
                            value={pairNumber}
                            onChange={setPairNumber}
                            placeholder="Оберіть номер пари"
                        />
                    </div>
                    <div className="form-group">
                        <label>Вид заняття</label>
                        <Select
                            options={types}
                            value={type}
                            onChange={setType}
                            placeholder="Оберіть вид заняття"
                        />
                    </div>
                    <div className="form-group">
                        <label>Кабінет</label>
                        <input
                            type="text"
                            value={classroom}
                            onChange={(e) => setClassroom(e.target.value)}
                            placeholder="Введіть кабінет"
                        />
                    </div>
                    <div className="form-group">
                        <label>Дата проведення</label>
                        <DatePicker
                            selected={date}
                            onChange={(selectedDate) => setDate(selectedDate)}
                            dateFormat="dd.MM.yyyy"
                            placeholderText="Оберіть дату"
                        />
                    </div>
                    <button type="submit" className="submit-button">Призначити пару</button>
                </form>

                <div className="logs">
                    <h2>Призначені пари:</h2>
                    {logs.length === 0 ? (
                        <p>Призначити пару.</p>
                    ) : (
                        <ul>
                            {logs.map((log, index) => (
                                <li key={index}>
                                    <strong>{log.timestamp}</strong> - Група: {log.group}, Підгрупа: {log.subgroup},
                                    Пара: {log.pairNumber}, Тип: {log.type}, Предмет: {log.subject}, Кабінет: {log.classroom}, Викладач: {log.teacher}, Дата: {log.date}
                                </li>
                            ))}
                        </ul>
                    )}
                    <button onClick={deleteLastLog} disabled={logs.length === 0}>
                        Видалити останню пару
                    </button>
                </div>
            </div>
            <Footer />
        </div>
    );
};

export default TeacherPage;
