import type Route from './interfaces/Route.ts';
import { createElement } from 'react';

// page components
// general pages
import NotFoundPage from './pages/general-pages/NotFoundPage.tsx';
import LoginRegisterPage from './pages/general-pages/LoginRegisterPage.tsx';

// user pages
import UserHomePage from './pages/user-pages/UserHomePage.tsx';
import UserFeedPage from './pages/user-pages/UserFeedPage.tsx';
import UserMessagesPage from './pages/user-pages/UserMessagesPage.tsx';
import UserProfilePage from './pages/user-pages/UserProfilePage.tsx';

export default [
  NotFoundPage,
  LoginRegisterPage,
  UserHomePage,
  UserFeedPage,
  UserMessagesPage,
  UserProfilePage
]
  // map the route property of each page component to a Route & sort by index
  .map(x => (({ element: createElement(x), ...x.route }) as Route))
  .sort((a, b) => (a.index || 0) - (b.index || 0));