<?php
   session_start();
?>
<!DOCTYPE html>
   <head>
      <meta charset="UTF-8">
      <title>Расширенный поиск</title>
      <link rel="stylesheet" href="style.css">
      <link rel="shortcut icon" href="book.png" type="image/png">
      <link href="https://fonts.googleapis.com/css?family=Arimo&display=swap" rel="stylesheet">
      <link href="https://fonts.googleapis.com/css?family=Roboto+Slab&display=swap" rel="stylesheet">
      <style>
         #search
         {
            float: left;
            width: 30%;
         }
         #results
         {
            width: 60%;
            display: inline-block;
         }
         #search input
         {
            position: relative;
            z-index: 100;
            margin-top: 10px;
         }

         form
         {
            margin-left: 10px;
            margin-right: 10px;
            border: 1px solid black;
         }
         #searchTitle
         {
            color: white;
            margin: 0;
            text-align: center;
            background-color: rgb(39, 170, 226);
         }

         #formSearch td
         {
            border: none;
         }
         .column1
         {
            padding-top: 5px;
            padding-left: 5%;
            text-align: left;
            width: 30%;
         }
         .column2
         {
            padding-top: 5px;
         }
         .column2 input
         {
            display: inline-block;
            width: 70%;
         }
         tr button
         {
            display: inline-block;
            width: 20%;
            margin-top: 20px;
         }

         #warning
         {
            background-color: red;
            color: white;
            display: none;
            height: 37px;
            width: 226px;
            position: absolute;
            z-index: 10;
            margin-left: -220px;
            margin-top: -2px;
            font-size: 10px;
            padding: 0;
         }
         #siteHead
         {
            position: relative;
            z-index: 200;
         }


         .note
         {
            border: 1px solid rgb(10, 98, 136);
            border-radius: 15px;
            width: 100%;
            font-family: Verdana, Geneva, Tahoma, sans-serif;
            background-color: rgb(39, 170, 226);
            color: white;
            margin-bottom: 10px;
         }
         .note table
         {
            margin: 10px;
            width: 98%;
         }
         .note table td
         {
            border: none;
         }
         .note .column1
         {
            padding-left: 0;
            text-align: left;
            width: 75%;
         }
         .note .column2
         {
            text-align: left;
            width: 25%;
         }
         .note .column2 input
         {
            width: 90%;
         }
         .note table #quantity
         {
            font-size: 1.2em;
         }

      </style>
      <script src="http://code.jquery.com/jquery-1.8.3.js"></script>
      <script src="jquery.maskedinput.min.js"></script>
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
                        <li><a href="login_page.php" style="width: 263px;">Войти</a></li>
                     <?php elseif($_SESSION['admin_status'] == "1"): ?>
                        <li title="Добавить новую книгу в каталог"><a href="adding_book.php">Добавление новой книги</a></li>
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

      <!--Форма для расширенного поиска-->
      <form id="search" onsubmit="return validate();" method="post">
            <p id="searchTitle" >Расширенный поиск</p>

            <table id="formSearch">
               <tr title="Укажите одного или более авторов, слова на русском языке">
                  <td class="column1">Автор:</td>
                  <td class="column2"><input type="text" id="authorBook" name="authorBook" autofocus onkeyup="checkForm()"><span id="warning">Вводите только фамилии, имена и инициалы</span></td>
               </tr>
               <tr title="В этом поле укажите заглавие книги">
                  <td class="column1">Название:</td>
                  <td class="column2"><input type="text" id="titleBook" name="titleBook" onkeyup="validate()"></td>
               </tr>
               <tr title="Укажите год публикации, вводить только цифры">
                  <td class="column1">Год публикации: </td>
                  <td class="column2"><input type="text" id="yearBook" name="yearBook" onkeyup="validate()"></td>
               </tr>
               <tr title="ISBN - уникальный номер книги, обычно указывается рядом со штрих-кодом, вводить только цифры">
                  <td class="column1">Номер ISBN:</td>
                  <td class="column2"><input type="text" id="isbnBook" name="isbnBook" onkeyup="validate()"></td>
               </tr>
               <tr>
                  <td colspan="2" id="serchStart"><button title="Начать поиск" id="searchButton" name="searchButton" disabled>Поиск</button></td>
               </tr>
            </table>
      </form>
      
      <!--Результаты поиска-->
      <div id="results">
         <?php
            if(isset($_POST['searchButton']))
            {
               // получаем введённые данные со всех полей формы
               $author = $_POST['authorBook'];
               $title = $_POST['titleBook'];
               $year = $_POST['yearBook'];
               $isbn = $_POST['isbnBook'];

               // Удаляем разделители
               $isbn = str_replace("-", "", $isbn);

               // Подключение к БД
               $mysqli = new mysqli("localhost", "root", "root", "books-bd");


               if (!empty($author) && !empty($title) && !empty($year) && !empty($isbn))
               {
                  // Ведется поиск по всем четрыем полям
                  $result = $mysqli->query("SELECT * FROM `catalog` WHERE `author` LIKE '%$author%' AND `title` LIKE '%$title%' AND `year` = '$year' AND `isbn` = '$isbn'");
               }
               else if (!empty($author) && !empty($title) && !empty($year) && empty($isbn))
               {
                  // Ведется поиск по: автор, название, год
                  $result = $mysqli->query("SELECT * FROM `catalog` WHERE `author` LIKE '%$author%' AND `title` LIKE '%$title%' AND `year` = '$year'");
               }
               else if (!empty($author) && !empty($title) && empty($year) && !empty($isbn))
               {
                  // Ведется поиск по: автор, название, номер
                  $result = $mysqli->query("SELECT * FROM `catalog` WHERE `author` LIKE '%$author%' AND `title` LIKE '%$title%' AND `isbn` = '$isbn'");
               }
               else if (empty($author) && !empty($title) && !empty($year) && !empty($isbn))
               {
                  // Ведется поиск по: название, год, номер
                  $result = $mysqli->query("SELECT * FROM `catalog` WHERE `title` LIKE '%$title%' AND `year` = '$year' AND `isbn` = '$isbn'");
               }
               else if (!empty($author) && empty($title) && !empty($year) && !empty($isbn))
               {
                  // Ведется поиск по: автор, год, номер
                  $result = $mysqli->query("SELECT * FROM `catalog` WHERE `author` LIKE '%$author%' AND `year` = '$year' AND `isbn` = '$isbn'");
               }
               else if (!empty($author) && !empty($title) && empty($year) && empty($isbn))
               {
                  // Ведется поиск по: автор, название
                  $result = $mysqli->query("SELECT * FROM `catalog` WHERE `author` LIKE '%$author%' AND `title` LIKE '%$title%'");
               }
               else if (!empty($author) && empty($title) && !empty($year) && empty($isbn))
               {
                  // Ведется поиск по: автор, год
                  $result = $mysqli->query("SELECT * FROM `catalog` WHERE `author` LIKE '%$author%' AND `year` = '$year'");
               }
               else if (!empty($author) && empty($title) && empty($year) && !empty($isbn))
               {
                  // Ведется поиск по: автор, номер
                  $result = $mysqli->query("SELECT * FROM `catalog` WHERE `author` LIKE '%$author%' AND `isbn` = '$isbn'");
               }
               else if (empty($author) && !empty($title) && !empty($year) && empty($isbn))
               {
                  // Ведется поиск по: название, год
                  $result = $mysqli->query("SELECT * FROM `catalog` WHERE `title` LIKE '%$title%' AND `year` = '$year'");
               }
               else if (empty($author) && !empty($title) && empty($year) && !empty($isbn))
               {
                  // Ведется поиск по: название, номер
                  $result = $mysqli->query("SELECT * FROM `catalog` WHERE `title` LIKE '%$title%' AND `isbn` = '$isbn'");
               }
               else if (empty($author) && empty($title) && !empty($year) && !empty($isbn))
               {
                  // Ведется поиск по: год, номер
                  $result = $mysqli->query("SELECT * FROM `catalog` WHERE `year` = '$year' AND `isbn` = '$isbn'");
               }
               else if (!empty($author) && empty($title) && empty($year) && empty($isbn))
               {
                  // Ведется поиск по: автор
                  $result = $mysqli->query("SELECT * FROM `catalog` WHERE `author` LIKE '%$author%'");
               }
               else if (empty($author) && !empty($title) && empty($year) && empty($isbn))
               {
                  // Ведется поиск по: название
                  $result = $mysqli->query("SELECT * FROM `catalog` WHERE `title` LIKE '%$title%'");
               }
               else if (empty($author) && empty($title) && !empty($year) && empty($isbn))
               {
                  // Ведется поиск по: год
                  $result = $mysqli->query("SELECT * FROM `catalog` WHERE `year` = '$year'");
               }
               else if (empty($author) && empty($title) && empty($year) && !empty($isbn))
               {
                  // Ведется поиск по: номер
                  $result = $mysqli->query("SELECT * FROM `catalog` WHERE `isbn` = '$isbn'");
               }

               // Результат поиска
               while ($data_array = mysqli_fetch_assoc($result))
               {
                  if ($_SESSION['login'] != "" && $_SESSION['admin_status'] == "1")
                  {
                     echo '<div class="note">'.
                              "<table>".
                                 "<tr>".
                                    "<td id='title' class='column1'>".$data_array['title']." / ".$data_array['author']."</td>".
                                    "<td class='column2'><input type='button' value='Редактировать запись' id='editNote'></td>".
                                 "</tr>".
                                 "<tr>".
                                    "<td class='column1'>"."Издательство: ".$data_array['publishing']."</td>".
                                    "<td class='column2'><input type='button' value='Удалить запись' id='deleteNote'></td>".
                                 "</tr>".
                                 "<tr>".
                                    "<td class='column1' colspan='2'>"."ISBN: ".$data_array['isbn']."</td>".
                                 "</tr>".
                                 "<tr>".
                                    "<td id='quantity' colspan='2' class='column1'>"."Доступно экземпляров: ".$data_array['count']."</td>".
                                 "</tr>".
                              "</table>".
                           "</div>";
                  }
                  else 
                  {
                        echo '<div class="note">'.
                              "<table>".
                                 "<tr>".
                                    "<td id='title' class='column1'>".$data_array['title']." / ".$data_array['author']."</td>".
                                    "<td class='column2'><input type='button' value='Забронировать экземпляр' id='makeReserv'></td>".
                                 "</tr>".
                                 "<tr>".
                                    "<td class='column1' colspan='2'>"."Издательство: ".$data_array['publishing']."</td>".
                                 "</tr>".
                                    "<tr>".
                                    "<td class='column1' colspan='2'>"."ISBN: ".$data_array['isbn']."</td>".
                                 "</tr>".
                                 "<tr>".
                                    "<td id='quantity' colspan='2' class='column1'>"."Доступно экземпляров: ".$data_array['count']."</td>".
                                 "</tr>".
                              "</table>".
                           "</div>";
                  }
               }

               // Прекращение соединения с БД
               mysqli_close($mysqli);
            }
         ?>
      </div>


      <script>
         $(function(){
            $('#isbnBook').mask("999-9-99-999999-9");
            $('#yearBook').mask("9999");
         });

         function checkForm()
         {
            var value = $('#authorBook').val();
            var pattern = new RegExp("^[А-Яа-яЁё\. ]+$");
            var check = pattern.test(value);

            if (value == "")
               check = true;


            if (check)
               $('#warning').css('display', 'none');
            else
               $('#warning').css('display', 'inline-block');


            validate();
         }

         function validate()
         {
            var warn = $('#warning');
            var author = $('#authorBook');
            var title = $('#titleBook');
            var year = $('#yearBook');
            var isbn = $('#isbnBook');
            

            var h = year.val();
            h = parseInt(h);
            var x = h.toString();

            var j = isbn.val();
            j = parseInt(j);
            var y = j.toString();



            if ((warn.css('display') != "none") || (author.val() == "" && title.val() == "" && x.length < 4 && y.length < 13))
               $('#searchButton').attr('disabled', 'disabled');
            else
               $('#searchButton').removeAttr('disabled');
         }
      </script>
   
   </body>
</html>