import React, { useState } from 'react';
import Select from 'react-select';
import UserCard from '../../Components/Usercard/Usercard';
import profileimg from './../../img_src/account frame.svg';
import './Teachersetpage.css';

const groups = [
  { value: 'group1', label: 'Група 1' },
  { value: 'group2', label: 'Група 2' },
  { value: 'group3', label: 'Група 3' },
];

const students = [
  { value: 'bachmanyuk', label: 'Бачманюк Д.О.' },
  { value: 'galenyuk', label: 'Галенюк М.П.' },
  { value: 'parypa', label: 'Парипа А.О.' },
];

const subjects = [
  { value: 'ukr_language', label: 'Українська мова' },
  { value: 'math', label: 'Математика' },
  { value: 'programming', label: 'Програмування' },
];

const Teachersetpage = () => {
  const [group, setGroup] = useState(null);
  const [student, setStudent] = useState(null);
  const [subject, setSubject] = useState(null);
  const [score, setScore] = useState('');
  const [grades, setGrades] = useState([]);

  const user = {
    avatar: profileimg,
    firstName: "Вася",
    lastName: "Галенюк",
    middleName: "Олександрович",
    group: "KI-21",
    institute: "КІТІ",
    course: 2,
  };

  const handleSubmit = (e) => {
    e.preventDefault();

    if (group && student && subject && score) {
      const newGrade = {
        group: group.label,
        student: student.label,
        subject: subject.label,
        score,
      };
      setGrades([...grades, newGrade]);
      setGroup(null);
      setStudent(null);
      setSubject(null);
      setScore('');
    } else {
      alert('Будь ласка, заповніть всі поля перед призначенням оцінки.');
    }
  };

  const handleDeleteLastLog = () => {
    if (grades.length > 0) {
      setGrades(grades.slice(0, grades.length - 1)); // Видаляємо останній елемент
    }
  };

  return (
    <div className="ts-page">
      <main className="ts-content">
        <div className="ts-container">
          <UserCard user={user} />
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
                  max="60"  min="1"
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
