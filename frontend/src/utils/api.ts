export function getAuthHeaders(): { 'Authorization': string } {
  const token = localStorage.getItem('token');
  return {
    'Authorization': `Bearer ${token || ''}`
  };
}


export function getCurrentUserId(): number | null {
  const token = localStorage.getItem('token');
  if (!token) return null;

  try {
    const parts = token.split('.');
    if (parts.length !== 3) return null;

    const payload = parts[1];
    const paddedPayload = payload + '='.repeat((4 - payload.length % 4) % 4);
    const decodedPayload = JSON.parse(atob(paddedPayload));

    const userId = decodedPayload.sub || decodedPayload.user_id || decodedPayload.nameid;
    if (userId) {
      const parsedId = parseInt(userId, 10);
      return isNaN(parsedId) ? null : parsedId;
    }

    return null;
  } catch (error) {
    console.error('Error decoding JWT token:', error);
    return null;
  }
}

export interface UserInfo {
  id: number;
  username: string;
  createdAt: string;
}

/**
 * Get user information by ID
 */
export async function getUserById(userId: number): Promise<UserInfo> {
  const response = await fetch(`/api/Users/${userId}`, {
    method: 'GET',
    headers: {
      'Content-Type': 'application/json',
      ...getAuthHeaders()
    }
  });

  if (!response.ok) {
    const errorText = await response.text();
    throw new Error(errorText || `Failed to get user: ${response.status}`);
  }

  return await response.json();
}

