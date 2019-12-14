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
   $scope.IdOfTasks = 1;
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
   $scope.numOfCurrentDesk = -1;
   $scope.numTaskDelete = -1;

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
   $scope.processingAddTask = function (desk_id, val) {
      // проверка, указано ли название задачи
      if (typeof $scope.taskName === 'undefined' || $scope.taskName.length === 0) {
         $scope.showErrorAddTaskName = true;
         $scope.showBlackout = true;
      }
      else if ($scope.taskDate.getFullYear() < 2019 || ($scope.taskDate.getFullYear() == 2019 && ($scope.taskDate.getDate() < 14 || $scope.taskDate.getMonth() < 11)))
      {
         $scope.showErrorAddTaskDate = true;
         $scope.showBlackout = true;
      }
      else
      {
         // Создаем объект с информацией о задаче
         var task = {
            id: $scope.desks[desk_id].tasks.length,
            name: $scope.taskName,
            importance: val,
            date : {
               day: $scope.taskDate.getDate(),
               month: $scope.taskDate.getMonth() + 1,
               year: $scope.taskDate.getFullYear(),
               hour: $scope.taskDate.getHours(),
               minute: $scope.taskDate.getMinutes()
            },
            description: $scope.description
         }
         // Добавление к соответствующей доске
         $scope.desks[desk_id].tasks.push(task);
         // Закрываем окно добавления и очищаем его
         $scope.showAddTaskBlock = false;
         $scope.taskName = '';
         $scope.taskDate = '';
         $scope.val = 3;
      }
   }

   // Показ подробной информации по задаче
   $scope.showInfoAboutTask = function (desk_id, task_id) {
      // открыть окно
      $scope.showTaskInfo = true;
      // Загрузить в него информацию
      var task = $scope.desks[desk_id].tasks[task_id];
      $scope.taskInfoTitle = task.name;
      $scope.taskInfoDate = task.date.day.toString() + ':' + task.date.month.toString() + ':' + task.date.year.toString() + '\tВремя: ' + task.date.hour.toString() + ':' + task.date.minute.toString();
      $scope.taskInfoImport = task.importance.toString();
      $scope.taskInfoDescrip = task.description;

      $scope.taskInfoId = task_id;
      $scope.taskInfoDeskId = desk_id;
   }
   // Закрытие окна показа подробной информации по задаче
   $scope.closeInfoAboutTask = function () {
      $scope.showTaskInfo = false;
   }


   // Закрытия сообщения об ошибке при добавлении задачи
   $scope.closeWindowErrorAddTaskName = function () {
      $scope.showErrorAddTaskName = false;
      $scope.showBlackout = !$scope.showBlackout;
   }
   $scope.closeWindowErrorAddTaskDate = function () {
      $scope.showErrorAddTaskDate = false;
      $scope.showBlackout = !$scope.showBlackout;
   }

   // Сортировка задач на доске по степени важности
   $scope.sortTasksByImportance = function (desk_id) {
      // Выбираем задачи с указанной доски
      var arr = $scope.desks[desk_id].tasks;

      // Сортировка вставками
      for (let i = 1; i < arr.length; i++)
      {
         const temp = arr[i];
         let j = i;
         while (j > 0 && arr[j - 1].importance < temp.importance)
         {
            arr[j] = arr[j - 1];
            j--;
         }
         arr[j] = temp;
      }
      // Вставляем на место отсортированный массив
      $scope.desks[desk_id].tasks = arr;
   }

   // Сортировка задач на доске по порядку добавления
   $scope.sortTasksByAdding = function (desk_id) {
      // Выбираем задачи с указанной доски
      var arr = $scope.desks[desk_id].tasks;

      // Сортировка вставками
      for (let i = 1; i < arr.length; i++) {
         const temp = arr[i];
         let j = i;
         while (j > 0 && arr[j - 1].id > temp.id) {
            arr[j] = arr[j - 1];
            j--;
         }
         arr[j] = temp;
      }
      // Вставляем на место отсортированный массив
      $scope.desks[desk_id].tasks = arr;
   }

   // Сортировка задач на доске по срокам сдачи
   $scope.sortTasksByTime = function (desk_id) {
      // Выбираем задачи с указанной доски
      var arr = $scope.desks[desk_id].tasks;

      // Сортировка вставками
      for (let i = 1; i < arr.length; i++) {
         const temp = arr[i];
         let j = i;
         while (j > 0 && ((arr[j - 1].date.hour > temp.date.hour) || (arr[j-1].date.hour == temp.date.hour && arr[j-1].date.minute > temp.date.minute))){
            arr[j] = arr[j - 1];
            j--;
         }
         arr[j] = temp;
      }
      // Вставляем на место отсортированный массив
      $scope.desks[desk_id].tasks = arr;
   }

   // 
   $scope.openWindowAddTaskBlock = function (desk_id) {
      $scope.addTaskDeskId = desk_id;

      var now = new Date(2019, 11, 14);
      $scope.taskDate = now;
      $scope.showAddTaskBlock = true;
   }
   $scope.closeWindowAddtaskBlock = function () {
      $scope.showAddTaskBlock = false;
   }


   // Удаление задачи с доски
   $scope.deleteTaskFromDesk = function (desk_id, task_id) {
      $scope.numOfCurrentDesk = desk_id;
      $scope.numTaskDelete = task_id;

      $scope.showConfirmDeleteTask = true;
      $scope.showBlackout = true;  
   }
   $scope.confirmDeleteTask = function () {
      // Удлаение данных из массива
      $scope.desks[$scope.numOfCurrentDesk].tasks.splice($scope.numTaskDelete, 1);
      // Перенумерация оставшихся на доске задач
      for (let i = $scope.numTaskDelete; i < $scope.desks[$scope.numOfCurrentDesk].tasks.length; i++)
         $scope.desks[$scope.numOfCurrentDesk].tasks[i].id--;
      $scope.showSuccesDeleteTask = true;
      $scope.showConfirmDeleteTask = false;
   }
   $scope.cnacelDeleteTask = function () {
      $scope.showConfirmDeleteTask = false;
      $scope.showBlackout = false;
   }
   $scope.closeWindowSuccesDeleteTask = function () {
      $scope.showSuccesDeleteTask = false;
      $scope.showBlackout = false;
      $scope.showTaskInfo = false;
   }


   // Открыть окно редактирования задачи
   $scope.openWindowEditTask = function (task_id, desk_id) {
      var task = $scope.desks[desk_id].tasks[task_id];

      $scope.showWindowEditTask = true;
      $scope.editTaskInfoTitle = $scope.taskInfoTitle;
      $scope.editTaskInfoDate = new Date(task.date.year.toString(), task.date.month.toString() - 1, task.date.day.toString(), task.date.hour.toString(), task.date.minute.toString());
      $scope.editTaskInfoImport = parseInt($scope.taskInfoImport);
      $scope.editTaskInfoDescrip = task.description;
   }
   // Сохранить изменения
   $scope.saveEditTaskChange = function (desk_id, task_id) {

      var task = $scope.desks[desk_id].tasks[task_id];

      task.name = $scope.editTaskInfoTitle;
      task.importance = $scope.editTaskInfoImport;
      task.date.day = $scope.editTaskInfoDate.getDate();
      task.date.month = $scope.editTaskInfoDate.getMonth() + 1;
      task.date.year = $scope.editTaskInfoDate.getFullYear();
      task.date.hour = $scope.editTaskInfoDate.getHours();
      task.date.minute = $scope.editTaskInfoDate.getMinutes();
      task.description = $scope.editTaskInfoDescrip;
   }

   // Закрыть окно редактирования задачи
   $scope.closeWindowEditTask = function () {
      $scope.showWindowEditTask = false;
   }

});