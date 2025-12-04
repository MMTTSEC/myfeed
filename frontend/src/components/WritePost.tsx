import { useState, useRef, useEffect } from "react";
import '../styles/writepost.css';
import 'bootstrap-icons/font/bootstrap-icons.css';

export default function WritePost() {
  const MAX_CHARS = 300;
  const [text, setText] = useState("");
  const textAreaRef = useRef<HTMLTextAreaElement | null>(null);

  useEffect(() => {
    resizeTextArea();
  }, [text]);

  function resizeTextArea() {
    const textArea = textAreaRef.current;
    if (!textArea) return;

    textArea.style.height = "auto";
    textArea.style.height = textArea.scrollHeight + "px";
  }

  function handleChange(e: React.ChangeEvent<HTMLTextAreaElement>) {
    let newText = e.target.value;

    if (newText.length > MAX_CHARS) {
      newText = newText.slice(0, MAX_CHARS);
    }

    setText(newText);
    resizeTextArea();
  }

  function handlePaste(e: React.ClipboardEvent<HTMLTextAreaElement>) {
    e.preventDefault();

    let paste = e.clipboardData.getData("text");

    paste = paste
      .replace(/\r?\n|\r/g, " ")  // remove ALL linebreaks
      .replace(/\t/g, " ")        // remove tabs
      .replace(/ +/g, " ")        // collapse multiple spaces
      .trim();

    const cleaned = (text + paste).slice(0, MAX_CHARS);

    setText(cleaned);
    resizeTextArea();
  }

  function handleSubmit(e: React.FormEvent) {
    e.preventDefault();
    if (!text.trim()) return;

    setText("");
  }

  return <>
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
          style={{ overflow: "hidden", resize: "none" }}
        ></textarea>
        <div className="char-count">{MAX_CHARS - text.length}</div>
        
        <input type="submit" className="write-post-submit-button" value="Post" />
      </form>
    </div>
  </>;
}