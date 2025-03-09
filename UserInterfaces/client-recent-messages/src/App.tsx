import { useState } from 'react'
import './App.css'

interface Message {
  id: number;
  content: string;
  savedAt: string;
}

interface ApiResponse {
  messages: Message[];
}

function App() {
  const [messages, setMessages] = useState<Message[]>([]);
  const [isLoading, setIsLoading] = useState(false);

  const fetchMessages = async () => {
    try {
      setIsLoading(true);
      const response = await fetch('https://localhost:7054/api/Message/list');
      if (response.ok) {
        const data: ApiResponse = await response.json();
        console.log('Received data:', data);
        setMessages(data.messages || []);
      } else {
        console.error('Failed to fetch messages');
        setMessages([]);
      }
    } catch (error) {
      console.error('Error fetching messages:', error);
      setMessages([]);
    } finally {
      setIsLoading(false);
    }
  };

  return (
    <div className="container">
      <h1>Recent Messages</h1>
      <button onClick={fetchMessages} className="fetch-button" disabled={isLoading}>
        {isLoading ? 'Loading...' : 'Get recent messages'}
      </button>

      <div className="messages">
        <h2>Messages list</h2>
        {messages.length === 0 ? (
          <p className="no-messages">No messages</p>
        ) : (
          <div className="messages-list">
            {messages.map((message) => (
              <div key={message.id} className="message-item">
                <div className="message-header">
                  <span className="message-id">ID: {message.id}</span>
                  <span className="message-date">
                    {new Date(message.savedAt).toLocaleString()}
                  </span>
                </div>
                <div className="message-content">{message.content}</div>
              </div>
            ))}
          </div>
        )}
      </div>
    </div>
  )
}

export default App
