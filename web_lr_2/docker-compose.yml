version: "3.1"
secrets:
  mongo-admin-passwd:
    file: mongo-admin-passwd
services:
  mongodb:
    # Базовый образ
    image: mongo:3
    # Имя создаваемого контейнера
    container_name: db
    # Переменные окружения
    environment:
      MONGO_INITDB_ROOT_USERNAME: admin
      MONGO_INITDB_ROOT_PASSWORD_FILE: /run/secrets/mongo-admin-passwd
    # Используемые порты
    ports:
      - 27017:27017
    volumes:
      - ./mongo-init.js:/docker-entrypoint-initdb.d/mongo-init.js
    secrets:
      - mongo-admin-passwd