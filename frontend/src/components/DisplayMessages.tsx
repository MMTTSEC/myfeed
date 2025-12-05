import type Message from '../interfaces/Message';
import '../styles/displaymessages.css';

function fetchMessages(): Message[] {
  return [
    {
      id: '1',
      sender: 'Zenty',
      receiver: 'MMTTSEC',
      content: 'Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore.',
      createdAt: '2025-12-04T23:15:00',
    },
    {
      id: '2',
      sender: 'MMTTSEC',
      receiver: 'Zenty',
      content: 'Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore.',
      createdAt: '2025-12-05T09:45:00',
    },
    {
      id: '3',
      sender: 'Zenty',
      receiver: 'MMTTSEC',
      content: 'Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore.',
      createdAt: '2025-12-05T10:30:00',
    },
  ];
}

function MessageCard({ message }: { message: Message }) {
  const formatDate = (dateString: string) => {
    const date = new Date(dateString);
    const datePart = date.toLocaleDateString('en-US', { month: 'short', day: '2-digit', year: 'numeric' });
    const timePart = date.toLocaleTimeString('en-US', { hour: '2-digit', minute: '2-digit', hour12: false });
    return `${datePart} · ${timePart}`;
  };

  return (
    <article className={message.sender === 'Zenty' ? 'message-card your-message' : 'message-card'}>

      <div className="message-header"> 
        <figure className="message-sender-avatar">
          <strong className="message-sender-avatar-initial">{message.sender.charAt(0).toUpperCase()}</strong>
        </figure>
        <span className="message-info">
          {message.sender === 'Zenty'
            ? <strong className="message-sender">YOU</strong>
            : <a href="#"><strong className="message-sender-at">@</strong><strong className="message-sender">{message.sender}</strong></a>
          }
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

export default function DisplayMessages() {
  const messages = fetchMessages();

  if (messages.length === 0) {
    return <div className="messages-empty">No messages available.</div>;
  }

  return (
    <div className="messages-container">
      {messages.map((message) => (
        <MessageCard key={message.id} message={message} />
      ))}
    </div>
  );
}