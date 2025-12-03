import Navigation from '../../components/Navigation';

UserFeedPage.route = {
  path: '/feed'
};

export default function UserFeedPage() {
  return <>
    <section className="left-column UserFeedPage">
      <figure className="logo"><a href="/home"><span>mF</span></a></figure>
      <Navigation />
    </section>
    <section className="center-column UserFeedPage">
    
    </section>
    <section className="right-column UserFeedPage">
    
    </section>
  </>;
}