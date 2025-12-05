import Navigation from '../../components/Navigation';
import Footer from '../../components/Footer';
import DisplayFeed from '../../components/DisplayFeed';

UserProfilePage.route = {
  path: '/profile/'
};

export default function UserProfilePage() {
  return <>
    <section className="left-column UserProfilePage">
      <Navigation currentPath={UserProfilePage.route.path} />
      <Footer />
    </section>
    <section className="center-column UserProfilePage">
      <div className="main-container">
        <DisplayFeed currentPath={UserProfilePage.route.path} />
      </div>
    </section>
    <section className="right-column UserProfilePage">
        
    </section>
  </>;
}