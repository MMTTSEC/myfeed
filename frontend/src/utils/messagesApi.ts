import { getAuthHeaders } from './api';

export interface MessageResponse {
  id: number;
  senderId: number;
  sender: string;
  receiverId: number;
  receiver: string;
  content: string;
  createdAt: string;
}

export interface SendMessageRequest {
  receiverId: number;
  message: string;
}

/**
 * Send a direct message
 */
export async function sendMessage(receiverId: number, message: string): Promise<void> {
  const response = await fetch('/api/DirectMessages', {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json',
      ...getAuthHeaders()
    },
    body: JSON.stringify({ receiverId, message } as SendMessageRequest)
  });

  if (!response.ok) {
    const errorText = await response.text();
    throw new Error(errorText || `Failed to send message: ${response.status}`);
  }
}

/**
 * Get conversation between current user and another user
 */
export async function getConversation(otherUserId: number): Promise<MessageResponse[]> {
  const response = await fetch(`/api/DirectMessages/conversation/${otherUserId}`, {
    method: 'GET',
    headers: {
      'Content-Type': 'application/json',
      ...getAuthHeaders()
    }
  });

  if (!response.ok) {
    const errorText = await response.text();
    throw new Error(errorText || `Failed to get conversation: ${response.status}`);
  }

  return await response.json();
}

