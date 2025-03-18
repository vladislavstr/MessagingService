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

## Задача - написать простой web-сервис обмена сообщениями.

### Сервис состоит из трех компонентов:
- Web-сервер
- SQL БД (желательно Postgresql)
- 3 клиента (первый пишет сообщения, второй отображает их в реальном времени, третий позволяет просмотреть список сообщений за последнюю минуту).

Все три клиентские части можно реализовать как отдельными web-приложениями, так и одним c разделение клиентов по url (как удобнее).
Каждое сообщение состоит из текста до 128 символов, метки даты/времени (устанавливается на сервере) и порядкового номера (приходит от клиента).

### Схема работы системы следующая: 
- первый клиент пишет потоком произвольные (по контенту) сообщения в сервис (на одно сообщение один вызов к API)
- сервис обрабатывает каждое сообщение, записывает его в базу и перенаправляет его второму клиенту по веб-сокету
- второй клиент при считывает по веб-сокету поток сообщений от сервера и отображает их в порядке прихода с сервера (с отображением метки времени и порядкового номера)
- через третий клиент пользователь может отобразить историю сообщений за последние 10 минут

### Серверная часть имеет REST или GraphQL (на выбор) API c 2 методами:
- отправить одно сообщение
- получить список сообщений за диапазон дат

**Желательно:** сгенерить swagger-документацию (для REST-api).

**Перечень языков для разработки:** C# или Go

**Архитектурные требования:**
- MVC или подобная

- Слой DAL без использования ORM

- Ведение логов, чтобы по ним можно было понять текущее состояние работы приложения (детальность логов на свое усмотрение, при этом качество логирования является одним из критериев оценки выполненного тестового задания)

***Приложение нужно оформить в виде docker-образов и выложить на github.
Оформить docker-compose файл, при запуске которого стартуют все компоненты системы
Дизайн не важен, т.к. это задание по большей части по backend-разработке.
Главное, чтобы сообщения отображались и их можно было прочитать. Клиентские приложения(е) - это по сути тестовые утилиты, которые должен уметь писать backend-разработчик.***
