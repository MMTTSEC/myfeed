import { useState, useEffect } from 'react';
import { Link } from 'react-router-dom';
import { getFollowers, getFollowingForUser, type FollowingUser } from '../utils/followsApi';
import '../styles/followersfollowingmodal.css';
import 'bootstrap-icons/font/bootstrap-icons.css';

interface FollowersFollowingModalProps {
  userId: number;
  username: string;
  isOpen: boolean;
  initialTab: 'followers' | 'following';
  onClose: () => void;
}

export default function FollowersFollowingModal({
  userId,
  username,
  isOpen,
  initialTab,
  onClose
}: FollowersFollowingModalProps) {
  const [activeTab, setActiveTab] = useState<'followers' | 'following'>(initialTab);
  const [followers, setFollowers] = useState<FollowingUser[]>([]);
  const [following, setFollowing] = useState<FollowingUser[]>([]);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    if (isOpen) {
      setActiveTab(initialTab);
      fetchData();
    }
  }, [isOpen, initialTab, userId]);

  const fetchData = async () => {
    setLoading(true);
    setError(null);

    try {
      const [followersData, followingData] = await Promise.all([
        getFollowers(userId),
        getFollowingForUser(userId)
      ]);
      setFollowers(followersData);
      setFollowing(followingData);
    } catch (err) {
      setError(err instanceof Error ? err.message : 'Failed to load data');
    } finally {
      setLoading(false);
    }
  };

  if (!isOpen) return null;

  const currentList = activeTab === 'followers' ? followers : following;
  const count = activeTab === 'followers' ? followers.length : following.length;

  return (
    <div className="modal-overlay" onClick={onClose}>
      <div className="modal-content" onClick={(e) => e.stopPropagation()}>
        <div className="modal-header">
          <h2>{username}</h2>
          <button className="modal-close" onClick={onClose}>
            <i className="bi bi-x-lg"></i>
          </button>
        </div>

        <div className="modal-tabs">
          <button
            className={`modal-tab ${activeTab === 'followers' ? 'active' : ''}`}
            onClick={() => setActiveTab('followers')}
          >
            Followers ({followers.length})
          </button>
          <button
            className={`modal-tab ${activeTab === 'following' ? 'active' : ''}`}
            onClick={() => setActiveTab('following')}
          >
            Following ({following.length})
          </button>
        </div>

        <div className="modal-body">
          {loading ? (
            <div className="modal-loading">Loading...</div>
          ) : error ? (
            <div className="modal-error">Error: {error}</div>
          ) : count === 0 ? (
            <div className="modal-empty">
              {activeTab === 'followers' 
                ? 'No followers yet.' 
                : 'Not following anyone yet.'}
            </div>
          ) : (
            <ul className="modal-list">
              {currentList.map((user) => (
                <li key={user.id} className="modal-list-item">
                  <Link 
                    to={`/profile/${user.id}`} 
                    onClick={onClose}
                    className="modal-user-link"
                  >
                    <span className="modal-user-avatar">
                      {user.username.charAt(0).toUpperCase()}
                    </span>
                    <span className="modal-username">@{user.username}</span>
                  </Link>
                </li>
              ))}
            </ul>
          )}
        </div>
      </div>
    </div>
  );
}

