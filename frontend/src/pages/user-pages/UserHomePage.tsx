import Navigation from '../../components/Navigation';

UserHomePage.route = {
  path: '/home'
};

export default function UserHomePage() {
  return <>
    <section className="left-column">
      <figure className="logo"><a href="/home"><span>mF</span></a></figure>
      <Navigation />
    </section>
    <section className="center-column">

    </section>
    <section className="right-column">

    </section>
  </>;
}