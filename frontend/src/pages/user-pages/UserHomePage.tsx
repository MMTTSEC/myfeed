import Navigation from '../../components/Navigation';

UserHomePage.route = {
  path: '/home'
};

export default function UserHomePage() {
  return <>
    <section className="left-column UserHomePage">
      <figure className="logo"><a href="/home"><span>mF</span></a></figure>
      <Navigation currentPath={ UserHomePage.route.path } />
    </section>
    <section className="center-column UserHomePage">

    </section>
    <section className="right-column UserHomePage">

    </section>
  </>;
}