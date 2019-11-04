<?php
   session_start();
?>
<!DOCTYPE html>
   <head>
      <meta charset="UTF-8">
      <title>Главная страница</title>
      <link rel="shortcut icon" href="book.png" type="image/png">
      <link rel="stylesheet" href="style.css">
      <link href="https://fonts.googleapis.com/css?family=Arimo&display=swap" rel="stylesheet">
      <link href="https://fonts.googleapis.com/css?family=Roboto+Slab&display=swap" rel="stylesheet">
      <style>
         .valid
         {
            border: 3px solid green;
         }
         .undef
         {
            border: 3px solid grey;
         }


         #authorization
         {
           width: 20%;
           float: left;
           margin-top: 0;
           margin-left: 15px;
           border: 1px solid black;
         }
         #authorization span
         {
            margin-left: 2%;
            display:  inline-block;
            width: 10%;
         }
         #authorization input
         {
            display: inline-block;
            margin-left: 15%;
            width: 50%;
         }
         #authorization input[type="button"]
         {
            margin-left: 5%;
         }


         #notice
         {
            margin-left: 15px;
            margin-top: 0;
            clear: both;
            border: 1px solid black;
            width: 20%;
            overflow: hidden;
            float: left;
         }
         #results
         {
            margin-left: 23%;
            margin-top: 0;
            width: 75%;
            overflow: hidden;
         }
         #toComeIn
         {
            margin-left: 5%;
         }
         #join
         {
            margin-left: 7%;
         }

         #noticeTitle, #authorizationTitle
         {
            color: white;
            text-align: center;
            margin: 0;
            background-color: rgb(39, 170, 226);
         }

         #search
         {
            margin-left: 0;
         }
         #searchPlace
         {
            margin: 5px;
            height: 20px;
            width: 20%;
         }
         #search button
         {
            height: 28px;
            width: 80px;
         }

         #registration
         {
            z-index: 150;
            display: none;

            width: 30%;
            height: 50%;
            position: absolute;
            left: 35%;
            top: 30%;
            border: 1px solid black;
            padding: 0;
            border-radius: 20px;
            background-color: rgb(39, 170, 226);
         }
         #registration p
         {
            font-size: 1.5em;
            color: white;
            text-align: center;
         }
         #closeCross
         {
            cursor: pointer;
            margin-left: 94%;
            margin-top: 0;
         }
         #registration input
         {
            width: 70%;
            margin-left: 10px;
            margin-right: 10px;
            margin-top: 15px;
            margin-bottom: 15px;
            height: 100%;
            position: relative;
            z-index: 170;
         }
         #registration table td
         {
            background-color: rgb(39, 170, 226);
            border: none;
         }

         #blackout
         {
            background-color: rgba(1, 1, 1, 0.75);
            position: fixed;
            left: 0;
            right: 0;
            top: 0;
            bottom: 0;

            z-index: 100;
            display: none;
         }

         #message-blackout
         {
            background-color: rgba(1, 1, 1, 0.75);
            position: fixed;
            left: 0;
            right: 0;
            top: 0;
            bottom: 0;

            z-index: 100;
         }

         .warnings
         {
            color: white;
            background-color: rgb(224, 45, 45);
            display: inline-block;
            height: 40px;
            width: 316px;
            position: absolute;
            margin-left: -320px;
            margin-top: 1px;
            font-size: 11px;
            padding: 0;
            z-index: 160;
         }
         #warning1, #warning2, #warning3, #warning4
         {
            display: none;
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
         .note .column1
         {
            text-align: left;
            width: 90%;
         }
         .note .column2
         {
            text-align: left;
         }
         .note .column2 input
         {
            width: 100%;
         }
         .note td
         {
            border: none;
         }
         .note table #title
         {
            font-size: 1.5em;
         }
         .note table #quantity
         {
            font-size: 1.2em;
         }
         .note input[type="text"]
         {
            display: none;
         }


         .message
         {
            color: white;
            width: 40%;
            border: 1px solid black;
            border-radius: 10px;
            font-family: Verdana, Geneva, Tahoma, sans-serif;
            background-color: rgb(89, 187, 230);
            padding: 10px;

            position: absolute;
            left: 30%;
            top: 20%;
         }
         .messageTitle
         {
            margin-top: 0;
            text-align: center;
         }
         .messageText
         {
            text-align: center;
         }
         .message input
         {
            width: 20%;
            margin-left: 40%;
         }

         #login-error
         {
            z-index: 300;
            display: block;
         }

         #message-delete-note
         {
            z-index: 300;
            display: block;
         }
      </style>

      <script>
         // Функция показа
         function show(state) {
            document.getElementById("registration").style.display = state;
            document.getElementById("blackout").style.display = state;
            document.getElementById("userSurname").focus();
         }
      </script>
      <script>
         // Закрыть окно ошибки
         function close_error()
         {
            document.getElementById("login-error").style.display = 'none';
            document.getElementById("message-blackout").style.display = 'none';
         }
      </script>
      <script>
         // Закрыть окно уведомления
         function close_message()
         {
            document.getElementById("login-error").style.display = 'none';
            document.getElementById("message-blackout").style.display = 'none';
         }
      </script>
      <script>
         function close_error()
         {
            document.getElementById("message-delete-note").style.display = 'none';
            document.getElementById("message-blackout").style.display = 'none';
         }
      </script>
      <script type="text/javascript" src="https://ajax.googleapis.com/ajax/libs/jquery/1.10.1/jquery.min.js"></script>
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
         <tr id="secondRaw">
            <!--Поисковик-->
            <td colspan="4">
               <form id="search" method="post">
                  <input type="text" placeholder="Поиск" id="searchPlace" name="searchPlace" required
                     autofocus>
                  <button type="submit" title="Начать поиск" name="searchStart">Поиск</button>
               </form>
            </td>
         </tr>
      </table>

      
      <?php if($_SESSION['login'] == ""): ?>
      
         <!--Форма для авторизации-->
         <form id="authorization" method="post"> 
            <p id="authorizationTitle">Вход в систему</p>
            <p title="укажите свой логин"><span>Логин:</span> <input type="text" id="loginAuth" name="loginAuth" onkeyup="auth_check()"></p>
            <p title="укажите пароль от профиля"><span>Пароль:</span> <input type="password" id="passAuth" name="passAuth" onkeyup="auth_check()"></p>
            <p><button id="toComeIn" name="toComeIn" type="submit" disabled>Войти</button> <input id="join" type="button" onclick="show('block')" value="Зарегистрироваться" title="пройти регистрацию"></p>
         </form>
         
      <?php else: ?>
         <!--Для зарегистрированных пользоватлей-->
         <!--Уведомления-->
         <div id="notice">
            <p id="noticeTitle">Уведомления</p>
            <p>Уведомление 1</p>
            <p>Уведомление 2</p>
            <p>Уведомление 3</p>
         </div>

      <?php endif; ?>

      <?php
         // Скрипт авторизации на сайте 
         if(isset($_POST['toComeIn']))
         {
            // Получаем данные из формы
            $login = filter_var(trim($_POST['loginAuth']), FILTER_SANITIZE_STRING);
            $password = filter_var(trim($_POST['passAuth']), FILTER_SANITIZE_STRING);
            // хэширование пароля
            $password = md5($password."cgfhjibv");
            // Подключение к БД
            $mysqli = new mysqli("localhost", "root", "root", "register-bd");
            // Результат запроса к БД
            $result = $mysqli->query("SELECT * FROM `users` WHERE `login` = '$login' AND `password` = '$password'");
            // Преобразовываем в массив
            $user = mysqli_fetch_assoc($result);
            if(count($user) != 0)
            {
               // Запоминаем данные пользователя
               $_SESSION['login'] = $user['login'];
               $_SESSION['username'] = $user['Name'];
               $_SESSION['surname'] = $user['Surname'];
               $_SESSION['admin_status'] = $user['admin_status'];
            }
            else 
            {
               // Вывод ошибки
               echo 
                  "<div class='message' id='login-error'>".
                     "<p class='messageTitle'>Ошибка</p>".
                     "<p class='messageText'>Пользователь с указанным логином не зарегистрирован в системе, перепроверьте данные</p>".
                     "<input type='button' value='ОК' id='okButton' onclick='close_error()'>".
                  "</div>".
                  "<div id='message-blackout'></div>";
            }
            mysqli_close($mysqli);
         }
      ?>

      
      <!--Результаты поиска-->
      <div id="results">
         <?php
            if(isset($_POST['searchStart']))
            {
               // Информация для поиска
               $data = $_POST['searchPlace'];
               // Подключение к БД
               $mysqli = new mysqli("localhost", "root", "root", "books-bd");
               // Результат запроса к БД по всем полям ведем поиск
               $result = $mysqli->query("SELECT * FROM `catalog` WHERE `author` LIKE '%$data%' OR `title` LIKE '%$data%' OR `publishing` LIKE '%$data%' OR `year` = '$data' OR `isbn` = '$data'");
               
               if(mysqli_num_rows($result) == 0)
               {
                  echo "<p>"."Не найдено соответствий"."</p>";
               }
               else
               {
                  while ($data_array = mysqli_fetch_assoc($result))
                  {
                     if ($_SESSION['login'] != "" && $_SESSION['admin_status'] == "1")
                     {
                        echo '<div class="note">'.
                                 "<form method='post'>".
                                 "<table>".
                                    "<tr>".
                                       "<td id='title' class='column1'>".$data_array['title']." / ".$data_array['author']."</td>".
                                       "<td class='column2'><input type='button' value='Редактировать запись' id='editNote'></td>".
                                    "</tr>".
                                    "<tr>".
                                       "<td class='column1'>"."Издательство: ".$data_array['publishing']."</td>".
                                       "<td class='column2'><input type='submit' value='Удалить запись' id='deleteNote' name='deleteNote'></td>".
                                    "</tr>".
                                    "<tr>".
                                       "<td class='column1' colspan='2'>"."ISBN: ".$data_array['isbn']."</td>".
                                    "</tr>".
                                    "<tr>".
                                       "<td id='quantity' colspan='2' class='column1'>"."Доступно экземпляров: ".$data_array['count']."</td>".
                                    "</tr>".
                                 "</table>".
                                 "<input type='text' value='".$data_array['id']."' name='id'>".
                                 "</form>".
                              "</div>";
                     }
                     else 
                     {
                           echo '<div class="note">'.
                                 "<table>".
                                    "<tr>".
                                       "<td id='title' class='column1'>".$data_array['title']." / ".$data_array['author']."</td>".
                                       "<td class='column2'><input type='button' value='Забронировать экземпляр' id='makeReserv');'></td>".
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
               }

               // Разрыв соединения с БД
               mysqli_close($mysqli);
            }
         ?>
      </div>

      <!--Удаление записи из каталога-->
      <?php
         if(isset($_POST['deleteNote']))
         {
            // Получаем id записи в каталоге
            $id = $_POST['id'];
            // Подключение к БД
            $mysqli = new mysqli("localhost", "root", "root", "books-bd");
            // Запрос на удаление
            $mysqli->query("DELETE FROM `catalog` WHERE `id` = '$id'");
            // Прекращение соединения
            mysqli_close($mysqli);

            // Вывод уведомления
            echo
               "<div class='message' id='message-delete-note'>".
                  "<p class='messageTitle'>Уведомление</p>".
                  "<p class='messageText'>Запись успешно удалена из каталога</p>".
                  '<input type="button" value="ОК" id="okButton" onclick="close_error()">'.
               "</div>".
               "<div id='message-blackout'></div>";
         }
      ?>

   
      <!--Затемнение-->
      <div id="blackout"></div>


      <!--Регистарция-->
      <form id="registration" class="form js-validate" action="check.php" method="post">
         <img src="cross.png" title="закрыть" alt="закрыть" width="20px" id="closeCross" onclick="show('none')">
         <p>Информация для регистрации</p>
         <table>
            <tr>
               <td><input data-rule="data" type="text" placeholder="Фамилия" id="userSurname" class="undef" name="userSurname"><span class="warnings" id="warning1">Неверный формат: вводите только буквы</span></td>
            </tr>
            <tr>
               <td><input data-rule="data" type="text" placeholder="Имя" id="userName" class="undef" name="userName"><span class="warnings" id="warning2">Неверный формат: вводите только буквы</span></td>
            </tr>
            <tr>
               <td><input data-rule="email" type="text" placeholder="Логин - уникальное имя" id="userMail" class="undef" name="userMail"><span class="warnings" id="warning3">Неверный формат: допустимы только цифры и латиница</span></td>
            </tr>
            <tr>
               <td><input data-rule="pass" type="password" placeholder="Пароль" id="userPass" class="undef" name="userPass"><span class="warnings" id="warning4">Поле обязательно для заполнения</span></td>
            </tr>
            <tr>
               <td><button type="submit" id="regButton" title="Подтвердить регистрацию" disabled>Регистарция</button></td>
            </tr>
         </table>
      </form>
               



   <script>
       // перебираем все поля формы регистрации
      
      let inputs = document.querySelectorAll('input[data-rule]');
         
      for (let input of inputs) {
            input.addEventListener('blur', function () {
               let rule = this.dataset.rule;
               let value = this.value; // содержимое инпута
               let check;

               var dataPattern = new RegExp("[A-Za-zА-Яа-яёЁ]+$");
               var reg = /^([a-zA-Z0-9_\-\.])/;

               switch(rule) // обработка зависит от типа инпута
               {
                  case "data": 
                     check = dataPattern.test(value);
                     break;
                  case "email":
                     check = reg.test(value);
                     break;
                  case "pass":
                     check = (value != "");
                     break;
               }
               
               this.classList.remove("valid");


               var thisId = this.id;
               if(check)
               {
                  this.className = "valid";
                  switch(thisId)
                  {
                     case "userSurname":
                        document.getElementById("warning1").style.display = "none";
                        break;
                     case "userName":
                        document.getElementById("warning2").style.display = "none";
                        break;
                     case "userMail":
                        document.getElementById("warning3").style.display = "none";
                        break;
                     case "userPass":
                        document.getElementById("warning4").style.display = "none";
                        break;
                  }
               }
               else
               {
                  this.classList.remove("undef");
                  switch (thisId) {
                     case "userSurname":
                        document.getElementById("warning1").style.display = "inline-block";
                        break;
                     case "userName":
                        document.getElementById("warning2").style.display = "inline-block";
                        break;
                     case "userMail":
                        document.getElementById("warning3").style.display = "inline-block";
                        break;
                     case "userPass":
                        document.getElementById("warning4").style.display = "inline-block";
                        break;
                  }
               }
               checkForms();

            });
         }

         function checkForms()
         {
            var surname = $('#userSurname');
            var name = $('#userName');
            var mail = $('#userMail');
            var pass = $('#userPass');

            if (surname.attr('class') == "valid" && name.attr('class') == "valid" && mail.attr('class') == "valid" && pass.attr('class') == "valid")
               $('#regButton').removeAttr('disabled');
            else
               $('#regButton').attr('disabled', 'disabled');
         }
   </script>
      
   <script>
      // Проверка формы авторизации на заполненность
      function auth_check()
      {
         var login = document.getElementById("loginAuth").value;
         var password = document.getElementById("passAuth").value;

         
         if (login != "" && password != "")
            $('#toComeIn').removeAttr('disabled');
         else
               $('#toComeIn').attr('disabled', 'disabled');
      }
   </script>
   </body>
</html>