import { useState, useRef, useEffect } from "react";
import { createPost } from '../utils/postsApi';
import '../styles/writepost.css';
import 'bootstrap-icons/font/bootstrap-icons.css';

const MAX_CHARS = 300;

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

interface WritePostProps {
  onPostCreated?: () => void;
}

export default function WritePost({ onPostCreated }: WritePostProps) {
  const [text, setText] = useState("");
  const [isSubmitting, setIsSubmitting] = useState(false);
  const [error, setError] = useState<string | null>(null);
  const [success, setSuccess] = useState(false);
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
    setSuccess(false);

    try {
      // Use first 100 chars as title, rest as body
      const title = text.slice(0, 100).trim();
      const body = text.slice(100).trim() || title;
      
      await createPost(title, body);
      
      setText("");
      setSuccess(true);
      
      // Trigger refresh callback
      if (onPostCreated) {
        onPostCreated();
      }
      
      // Clear success message after 2 seconds
      setTimeout(() => setSuccess(false), 2000);
    } catch (err) {
      setError(err instanceof Error ? err.message : 'Failed to create post');
    } finally {
      setIsSubmitting(false);
    }
  }

  return (
    <div className="write-post-container">
      <form className="write-post-form" onSubmit={handleSubmit}>
        <textarea
          ref={textAreaRef}
          className="write-post-textarea"
          placeholder="Write something..."
          value={text}
          onChange={handleChange}
          onPaste={handlePaste}
          rows={1}
          disabled={isSubmitting}
        ></textarea>

        <div className="char-count">{MAX_CHARS - text.length}</div>
        <input 
          type="submit" 
          className="write-post-submit-button" 
          value={isSubmitting ? "Posting..." : "Post"}
          disabled={isSubmitting || !text.trim()}
        />
      </form>
      {error && (
        <div style={{ color: 'red', marginTop: '8px', fontSize: '14px' }}>
          {error}
        </div>
      )}
      {success && (
        <div style={{ color: 'green', marginTop: '8px', fontSize: '14px' }}>
          Post created successfully!
        </div>
      )}
    </div>
  );
}