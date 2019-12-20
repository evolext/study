//задаем переменную пути к нашим статическим файлам
var path = require('path');
//подключаем модуль экспресс
var express = require('express');
// Модель для конвертации изображений
var base64Img = require('base64-img');
//помещаем в эту переменную функцию express
var app = express();
//переменная отвечающая за сервер построенная на библиотеке http
var server = require('http').createServer(app);
//подключаем socket.io и отслеживаем наш сервер
var io = require('socket.io').listen(server);
//подключаем статитическую директорию с нашими файлами 
app.use(express.static(path.join(__dirname, 'public')));
//задаем какой порт будет отслеживать наш сервер
server.listen(3000);
//прописываем какие конкретно url-адреса мы отслеживаем
app.get('/', function(request, respons){
    respons.sendFile(__dirname + '/drawing.html');
});

// Конвертация изначального заднего фона полотна
var dataImg = base64Img.base64Sync('public/back.png');

//массивы содержащие текущих пользователей чата и подключения
//users = [];
connections = [];

//отслеживаем событие connection(подключение пользователя)
io.sockets.on('connection', function(socket) {
    //добавляем нового пользователя
    connections.push(socket);

    // Установка заднего фона у пользователей
    socket.emit('setbackground', dataImg);

    //удаляем соединение из массива connections в случае отключение пользователя от сервера(событие disconnect)
    socket.on('disconnect', function(data) {
        connections.splice(connections.indexOf(socket), 1);
    });

    // Отрисовывание у всех пользователей
    socket.on('sendcanvas', function(data){
        socket.broadcast.emit('setcanvas', data);
    });

    // Запрос на очистку полотна у всех пользователей
    socket.on('sendcls', function(){
        io.sockets.emit('setcls');
    });

    // Установка заднего фона полотна для только присоединившихся пользователей
    socket.on('getback', function () {
        io.sockets.emit('setbackground', dataImg);
    });

    // Сохранение изменённого заднего фона полотна
    socket.on('sendbackground', function (data) {
        dataImg = data;
        io.sockets.emit('setbackground', dataImg);
    });

});
