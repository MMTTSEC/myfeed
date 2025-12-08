import { useState } from 'react';
import { useParams, useLocation } from 'react-router-dom';
import MobileHeader from '../../components/MobileHeader';
import Navigation from '../../components/Navigation';
import Footer from '../../components/Footer';
import HandlePageHeader from '../../components/HandlePageHeader';
import DisplayFeed from '../../components/DisplayFeed';
import DisplayFollowing from '../../components/DisplayFollowing';
import ProtectedRoute from '../../components/ProtectedRoute';

// Route handles both /profile/ (own profile) and /profile/:userId (other user's profile)
UserProfilePage.route = {
  path: '/profile/:userId?'
};

export default function UserProfilePage() {
  const { userId } = useParams<{ userId?: string }>();
  const location = useLocation();
  const [refreshTrigger, setRefreshTrigger] = useState(0);

  const handlePostCreated = () => {
    setRefreshTrigger(prev => prev + 1);
  };

  // Get the current path for navigation
  const currentPath = location.pathname;
  const profileUserId = userId ? parseInt(userId, 10) : undefined;

  return (
    <ProtectedRoute>
      <MobileHeader currentPath={currentPath} />
      <section className="left-column UserProfilePage">
        <Navigation currentPath={currentPath} />
        <Footer />
      </section>
      <section className="center-column UserProfilePage">
        <div className="main-container">
          <HandlePageHeader 
            currentPath={currentPath} 
            onPostCreated={handlePostCreated}
            profileUserId={profileUserId}
          />
          <DisplayFeed 
            currentPath={currentPath} 
            refreshTrigger={refreshTrigger}
            profileUserId={profileUserId}
          />
        </div>
      </section>
      <section className="right-column UserProfilePage">
        <DisplayFollowing />
      </section>
    </ProtectedRoute>
  );
}