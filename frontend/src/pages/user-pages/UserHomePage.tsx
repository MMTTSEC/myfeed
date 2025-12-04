import Navigation from '../../components/Navigation';
import WritePost from '../../components/WritePost';
import Footer from '../../components/Footer';

UserHomePage.route = {
  path: '/home'
};

export default function UserHomePage() {
  return <>
    <section className="left-column UserHomePage">
      <Navigation currentPath={UserHomePage.route.path} />
      <Footer />
    </section>
    <section className="center-column UserHomePage">
      <div className="main-container">
        <WritePost />
      </div>
    </section>
    <section className="right-column UserHomePage">

    </section>
  </>;
}