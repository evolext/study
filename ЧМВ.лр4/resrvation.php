<?php
   session_start();
?>

<!DOCTYPE html>
<html>
   <head>
      <meta charset="UTF-8">
      <title>Список бронирований</title>
      <link rel="stylesheet" href="style.css">
      <link rel="shortcut icon" href="book.png" type="image/png">
      <link href="https://fonts.googleapis.com/css?family=Arimo&display=swap" rel="stylesheet">
      <link href="https://fonts.googleapis.com/css?family=Roboto+Slab&display=swap" rel="stylesheet">
      <style>
         p
         {
            margin-left: 5%;
         }
         #information
         {
            margin-left: 5%;
            width: 80%;
         }
         .column2
         {
            width: 45%;
         }
         .column3, .column4
         {
            width: 20%;
         }
         .column5
         {
            width: 10%;
         }
      </style>
   </head>
   <body>
      <header>
         <p><a href="index.php"><img src="book.png" width="70px"></a>Новосибирский книжный каталог</p>
      </header>
      <!--Шапка сайта-->
      <table id="siteHead">
        <tr>
            <td id="col1" title="Перейти на главную страницу"><a href="index.php">Главная</a></li></td>
            <td id="userMenu">
               <p>Личный кабинет</p>
                  <ul>
                     <!--Вход/выход в зависимости от статуса авторизации-->
                     <?php if($_SESSION['login'] == ""): ?>
                        <li><a href="" style="width: 263px;">Войти</a></li>
                     <?php elseif($_SESSION['admin_status'] == "1"): ?>
                        <li title="Добавить новую книгу в каталог"><a href="adding_book.html">Добавление новой книги</a></li>
                        <li><a href="exit.php">Выход</a></li>
                     <?php else: ?>
                        <li title="Посмотреть список забронированных книг"><a href="resrvation.php">Список бронирований</a></li>
                        <li><a href="exit.php">Выход</a></li>
                     <?php endif; ?>
                  </ul>
               </div>
            </td>
            <td id="col3" title="Поиск по нескольким полям"><a href="advanced_search.php">Расширенный поиск</a></td>
            <td id="col4">
            </td>
         </tr>
      </table>
      
      
      <p><?php echo $_SESSION['surname']," ",$_SESSION['username']; ?></p>
      <table id="information">
         <tr>
            <td class="column1">№</td>
            <td class="column2">Заглавие</td>
            <td class="column3">Дата выдачи</td>
            <td class="column4">Срок возврата</td>
            <td class="column5">Количество</td>
         </tr>
         <tr>
            <td class="column1">1</td>
            <td class="column2">Математическая статистика / Г.И. Ивченко</td>
            <td class="column3">10 сентября 2019</td>
            <td class="column4">12 ноября 2019</td>
            <td class="column5">1</td>
         </tr>
         <tr>
            <td class="column1"></td>
            <td class="column2"></td>
            <td class="column3"></td>
            <td class="column4"></td>
            <td class="column5"></td>
         </tr>
      </table>
   </body>
</html>