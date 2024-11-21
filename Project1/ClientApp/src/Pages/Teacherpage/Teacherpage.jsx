import React, { useState } from 'react';
import Select from 'react-select';
import './Teacherpage.css';

const groups = [
  { value: 'group1', label: 'Група 1' },
  { value: 'group2', label: 'Група 2' },
  { value: 'group3', label: 'Група 3' },
];

const subgroups = [
  { value: 'subgroup1', label: 'Підгрупа 1' },
  { value: 'subgroup2', label: 'Підгрупа 2' },
];

const pairNumbers = [
  { value: '1', label: 'Пара 1' },
  { value: '2', label: 'Пара 2' },
  { value: '3', label: 'Пара 3' },
];

const types = [
  { value: 'lecture', label: 'Лекція' },
  { value: 'lab', label: 'Лабораторна' },
  { value: 'practical', label: 'Практична' },
];

const TeacherPage = () => {
  const [group, setGroup] = useState(null);
  const [subgroup, setSubgroup] = useState(null);
  const [pairNumber, setPairNumber] = useState(null);
  const [type, setType] = useState(null);
  const [subject, setSubject] = useState('');
  const [classroom, setClassroom] = useState('');
  const [teacherName, setTeacherName] = useState('');
  const [logs, setLogs] = useState([]);

  const handleSubmit = (e) => {
    e.preventDefault();

    const logEntry = {
      group: group?.label || 'Не вказано',
      subgroup: subgroup?.label || 'Не вказано',
      pairNumber: pairNumber?.label || 'Не вказано',
      type: type?.label || 'Не вказано',
      subject: subject || 'Не вказано',
      classroom: classroom || 'Не вказано',
      teacherName: teacherName || 'Не вказано',
      timestamp: new Date().toLocaleString(),
    };

    setLogs((prevLogs) => [...prevLogs, logEntry]);

    setGroup(null);
    setSubgroup(null);
    setPairNumber(null);
    setType(null);
    setSubject('');
    setClassroom('');
    setTeacherName('');
  };

  // Функція для видалення останнього лога
  const deleteLastLog = () => {
    setLogs((prevLogs) => prevLogs.slice(0, prevLogs.length - 1));
  };

  return (
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
          <label>Тип пари</label>
          <Select
            options={types}
            value={type}
            onChange={setType}
            placeholder="Оберіть вид заняття"
          />
        </div>
        <div className="form-group">
          <label>Назва пари</label>
          <input
            type="text"
            value={subject}
            onChange={(e) => setSubject(e.target.value)}
            placeholder="Введіть назву предмету"
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
          <label>Ініціали викладача</label>
          <input
            type="text"
            value={teacherName}
            onChange={(e) => setTeacherName(e.target.value)}
            placeholder="Введіть ініціали викладача"
          />
        </div>
        <button type="submit">Призначити пару</button>
      </form>

      <div className="logs">
        <h2>Пар не призначено:</h2>
        {logs.length === 0 ? (
          <p>Призначити пару.</p>
        ) : (
          <ul>
            {logs.map((log, index) => (
              <li key={index}>
                <strong>{log.timestamp}</strong> - Група: {log.group}, Підгрупа: {log.subgroup}, 
                Пара: {log.pairNumber}, Тип: {log.type}, Предмет: {log.subject}, Кабінет: {log.classroom}, Викладач: {log.teacherName}
              </li>
            ))}
          </ul>
        )}
        <button onClick={deleteLastLog} disabled={logs.length === 0}>
          Видалити останню пару
        </button>
      </div>
    </div>
  );
};

export default TeacherPage;
