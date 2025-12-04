import Navigation from '../../components/Navigation';
import Footer from '../../components/Footer';
import WritePost from '../../components/WritePost';
import DisplayFeed from '../../components/DisplayFeed';

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
        <DisplayFeed currentPath={UserFeedPage.route.path} />
      </div>
    </section>
    <section className="right-column UserFeedPage">
    
    </section>
  </>;
}