import { Link, useLocation } from "react-router-dom";
import '../../styles/notfoundpage.css';

NotFoundPage.route = {
  path: '*'
};

export default function NotFoundPage() {
  return <>
    <section className="center-column NotFoundPage">
      <div className="main-container">
        <h1>Not Found: 404</h1>
        <p>
          We are sorry, but there doesn't seem to be any page on this
          site that matches the url:
        </p>
        <h3><strong><i>{useLocation().pathname.slice(1)}</i></strong></h3>
        <p>Please <Link to="/home">visit the start page</Link> instead.</p>
      </div>
    </section>
  </>;
}