// index.js

// Подключаем модули
var express = require('express');
var app = express();
// Модуль mongodb
var MongoClient = require('mongodb').MongoClient;

// Путь до БД
var path = "mongodb://localhost:27017/";


var server = require('http').createServer(app);

var io = require('socket.io').listen(server);

// Запуст сервера и какой порт отслеживать
server.listen(3000);

// Очищаем коллекцию в БД
MongoClient.connect(path, function (err, client) {
   var db = client.db('messages');
   var collection = db.collection('info')
   collection.remove({});
   client.close();
});

// Число пользователей онлайн
var currentNumberOfUsers = 0;
// Структура для хранения информации о текущих пользователях
users = [];
// Все подключения на текущий момент
connections = [];



// Отслеживать главную страницу
app.get('/', function (request, response) {
   // response - ответ, request - запрос

   // Отправить файл главной страницы, dirname - директория текущая
   response.sendFile(__dirname + '/index.html');
});


// Отслеживаем событие connection
io.sockets.on('connection', function (socket) {
   // socket - объект подключения

   // Сообщение для проверки
   console.log('Новое соединение');
   // Увеличиваем число пользователей
   currentNumberOfUsers++;
   connections.push(socket);

   // Приветствуем нового пользователя
   socket.emit('set_mess', { msg: "Привет, добро пожаловать в наш чат!", name: "admin" });

   // Вывод истории сообщений (если есть)
   MongoClient.connect(path, function (err, client) {
      var db = client.db('messages');
      var collection = db.collection('info');

      db.collection('info').find({}).toArray(function (err, result) {
         if (err) throw err;

         for (let i = 0; i < result.length; i++)
            socket.emit('set_mess', { msg: result[i]["msg"], name: result[i]["name"] });

      });

      client.close();
   });
   
   // Удаление при отключении
   socket.on('disconnect', function (data){
      // Удаление из структуры
      connections.splice(connections.indexOf(socket), 1);

      // Сообщение для проверки
      console.log('Соединение прекращено');
      // Уменьшаем число пользователей
      currentNumberOfUsers--;
      // Обновляем число пользовтаелей
      io.sockets.emit('set_num_of_users', currentNumberOfUsers);
   });

   // Сервер получает сообщение от клиента
   socket.on('send_mess', function (data) {
      // Запись полученного сообщения в БД
      MongoClient.connect(path, function (err, client) {
         var db = client.db('messages');
         var collection = db.collection('info')
         collection.insertOne(data);
         client.close();
      });
      // Рассылка всем клиентам сообщения
      io.sockets.emit('set_mess', {msg: data.msg, name: data.name});
   });

   // Запрос от клиента: число пользователей
   socket.on('request_num_of_users', function () {
      io.sockets.emit('set_num_of_users', currentNumberOfUsers);
   });
});