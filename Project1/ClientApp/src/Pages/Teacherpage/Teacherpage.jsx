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

  const handleSubmit = (e) => {
    e.preventDefault();
    console.log({
      group,
      subgroup,
      pairNumber,
      type,
      subject,
      classroom,
      teacherName,
    });
  };

  return (
    <div className="teacher-page-container">
      <p>Призначити пару</p>
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
          <label>Тип предмету</label>
          <Select
            options={types}
            value={type}
            onChange={setType}
            placeholder="Оберіть тип предмету"
          />
        </div>
        <div className="form-group">
          <label>Назва предмету</label>
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
          <label>Ім'я викладача</label>
          <input
            type="text"
            value={teacherName}
            onChange={(e) => setTeacherName(e.target.value)}
            placeholder="Введіть ім'я викладача"
          />
        </div>
        <button type="submit">Призначити</button>
      </form>
    </div>
  );
};

export default TeacherPage;
