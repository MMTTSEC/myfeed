import { useState } from 'react';
import MobileHeader from '../../components/MobileHeader';
import Navigation from '../../components/Navigation';
import Footer from '../../components/Footer';
import HandlePageHeader from '../../components/HandlePageHeader';
import DisplayFeed from '../../components/DisplayFeed';
import ProtectedRoute from '../../components/ProtectedRoute';

UserFeedPage.route = {
  path: '/feed'
};

export default function UserFeedPage() {
  const [refreshTrigger, setRefreshTrigger] = useState(0);

  const handlePostCreated = () => {
    setRefreshTrigger(prev => prev + 1);
  };

  return (
    <ProtectedRoute>
      <MobileHeader currentPath={UserFeedPage.route.path} />
      <section className="left-column UserFeedPage">
        <Navigation currentPath={UserFeedPage.route.path} />
        <Footer />
      </section>
      <section className="center-column UserFeedPage">
        <div className="main-container">
          <HandlePageHeader currentPath={UserFeedPage.route.path} onPostCreated={handlePostCreated} />
          <DisplayFeed currentPath={UserFeedPage.route.path} refreshTrigger={refreshTrigger} />
        </div>
      </section>
      <section className="right-column UserFeedPage">
    
      </section>
    </ProtectedRoute>
  );
}