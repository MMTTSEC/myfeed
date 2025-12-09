import { useState } from 'react';
import { useParams, useLocation } from 'react-router-dom';
import MobileHeader from '../../components/MobileHeader';
import Navigation from '../../components/Navigation';
import Footer from '../../components/Footer';
import DisplayMessages from '../../components/DisplayMessages';
import WriteMessage from '../../components/WriteMessage';
import DisplayConversations from '../../components/DisplayConversations';
import ProtectedRoute from '../../components/ProtectedRoute';
import { useChatNotifications } from '../../hooks/useChatNotifications';

UserMessagesPage.route = {
  path: '/messages/:userId?'
};

export default function UserMessagesPage() {
  const { userId } = useParams<{ userId?: string }>();
  const location = useLocation();
  const currentPath = location.pathname;
  const selectedUserId = userId ? parseInt(userId, 10) : undefined;
  const [refreshTrigger, setRefreshTrigger] = useState(0);

  // Get connection for DisplayMessages (notifications are handled globally in App.tsx)
  const { connection } = useChatNotifications(selectedUserId);

  const handleMessageSent = () => {
    setRefreshTrigger(prev => prev + 1);
  };

  return (
    <ProtectedRoute>
      <MobileHeader currentPath={currentPath} />
      <section className="left-column UserMessagesPage">
        <Navigation currentPath={currentPath} />
        <Footer />
      </section>
      <section className="center-column UserMessagesPage">
        <div className="main-container">
          <DisplayMessages selectedUserId={selectedUserId} refreshTrigger={refreshTrigger} connection={connection} />
          {selectedUserId && <WriteMessage receiverId={selectedUserId} onMessageSent={handleMessageSent} />}
        </div>
      </section>
      <section className="right-column UserMessagesPage">
        <DisplayConversations selectedUserId={selectedUserId} />
      </section>
    </ProtectedRoute>
  );
}