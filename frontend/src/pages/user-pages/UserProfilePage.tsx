import Navigation from '../../components/Navigation';
import Footer from '../../components/Footer';

UserProfilePage.route = {
  path: '/profile/:userName'
};

export default function UserProfilePage() {
  return <>
    <section className="left-column UserProfilePage">
      <Navigation currentPath={UserProfilePage.route.path} />
      <Footer />
    </section>
    <section className="center-column UserProfilePage">
      <div className="main-container">
        
      </div>
    </section>
    <section className="right-column UserProfilePage">
        
    </section>
  </>;
}