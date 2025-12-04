import Navigation from '../../components/Navigation';

UserMessagesPage.route = {
  path: '/messages'
};

export default function UserMessagesPage() {
  return <>
    <section className="left-column UserMessagesPage">
      <figure className="logo"><a href="/home"><span>mF</span></a></figure>
      <Navigation currentPath={ UserMessagesPage.route.path }/>
    </section>
    <section className="center-column UserMessagesPage">
      <div className="main-container">
        
      </div>
    </section>
    <section className="right-column UserMessagesPage">
  
    </section>
  </>;
}