import { useState, useEffect } from 'react';
import { Link } from 'react-router-dom';
import { getFollowing, type FollowingUser } from '../utils/followsApi';
import '../styles/displayfollowing.css';

export default function DisplayFollowing() {
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
        setError(err instanceof Error ? err.message : 'Failed to load following list');
      } finally {
        setLoading(false);
      }
    }

    fetchFollowing();
  }, []);

  if (loading) {
    return (
      <div className="following-container">
        <h2>Following</h2>
        <p>Loading...</p>
      </div>
    );
  }

  if (error) {
    return (
      <div className="following-container">
        <h2>Following</h2>
        <p style={{ color: 'red' }}>Error: {error}</p>
      </div>
    );
  }

  return (
    <div className="following-container">
      <h2>Following ({following.length})</h2>
      {following.length === 0 ? (
        <p>Not following anyone yet.</p>
      ) : (
        <ul>
          {following.map((user) => (
            <li key={user.id}>
              <Link to={`/profile/${user.id}`}>@{user.username}</Link>
            </li>
          ))}
        </ul>
      )}
    </div>
  );
}