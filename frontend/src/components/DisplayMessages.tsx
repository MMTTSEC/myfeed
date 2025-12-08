import { useState, useEffect, useRef } from 'react';
import { Link } from 'react-router-dom';
import { getConversation, type MessageResponse } from '../utils/messagesApi';
import { getCurrentUserId } from '../utils/api';
import '../styles/displaymessages.css';

interface MessageCardProps {
  message: MessageResponse;
  currentUserId: number;
}

function MessageCard({ message, currentUserId }: MessageCardProps) {
  const formatDate = (dateString: string) => {
    const date = new Date(dateString);
    const datePart = date.toLocaleDateString('en-US', { month: 'short', day: '2-digit', year: 'numeric' });
    const timePart = date.toLocaleTimeString('en-US', { hour: '2-digit', minute: '2-digit', hour12: false });
    return `${datePart} · ${timePart}`;
  };

  
  const senderId = Number(message.senderId);
  const userId = Number(currentUserId);
  const isOwnMessage = senderId === userId && !isNaN(senderId) && !isNaN(userId);
  

  if (import.meta.env.DEV) {
    console.log(`Message ${message.id}: senderId=${senderId}, currentUserId=${userId}, isOwnMessage=${isOwnMessage}, sender="${message.sender}"`);
  }
  
  const avatarInitial = message.sender && message.sender.length > 0 
    ? message.sender.charAt(0).toUpperCase() 
    : '?';

  return (
    <article className={isOwnMessage ? 'message-card your-message' : 'message-card'}>
      <div className="message-header"> 
        <figure className="message-sender-avatar">
          <strong className="message-sender-avatar-initial">{avatarInitial}</strong>
        </figure>
        <span className="message-info">
          {isOwnMessage ? (
            <strong className="message-sender">YOU</strong>
          ) : (
            <Link to={`/profile/${message.senderId}`}>
              <strong className="message-sender-at">@</strong><strong className="message-sender">{message.sender}</strong>
            </Link>
          )}
        </span>
      </div>

      <div className="message-content">
        {message.content}
      </div>

      <div className="message-footer"> 
        <span className="message-info">
          {formatDate(message.createdAt)}
        </span>
      </div>
    </article>
  );
}

interface DisplayMessagesProps {
  selectedUserId?: number;
  refreshTrigger?: number;
}

export default function DisplayMessages({ selectedUserId, refreshTrigger }: DisplayMessagesProps) {
  const [messages, setMessages] = useState<MessageResponse[]>([]);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);
  const messagesEndRef = useRef<HTMLDivElement>(null);
  const currentUserId = getCurrentUserId();

  useEffect(() => {
    async function fetchMessages() {
      if (!selectedUserId || !currentUserId) {
        setMessages([]);
        return;
      }

      setLoading(true);
      setError(null);

      try {
        const conversation = await getConversation(selectedUserId);
        const sortedMessages = [...conversation].sort((a, b) => {
          const dateA = new Date(a.createdAt).getTime();
          const dateB = new Date(b.createdAt).getTime();
          if (dateA !== dateB) {
            return dateA - dateB;
          }
         
          return a.id - b.id;
        });
        setMessages(sortedMessages);
      } catch (err) {
        setError(err instanceof Error ? err.message : 'Failed to load messages');
      } finally {
        setLoading(false);
      }
    }

    fetchMessages();
  }, [selectedUserId, currentUserId, refreshTrigger]);

  // Scroll to bottom when messages change
  useEffect(() => {
    messagesEndRef.current?.scrollIntoView({ behavior: 'smooth' });
  }, [messages]);

  if (!selectedUserId) {
    return (
      <div className="messages-empty">
        Select a conversation from the list to view messages.
      </div>
    );
  }

  if (loading) {
    return <div className="messages-empty">Loading messages...</div>;
  }

  if (error) {
    return <div className="messages-empty">Error: {error}</div>;
  }

  if (messages.length === 0) {
    return <div className="messages-empty">No messages yet. Start the conversation!</div>;
  }

  if (!currentUserId) {
    return <div className="messages-empty">Unable to get user ID</div>;
  }

  return (
    <div className="messages-container">
      {messages.map((message, index) => (
        <MessageCard 
          key={`msg-${message.id}-${index}`} 
          message={message} 
          currentUserId={currentUserId} 
        />
      ))}
      <div ref={messagesEndRef} />
    </div>
  );
}