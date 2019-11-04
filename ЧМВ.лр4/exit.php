<?php
   session_start();

   session_unset();
   session_destroy();

   // Возврат на главную
   header('Location: /');
?>