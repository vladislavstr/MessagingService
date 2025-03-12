# Build stage for all client applications
FROM node:20-alpine AS build

# Set working directory
WORKDIR /app

# Copy package.json files
COPY client-recent-messages/package*.json ./client-recent-messages/
COPY client-sender/package*.json ./client-sender/
COPY client-websocket/package*.json ./client-websocket/

# Install dependencies and required TypeScript types for all applications
RUN cd client-recent-messages && npm install && npm install --save-dev @types/node @types/dom && cd .. && \
    cd client-sender && npm install && npm install --save-dev @types/node @types/dom && cd .. && \
    cd client-websocket && npm install && npm install --save-dev @types/node @types/dom

# Copy source code
COPY client-recent-messages/ ./client-recent-messages/
COPY client-sender/ ./client-sender/
COPY client-websocket/ ./client-websocket/

# Build all applications
RUN cd client-recent-messages && npm run build && cd .. && \
    cd client-sender && npm run build && cd .. && \
    cd client-websocket && npm run build

# Production stage
FROM nginx:alpine

# Copy built assets from builder stage
COPY --from=build /app/client-recent-messages/dist /usr/share/nginx/html/recent-messages
COPY --from=build /app/client-sender/dist /usr/share/nginx/html/sender
COPY --from=build /app/client-websocket/dist /usr/share/nginx/html/websocket

# Copy nginx configuration
COPY nginx.conf /etc/nginx/conf.d/default.conf

EXPOSE 80

CMD ["nginx", "-g", "daemon off;"] 