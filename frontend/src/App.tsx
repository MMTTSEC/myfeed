import { useLocation } from 'react-router-dom';
import Main from './partials/Main.tsx';
import NotificationToast from './components/NotificationToast';
import { useChatNotifications } from './hooks/useChatNotifications';
import './styles/notificationtoast.css';

export default function App() {
  const location = useLocation();
  window.scrollTo({ top: 0, left: 0, behavior: 'instant' });

  
  const messagesMatch = location.pathname.match(/^\/messages\/(\d+)$/);
  const activeConversationUserId = messagesMatch ? parseInt(messagesMatch[1], 10) : undefined;

  // Setup global chat notifications
  const { notifications, removeNotification } = useChatNotifications(activeConversationUserId);

  return <>
      <Main />
      
      {/* Global Notification Container */}
      <div className="notification-container">
        {notifications.map((notification) => (
          <NotificationToast
            key={notification.id}
            message={notification}
            onClose={() => removeNotification(notification.id)}
          />
        ))}
      </div>
  </>;
};