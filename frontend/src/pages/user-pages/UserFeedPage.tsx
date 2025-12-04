import Navigation from '../../components/Navigation';
import WritePost from '../../components/WritePost';

UserFeedPage.route = {
  path: '/feed'
};

export default function UserFeedPage() {
  return <>
    <section className="left-column UserFeedPage">
      <figure className="logo"><a href="/home"><span>mF</span></a></figure>
      <Navigation currentPath={ UserFeedPage.route.path } />
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