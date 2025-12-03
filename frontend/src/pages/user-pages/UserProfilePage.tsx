import Navigation from '../../components/Navigation';

UserProfilePage.route = {
  path: '/profile/:userName'
};

export default function UserProfilePage() {
  return <>
    <section className="left-column UserProfilePage">
      <figure className="logo"><a href="/home"><span>mF</span></a></figure>
      <Navigation />
    </section>
    <section className="center-column UserProfilePage">
        
    </section>
    <section className="right-column UserProfilePage">
        
    </section>
  </>;
}