import { getAuthHeaders } from './api';

/**
 * Like a post
 */
export async function likePost(postId: number): Promise<void> {
  const response = await fetch('/api/Likes', {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json',
      ...getAuthHeaders()
    },
    body: JSON.stringify({ postId })
  });

  if (!response.ok) {
    const errorText = await response.text();
    throw new Error(errorText || `Failed to like post: ${response.status}`);
  }
}

/**
 * Unlike a post
 */
export async function unlikePost(postId: number): Promise<void> {
  const response = await fetch(`/api/Likes?postId=${postId}`, {
    method: 'DELETE',
    headers: {
      'Content-Type': 'application/json',
      ...getAuthHeaders()
    }
  });

  if (!response.ok) {
    const errorText = await response.text();
    throw new Error(errorText || `Failed to unlike post: ${response.status}`);
  }
}

/**
 * Get like count for a post
 */
export async function getLikeCount(postId: number): Promise<number> {
  const response = await fetch(`/api/Likes/${postId}/count`, {
    method: 'GET',
    headers: {
      'Content-Type': 'application/json',
      ...getAuthHeaders()
    }
  });

  if (!response.ok) {
    const errorText = await response.text();
    throw new Error(errorText || `Failed to get like count: ${response.status}`);
  }

  const data = await response.json();
  return data.likeCount;
}

/**
 * Check if current user has liked a post
 */
export async function checkIfLiked(postId: number): Promise<boolean> {
  const response = await fetch(`/api/Likes/${postId}/check`, {
    method: 'GET',
    headers: {
      'Content-Type': 'application/json',
      ...getAuthHeaders()
    }
  });

  if (!response.ok) {
    const errorText = await response.text();
    throw new Error(errorText || `Failed to check like status: ${response.status}`);
  }

  const data = await response.json();
  return data.hasLiked;
}

