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
          content: 'Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex.',
          createdAt: '2025-12-04T22:05:00',
          likesCount: '362',
        },
        {
          id: '2',
          author: 'MMTTSEC',
          content: 'Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex.',
          createdAt: '2025-12-05T16:14:00',
          likesCount: '794',
        },
        {
          id: '3',
          author: 'mycookie5',
          content: 'Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex.',
          createdAt: '2025-12-06T13:26:00',
          likesCount: '173',
        },
        {
          id: '4',
          author: 'Zenty',
          content: 'Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex.',
          createdAt: '2025-12-06T22:30:00',
          likesCount: '999',
        },
      ];
    case 'followedPosts':
      return [
        {
          id: '2',
          author: 'MMTTSEC',
          content: 'Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex.',
          createdAt: '2025-12-05T16:14:00',
          likesCount: '794',
        },
        {
          id: '3',
          author: 'mycookie5',
          content: 'Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex.',
          createdAt: '2025-12-06T13:26:00',
          likesCount: '173',
        },
      ];
    case 'userPosts':
      return [
        {
          id: '1',
          author: 'Zenty',
          content: 'Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex.',
          createdAt: '2025-12-04T22:05:00',
          likesCount: '362',
        },
        {
          id: '4',
          author: 'Zenty',
          content: 'Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex.',
          createdAt: '2025-12-06T22:30:00',
          likesCount: '999',
        },
      ];
    default:
      return [];
  }
}

function PostCard({ post }: { post: Post }) {
  const formatDate = (dateString: string) => {
    const date = new Date(dateString);
    return date.toLocaleDateString('en-US', { month: 'short', day: '2-digit', year: 'numeric' });
  };

  return (
    <article className="post-card">

      <div className="post-header">
        <figure className="post-author-avatar">
          <strong className="post-author-avatar-initial">{post.author.charAt(0).toUpperCase()}</strong>
        </figure>
        <span className="post-info">
          <a href="#"><strong className="post-author-at">@</strong><strong className="post-author">{post.author}</strong></a> · {formatDate(post.createdAt)}
        </span>
      </div>

      <div className="post-content">
        {post.content}
      </div>

      <div className="post-footer">
        <div className="post-likes-container">
          <a
            href="#"
            className={`toggle-likes ${post.author === 'MMTTSEC' ? 'liked' : ''}`}
          >
            <i className="bi bi-heart"></i>
            <span className="post-likes">{post.likesCount}</span>
          </a>
        </div>
      </div>

    </article>
  );
}

export default function DisplayFeed({ currentPath }: NavigationProps) {
  const feedType = getFeedType(currentPath);
  const posts = fetchPostsOfType(feedType);
  const sortedPosts = posts.slice().sort((a, b) => new Date(b.createdAt).getTime() - new Date(a.createdAt).getTime());

  if (!feedType) {
    return <div className="feed-placeholder">Unknown feed type.</div>;
  }

  if (posts.length === 0) {
    return <div className="feed-empty">No posts available.</div>;
  }

  return (
    <div className="feed-container">
      {sortedPosts.map((post) => (
        <PostCard key={post.id} post={post} />
      ))}
    </div>
  );
}