<?php
   session_start();
?>
<!DOCTYPE html>
<html>
   <head>
      <meta charset="UTF-8">
      <title>Регистрация / авторизация</title>
      <link rel="stylesheet" href="style.css">
      <link rel="shortcut icon" href="book.png" type="image/png">
      <link href="https://fonts.googleapis.com/css?family=Arimo&display=swap" rel="stylesheet">
      <link href="https://fonts.googleapis.com/css?family=Roboto+Slab&display=swap" rel="stylesheet">
      <script type="text/javascript" src="https://ajax.googleapis.com/ajax/libs/jquery/1.10.1/jquery.min.js"></script>
      <style>
         .valid
         {
            border: 3px solid green;
         }
         .undef
         {
            border: 3px solid grey;
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
            display: block;
         }

         #authorization
         {
            width: 30%;
            font-size: 1.2em;
            color: white;

            position: absolute;
            display: block;
            z-index: 200;
            left: 35%;
         }
         #authorization table td
         {
            padding: 10px;
            border: none;
         }
         #authorization table .column1
         {
            padding-left: 30px;
            text-align: left;
         }
         #authorization table .column2
         {
            padding-left: 15px;
            text-align: left;
         }
         #authorization table .column2 input
         {
            width: 90%;
         }
         #authorization table .column1 input
         {
            width: 60%;
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

         #return
         {
            cursor: pointer;
            position: relative;
            left: 94%;
            top: 15px;
         }

         .error
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
         #errorTitle
         {
            margin-top: 0;
            text-align: center;
         }
         #mainText
         {
            text-align: center;
         }
         #login-error input
         {
            width: 20%;
            margin-left: 40%;
         }

         #login-error
         {
            z-index: 300;
            display: block;
         }
      </style>

      <script>
         // Функция показа формы регистрации
         function show(state) 
         {
            if (state == 'auth')
            {
               document.getElementById("registration").style.display = 'none';
               document.getElementById("authorization").style.display = 'block';
            }
            else
            {
               document.getElementById("registration").style.display = 'block';
               document.getElementById("authorization").style.display = 'none';
               document.getElementById("userSurname").focus();
            }     
         }
      </script>
      <script>
         function close_error()
         {
            document.getElementById("login-error").style.display = 'none';
            document.getElementById("error-blackout").style.display = 'none';
         }
      </script>
   </head>
   <body>
      <!--Шапка сайта-->
      <header>
         <p><a href=""><img src="book.png" width="70px"></a>Новосибирский книжный каталог</p>
      </header>
      <table id="siteHead">
         <tr>
            <td id="col1" title="Перейти на главную страницу"><a href="">Главная</a></li></td>
            <td id="userMenu">
               <p>Личный кабинет</p>
            </td>
            <td id="col3" title="Поиск по нескольким полям"><a href="">Расширенный поиск</a></td>
            <td id="col4">
            </td>
         </tr>
      </table>


       <!--Затемнение-->
      <div id="blackout"></div>

      <!--Форма для авторизации-->
      <form id="authorization" method="post">
         <a href="index.php"><img src="return.png" title="вернуться назад" alt="на главную" width="20px" id="return"></a>
         <table>
            <tr>
               <td colspan="2">Вход в систему</td>
            </tr>
            <tr>
               <td class="column1">Логин:</td>
               <td class="column2"><input type="text" id="loginAuth" name="loginAuth" onkeyup="auth_check()" autofocus></td>
            </tr>
            <tr>
               <td class="column1">Пароль:</td>
               <td class="column2"><input type="password" id="passAuth" name="passAuth" onkeyup="auth_check()"></td>
            </tr>
            <tr>
               <td class="column1"><input id="toComeIn" name="toComeIn" type="submit" value="Войти" disabled></td>
               <td class="column2"><input id="join" type="button" onclick="show('regitr')" value="Зарегистрироваться" title="пройти регистрацию"></td>         
            </tr>
         </table>
      </form>

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
               
               // Возврат на страницу
               header('Location: /');
            }
            else 
            {
               // Вывод ошибки
               echo 
                  "<div class='error' id='login-error'>".
                     "<p id='errorTitle'>Ошибка</p>".
                     "<p id='mainText'>Пользователь с указанным логином не зарегистрирован в системе, перепроверьте данные</p>".
                     "<input type='button' value='ОК' id='okButton' onclick='close_error()'>".
                  "</div>".
                  "<div id='error-blackout'></div>";
            }
            mysqli_close($mysqli);

            
         }
      ?>



      <!--Форма для регистарции-->
      <form id="registration" class="form js-validate" action="check.php" method="post">
         <img src="cross.png" title="закрыть" alt="закрыть" width="20px" id="closeCross" onclick="show('auth')">
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
