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
  return (
    <ProtectedRoute>
      <section className="left-column UserProfilePage">
        <Navigation currentPath={UserProfilePage.route.path} />
        <Footer />
      </section>
      <section className="center-column UserProfilePage">
        <div className="main-container">
          <HandlePageHeader currentPath={UserProfilePage.route.path} />
          <DisplayFeed currentPath={UserProfilePage.route.path} />
        </div>
      </section>
      <section className="right-column UserProfilePage">
        <DisplayFollowing />
      </section>
    </ProtectedRoute>
  );
}