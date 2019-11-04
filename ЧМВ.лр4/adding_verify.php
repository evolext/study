<?php
   session_start();

   // Получаем данные из заполненной формы
   $author = filter_var(trim($_POST['authors']), FILTER_SANITIZE_STRING);
   $title = filter_var(trim($_POST['title']), FILTER_SANITIZE_STRING);
   $publishing = filter_var(trim($_POST['publishing']), FILTER_SANITIZE_STRING);
   $year = filter_var(trim($_POST['year']), FILTER_SANITIZE_STRING);
   $isbn = filter_var(trim($_POST['isbn']), FILTER_SANITIZE_STRING);
   $count = filter_var(trim($_POST['count']), FILTER_SANITIZE_STRING);

   // Уадаляем разделители из isbn
   $isbn = str_replace("-", "", $isbn);

   //echo $author," ",$title," ",$publishing," ",$year," ",$isbn," ",$count;

   // Установление соединения с БД
   $mysqlii = new mysqli("localhost", "root", "root", "books-bd");

   // Непосредственно добалвение записи в таблицу
   $mysqlii->query("INSERT INTO `catalog` (`author`, `title`, `publishing`, `year`, `isbn`, `count`) VALUES('$author', '$title', '$publishing', '$year', '$isbn', '$count')");

   // Разрыв соединения с БД
   mysqli_close($mysqlii);

   // Возврат на страницу добавления
   header('Location: /adding_book.php');
?>