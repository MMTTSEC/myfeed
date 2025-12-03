import { Link, useLocation } from "react-router-dom";

NotFoundPage.route = {
  path: '*'
};

export default function NotFoundPage() {
  return <>
    <section className="center-column NotFoundPage">
      <div className="main-container">
        <h2>Not Found: 404</h2>
        <p>
          We are sorry, but there doesn't seem to be any page on this
          site that matches the url:
        </p>
        <p><strong><i>{useLocation().pathname.slice(1)}</i></strong></p>
        <p>Please <Link to="/home">visit the start page</Link> instead.</p>
      </div>
    </section>
  </>;
}