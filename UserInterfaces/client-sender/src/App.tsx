import { useState } from 'react'
import './App.css'

interface Message {
  content: string;
  sentAt: string;
}

function App() {
  const [isLoading, setIsLoading] = useState(false);
  
  const getCurrentUtcTime = () => {
    const now = new Date();
    const utcOffset = now.getTimezoneOffset();
    const utcTime = new Date(now.getTime() - utcOffset * 60000);
    return utcTime.toISOString();
  };

  const [message, setMessage] = useState<Message>({
    content: '',
    sentAt: getCurrentUtcTime()
  });

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    
    try {
      setIsLoading(true);
      console.log('Sending data:', {
        content: message.content,
        sentAt: getCurrentUtcTime()
      });

        const response = await fetch('http://localhost:5120/api/Message/save', {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify({
          content: message.content,
          sentAt: getCurrentUtcTime()
        })
      });

      if (response.ok) {
        const responseText = await response.text();
        alert(responseText);
        setMessage({
          content: '',
          sentAt: getCurrentUtcTime()
        });
      } else {
        const errorData = await response.text();
        console.error('Server error:', errorData);
        alert('Error sending message');
      }
    } catch (error) {
      console.error('Error:', error);
      alert('An error occurred while sending the message');
    } finally {
      setIsLoading(false);
    }
  };

  return (
    <div className="container">
      <h1>Send Message</h1>
      <form onSubmit={handleSubmit}>
        <div className="form-group">
          <textarea
            value={message.content}
            onChange={(e) => setMessage({ ...message, content: e.target.value })}
            placeholder="Enter message text"
            required
            disabled={isLoading}
          />
        </div>
        <button type="submit" disabled={isLoading}>
          {isLoading ? 'Loading...' : 'Send'}
        </button>
      </form>
    </div>
  )
}

export default App
