import { useState, useEffect } from 'react'
import * as signalR from '@microsoft/signalr'
import './App.css'

interface Message {
  id: number;
  content: string;
  savedAt: string;
}

function App() {
  const [hubConnection, setHubConnection] = useState<signalR.HubConnection | null>(null);
  const [connectionStatus, setConnectionStatus] = useState<string>('Disconnected');
  const [messages, setMessages] = useState<Message[]>([]);

  useEffect(() => {
    const connection = new signalR.HubConnectionBuilder()
        .withUrl('http://localhost:5120/chat')
      .withAutomaticReconnect()
      .build();

    setHubConnection(connection);

    return () => {
      if (connection) {
        connection.stop();
      }
    };
  }, []);

  useEffect(() => {
    if (hubConnection) {
      hubConnection.on('Send', (message: Message) => {
        setMessages(prev => [...prev, message]);
      });

      hubConnection.onclose(() => {
        setConnectionStatus('Disconnected');
      });

      hubConnection.onreconnecting(() => {
        setConnectionStatus('Reconnecting...');
      });

      hubConnection.onreconnected(() => {
        setConnectionStatus('Connected');
      });
    }
  }, [hubConnection]);

  const connectToHub = async () => {
    if (!hubConnection) return;

    try {
      if (hubConnection.state === signalR.HubConnectionState.Disconnected) {
        await hubConnection.start();
        setConnectionStatus('Connected');
      } else if (hubConnection.state === signalR.HubConnectionState.Connected) {
        await hubConnection.stop();
        setConnectionStatus('Disconnected');
      }
    } catch (error) {
      console.error('Error connecting to hub:', error);
      setConnectionStatus('Error connecting');
    }
  };

  return (
    <div className="container">
      <h1>WebSocket Client</h1>
      <div className="connection-status">
        <button onClick={connectToHub} className="connection-button">
          {connectionStatus === 'Connected' ? 'Disconnect' : 'Connect'}
        </button>
        <span className="status-indicator" style={{ 
          color: connectionStatus === 'Connected' ? 'green' : 
                 connectionStatus === 'Disconnected' ? 'red' : 'orange',
          marginLeft: '10px'
        }}>
          {connectionStatus}
        </span>
      </div>

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
