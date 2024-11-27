import React, { useState, useEffect } from 'react';
import Select from 'react-select';
import UsercardTeacher from '../../Components/UsercardTeacher/UsercardTeacher';
import profileimg from './../../img_src/account frame.svg';
import './Teachersetpage.css';

const Teachersetpage = () => {
    const [groups, setGroups] = useState([]);
    const [students, setStudents] = useState([]);
    const [subjects, setSubjects] = useState([]);
    const [group, setGroup] = useState(null);
    const [student, setStudent] = useState(null);
    const [subject, setSubject] = useState(null);
    const [score, setScore] = useState('');
    const [grades, setGrades] = useState([]);
    const [error, setError] = useState(null);
    const [userData, setUserData] = useState(null);

    // Завантаження даних користувача
    useEffect(() => {
        const token = localStorage.getItem('authToken');

        if (!token) {
            setError('Authorization token is missing');
            return;
        }

        fetch('/api/teacheraccount/data', {
            method: 'GET',
            headers: {
                'Authorization': `Bearer ${token}`,
                'Content-Type': 'application/json',
            },
        })
            .then(response => {
                if (!response.ok) {
                    throw new Error('Failed to fetch user data');
                }
                return response.json();
            })
            .then(data => setUserData(data))
            .catch(error => setError(error.message));
    }, []);

    // Завантаження груп, студентів, дисциплін
    useEffect(() => {
        const fetchData = async () => {
            try {
                const token = localStorage.getItem('authToken');

                const [groupsRes, studentsRes, subjectsRes] = await Promise.all([
                    fetch('/api/teacheraccount/groups', { headers: { Authorization: `Bearer ${token}` } }),
                    fetch('/api/teacheraccount/students', { headers: { Authorization: `Bearer ${token}` } }),
                    fetch('/api/teacheraccount/subjects', { headers: { Authorization: `Bearer ${token}` } }),
                ]);

                const [groupsData, studentsData, subjectsData] = await Promise.all([
                    groupsRes.json(),
                    studentsRes.json(),
                    subjectsRes.json(),
                ]);

                setGroups(groupsData.map(group => ({ value: group.id, label: group.name })));
                setStudents(studentsData.map(student => ({ value: student.id, label: student.name })));
                setSubjects(subjectsData.map(subject => ({ value: subject.id, label: subject.title })));
            } catch (err) {
                setError('Failed to load data');
            }
        };

        fetchData();
    }, []);

    if (error) {
        return <div>Error: {error}</div>;
    }

    if (!userData) {
        return <div>Loading...</div>;
    }

    const user = {
        avatar: profileimg,
        firstName: userData.name,
        lastName: userData.surName,
        middleName: userData.thirdName,
    };

    const handleSubmit = async (e) => {
        e.preventDefault();

        if (group && student && subject && score) {
            try {
                const token = localStorage.getItem('authToken');

                const response = await fetch('/api/teacheraccount/add-grade', {
                    method: 'POST',
                    headers: {
                        'Authorization': `Bearer ${token}`,
                        'Content-Type': 'application/json',
                    },
                    body: JSON.stringify({
                        studentId: student.value,
                        subjectId: subject.value,
                        value: parseFloat(score),
                    }),
                });

                if (!response.ok) {
                    throw new Error('Failed to add grade');
                }

                alert('Оцінку успішно додано');
                setGrades([
                    ...grades,
                    {
                        group: group.label,
                        student: student.label,
                        subject: subject.label,
                        score,
                    },
                ]);
                setGroup(null);
                setStudent(null);
                setSubject(null);
                setScore('');
            } catch (error) {
                alert('Помилка при додаванні оцінки');
            }
        } else {
            alert('Будь ласка, заповніть всі поля перед призначенням оцінки.');
        }
    };

    const handleDeleteLastLog = async () => {
        if (grades.length > 0) {
            const lastGrade = grades[grades.length - 1];

            try {
                const token = localStorage.getItem('authToken');

                const response = await fetch('/api/teacheraccount/delete-grade', {
                    method: 'DELETE',
                    headers: {
                        'Authorization': `Bearer ${token}`,
                        'Content-Type': 'application/json',
                    },
                    body: JSON.stringify({
                        studentId: students.find(s => s.label === lastGrade.student)?.value,
                        subjectId: subjects.find(s => s.label === lastGrade.subject)?.value,
                    }),
                });

                if (!response.ok) {
                    throw new Error('Failed to delete grade');
                }

                // Видаляємо останню оцінку зі стану
                setGrades(grades.slice(0, grades.length - 1));
                alert('Останню оцінку успішно видалено');
            } catch (error) {
                alert('Помилка при видаленні оцінки');
            }
        } else {
            alert('Список оцінок порожній, немає що видаляти.');
        }
    };

    return (
        <div className="ts-page">
            <main className="ts-content">
                <div className="ts-container">
                    <UsercardTeacher user={user} />
                    <div className="form-container">
                        <form onSubmit={handleSubmit} className="ts-form">
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
                                <label>ПІП студента</label>
                                <Select
                                    options={students}
                                    value={student}
                                    onChange={setStudent}
                                    placeholder="Оберіть студента"
                                />
                            </div>
                            <div className="form-group">
                                <label>Назва дисципліни</label>
                                <Select
                                    options={subjects}
                                    value={subject}
                                    onChange={setSubject}
                                    placeholder="Оберіть дисципліну"
                                />
                            </div>
                            <div className="form-group">
                                <label>Кількість балів</label>
                                <input
                                    type="number"
                                    value={score}
                                    onChange={(e) => setScore(e.target.value)}
                                    placeholder="Введіть кількість балів"
                                    max="60"
                                    min="1"
                                />
                            </div>
                            <button type="submit">Призначити</button>
                        </form>
                    </div>

                    <div className="logs-container">
                        <h2>Список призначених оцінок</h2>
                        {grades.length > 0 ? (
                            <ul>
                                {grades.map((grade, index) => (
                                    <li key={index}>
                                        {`${grade.student} - ${grade.subject} (Група: ${grade.group}, Балів: ${grade.score})`}
                                    </li>
                                ))}
                            </ul>
                        ) : (
                            <p>Оцінок ще немає.</p>
                        )}
                        <button onClick={handleDeleteLastLog} className="delete-log-btn">
                            Видалити останню оцінку
                        </button>
                    </div>
                </div>
            </main>
        </div>
    );
};

export default Teachersetpage;
