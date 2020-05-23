using System;
using System.Collections.Generic;
using System.Text;

namespace Fractional_Calc
{
    // Абстрактный класс-шаблон "память"
    abstract class TMemory<T>
    {
        // Режимы работы памяти выключена\включена
        protected string[] MODE = { "_Off", "_On" };
        // Текущий режим работы памяти
        protected string FState;
        // Значение для FNumber по умолчанию
        protected virtual T default_value { get; }
        // Значение, хранящеся в памяти
        protected T FNumber;

        // Конструктор класса
        // Устанавливаем режим работы и значениие в FNumber по умлочанию
        public TMemory()
        {
            // Устанавливаем память в состояние "выключена"
            FState = MODE[0];
            // Устанавливаем в память значение по умолчанию
            FNumber = default_value;
        }

        // Функция "записать"
        // Записывает значение @data в FNumber
        // Режим устанавливается "включена"
        public void MSave(T data)
        {
            FNumber = data;
            FState = MODE[1];
        }

        // Функция "взять"
        // выгружает копию объекта из FNumber, если там есть значение
        public T MRead()
        {
            // Проверяем, включена ли память
            if (FState == MODE[0])
                throw new Exception("Error: memory mode off");
            return FNumber;
        }

        // Функциия "Добавить"
        // В поле FNumber записывается объект типа T,
        // который является результатом сложения FNumber и data
        public abstract void MAdd(T data);

        // Функциия "очистить"
        // В поле FNumber устанавливается значение по умолчанию
        public void MClear()
        {
            FNumber = default_value;
            // Устанавливаем память в режим "выключена"
            FState = MODE[0];
        }

        // Функция Чтения состояния памяти
        // Возвращает FState
        public string ReadMemoryState()
        {
            return FState;
        }

    }

    // Память конкретно для реализациии калькулятора простых дробей
    class FracMemory : TMemory<TFrac>
    {
        // Определение значения по умолчанию для конкретной реализации
        // дробь 0\1
        protected override TFrac default_value
        {
            get
            {
                return new TFrac(0, 1);
            }
        }
        // Реализуем операцию "добавить"
        public override void MAdd(TFrac data)
        {
            FNumber += data;
            // Установим память в режим "включена", 
            // если до этого была выключена
            if (FState == MODE[0])
                FState = MODE[1];
        }
    }
}