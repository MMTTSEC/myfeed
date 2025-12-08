import { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import { getCurrentUserId, getUserById, type UserInfo } from '../utils/api';
import { checkIfFollowing, followUser, unfollowUser, getFollowers, getFollowingForUser } from '../utils/followsApi';
import FollowersFollowingModal from './FollowersFollowingModal';
import '../styles/displayprofileheader.css';
import 'bootstrap-icons/font/bootstrap-icons.css';

interface DisplayProfileHeaderProps {
  profileUserId?: number;
  profileUsername?: string;
  profileCreatedAt?: string;
}

export default function DisplayProfileHeader({ 
  profileUserId, 
  profileUsername, 
  profileCreatedAt 
}: DisplayProfileHeaderProps) {
  const navigate = useNavigate();
  const currentUserId = getCurrentUserId();
  const [userInfo, setUserInfo] = useState<UserInfo | null>(null);
  
  // Determine which user's profile to show
  const targetUserId = profileUserId || currentUserId;
  const isOwnProfile = targetUserId === currentUserId;

  // Fetch user info if not provided
  useEffect(() => {
    async function fetchUserInfo() {
      if (profileUsername && profileCreatedAt) {
        // Info already provided
        if (profileUserId) {
          setUserInfo({ id: profileUserId, username: profileUsername, createdAt: profileCreatedAt });
        }
        return;
      }

      if (!targetUserId) {
        return;
      }

      try {
        const info = await getUserById(targetUserId);
        setUserInfo(info);
      } catch (error) {
        console.error('Failed to fetch user info:', error);
      }
    }

    fetchUserInfo();
  }, [profileUserId, profileUsername, profileCreatedAt, targetUserId]);
  
  const [isFollowing, setIsFollowing] = useState(false);
  const [isLoading, setIsLoading] = useState(true);
  const [isToggling, setIsToggling] = useState(false);
  const [followersCount, setFollowersCount] = useState<number | null>(null);
  const [followingCount, setFollowingCount] = useState<number | null>(null);
  const [modalOpen, setModalOpen] = useState(false);
  const [modalTab, setModalTab] = useState<'followers' | 'following'>('followers');

  useEffect(() => {
    async function checkFollowStatus() {
      if (isOwnProfile || !targetUserId || !currentUserId) {
        setIsLoading(false);
        return;
      }

      try {
        const following = await checkIfFollowing(targetUserId);
        setIsFollowing(following);
      } catch (error) {
        console.error('Failed to check follow status:', error);
      } finally {
        setIsLoading(false);
      }
    }

    checkFollowStatus();
  }, [targetUserId, currentUserId, isOwnProfile]);

  // Fetch follower and following counts
  useEffect(() => {
    async function fetchCounts() {
      if (!targetUserId) return;

      try {
        const [followers, following] = await Promise.all([
          getFollowers(targetUserId),
          getFollowingForUser(targetUserId)
        ]);
        setFollowersCount(followers.length);
        setFollowingCount(following.length);
      } catch (error) {
        console.error('Failed to fetch follower/following counts:', error);
      }
    }

    fetchCounts();
  }, [targetUserId]);

  const handleFollowToggle = async () => {
    if (!targetUserId || isToggling || isLoading || isOwnProfile) return;

    const previousFollowing = isFollowing;
    setIsFollowing(!previousFollowing);
    setIsToggling(true);

    try {
      if (previousFollowing) {
        await unfollowUser(targetUserId);
      } else {
        await followUser(targetUserId);
      }
    } catch (error) {
      // Revert on error
      setIsFollowing(previousFollowing);
      console.error('Failed to toggle follow:', error);
    } finally {
      setIsToggling(false);
    }
  };

  const handleMessage = () => {
    if (targetUserId) {
      navigate(`/messages/${targetUserId}`);
    } else {
      navigate('/messages');
    }
  };

  const handleDeleteAccount = () => {
    // TODO: Implement delete account functionality
    if (window.confirm('Are you sure you want to delete your account? This action cannot be undone.')) {
      console.log('Delete account not yet implemented');
    }
  };

  const formatDate = (dateString?: string) => {
    if (!dateString) return 'Unknown';
    const date = new Date(dateString);
    return date.toLocaleDateString('en-US', { month: 'long', year: 'numeric' });
  };

  const displayUsername = userInfo?.username || profileUsername || 'Loading...';
  const displayDate = userInfo?.createdAt ? formatDate(userInfo.createdAt) : (profileCreatedAt ? formatDate(profileCreatedAt) : '');

  const handleOpenModal = (tab: 'followers' | 'following') => {
    setModalTab(tab);
    setModalOpen(true);
  };

  const handleCloseModal = () => {
    setModalOpen(false);
    // Refresh counts when modal closes in case they changed
    if (targetUserId) {
      Promise.all([
        getFollowers(targetUserId),
        getFollowingForUser(targetUserId)
      ]).then(([followers, following]) => {
        setFollowersCount(followers.length);
        setFollowingCount(following.length);
      }).catch(error => {
        console.error('Failed to refresh counts:', error);
      });
    }
  };

  return (
    <>
      <div className="profile-header">
        <div className="profile-header-left">
          <h1 className='profile-name'><span className="profile-name-at">@</span>{displayUsername}</h1>
          {displayDate && (
            <span className="profile-creation-date">
              <i className="bi bi-calendar"></i>Joined {displayDate}
            </span>
          )}
          <div className="profile-stats">
            <button 
              className="profile-stat-button"
              onClick={() => handleOpenModal('followers')}
            >
              <strong>{followersCount !== null ? followersCount : '...'}</strong> Followers
            </button>
            <button 
              className="profile-stat-button"
              onClick={() => handleOpenModal('following')}
            >
              <strong>{followingCount !== null ? followingCount : '...'}</strong> Following
            </button>
          </div>
        </div>
      <div className="profile-header-right">
        {isOwnProfile ? (
          <>
            <button 
              className="profile-action button-delete-account"
              onClick={handleDeleteAccount}
            >
              <i className="bi bi-exclamation-triangle"></i>
              Delete Account
            </button>
          </>
        ) : (
          <>
            <button 
              className="profile-action button-message"
              onClick={handleMessage}
            >
              <i className="bi bi-chat"></i>
              Message
            </button>
            {!isLoading && (
              <button 
                className={`profile-action ${isFollowing ? 'button-unfollow' : 'button-follow'}`}
                onClick={handleFollowToggle}
                disabled={isToggling}
              >
                <i className={`bi ${isFollowing ? 'bi-person-dash' : 'bi-person-plus'}`}></i>
                {isToggling ? '...' : (isFollowing ? 'Unfollow' : 'Follow')}
              </button>
            )}
          </>
        )}
      </div>
    </div>
    {targetUserId && (
      <FollowersFollowingModal
        userId={targetUserId}
        username={displayUsername}
        isOpen={modalOpen}
        initialTab={modalTab}
        onClose={handleCloseModal}
      />
    )}
    </>
  );
}