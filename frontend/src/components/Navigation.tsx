import '../styles/Navigation.css';
import 'bootstrap-icons/font/bootstrap-icons.css';

interface NavigationProps {
  currentPath: string;
}

export default function Navigation({ currentPath }: NavigationProps) {

  return <>
    <div className="left-column-container">
      <figure className="logo"><a href="/home"><span>mF</span></a></figure>
      <nav className="navigation-menu">
        <ul className="nav-list">

          <li className={`nav-item ${currentPath === "/home" ? "active" : ""}`}>
            <a href="/home"><i className="bi bi-house-door"></i>Home</a>
          </li>

          <li className={`nav-item ${currentPath === "/feed" ? "active" : ""}`}>
            <a href="/feed"><i className="bi bi-people"></i>My Feed</a>
          </li>

          <li className={`nav-item ${currentPath === "/messages" ? "active" : ""}`}>
            <a href="/messages"><i className="bi bi-chat-left"></i>Messages</a>
          </li>

          <li className={`nav-item ${currentPath === "/profile/:userName" ? "active" : ""}`}>
            <a href="/profile/:userName"><i className="bi bi-person"></i>Profile</a>
          </li>

          <li className="nav-item logout-button">
            <a href="#"><i className="bi bi-box-arrow-right"></i>Sign Out</a>
          </li>

        </ul>
      </nav>
    </div>
  </>;
}