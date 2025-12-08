import Navigation from '../../components/Navigation';
import Footer from '../../components/Footer';
import HandlePageHeader from '../../components/HandlePageHeader';
import DisplayFeed from '../../components/DisplayFeed';
import ProtectedRoute from '../../components/ProtectedRoute';

UserFeedPage.route = {
  path: '/feed'
};

export default function UserFeedPage() {
  return (
    <ProtectedRoute>
      <section className="left-column UserFeedPage">
        <Navigation currentPath={UserFeedPage.route.path} />
        <Footer />
      </section>
      <section className="center-column UserFeedPage">
        <div className="main-container">
          <HandlePageHeader currentPath={UserFeedPage.route.path} />
          <DisplayFeed currentPath={UserFeedPage.route.path} />
        </div>
      </section>
      <section className="right-column UserFeedPage">
    
      </section>
    </ProtectedRoute>
  );
}