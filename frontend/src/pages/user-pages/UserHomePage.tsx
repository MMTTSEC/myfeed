import Navigation from '../../components/Navigation';

UserHomePage.route = {
  path: '/home'
};

export default function UserHomePage() {
  return <>
    <section className="left-column">
      <Navigation />
    </section>
    <section className="center-column">

    </section>
    <section className="right-column">

    </section>
  </>;
}