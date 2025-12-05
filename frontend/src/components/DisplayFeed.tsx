import type NavigationProps from '../interfaces/NavigationProps';
import type Post from '../interfaces/Post';
import '../styles/displayfeed.css';
import 'bootstrap-icons/font/bootstrap-icons.css';

function getFeedType(path: string): string {
  if (path === '/home') return 'allPosts';
  if (path === '/feed') return 'followedPosts';
  if (path.startsWith('/profile/')) return 'userPosts';
  return '';
}

function fetchPostsOfType(type: string): Post[] {
  switch (type) {
    case 'allPosts':
      return [
        {
          id: '1',
          author: 'Zenty',
          content: 'This is a post',
          createdAt: '2025-12-05',
        },
      ];
    case 'followedPosts':
      return [];
    case 'userPosts':
      return [];
    default:
      return [];
  }
}

function PostCard({ post }: { post: Post }) {
  return (
    <article className="post-card">

      <div className="post-header">
        <figure className="post-author-avatar">
          <strong>{post.author.charAt(0)}</strong>
        </figure>
        <strong>{post.author}</strong>
        <span className="post-date">{post.createdAt}</span>
      </div>

      <div className="post-content">
        {post.content}
      </div>
      
    </article>
  );
}

export default function DisplayFeed({ currentPath }: NavigationProps) {
  const feedType = getFeedType(currentPath);
  const posts = fetchPostsOfType(feedType);

  return <>
  
  </>
}