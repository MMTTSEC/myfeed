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

