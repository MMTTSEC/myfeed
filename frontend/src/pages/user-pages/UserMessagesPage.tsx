import Navigation from '../../components/Navigation';
import Footer from '../../components/Footer';

UserMessagesPage.route = {
  path: '/messages'
};

export default function UserMessagesPage() {
  return <>
    <section className="left-column UserMessagesPage">
      <Navigation currentPath={UserMessagesPage.route.path} />
      <Footer />
    </section>
    <section className="center-column UserMessagesPage">
      <div className="main-container">
        
      </div>
    </section>
    <section className="right-column UserMessagesPage">
  
    </section>
  </>;
}