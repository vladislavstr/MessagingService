import { useState } from 'react'
import './App.css'

interface Message {
  content: string;
  sentAt: string;
}

function App() {
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
      console.log('Отправляемые данные:', {
        content: message.content,
        sentAt: getCurrentUtcTime()
      });

      const response = await fetch('https://localhost:7054/api/Message/create-message', {
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
        console.error('Ошибка сервера:', errorData);
        alert('Ошибка при отправке сообщения');
      }
    } catch (error) {
      console.error('Ошибка:', error);
      alert('Произошла ошибка при отправке сообщения');
    }
  };

  return (
    <div className="container">
      <h1>Отправка сообщения</h1>
      <form onSubmit={handleSubmit}>
        <div className="form-group">
          <textarea
            value={message.content}
            onChange={(e) => setMessage({ ...message, content: e.target.value })}
            placeholder="Введите текст сообщения"
            required
          />
        </div>
        <button type="submit">Отправить</button>
      </form>
    </div>
  )
}

export default App
