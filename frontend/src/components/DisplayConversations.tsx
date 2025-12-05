import '../styles/displayconversations.css';

export default function DisplayConversations() {
  return <>
    <div className="conversations-container">
      <h2>Conversations (3)</h2>
      <ul>
        <li><a href="/messages/">@mycookie5</a></li>
        <li className="active"><a href="/messages/">@MMTTSEC</a></li>
        <li><a href="/messages/">@MSU98</a></li>
      </ul>
    </div>
  </>
}