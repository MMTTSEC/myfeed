import Navigation from '../../components/Navigation';
import Footer from '../../components/Footer';
import HandlePageHeader from '../../components/HandlePageHeader';
import DisplayFeed from '../../components/DisplayFeed';
import ProtectedRoute from '../../components/ProtectedRoute';


UserHomePage.route = {
  path: '/home'
};

export default function UserHomePage() {
  return (
    <ProtectedRoute>
      <section className="left-column UserHomePage">
        <Navigation currentPath={UserHomePage.route.path} />
        <Footer />
      </section>
      <section className="center-column UserHomePage">
        <div className="main-container">
          <HandlePageHeader currentPath={UserHomePage.route.path} />
          <DisplayFeed currentPath={UserHomePage.route.path} />
        </div>
      </section>
      <section className="right-column UserHomePage">

      </section>
    </ProtectedRoute>
  );
}