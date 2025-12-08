import { useState } from 'react';
import MobileHeader from '../../components/MobileHeader';
import Navigation from '../../components/Navigation';
import Footer from '../../components/Footer';
import HandlePageHeader from '../../components/HandlePageHeader';
import DisplayFeed from '../../components/DisplayFeed';
import DisplayFollowing from '../../components/DisplayFollowing';
import ProtectedRoute from '../../components/ProtectedRoute';

UserProfilePage.route = {
  path: '/profile/'
};

export default function UserProfilePage() {
  const [refreshTrigger, setRefreshTrigger] = useState(0);

  const handlePostCreated = () => {
    setRefreshTrigger(prev => prev + 1);
  };

  return (
    <ProtectedRoute>
      <MobileHeader currentPath={UserProfilePage.route.path} />
      <section className="left-column UserProfilePage">
        <Navigation currentPath={UserProfilePage.route.path} />
        <Footer />
      </section>
      <section className="center-column UserProfilePage">
        <div className="main-container">
          <HandlePageHeader currentPath={UserProfilePage.route.path} onPostCreated={handlePostCreated} />
          <DisplayFeed currentPath={UserProfilePage.route.path} refreshTrigger={refreshTrigger} />
        </div>
      </section>
      <section className="right-column UserProfilePage">
        <DisplayFollowing />
      </section>
    </ProtectedRoute>
  );
}