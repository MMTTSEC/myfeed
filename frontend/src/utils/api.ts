export function getAuthHeaders(): { 'Authorization': string } {
  const token = localStorage.getItem('token');
  return {
    'Authorization': `Bearer ${token || ''}`
  };
}

