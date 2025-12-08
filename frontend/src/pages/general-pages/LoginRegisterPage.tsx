import React, { useState, useEffect } from "react";
import { useNavigate } from "react-router-dom";
import { BsPersonFill, BsKeyFill } from "react-icons/bs";
import '../../styles/loginregisterpage.css';

LoginRegisterPage.route = {
  path: "/",
};

export default function LoginRegisterPage() {
  const navigate = useNavigate();
  const [activeTab, setActiveTab] = useState<"login" | "register">("login");

  const [username, setUsername] = useState("");
  const [password, setPassword] = useState("");
  const [confirmPassword, setConfirmPassword] = useState("");

  const [errors, setErrors] = useState<string[]>([]);

  // Redirect if already logged in
  useEffect(() => {
    const token = localStorage.getItem('token');
    if (token) {
      navigate('/home');
    }
  }, [navigate]);

  const validateRequired = (value: string, field: string) =>
    value.trim() === "" ? `${field} is required.` : null;

  const validatePasswordLength = (value: string) =>
    value.length < 8 ? "Password must be at least 8 characters long." : null;

  const validatePasswordLetters = (value: string) =>
    !/[a-z]/.test(value) || !/[A-Z]/.test(value)
      ? "Password must contain both uppercase and lowercase letters."
      : null;

  const validatePasswordMatch = (pass: string, confirm: string) =>
    pass !== confirm ? "Passwords do not match." : null;

  const handleLogin = async (e: React.FormEvent) => {
    e.preventDefault();
    const newErrors: string[] = [];

    const reqUser = validateRequired(username, "Username");
    const reqPass = validateRequired(password, "Password");

    if (reqUser) newErrors.push(reqUser);
    if (reqPass) newErrors.push(reqPass);

    if (newErrors.length > 0) return setErrors(newErrors);

    setErrors([]);

    try {
      const response = await fetch('/api/Users/login', {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify({
          username: username,
          password: password
        })
      });

      if (response.status === 200) {
        // Login successful - extract token and redirect
        const data = await response.json();
        const token = data.token;
        
        if (token) {
          localStorage.setItem('token', token);
          navigate('/home');
        } else {
          setErrors(["Login successful but no token received."]);
        }
      } else if (response.status === 401) {
        // Unauthorized - get error message from API
        const errorData = await response.text();
        setErrors([errorData || "Invalid username or password."]);
      } else {
        setErrors(["An unexpected error occurred. Please try again."]);
      }
    } catch (error) {
      setErrors(["Network error. Please check your connection and try again."]);
    }
  };

  const handleRegister = async (e: React.FormEvent) => {
    e.preventDefault();
    const newErrors: string[] = [];

    const reqUser = validateRequired(username, "Username");
    const reqPass = validateRequired(password, "Password");
    const reqConf = validateRequired(confirmPassword, "Confirm Password");

    const lenPass = validatePasswordLength(password);
    const letPass = validatePasswordLetters(password);
    const matchPass = validatePasswordMatch(password, confirmPassword);

    if (reqUser) newErrors.push(reqUser);
    if (reqPass) newErrors.push(reqPass);
    if (reqConf) newErrors.push(reqConf);
    if (lenPass) newErrors.push(lenPass);
    if (letPass) newErrors.push(letPass);
    if (matchPass) newErrors.push(matchPass);

    if (newErrors.length > 0) return setErrors(newErrors);

    setErrors([]);

    try {
      const response = await fetch('/api/Users/register', {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify({
          username: username,
          password: password
        })
      });

      if (response.status === 201) {
        // Registration successful - switch to login tab
        setActiveTab("login");
        setUsername("");
        setPassword("");
        setConfirmPassword("");
        setErrors([]);
      } else if (response.status === 400) {
        // Bad request - get error message from API
        const errorData = await response.text();
        setErrors([errorData || "Registration failed. Please try again."]);
      } else {
        setErrors(["An unexpected error occurred. Please try again."]);
      }
    } catch (error) {
      setErrors(["Network error. Please check your connection and try again."]);
    }
  };

  return (
    <section className="center-column LoginRegisterPage">
      <div className="main-container">
        {/* Logo */}
        <figure className="logo">
          <span>mF</span>
        </figure>

        {/* Tabs */}
        <div className="tabs">
          <button
            className={activeTab === "login" ? "active" : ""}
            onClick={() => setActiveTab("login")}
          >
            Login
          </button>

          <button
            className={activeTab === "register" ? "active" : ""}
            onClick={() => setActiveTab("register")}
          >
            Register
          </button>
        </div>

        {/* Errors */}
        {errors.length > 0 && (
          <ul className="error-box">
            {errors.map((e, idx) => (
              <li key={idx}>{e}</li>
            ))}
          </ul>
        )}

        {/* LOGIN FORM */}
        {activeTab === "login" && (
          <form className="auth-form login" onSubmit={handleLogin}>
            <label>
              <BsPersonFill />
              Username:
            </label>
            <input
              type="text"
              placeholder="Your Username..."
              value={username}
              onChange={(e) => setUsername(e.target.value)}
            />

            <label>
              <BsKeyFill />
              Password:
            </label>
            <input
              type="password"
              placeholder="Your Password..."
              value={password}
              onChange={(e) => setPassword(e.target.value)}
            />

            <button type="submit">Sign in</button>
          </form>
        )}

        {/* REGISTER FORM */}
        {activeTab === "register" && (
          <form className="auth-form register" onSubmit={handleRegister}>
            <label>
              <BsPersonFill />
              Username:
            </label>
            <input
              type="text"
              placeholder="Your Username..."
              value={username}
              onChange={(e) => setUsername(e.target.value)}
            />

            <label>
              <BsKeyFill />
              Password:
            </label>
            <input
              type="password"
              placeholder="Your Password..."
              value={password}
              onChange={(e) => setPassword(e.target.value)}
            />

            <label>
              <BsKeyFill />
              Confirm Password:
            </label>
            <input
              type="password"
              placeholder="Your Password Again..."
              value={confirmPassword}
              onChange={(e) => setConfirmPassword(e.target.value)}
            />

            <button type="submit">Register</button>
          </form>
        )}
      </div>
    </section>
  );
}
