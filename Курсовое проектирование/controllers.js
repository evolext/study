myApp.controller('title', ['$scope', function ($scope) {
   $scope.title = 'Менеджер моих задач';
}]);

myApp.controller('formValid', ['$scope', function ($scope) {
   $scope.checkEmpty = function () {
      if ($scope.userLogin.length === 0 || typeof $scope.userLogin === 'undefined') 
      {
         $scope.message = { 'display': 'block'};
      }
   } 
}]);

myApp.controller('myCtrl', function ($scope) {
   // информация о текущем количестве досок
   $scope.IdOfDesks = 1;
   // массив с информацией о досках
   $scope.desks = [];
   $scope.desks.push({ 
      id: 0, // идентификатор доски
      name: 'Личная доска', // название доски
      tasks: [] // массив объектов-задач
   });  
   // Значение для полузнка степени важности задачи
   $scope.val = 3;
   $scope.numDeskDelete = -1;

   // Открытие окна добавления новой доски
   $scope.openWindowAddDesk = function () {
      $scope.showAddDesk = !$scope.showAddDesk;
      $scope.showBlackout = !$scope.showBlackout;
   }
   // Закрытие окна добавления новой доски
   $scope.closeWindowAddDesk = function () {
      $scope.showAddDesk = !$scope.showAddDesk;
      $scope.showBlackout = !$scope.showBlackout;
   }
   // Закрытие окна ошибки
   $scope.closeWindowErrorAddDesk = function () {
      $scope.errorAddDesk = false;
   }
   // Закрытие окна уведомления о добавлении доски и затемнения
   $scope.closeWindowSuccesAddDesk = function () {
      $scope.successAddDesk = false;
      $scope.showBlackout = false;
   }

   $scope.checkData = function () {
      if (typeof $scope.deskName === 'undefined' || $scope.deskName.length === 0) {
         $scope.errorAddDesk = true;
      }
      else {
         // Тут вносим информацию о созданной доске в массив
         $scope.desks.push({
               id: $scope.IdOfDesks++,
               name: $scope.deskName,
               tasks: []
            }
         );
         // Очищаем поле с названием доски
         $scope.deskName = '';
         // Уведомление об упешном создании доски
         $scope.successAddDesk = true;

         // Закрываем окно добавления
         $scope.showAddDesk = false;
      }
   }

   // Функция удаления доски
   $scope.deleteDesk = function (desk_id) {
      // Запрос подтверждение удаления
      $scope.showConfirmDeleteDesk = true;
      $scope.showBlackout = true;

      // Сохраняем номер удаляемой доски
      $scope.numDeskDelete = desk_id;

   }
   // При отмене удаления доски
   $scope.cnacelDeleteDesk = function () {
      // Закрываем окна подтверждения удаления и затемнение
      $scope.showConfirmDeleteDesk = !$scope.showConfirmDeleteDesk;
      $scope.showBlackout = !$scope.showBlackout;
   }
   // При подтверждении удаления доски
   $scope.confirmDeleteDesk = function () {
      // Удаляем нужный объект из массива с информацией о досках
      if ($scope.numDeskDelete >= 0)
      {
         $scope.desks.splice($scope.numDeskDelete, 1);
         $scope.IdOfDesks--;
         // перенумеруем доски
         for (let i = $scope.numDeskDelete; i < $scope.desks.length; i++)
            $scope.desks[i].id--;
      }
         
      $scope.showConfirmDeleteDesk = !$scope.showConfirmDeleteDesk;
      $scope.showSuccesDeleteDesk = true;
   }

   // Закрытия уведомление об удлаении доски
   $scope.closeWindowSuccesDeleteDesk = function () {
      $scope.showSuccesDeleteDesk = false;
      $scope.showBlackout = !$scope.showBlackout;
   }
   

   // Обработка процесса добавления задачи
   $scope.processingAddTask = function (desk_id) {
      // проверка, указано ли название задачи
      if (typeof $scope.taskName === 'undefined' || $scope.taskName.length === 0) {
         alert('Укажите название задачи!');
      }
      else if (typeof $scope.taskDate === 'undefined' || $scope.taskDate.length === 0)
      {
         alert('Укажите срок выполнения задачи!')
      }
      else
      {
         // Создаем объект с информацией о задаче
         var task = {
            name: $scope.taskName,
            importance: $scope.val,
            date : {
               day: $scope.taskDate.getDate(),
               month: $scope.taskDate.getMonth(),
               year: $scope.taskDate.getFullYear(),
               hour: $scope.taskDate.getHours(),
               minute: $scope.taskDate.getMinutes()
            }
         }
         // Добавление к соответствующей доске
         $scope.desks[desk_id].tasks.push(task);

         // Закрываем окно добавления и очищаем его
         $scope.showAddTaskBlock = !$scope.showAddTaskBlock;
         $scope.taskName = '';
         $scope.taskDate = '';
         $scope.val = 3;


      }
   }

});