<?php
   session_start();


   // Данные из формы получаем
   $login = filter_var(trim($_POST['loginAuth']), FILTER_SANITIZE_STRING);
   $password = filter_var(trim($_POST['passAuth']), FILTER_SANITIZE_STRING);

   // хэширование пароля
   $password = md5($password."cgfhjibv");

   
   $mysqli = new mysqli("localhost", "root", "root", "register-bd");

   $result = $mysqli->query("SELECT * FROM `users` WHERE `login` = '$login' AND `password` = '$password'");
   // Преобразовываем в массив
   $user = mysqli_fetch_assoc($result);

   if(count($user) == 0)
   {
      echo "Пользователь не найден";
      exit();
   }   

   // Запоминаем данные пользователя
   $_SESSION['login'] = $user['login'];
   $_SESSION['username'] = $user['Name'];
   $_SESSION['surname'] = $user['Surname'];
   $_SESSION['admin_status'] = $user['admin_status'];

   mysqli_close($mysqli);


   // Возврат на страницу
   header('Location: /');
?>