import React from 'react';

const StudentList = ({ students }) => {
  return (
    <div className="grades-table">
      <table>
        <thead>
          <tr>
            <th>Ініціали</th>
            <th>Бали</th>
          </tr>
        </thead>
        <tbody>
          {students.map((student, index) => (
            <tr key={index}>
              <td>{student.initials}</td>
              <td>{student.scores}</td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
};

export default StudentList;
