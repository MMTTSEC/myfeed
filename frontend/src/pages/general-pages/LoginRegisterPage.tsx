import React, { useState } from "react";
import { BsPersonFill, BsKeyFill } from "react-icons/bs";
import { MdEmail } from "react-icons/md";
import '../../styles/loginregisterpage.css';

LoginRegisterPage.route = {
  path: "/",
};

export default function LoginRegisterPage() {
  const [activeTab, setActiveTab] = useState<"login" | "register">("login");

  const [username, setUsername] = useState("");
  const [email, setEmail] = useState(""); 
  const [password, setPassword] = useState("");
  const [confirmPassword, setConfirmPassword] = useState("");

  const [errors, setErrors] = useState<string[]>([]);

  const validateRequired = (value: string, field: string) =>
    value.trim() === "" ? `${field} is required.` : null;

  const validateEmailFormat = (value: string) =>
    !/^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(value) ? "Invalid email format." : null;

  const validatePasswordLength = (value: string) =>
    value.length < 8 ? "Password must be at least 8 characters long." : null;

  const validatePasswordLetters = (value: string) =>
    !/[a-z]/.test(value) || !/[A-Z]/.test(value)
      ? "Password must contain both uppercase and lowercase letters."
      : null;

  const validatePasswordMatch = (pass: string, confirm: string) =>
    pass !== confirm ? "Passwords do not match." : null;

  const handleLogin = (e: React.FormEvent) => {
    e.preventDefault();
    const newErrors: string[] = [];

    const reqUser = validateRequired(username, "Username");
    const reqPass = validateRequired(password, "Password");

    if (reqUser) newErrors.push(reqUser);
    if (reqPass) newErrors.push(reqPass);

    if (newErrors.length > 0) return setErrors(newErrors);

    setErrors([]);
    console.log("LOGIN SUCCESS", { username, password });
  };

  const handleRegister = (e: React.FormEvent) => {
    e.preventDefault();
    const newErrors: string[] = [];

    const reqUser = validateRequired(username, "Username");
    const reqEmail = validateRequired(email, "Email");
    const reqPass = validateRequired(password, "Password");
    const reqConf = validateRequired(confirmPassword, "Confirm Password");

    const emailFormat = validateEmailFormat(email);
    const lenPass = validatePasswordLength(password);
    const letPass = validatePasswordLetters(password);
    const matchPass = validatePasswordMatch(password, confirmPassword);

    if (reqUser) newErrors.push(reqUser);
    if (reqEmail) newErrors.push(reqEmail);
    if (reqPass) newErrors.push(reqPass);
    if (reqConf) newErrors.push(reqConf);
    if (emailFormat) newErrors.push(emailFormat);
    if (lenPass) newErrors.push(lenPass);
    if (letPass) newErrors.push(letPass);
    if (matchPass) newErrors.push(matchPass);

    if (newErrors.length > 0) return setErrors(newErrors);

    setErrors([]);
    console.log("REGISTER SUCCESS", { username, email, password });
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
              <MdEmail />
              Email:
            </label>
            <input
              type="email"
              placeholder="Your Email..."
              value={email}
              onChange={(e) => setEmail(e.target.value)}
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
