import React, { useEffect } from "react";
import { useNavigate } from "react-router-dom";

const LoginSuccess = () => {
    const navigate = useNavigate();

    useEffect(() => {
        const params = new URLSearchParams(window.location.search);
        const token = params.get("token");
        if (token) {
            localStorage.setItem("authToken", token); // Зберігаємо токен
            navigate("/main"); // Повертаємося на головну сторінку
        }
    }, [navigate]);

    return <h1>Logging in...</h1>;
};

export default LoginSuccess;