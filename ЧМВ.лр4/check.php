<?php
   session_start();

   $surname = filter_var(trim($_POST['userSurname']), FILTER_SANITIZE_STRING);
   $name = filter_var(trim($_POST['userName']), FILTER_SANITIZE_STRING);
   $mail = filter_var(trim($_POST['userMail']), FILTER_SANITIZE_STRING);
   $password = filter_var(trim($_POST['userPass']), FILTER_SANITIZE_STRING);


   // хэширование пароля
   $password = md5($password."cgfhjibv");


   $mysqli = new mysqli("localhost", "root", "root", "register-bd");

   $mysqli->query("INSERT INTO `users` (`login`, `password`, `Name`, `Surname`) VALUES('$mail', '$password', '$name', '$surname')");
    
   mysqli_close($mysqli);


   // Автоматическая авторизаци после успешной регистрации
   $_SESSION['login'] = $mail;
   $_SESSION['username'] = $name;
   $_SESSION['surname'] = $surname;




   // Возврат на страницу
   header('Location: /');
?>