var express = require('express');
var app = express();
var server = require('http').createServer(app);
var io = require('socket.io').listen(server);
const fs = require("fs");

// Модуль mongodb
var MongoClient = require('mongodb').MongoClient;

// Путь до БД
var path = "mongodb://localhost:27017/";

server.listen(3003);

// Указание директории с файлами
app.use(express.static(__dirname + "/public"));

app.get('/', function(request, response) {
    response.sendFile(__dirname + '/index.html');
});

io.sockets.on('connection', function (socket) {

    // Тут должен быть вывод комментариев из БД, но пока даже не получается писоединиться
    MongoClient.connect(path, function (err, client) {
        console.log('Connected to MongoDB server');

        var db = client.db('config');
        // var db = client.db('comments');

        // db.collection('comments').find({}).toArray(function (err, result) {
        //    if (err) throw err;
  
        //    for (let i = 0; i < result.length; i++)
        //       socket.emit('set_mess', { "name": result[i]["name"], "comment": result[i]["comment"] });
  
        // });
        client.close();
    });

    // Вывод комментариев из БД (вариант с текстовым файлом)
    // var data = fs.readFileSync("info/test.txt", "utf8", function() {});
    // var split_lines = data.split('\n');
    // for (let i = 0; i < split_lines.length - 1; i++)
    // {
    //     socket.emit('set_mess', { "name": split_lines[i].split('\t')[0], "comment": split_lines[i].split('\t')[1]});
    // }


    // Сервер получает сообщение от клиента
    socket.on('send_mess', function (data) {

        // Добавление комментария в базу данных
        // MongoClient.connect(path, function (err, client) {
        //     var db = client.db('database');
        //     var collection = db.collection('comments')
        //     collection.insertOne(data);
        //     client.close();
        //  });

        // Добавление комментария в базу данных (варриант с текстовым файлом)
        //fs.appendFile("info/test.txt", data.name + '\t' + data.comment + '\n', function() {});


        // Отображение комментария на странице
        io.sockets.emit('set_mess', data);
    });

});