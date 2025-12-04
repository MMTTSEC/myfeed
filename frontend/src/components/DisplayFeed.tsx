import '../styles/displayfeed.css';
import 'bootstrap-icons/font/bootstrap-icons.css';

interface NavigationProps {
  currentPath: string;
}

function currentFeedToDisplay({ currentPath }: NavigationProps) {
  switch (currentPath) {
    case '/home':
      return 'allPosts';
    case '/feed':
      return 'followedPosts';
    default:
      if (currentPath.startsWith('/profile/')) {
        return 'userPosts';
      }
      return '';
  }
}

export default function DisplayFeed({ currentPath }: NavigationProps) {
  const typeOfFeed = currentFeedToDisplay({ currentPath });

  return <>
  
  </>
}