import React from 'react';
import PropTypes from 'prop-types';

const PointCard = ({ grades }) => {
    return (
        <div className="latest-grades">
            <div className="grades-container">
                <div className="grades-header">
                    <span>Предмет</span>
                    <span>Кількість балів</span>
                </div>
                {grades.map((grade) => (
                    <div className="grade-item" key={grade.subject}>
                        <span>{grade.subject}</span>
                        <span>{grade.totalScore}</span>
                    </div>
                ))}
            </div>
        </div>
    );
};

PointCard.propTypes = {
    grades: PropTypes.arrayOf(
        PropTypes.shape({
            subject: PropTypes.string.isRequired,
            totalScore: PropTypes.number.isRequired,
        })
    ).isRequired,
};

export default PointCard;
