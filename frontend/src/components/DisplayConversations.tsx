import { useState, useEffect } from 'react';
import { Link, useNavigate } from 'react-router-dom';
import { getFollowing, type FollowingUser } from '../utils/followsApi';
import '../styles/displayconversations.css';

interface DisplayConversationsProps {
  selectedUserId?: number;
}

export default function DisplayConversations({ selectedUserId }: DisplayConversationsProps) {
  const navigate = useNavigate();
  const [following, setFollowing] = useState<FollowingUser[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    async function fetchFollowing() {
      setLoading(true);
      setError(null);

      try {
        const followingList = await getFollowing();
        setFollowing(followingList);
      } catch (err) {
        setError(err instanceof Error ? err.message : 'Failed to load conversations');
      } finally {
        setLoading(false);
      }
    }

    fetchFollowing();
  }, []);

  const handleUserClick = (userId: number) => {
    navigate(`/messages/${userId}`);
  };

  if (loading) {
    return (
      <div className="conversations-container">
        <h2>Conversations</h2>
        <p>Loading...</p>
      </div>
    );
  }

  if (error) {
    return (
      <div className="conversations-container">
        <h2>Conversations</h2>
        <p style={{ color: 'red' }}>Error: {error}</p>
      </div>
    );
  }

  return (
    <div className="conversations-container">
      <h2>Conversations ({following.length})</h2>
      {following.length === 0 ? (
        <p>No conversations available. Follow users to start messaging.</p>
      ) : (
        <ul>
          {following.map((user) => (
            <li
              key={user.id}
              className={selectedUserId === user.id ? 'active' : ''}
            >
              <a
                href="#"
                onClick={(e) => {
                  e.preventDefault();
                  handleUserClick(user.id);
                }}
              >
                @{user.username}
              </a>
            </li>
          ))}
        </ul>
      )}
    </div>
  );
}