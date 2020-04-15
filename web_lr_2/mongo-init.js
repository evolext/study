// Данные о пользователях БД
var users = {
  // Общее число пользователей
  count: 5,
  allUsers: [
    {
      'user': 'cerym',
      'pwd': '123',
      'roles': [
        { 'role': 'dbAdmin', 'db': 'database' }
      ]
    },
    {
      'user': 'enavi',
      'pwd': '123',
      'roles': [
        { 'role': 'userAdmin', 'db': 'database' }
      ]
    },
    {
      'user': 'kreer',
      'pwd': '123',
      'roles': [
        { 'role': 'dbOwner', 'db': 'database' }
      ]
    },
    {
      'user': 'pabrg',
      'pwd': '123',
      'roles': [
        { 'role': 'read', 'db': 'database' }
      ]
    },
    {
      'user': 'lelki',
      'pwd': '123',
      'roles': [
        { 'role': 'readWrite', 'db': 'database' }
      ]
    }
  ]
}

// Создаем несистемную БД для необходиимых действий
db = db.getSiblingDB('database');

// Создаем три коллекции
db.createCollection('Dates');
db.createCollection('Units');
db.createCollection('Users');

// Создаем нужных пользователей БД
for (let i = 0; i < users.count; i++)
  db.createUser(users.allUsers[i]);

// Инициализируем по одному документу в каждой коллекции
db.Dates.insertOne({ "Unit ID": "0", "Time": "00:00", "Value": "0" });
db.Units.insertOne({ "Name": "admin", "Secret": "admin", "Descr": "0" });
db.Users.insertOne({ "Name": "admin", "User ID": "0", "Descr": "0", "Secret": "admin" });