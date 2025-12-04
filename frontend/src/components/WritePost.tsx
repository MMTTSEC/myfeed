import { useState, useRef, useEffect } from "react";
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

export default function WritePost() {
  const [text, setText] = useState("");
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

  function handleSubmit(event: React.FormEvent<HTMLFormElement>) {
    event.preventDefault();
    if (!text.trim()) return;

    console.log("Submitted post:", text);
    setText("");
  }

  return (
    <div className="write-post-container">
      <form className="write-post-form" onSubmit={handleSubmit}>
        <textarea
          ref={textAreaRef}
          className="write-post-textarea"
          placeholder="Write something that's on your mind..."
          value={text}
          onChange={handleChange}
          onPaste={handlePaste}
          rows={1}
        ></textarea>

        <div className="char-count">{MAX_CHARS - text.length}</div>
        <input type="submit" className="write-post-submit-button" value="Post" />
      </form>
    </div>
  );
}