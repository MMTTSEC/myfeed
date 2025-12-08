import { getAuthHeaders } from './api';
import type Post from '../interfaces/Post';

export interface PostResponse {
  id: number;
  authorId: number;
  author: string;
  title: string;
  body: string;
  createdAt: string;
}

export interface CreatePostRequest {
  title: string;
  body: string;
}

/**
 * Create a new post
 */
export async function createPost(title: string, body: string): Promise<void> {
  const response = await fetch('/api/Posts', {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json',
      ...getAuthHeaders()
    },
    body: JSON.stringify({ title, body } as CreatePostRequest)
  });

  if (!response.ok) {
    const errorText = await response.text();
    throw new Error(errorText || `Failed to create post: ${response.status}`);
  }

 
}

/**
 * Get all posts
 */
export async function getAllPosts(): Promise<PostResponse[]> {
  const response = await fetch('/api/Posts/all', {
    method: 'GET',
    headers: {
      'Content-Type': 'application/json',
      ...getAuthHeaders()
    }
  });

  if (!response.ok) {
    const errorText = await response.text();
    throw new Error(errorText || `Failed to get all posts: ${response.status}`);
  }

  return await response.json();
}

/**
 * Get the feed (posts from followed users)
 */
export async function getFeed(): Promise<PostResponse[]> {
  const response = await fetch('/api/Posts/feed', {
    method: 'GET',
    headers: {
      'Content-Type': 'application/json',
      ...getAuthHeaders()
    }
  });

  if (!response.ok) {
    const errorText = await response.text();
    throw new Error(errorText || `Failed to get feed: ${response.status}`);
  }

  return await response.json();
}

/**
 * Get all posts by a specific user
 */
export async function getPostsByUser(userId: number): Promise<PostResponse[]> {
  const response = await fetch(`/api/Posts/user/${userId}`, {
    method: 'GET',
    headers: {
      'Content-Type': 'application/json',
      ...getAuthHeaders()
    }
  });

  if (!response.ok) {
    const errorText = await response.text();
    throw new Error(errorText || `Failed to get posts by user: ${response.status}`);
  }

  return await response.json();
}

/**
 * Get a single post by ID
 */
export async function getPostById(id: number): Promise<PostResponse> {
  const response = await fetch(`/api/Posts/${id}`, {
    method: 'GET',
    headers: {
      'Content-Type': 'application/json',
      ...getAuthHeaders()
    }
  });

  if (!response.ok) {
    if (response.status === 404) {
      throw new Error('Post not found');
    }
    const errorText = await response.text();
    throw new Error(errorText || `Failed to get post: ${response.status}`);
  }

  return await response.json();
}

/**
 * Map backend PostResponse to frontend Post format
 */
export function mapPostResponseToPost(response: PostResponse): Post {
  // Combine title and body into content
  const content = response.title && response.body 
    ? `${response.title}\n${response.body}`.trim()
    : response.body || response.title || '';
  
  return {
    id: response.id.toString(),
    author: response.author,
    authorId: response.authorId,
    createdAt: response.createdAt,
    content: content,
    likesCount: '0' // TODO: Get actual like count from API
  };
}
