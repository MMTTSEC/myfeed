import { useState } from 'react';
import MobileHeader from '../../components/MobileHeader';
import Navigation from '../../components/Navigation';
import Footer from '../../components/Footer';
import HandlePageHeader from '../../components/HandlePageHeader';
import DisplayFeed from '../../components/DisplayFeed';
import ProtectedRoute from '../../components/ProtectedRoute';


UserHomePage.route = {
  path: '/home'
};

export default function UserHomePage() {
  const [refreshTrigger, setRefreshTrigger] = useState(0);

  const handlePostCreated = () => {
    setRefreshTrigger(prev => prev + 1);
  };

  return (
    <ProtectedRoute>
      <MobileHeader currentPath={UserHomePage.route.path} />
      <section className="left-column UserHomePage">
        <Navigation currentPath={UserHomePage.route.path} />
        <Footer />
      </section>
      <section className="center-column UserHomePage">
        <div className="main-container">
          <HandlePageHeader currentPath={UserHomePage.route.path} onPostCreated={handlePostCreated} />
          <DisplayFeed currentPath={UserHomePage.route.path} refreshTrigger={refreshTrigger} />
        </div>
      </section>
      <section className="right-column UserHomePage">

      </section>
    </ProtectedRoute>
  );
}