<?php
   session_start();
?>
<!DOCTYPE html>
   <head>
      <meta charset="UTF-8">
      <title>Добавление книги</title>
      <link rel="stylesheet" href="style.css">
      <link href="https://fonts.googleapis.com/css?family=Arimo&display=swap" rel="stylesheet">
      <link href="https://fonts.googleapis.com/css?family=Roboto+Slab&display=swap" rel="stylesheet">
      <style>
         div
         {
            margin-left: 20%;
            margin-right: 20%;
            padding: 0;
         }
         div p
         {
            text-align: center;
            margin-bottom: 15px;
         }
         form
         {
            border: 1px solid black;
         }
         #formTitle
         {
            color: white;
            margin-top: 0;
            background-color: rgb(39, 170, 226);
         }
         .column1
         {
            width: 20%;
            padding: 5px;
            text-align: left;
         }
         .column2 input
         {

            width: 90%;
         }
         button[type="submit"]
         {
            margin: 10px 0;
         }
         form table td
         {
            border: none;
         }

         #addingBook td
         {
            padding-top: 17px;
         }
         #addingBook input
         {
            position: relative;
            z-index: 200;
         }

         .warnings
         {
            color: white;
            background-color: rgb(235, 34, 34);
            display: inline-block;
            position: absolute;
            height: 40px;
            z-index: 100;
            margin-left: -630px;
            margin-top: -15px;
            width: 638px;
            font-size: 13px;
            padding: 0;
         }
         #warning1, #warning2, #warning3, #warning4, #warning5
         {
            display: none;
         }
         .valid
         {
            border: 3px solid green;
         }
         .undef
         {
            border: 3px solid grey;
         }
         #siteHead
         {
            position: relative;
            z-index: 1000;
         }




      </style>
      <script src="http://code.jquery.com/jquery-1.8.3.js"></script>
      <script src="jquery.maskedinput.min.js"></script>
   </head>
   <body>
      <header>
         <p><a href="index.html"><img src="book.png" width="70px"></a>Новосибирский книжный каталог</p>
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

      <div>
         <p>Добавление товара</p>
         <form action="adding_verify.php" method="post">
            <p id="formTitle">Информация о товаре</p>
            <table id="addingBook">
               <tr title="Укажите одного или более авторов">
                  <td class="column1">Авторы/авторы:</td>
                  <td class="column2"><input type="text" id="authors" name="authors" class="undef" data-rule="text"><span class="warnings" id="warning1">Поле обязательно для заполнения, вводите только инициалы, фамилии и имена</span></td>
               </tr>
               <tr title="Укажите заглавие книги">
                  <td class="column1">Название книги:</td>
                  <td class="column2"><input type="text" id="title" name="title" class="undef" data-rule="all"><span class="warnings" id="warning2">Поле обязательно для заполнения</span></td>
               </tr>
               <tr title="Укажите название издательства">
                  <td class="column1">Издательство:</td>
                  <td class="column2"><input type="text" id="publishing" name="publishing" class="undef" data-rule="text"><span class="warnings" id="warning3">Поле обязательно для заполнения</span></td>
               </tr>
               <tr title="Укажите год издания книги">
                  <td class="column1">Год издания:</td>
                  <td class="column2"><input type="text" title="Вводить только четыре цифры" id="year" name="year" class="undef" data-rule="year"><span class="warnings" id="warning4">Поле обязательно для заполнения, вводите только цифры</span></td>
               </tr>
               <tr title="Укажите индивидаульный номер ISBN, обычно, указывается рядом со штрих-кодом">
                  <td class="column1">Номер ISBN:</td>
                  <td class="column2"><input type="text" title="Вводить только цифры" id="isbn" name="isbn" class="undef" data-rule="isbn"><span class="warnings" id="warning5">Поле обязательно для заполнения, вводите только цифры</span></td>
               </tr>
               <tr title="Укажите количество добавляемых в каталог экземпляров книги">
                  <td class="column1">Количество:</td>
                  <td class="column2"><input type="number" value="1" min="1" id="count" name="count"></td>
               </tr>
               <tr>
                  <td colspan="2"><button type="submit" id="addBook" title="Подтвердить добавление" disabled>Добавить</button></td>
               </tr>
            </table>
         </form>

         
      </div>
      




      <script>
          $(function () {
               $('#isbn').mask("999-9-99-999999-9");
               $('#year').mask("9999");
            });

         
         let inputs = document.querySelectorAll('input[data-rule]');

         for (let input of inputs) 
         {
            input.addEventListener('blur', function () {
               let rule = this.dataset.rule;
               let value = this.value; // содержимое инпута
               let check;

               var help1, help2;

               var textPattern = new RegExp("[A-Za-zА-Яа-яёЁ \.]+$");

               switch (rule) // обработка зависит от типа инпута
               {
                  case "text":
                     check = textPattern.test(value);
                     break;
                  case "all":
                     check = (value != "" && value != undefined);
                     break;
                  case "year":
                     value = parseInt(value);
                     help1 = value.toString();
                     help2 = help1.length;

                     check = (help2 == 4);
                     break;
                  case "isbn":
                     help1 = value.replace(/[-_]/g, '');

                     check = (help1.length == 13);
                     break;
               }

               this.classList.remove("valid");
               var thisId = this.id;

               if(check)
               {
                  this.className = "valid";
                  switch(thisId)
                  {
                     case "authors":
                        document.getElementById("warning1").style.display = "none";
                        break;
                     case "title":
                        document.getElementById("warning2").style.display = "none";
                        break;
                     case "publishing":
                        document.getElementById("warning3").style.display = "none";
                        break;
                     case "year": 
                        document.getElementById("warning4").style.display = "none";
                        break;
                     case "isbn": 
                        document.getElementById("warning5").style.display = "none";
                        break;
                  }
               }
               else
               {
                  this.classList.remove("undef");
                  switch (thisId) {
                     case "authors":
                        document.getElementById("warning1").style.display = "inline-block";
                        break;
                     case "title":
                        document.getElementById("warning2").style.display = "inline-block";
                        break;
                     case "publishing":
                        document.getElementById("warning3").style.display = "inline-block";
                        break;
                     case "year":
                        document.getElementById("warning4").style.display = "inline-block";
                        break;
                     case "isbn":
                        document.getElementById("warning5").style.display = "inline-block";
                        break;
                  }
               }

               checkForm();

            });
         }


         function checkForm()
         {
            var authors = $('#authors');
            var title = $('#title');
            var publishing = $('#publishing');
            var year = $('#year');
            var isbn = $('#isbn');

            if (authors.attr('class') == "valid" && title.attr('class') == "valid" && publishing.attr('class') == "valid" && year.attr('class') == "valid" && isbn.attr('class') == "valid")
               $('#addBook').removeAttr('disabled');
            else
               $('#addBook').attr('disabled', 'disabled');


         }



      
      </script>

   </body>
</html>