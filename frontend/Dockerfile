# Stage 1: Build Angular application
FROM node:22-alpine AS build
WORKDIR /app

# Copy dependency specifications and install
COPY package*.json ./
RUN npm ci || npm install

# Copy source code and build for production
COPY . .
RUN npm run build

# Stage 2: Production Nginx Server
FROM nginx:alpine AS final
WORKDIR /usr/share/nginx/html

# Clean default nginx files
RUN rm -rf ./*

# Copy built browser bundle from Stage 1
COPY --from=build /app/dist/frontend/browser .

# Copy Nginx SPA & reverse proxy configuration
COPY nginx.conf /etc/nginx/conf.d/default.conf

EXPOSE 80

CMD ["nginx", "-g", "daemon off;"]
