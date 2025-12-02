import { useLocation } from 'react-router-dom';
import Main from './partials/Main.tsx';

export default function App() {

  useLocation();
  window.scrollTo({ top: 0, left: 0, behavior: 'instant' });

  return <>
      <Main />
  </>;
};