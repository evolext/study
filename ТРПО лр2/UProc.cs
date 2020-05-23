using System;
using System.Collections.Generic;
using System.Text;

namespace Fractional_Calc
{
    // Перечисления, обозначающие операции процессора
    enum TOprtn : byte
    {
        None,   // Операциия не установлена
        Add,    // Операция сложениия
        Sub,    // Операция вычитания
        Mul,    // Операция умножения
        Div     // Операция деления
    }

    // Перечисления, обозначающие функции процессора
    enum TFunc : byte
    {
        Reverse = 1,    // Функция инвертирования
        Square          // Функция вычисления квадрата значения
    }

    // Абстрактный класс-шаблон "Процессор"
    abstract class TProc<T>
    {
        // Левый операнд и операнд-значение
        protected T Lop_Res;
        // Правый операнд
        protected T Rop;
        // Установленная операция
        protected TOprtn Operation;

        // Значениие по умолчанию
        protected virtual T default_value { get; }

        // Конструктор класса
        public TProc()
        {
            // Инициализируем поля значениями по умолчанию
            Lop_Res = default_value;
            Rop = default_value;
            // Изначально операция не установлена
            Operation = TOprtn.None;
        }

        // Операция "Сброс процессора"
        // Восстанавливает поля к исходным значенииям
        public void ProcReset()
        {
            Lop_Res = default_value;
            Rop = default_value;
            Operation = TOprtn.None;
        }

        // "Сброс операции"
        public void OperationReset()
        {
            Operation = TOprtn.None;
        }

        // Операции "Выполнить операцию" и "Вычислить функцию"
        // Реализация зависиит от типа T
        public abstract void OperationRun();

        public abstract void FunctionRun(TFunc Func);


        // Операция "Читать левый операнд"
        // Создает и возвращает копию из Lop_Res
        public T ReadLop()
        {
            return Lop_Res;
        }

        // Операция "Записать левый операнд"
        // Записывает значение из @data в Lop_Res
        public void WriteLop(T data)
        {
            Lop_Res = data;
        }

        // Операция "Читать правый операнд"
        // Создает и возвращает копию из Rop
        public T ReadRop()
        {
            return Rop;
        }

        // Операция "Записать правый операнд"
        // Записывает значение из @data в Rop
        public void WriteRop(T data)
        {
            Rop = data;
        }

        // Операция "Читать состояние"
        // Возвращает значение поля Operation
        public TOprtn ReadOperation()
        {
            return Operation;
        }

        // Операция "Записать состояние"
        public void WriteOperation(TOprtn oprtn)
        {
            Operation = oprtn;
        }
    }


    // Реализация класса "Прроцессор" для типа "простая дробь"
    class FracProc : TProc<TFrac>
    {
        // Определение значения по умолчанию
        // дробь 0\1
        protected override TFrac default_value
        {
            get
            {
                return new TFrac(0, 1);
            }
        }

        // Операция "Выполнить операцию"
        // Выполнение операциии, указанной в Operation над Lop_Res и Rop
        public override void OperationRun()
        {
            switch (Operation)
            {
                case TOprtn.None:
                    // Операция не определена, никаких действий не предусматривается
                    break;
                case TOprtn.Add:
                    // Операция сложения
                    Lop_Res += Rop;
                    break;
                case TOprtn.Sub:
                    // Операция вычитания
                    Lop_Res -= Rop;
                    break;
                case TOprtn.Mul:
                    // Операция умножения
                    Lop_Res *= Rop;
                    break;
                case TOprtn.Div:
                    // Операция деления
                    Lop_Res /= Rop;
                    break;
                default:
                    // Если операция установлена некорректно
                    throw new Exception("Error: invalid operation");
            }
        }

        // Операция "Вычислить функкцию"
        // Выполняется функция @Func над Rop
        public override void FunctionRun(TFunc Func)
        {
            switch (Func)
            {
                case TFunc.Reverse:
                    // Операция инвертирования
                    Rop = Rop.Reverse();
                    break;
                case TFunc.Square:
                    // Операция возведения в квадрат
                    Rop = Rop.Square();
                    break;
                default:
                    // Еслии операция установлена некорректно
                    throw new Exception("Error: invalid function");
            }
        }

    }
}