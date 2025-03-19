# MessagingService

## Stack

- MediatR
- Aspire
- Scalar/Swagger
- Serilog
- Seq
- Mapperly
- Npgsql
- React
- MauiUI

## Instructions
1. Add .env and .env.prod files:
  - Create .env and .env.prod files in the root of the project.
  - Add the SSL_CERT_PASSWORD variable to both files.

2. Generate SSL_CERT_PASSWORD:
- You can generate a secure password using the following command:

```bash
openssl rand -base64 32
```

Copy the generated password and paste it into the SSL_CERT_PASSWORD variable in both .env and .env.prod files.

### UI Structure
1. \UserInterfaces\client-sender-messages:

    - The first client writes a stream of arbitrary (content-wise) messages to the service (one API call per message).

2. \UserInterfaces\client-websocket:

    - The second client reads a stream of messages from the server via WebSocket and displays them in the order they are received (with a timestamp and sequence number).

3. \UserInterfaces\client-recent:
   - Through the third client, the user can display the message history for the last 10 minutes.

**Additional Project**
- \UserInterfaces\Desktop:

This project duplicates the role of the second client as a desktop application. It is not orchestrated via Aspire or Docker Compose.

## Building and Running the Project
The project can be built and run using Docker Compose.

For production configuration:
```bash
docker-compose -f docker-compose.yml -f docker-compose.prod.yml build
docker-compose -f docker-compose.yml -f docker-compose.prod.yml up -d
```
For development configuration:
```bash
docker-compose -f docker-compose.yml -f docker-compose.override.yml build
docker-compose -f docker-compose.yml -f docker-compose.override.yml up -d
```

Using Aspire:
Building and running with Aspire (/Devtools/AspireHost) is only supported for the development configuration.

## Task - Develop a Simple Messaging Web Service.

### The service consists of three components:
- Web server.
- SQL database (preferably PostgreSQL).
- 3 clients (the first writes messages, the second displays them in real-time, and the third allows viewing the list of messages from the last minute).

All three client parts can be implemented as separate web applications or as a single application with clients separated by URLs (as preferred).
Each message consists of text up to 128 characters, a timestamp (set on the server), and a sequence number (provided by the client).

### The system workflow is as follows:
- The first client writes a stream of arbitrary (content-wise) messages to the service (one API call per message).
- The service processes each message, saves it to the database, and forwards it to the second client via WebSocket.
- The second client reads the stream of messages from the server via WebSocket and displays them in the order they are received (with the timestamp and sequence number).
- Through the third client, the user can display the message history for the last 10 minutes.

### The server-side has a REST or GraphQL (your choice) API with 2 methods:
- Send a single message
- Get a list of messages for a date range

**Preferably:** Generate Swagger documentation (for REST API).

**Development languages:** C# or Go

**Architectural requirements:**
- MVC or similar
- DAL layer without using ORM
- Logging to understand the current state of the application (the level of detail in the logs is at your discretion, but the quality of logging is one of the evaluation criteria for the completed task)

***The application should be packaged as Docker images and published on GitHub.
Create a docker-compose file that starts all components of the system when launched.
Design is not important, as this task is mostly about backend development.
The main goal is to ensure that messages are displayed and can be read. The client application(s) are essentially test utilities that a backend developer should be able to write.***