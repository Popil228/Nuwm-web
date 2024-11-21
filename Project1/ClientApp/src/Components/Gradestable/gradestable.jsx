import React from "react";
import "./gradestable.css";

const GradesTable = ({ grades }) => {
  return (
    <div className="grades-table">
      <h2>Список оцінок</h2>
      <table>
        <thead>
          <tr>
            <th>Предмет</th>
            <th>Оцінка</th>
          </tr>
        </thead>
        <tbody>
          {grades.map((grade, index) => (
            <tr key={index}>
              <td>{grade.subject}</td>
              <td>{grade.totalScore}</td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
};

export default GradesTable;
