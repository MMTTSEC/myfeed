import { useState, useRef, useEffect } from "react";
import { sendMessage } from '../utils/messagesApi';
import '../styles/writemessage.css';
import 'bootstrap-icons/font/bootstrap-icons.css';

const MAX_CHARS = 110;

function sanitizePaste(input: string): string {
  return input
    .replace(/\r?\n|\r/g, " ")  // remove all line breaks
    .replace(/\t/g, " ")        // replace tabs with spaces
    .replace(/ +/g, " ")        // collapse multiple spaces
    .trim();                    // remove leading/trailing spaces
}

function limitLength(text: string, max: number): string {
  return text.slice(0, max);
}

function useAutoResize(textAreaRef: React.RefObject<HTMLTextAreaElement | null>, value: string) {
  useEffect(() => {
    const textArea = textAreaRef.current;
    if (!textArea) return;

    textArea.style.height = "auto";
    textArea.style.height = textArea.scrollHeight + "px";
  }, [value, textAreaRef]);
}

interface WriteMessageProps {
  receiverId: number;
  onMessageSent?: () => void;
}

export default function WriteMessage({ receiverId, onMessageSent }: WriteMessageProps) {
  const [text, setText] = useState("");
  const [isSubmitting, setIsSubmitting] = useState(false);
  const [error, setError] = useState<string | null>(null);
  const textAreaRef = useRef<HTMLTextAreaElement | null>(null);

  useAutoResize(textAreaRef, text);

  function handleChange(event: React.ChangeEvent<HTMLTextAreaElement>) {
    const rawText = event.target.value;
    const limitedText = limitLength(rawText, MAX_CHARS);  // no sanitize
    setText(limitedText);
  }

  function handlePaste(event: React.ClipboardEvent<HTMLTextAreaElement>) {
    event.preventDefault();

    const pastedText = event.clipboardData.getData("text");
    const cleanedText = limitLength(sanitizePaste(pastedText), MAX_CHARS - text.length);

    setText(text + cleanedText);
  }

  async function handleSubmit(event: React.FormEvent<HTMLFormElement>) {
    event.preventDefault();
    if (!text.trim() || isSubmitting) return;

    setIsSubmitting(true);
    setError(null);

    try {
      await sendMessage(receiverId, text.trim());
      setText("");
      
      if (onMessageSent) {
        onMessageSent();
      }
    } catch (err) {
      setError(err instanceof Error ? err.message : 'Failed to send message');
    } finally {
      setIsSubmitting(false);
    }
  }

  return (
    <div className="write-message-container">
      <form className="write-message-form" onSubmit={handleSubmit}>
        <textarea
          ref={textAreaRef}
          className="write-message-textarea"
          placeholder="Say something..."
          value={text}
          onChange={handleChange}
          onPaste={handlePaste}
          rows={1}
          disabled={isSubmitting}
        ></textarea>

        <input 
          type="submit" 
          className="write-message-submit-button" 
          value={isSubmitting ? "Sending..." : "Send"}
          disabled={isSubmitting || !text.trim()}
        />
      </form>
      {error && (
        <div style={{ color: 'red', marginTop: '8px', fontSize: '14px' }}>
          {error}
        </div>
      )}
    </div>
  );
}