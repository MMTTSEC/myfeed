import { getAuthHeaders } from './api';

export interface FollowingUser {
  id: number;
  username: string;
}

/**
 * Follow a user
 */
export async function followUser(followeeId: number): Promise<void> {
  const response = await fetch('/api/Follows', {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json',
      ...getAuthHeaders()
    },
    body: JSON.stringify({ followeeId })
  });

  if (!response.ok) {
    const errorText = await response.text();
    throw new Error(errorText || `Failed to follow user: ${response.status}`);
  }
}

/**
 * Unfollow a user
 */
export async function unfollowUser(followeeId: number): Promise<void> {
  const response = await fetch(`/api/Follows?followeeId=${followeeId}`, {
    method: 'DELETE',
    headers: {
      'Content-Type': 'application/json',
      ...getAuthHeaders()
    }
  });

  if (!response.ok) {
    const errorText = await response.text();
    throw new Error(errorText || `Failed to unfollow user: ${response.status}`);
  }
}

/**
 * Get list of users that the current user is following
 */
export async function getFollowing(): Promise<FollowingUser[]> {
  const response = await fetch('/api/Follows/following', {
    method: 'GET',
    headers: {
      'Content-Type': 'application/json',
      ...getAuthHeaders()
    }
  });

  if (!response.ok) {
    const errorText = await response.text();
    throw new Error(errorText || `Failed to get following list: ${response.status}`);
  }

  return await response.json();
}

/**
 * Get list of users that a specific user is following
 */
export async function getFollowingForUser(userId: number): Promise<FollowingUser[]> {
  const response = await fetch(`/api/Follows/following?userId=${userId}`, {
    method: 'GET',
    headers: {
      'Content-Type': 'application/json',
      ...getAuthHeaders()
    }
  });

  if (!response.ok) {
    const errorText = await response.text();
    throw new Error(errorText || `Failed to get following list: ${response.status}`);
  }

  return await response.json();
}

/**
 * Get list of users that follow a specific user (followers)
 */
export async function getFollowers(userId: number): Promise<FollowingUser[]> {
  const response = await fetch(`/api/Follows/followers/${userId}`, {
    method: 'GET',
    headers: {
      'Content-Type': 'application/json',
      ...getAuthHeaders()
    }
  });

  if (!response.ok) {
    const errorText = await response.text();
    throw new Error(errorText || `Failed to get followers list: ${response.status}`);
  }

  return await response.json();
}

/**
 * Check if current user is following another user
 */
export async function checkIfFollowing(followeeId: number): Promise<boolean> {
  const response = await fetch(`/api/Follows/check/${followeeId}`, {
    method: 'GET',
    headers: {
      'Content-Type': 'application/json',
      ...getAuthHeaders()
    }
  });

  if (!response.ok) {
    const errorText = await response.text();
    throw new Error(errorText || `Failed to check follow status: ${response.status}`);
  }

  const data = await response.json();
  return data.isFollowing;
}

