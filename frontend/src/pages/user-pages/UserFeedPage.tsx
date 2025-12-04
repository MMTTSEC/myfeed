import Navigation from '../../components/Navigation';
import WritePost from '../../components/WritePost';
import Footer from '../../components/Footer';

UserFeedPage.route = {
  path: '/feed'
};

export default function UserFeedPage() {
  return <>
    <section className="left-column UserFeedPage">
      <Navigation currentPath={UserFeedPage.route.path} />
      <Footer />
    </section>
    <section className="center-column UserFeedPage">
      <div className="main-container">
        <WritePost />
      </div>
    </section>
    <section className="right-column UserFeedPage">
    
    </section>
  </>;
}