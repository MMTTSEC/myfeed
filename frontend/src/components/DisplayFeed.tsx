import type NavigationProps from '../interfaces/NavigationProps';
import '../styles/displayfeed.css';
import 'bootstrap-icons/font/bootstrap-icons.css';

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

function fetchPostsOfType(typeOfFeed: string) {
  switch (typeOfFeed) {
    case 'allPosts':
      // Fetch and return all posts
      return [];
      break;
    case 'followedPosts':
      // Fetch and return posts from followed users
      return [];
      break;
    case 'userPosts':
      // Fetch and return posts from a specific user
      return [];
      break;
    default:
      return [];
  }
}

export default function DisplayFeed({ currentPath }: NavigationProps) {
  const typeOfFeed = currentFeedToDisplay({ currentPath });
  const posts = fetchPostsOfType(typeOfFeed);

  return <>
  
  </>
}