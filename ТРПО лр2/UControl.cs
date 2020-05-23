using System;
using System.Collections.Generic;
using System.Text;

namespace Fractional_Calc
{
    // Состояния калькулятора
    enum TCtrlState
    {
        cStart,             // Начальное 
        cEditing,           // Ввод и редактирование
        cFunDone,           // Функция выполнена
        cValDone,           // Значение введено
        cExpDone,           // Выражение вычислено
        cOpChange,          // Операция измененаы
        cErrorDivByZero,    // Ошибка деления на ноь
        cErrorOverflow,     // Ошибка переполнения
        cMemoryWriteDone,   // Было загружено значение в память
        cMemoryReadDone     // Было выгружено значение из памяти
    }

    // Номер команды калькулятора
    enum NumCalcCmd : byte
    {
        Backspace,      // Забой символа в поле редактора
        ClearEditor,    // Очистить поле редактора
        ClearAll,       // Сбросить калькулятор
        MemorySave,     // Сохранить значение в память
        MemoryRead,     // Прочитать значение из памяти
        MemoryAdd,      // Прибавить значение в память
        MemoryClear,    // Очистить память
        AddDigit,       // Ввод цифры в поле редактора
        AddZero,        // Ввод нуля в поле редактора
        AddSep,         // Ввод разделителя
        ChangeSign,     // Сменить знак редактируемого числа
        Square,         // Возвести число в квадрат
        Reverse,        // Найти обратное число
        SetAdd,         // Выбрать операцию сложения
        SetSub,         // Выбрать операцию вычитания
        SetMul,         // Выбрать операцию умножения
        SetDiv,         // Выбрать операцию деления
        Evaluate,       // Вычислить выражение
        ReadEditor,     // Получить строковое значениие из редактора
        WriteEditor,    // Записать строковое значение в поле радектора
        ReadState       // Получить состояние калькулятора
    }



    // Класс управления калькулятором
    class TCtrl
    {
        // Последняя выполненная операция
        TOprtn LastOperation;

        // Текущее состояние калькулятора
        TCtrlState TState;

        // Экземплляр класса редактор
        TEditor ED;
        // Экземпляр класса процессор
        FracProc FP;
        // Экземпляр класса память
        FracMemory FM;
        // Экземпляр класса число, результат последней команды
        TFrac Number;

        // Конструктор класса
        public TCtrl()
        {
            // Устанавливаем начальное состояние
            TState = TCtrlState.cStart;
            LastOperation = TOprtn.None;

            // Инициализируем агрегированные объекты
            ED = new TEditor();
            FP = new FracProc();
            FM = new FracMemory();
            Number = new TFrac(0, 1);
        }

        // Выполнить команду калькулятора
        // @a - номер команды
        public string DoCalculatorCommand(NumCalcCmd a, params object[] d)
        {
            switch (a)
            {
                case NumCalcCmd.Backspace:
                    // Забой символа в поле редактора
                    return DoEditorCommand(8);

                case NumCalcCmd.ClearEditor:
                    // Очистить поле редактора
                    return DoEditorCommand(9);

                case NumCalcCmd.ClearAll:
                    // Сбросить калькулятор
                    return ResetCalculator();

                case NumCalcCmd.MemorySave:
                    // Сохранить значение в память
                    return DoMemoryCommand(1);

                case NumCalcCmd.MemoryRead:
                    // Прочитать значение из памяти
                    return DoMemoryCommand(2);

                case NumCalcCmd.MemoryAdd:
                    // Прибавить значение в память
                    return DoMemoryCommand(3);

                case NumCalcCmd.MemoryClear:
                    // Очистить память
                    return DoMemoryCommand(4);

                case NumCalcCmd.AddDigit:
                    // Ввод цифры в поле редактора
                    return DoEditorCommand(5, (int)d[0]);

                case NumCalcCmd.AddZero:
                    // Ввод нуля в поле редактора
                    return DoEditorCommand(6);

                case NumCalcCmd.AddSep:
                    // Ввод разделителя
                    return DoEditorCommand(7);

                case NumCalcCmd.ChangeSign:
                    // Сменить знак редактируемого числа
                    return DoEditorCommand(4);

                case NumCalcCmd.Square:
                    // Возвести число в квадрат
                    return EvalFunction(TFunc.Square);

                case NumCalcCmd.Reverse:
                    // Найти обратное число
                    return EvalFunction(TFunc.Reverse);

                case NumCalcCmd.SetAdd:
                    // Выбрать операцию сложения
                    return ChooseProcOperation(TOprtn.Add);

                case NumCalcCmd.SetSub:
                    // Выбрать операцию вычитания
                    return ChooseProcOperation(TOprtn.Sub);

                case NumCalcCmd.SetMul:
                    // Выбрать операцию умножения
                    return ChooseProcOperation(TOprtn.Mul);

                case NumCalcCmd.SetDiv:
                    // Выбрать операцию деления
                    return ChooseProcOperation(TOprtn.Div);

                case NumCalcCmd.Evaluate:
                    // Вычислить выражение
                    return EvalExpression();

                case NumCalcCmd.ReadEditor:
                    return DoEditorCommand(1);

                case NumCalcCmd.WriteEditor:
                    Number = new TFrac(d[0].ToString());
                    return DoEditorCommand(2, Number.GetFractionString());

                case NumCalcCmd.ReadState:
                    if (TState == TCtrlState.cErrorOverflow)
                        return "OverflowError";
                    else if (TState == TCtrlState.cErrorDivByZero)
                        return "DivideByZeroError";
                    else
                        return "WithoutError";

                default:
                    throw new Exception("Error: invalid num of operation");
            }
        }

        // "Выполнить команду редактора"
        // Управляет объектом типа TEditor, 
        // получает номер команды и входные данные для 
        // этой операции, если необходимо
        private string DoEditorCommand(params object[] args)
        {
            string result = System.String.Empty;
            // Если читаем состояние редактора
            if ((int)args[0] == 1)
            {
                try
                {
                    Number = new TFrac(ED.DoEdit(args));
                }
                catch
                {
                    Number = new TFrac(0, 1);
                }
                return ED.DoEdit(1);
            }
            // Если нужно сменить знак, то редактор не очиищается
            if ((int)args[0] == 4)
            {
                Number = new TFrac(ED.DoEdit(args));
                return Number.GetFractionString();
            }
            // Очиищаем редактор в следующиих случаях:
            //  1. Если операция уже выбрана
            //  2. Если левый операнд уже загружен
            //  3. Если выражение уже вычислено
            //  4. Если была вычислена функция
            //  5. Из памяти было загружено значение
            //  6. Произошла исключительная ситуация
            if (TState != TCtrlState.cEditing && TState != TCtrlState.cStart)
                ED.DoEdit(9);

            result = ED.DoEdit(args);
            // Сохраняем последнюю выполненную операцю в Number
            // при условии, что вид редактируемой дроби был изменен
            if ((int)args[0] >= 4 && (int)args[0] <= 9)
                Number = new TFrac(ED.DoEdit(1));
            // Изменяем состояние на "ввод и редактирование"
            TState = TCtrlState.cEditing;
            return result;
        }

        // "Выбрать операцию процессора"
        // @a - номер операции
        // 
        private string ChooseProcOperation(TOprtn a)
        {
            // Если операция не была выбрана
            if (FP.ReadOperation() == TOprtn.None)
            {
                // Записываем значение
                FP.WriteLop(Number);
                FP.WriteOperation(a);
                // Изменяем состояние на "значение введено"
                TState = TCtrlState.cValDone;
            }
            // Если меняем ранее выбранную операцию на другую
            else if (Number.IsEqual(FP.ReadLop()))
            {
                // Перезаписываем операцию
                FP.WriteOperation(a);
                // Изменяем состояние на "операция изменена"
                TState = TCtrlState.cOpChange;
            }
            else
            // Если выполняем сложное выражение
            {
                // Загружаем из редактора в правый операнд
                FP.WriteRop(Number);
                // Выполняем выыбранную ранее операцию
                FP.OperationRun();
                // Сохраняем результат выполнения
                Number = FP.ReadLop();
                // Записываем результат в редактор
                ED.DoEdit(2, Number.GetFractionString());
                // Устанавливаем новую операцию
                FP.WriteOperation(a);
                // Изменяем состояние на "операция изменена"
                TState = TCtrlState.cOpChange;

            }
            return Number.GetFractionString();
        }

        // "Вычислить выражение"
        private string EvalExpression()
        {
            // Загружаем данные из редактора в правый операнд,
            // при условии, что он еще не был туда загружен
            if (TState != TCtrlState.cExpDone && TState != TCtrlState.cMemoryReadDone)
                FP.WriteRop(Number);
            // Повторное выполнение последней операции
            if (FP.ReadOperation() == TOprtn.None)
                FP.WriteOperation(LastOperation);

            // Вычисляем выражение
            try
            {
                FP.OperationRun();
            }
            // При ошибке деленя на ноль
            catch (DivideByZeroException)
            {
                // Меняем состояние на "произошла исключительная ситуация"
                TState = TCtrlState.cErrorDivByZero;
                return Number.GetFractionString();
            }
            // При ошибке переполнения
            catch (OverflowException)
            {
                TState = TCtrlState.cErrorOverflow;
                return Number.GetFractionString();
            }
            // Меняем состояние на "выражение вычислено"
            TState = TCtrlState.cExpDone;

            // Сброс выполненной операции
            LastOperation = FP.ReadOperation();
            FP.OperationReset();

            // Считываем результат
            Number = FP.ReadLop();
            ED.DoEdit(2, Number.GetFractionString());
            return Number.GetFractionString();
        }

        // Вычислить функкцию
        // @a - номер выполняемой функции
        private string EvalFunction(TFunc a)
        {
            // Записываем значение из редактора
            FP.WriteRop(Number);

            // Вычисляем функцию 
            try
            {
                FP.FunctionRun(a);
            }
            // При ошибке переполнения
            catch(OverflowException)
            {
                // Меняем состояние на "произошла исключительная ситуация"
                TState = TCtrlState.cErrorOverflow;
                return Number.GetFractionString();
            }
            // При работе с дробями, имеющимии ноль в знаменателе
            catch(DivideByZeroException)
            {
                // Меняем состояние на "произошла исключительная ситуация"
                TState = TCtrlState.cErrorDivByZero;
                return Number.GetFractionString();
            }
            // Меняем состояние на "функция вычислена"
            TState = TCtrlState.cFunDone;
            // Сохраняем результат
            Number = FP.ReadRop();
            // Ззагружаем новое значение в редактор
            ED.DoEdit(2, Number.GetFractionString());
            return Number.GetFractionString();
        }

        // Выполнить команду памяти
        // @a - номер команды
        private string DoMemoryCommand(int a)
        {
            switch (a)
            {
                // Сохраняем значение из редактора в память
                case 1:
                    // Читаем текущее значение из редактора
                    FM.MSave(new TFrac(ED.DoEdit(1)));
                    // Меняем состояние на "значение загружено в память"
                    TState = TCtrlState.cMemoryWriteDone;
                    return FM.ReadMemoryState();

                // Читаем значение из памяти в редактор
                case 2:
                    Number = FM.MRead();
                    // Сохраняем значениие в редактор
                    ED.DoEdit(2, Number.GetFractionString());
                    // Если выгружаем из памяти не внутри выражения
                    if (TState != TCtrlState.cOpChange && TState != TCtrlState.cValDone)
                    {
                        FP.WriteLop(Number);
                        // Меняем состояние на "значение выгружено из памяти"
                        TState = TCtrlState.cMemoryReadDone;
                    }
                    return Number.GetFractionString();

                // Добавляем к текущему значениюю в памяти
                case 3:
                    // Добавляем значение
                    FM.MAdd(Number);
                    // Меняем состояние на "значение загружено в память"
                    TState = TCtrlState.cMemoryWriteDone;
                    // Заменяем на результат последнего действия
                    return Number.GetFractionString();

                // Очиищаем память
                case 4:
                    FM.MClear();
                    return FM.ReadMemoryState();

                default:
                    throw new Exception("Error: invalid num of operation");
            }
        }

        // Сброс калькулятора
        private string ResetCalculator()
        {
            // Очистка редактора
            ED.DoEdit(9);
            // Сброс процессора до начального состояния
            FP.ProcReset();
            // Очистка и выключение памяти
            FM.MClear();

            Number = new TFrac(0, 1);
            // Устанавливаем начальное состояние
            TState = TCtrlState.cStart;
            LastOperation = TOprtn.None;

            return Number.GetFractionString();
        }

    }
}