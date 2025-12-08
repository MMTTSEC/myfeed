import { useState, useEffect } from 'react';
import type NavigationProps from '../interfaces/NavigationProps';
import type Post from '../interfaces/Post';
import { getAllPosts, getFeed, getPostsByUser, mapPostResponseToPost } from '../utils/postsApi';
import { getCurrentUserId } from '../utils/api';
import '../styles/displayfeed.css';
import 'bootstrap-icons/font/bootstrap-icons.css';

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

interface DisplayFeedProps extends NavigationProps {
  refreshTrigger?: number;
}

export default function DisplayFeed({ currentPath, refreshTrigger }: DisplayFeedProps) {
  const [posts, setPosts] = useState<Post[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    async function fetchPosts() {
      setLoading(true);
      setError(null);

      try {
        let postResponses;
        
        if (currentPath === '/home') {
          postResponses = await getAllPosts();
        } else if (currentPath === '/feed') {
          postResponses = await getFeed();
        } else if (currentPath.startsWith('/profile/')) {
          const userId = getCurrentUserId();
          if (!userId) {
            setError('Unable to get user ID');
            setLoading(false);
            return;
          }
          postResponses = await getPostsByUser(userId);
        } else {
          setError('Unknown feed type');
          setLoading(false);
          return;
        }

        const mappedPosts = postResponses.map(mapPostResponseToPost);
        // Sort by createdAt (newest first)
        mappedPosts.sort((a, b) => new Date(b.createdAt).getTime() - new Date(a.createdAt).getTime());
        setPosts(mappedPosts);
      } catch (err) {
        setError(err instanceof Error ? err.message : 'Failed to load posts');
      } finally {
        setLoading(false);
      }
    }

    fetchPosts();
  }, [currentPath, refreshTrigger]);

  if (loading) {
    return <div className="feed-placeholder">Loading posts...</div>;
  }

  if (error) {
    return <div className="feed-placeholder">Error: {error}</div>;
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