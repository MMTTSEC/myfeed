import { useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import type { MessageResponse } from '../utils/messagesApi';
import '../styles/notificationtoast.css';

interface NotificationToastProps {
  message: MessageResponse;
  onClose: () => void;
}

export default function NotificationToast({ message, onClose }: NotificationToastProps) {
  const navigate = useNavigate();
  const [isVisible, setIsVisible] = useState(false);

  useEffect(() => {
    // Trigger animation
    setIsVisible(true);
    // Auto-close after 5 seconds
    const timer = setTimeout(() => {
      setIsVisible(false);
      setTimeout(onClose, 300);
    }, 5000);

    return () => clearTimeout(timer);
  }, [onClose]);

  const handleClick = () => {
    // Navigate to the conversation
    const otherUserId = message.senderId;
    navigate(`/messages/${otherUserId}`);
    onClose();
  };

  const avatarInitial = message.sender && message.sender.length > 0 
    ? message.sender.charAt(0).toUpperCase() 
    : '?';

  // Truncate message content if too long
  const truncatedContent = message.content.length > 50 
    ? message.content.substring(0, 50) + '...' 
    : message.content;

  return (
    <div 
      className={`notification-toast ${isVisible ? 'visible' : ''}`}
      onClick={handleClick}
    >
      <div className="notification-avatar">
        <strong>{avatarInitial}</strong>
      </div>
      <div className="notification-content">
        <div className="notification-sender">@{message.sender}</div>
        <div className="notification-message">{truncatedContent}</div>
      </div>
      <button 
        className="notification-close"
        onClick={(e) => {
          e.stopPropagation();
          setIsVisible(false);
          setTimeout(onClose, 300);
        }}
      >
        ×
      </button>
    </div>
  );
}

