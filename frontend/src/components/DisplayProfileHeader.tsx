import '../styles/displayprofileheader.css';
import 'bootstrap-icons/font/bootstrap-icons.css';

export default function DisplayProfileHeader() {

  return (
    <div className="profile-header">
      <div className="profile-header-left">
        <h1 className='profile-name'><span className="profile-name-at">@</span>Zenty</h1>
        <span className="profile-creation-date"><i className="bi bi-calendar"></i>Joined November 2025</span>
      </div>
      <div className="profile-header-right">
        <button className="profile-action button-delete-account">
          <i className="bi bi-exclamation-triangle"></i>
          Delete Account
        </button>
        <button className="profile-action button-message">
          <i className="bi bi-chat"></i>
          Message
        </button>
        <button className="profile-action button-follow">
          <i className="bi bi-person-plus"></i>
          Follow
        </button>
        <button className="profile-action button-unfollow">
          <i className="bi bi-person-dash"></i>
          Unfollow
        </button>
      </div>
    </div>
  );
}