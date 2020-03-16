// Заупск БД:
// "C:\Program Files\MongoDB\Server\4.2\bin\mongod.exe" --dbpath="c:\data\db"

var express = require('express');
var app = express();
var server = require('http').createServer(app);
var io = require('socket.io').listen(server);

// Модуль mongodb
//var MongoClient = require('mongodb').MongoClient;

// Путь до БД
var path = "mongodb://localhost:27017/";

server.listen(3003);

// Указание директории с файлами
app.use(express.static(__dirname + "/public"));

app.get('/', function(request, response) {
    response.sendFile(__dirname + '/index.html');
});

io.sockets.on('connection', function (socket) {

    // Вывод комментариев из БД
    // MongoClient.connect(path, function (err, client) {
    //     var db = client.db('database');
  
    //     db.collection('comments').find({}).toArray(function (err, result) {
    //        if (err) throw err;
  
    //        for (let i = 0; i < result.length; i++)
    //           socket.emit('set_mess', { "name": result[i]["name"], "comment": result[i]["comment"] });
  
    //     });
    //     client.close();
    //  });


    // Сервер получает сообщение от клиента
    socket.on('send_mess', function (data) {

        // Добавление комментария в базу данных
        // MongoClient.connect(path, function (err, client) {
        //     var db = client.db('database');
        //     var collection = db.collection('comments')
        //     collection.insertOne(data);
        //     client.close();
        //  });

        // Отображение комментария на странице
        io.sockets.emit('set_mess', data);
    });

});