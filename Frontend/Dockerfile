FROM node:13.7.0-alpine3.10 as node

WORKDIR /app

COPY package*.json ./

RUN npm install

COPY . .

ENTRYPOINT ["npm", "start"]