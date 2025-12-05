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
          createdAt: '2025-12-04',
        },
        {
          id: '2',
          author: 'MMTTSEC',
          content: 'This is also a post',
          createdAt: '2025-12-05',
        },
        {
          id: '3',
          author: 'mycookie5',
          content: 'Is this a post?',
          createdAt: '2025-12-05',
        },
        {
          id: '4',
          author: 'Zenty',
          content: 'Even this is a post',
          createdAt: '2025-12-05',
        },
      ];
    case 'followedPosts':
      return [
        {
          id: '2',
          author: 'MMTTSEC',
          content: 'This is also a post',
          createdAt: '2025-12-05',
        },
        {
          id: '3',
          author: 'mycookie5',
          content: 'Is this a post?',
          createdAt: '2025-12-05',
        },
      ];
    case 'userPosts':
      return [
        {
          id: '1',
          author: 'Zenty',
          content: 'This is a post',
          createdAt: '2025-12-04',
        },
        {
          id: '3',
          author: 'Zenty',
          content: 'Even this is a post',
          createdAt: '2025-12-05',
        },
      ];
    default:
      return [];
  }
}

function PostCard({ post }: { post: Post }) {
  return (
    <article className="post-card">

      <div className="post-header">
        <figure className="post-author-avatar">
          <strong>{post.author.charAt(0).toUpperCase()}</strong>
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

  if (!feedType) {
    return <div className="feed-placeholder">Unknown feed type.</div>;
  }

  if (posts.length === 0) {
    return <div className="feed-empty">No posts available.</div>;
  }

  return (
    <div className="feed-container">
      {posts.map((post) => (
        <PostCard key={post.id} post={post} />
      ))}
    </div>
  );
}