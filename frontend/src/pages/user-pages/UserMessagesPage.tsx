import Navigation from '../../components/Navigation';
import Footer from '../../components/Footer';
import DisplayMessages from '../../components/DisplayMessages';
import WriteMessage from '../../components/WriteMessage';

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
        <DisplayMessages />
        <WriteMessage />
      </div>
    </section>
    <section className="right-column UserMessagesPage">
  
    </section>
  </>;
}