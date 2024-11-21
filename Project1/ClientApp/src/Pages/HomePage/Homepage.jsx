import Header from "../../Components/Header/header";
import Sidebar from "../../Components/Sidebar/sidebar";
import Footer from '../../Components/Footer/footer';
import ScheduleCard from "../../Components/Schedulecard/Schedule";
import TeacherPage from '../Teacherpage/Teacherpage'
import "./Homepage.css";

const HomePage = () => {
    return (
        <div className="home-page">
            <Header />
            {/* <TeacherPage /> */}
            <Sidebar />
            <div className="content">
                <main>
                    <div className="blocks">
                        <ScheduleCard
                            time="8:00-9:20"
                            room="каб.244"
                            subject="Інженерія програмного забезпечення"
                            type="Лабораторна робота"
                            groups="КІ-21, КБ-21"
                            teacher="Бойчура М.В."
                        />
                    </div>
                </main>
            </div> 
            <Footer />
        </div>
    );
}

export default HomePage;
