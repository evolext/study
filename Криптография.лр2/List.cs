using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace encryption
{
    // Класс, реализующий элемент списка, содержит в себе всю информацию о символе
    // Поля класса:
    //              1. Символ(буква), которую кодируем
    //              2. Count - число вхождений символа в заданное сообщение
    //              3. Probability - вероятность появлнеия символа 
    //              4. CumulativeProb - кумулятивная вероятность
    //              5. Code - символ в закодированной виде
    public class Node
    {
        // Поле для хранения самого символа
        public string Symbol { get; set; }
        // Поле для хранения числа вхождений символа в текст
        public int Count { get; set; }
        // Поле для хранения вероятности
        public double Probability { get; set; }
        // Поле для хранения кумулятивной вероятности
        public double CumulativeProb { get; set; }
        // Поле для хранения шифра
        public string Code { get; set; }

        // Указатели на следующий и предыдущий элемент, 
        // реализация интерфейса списка
        public Node Prev { get; set; }
        public Node Next { get; set; }

        // Коструктор
        public Node(string data, int mode)
        {
            if (mode == 1)
                Symbol = data; // Если на вход подали сам символ
            else
                Code = data;   // Если задали его шифр
            Count = 1;
            Probability = .0;
            CumulativeProb = .0;
            Prev = null;
            Next = null;
        }
    }

    // Структура данных двусторонний список, 
    // элементами списка являются кодируемые символы и вся информация о них
    public class List
    {
        // Начало списка
        public Node Head;
        // Конец списка
        public Node Tail;
        // Размер списка = количество элементов
        private int Count;

        // Базовый конструктор
        public List()
        {
            Head = null;
            Tail = null;
            Count = 0;
        }

        // Возврат размера списка
        public int Size
        {
            get => Count;
        }

        // Добавление элемента в список
        public void AddSymbol(string data)
        {
            bool flag = false;

            // Размер списка увеличивается
            this.Count++;

            // Если такой символ уже есть в списке, то просто меняем информацию о количестве его вхождений
            for (var pointer = this.Head; pointer != null && !flag; pointer = pointer.Next)
            {
                if (pointer.Symbol == data)
                {
                    pointer.Count++;
                    flag = true;
                }
            }

            // Если символ не входил до этого в список, то создаем новую запись
            // и добавляем в конец списка
            if (!flag)
            {
                // Создаем новый элемент списка
                Node item = new Node(data, 1);

                // Если список пуст, то элемент будет его началом
                if (this.Head == null)
                    this.Head = item;
                else
                {
                    item.Prev = this.Tail;
                    this.Tail.Next = item;
                }
                // Новый элемент становится последним
                this.Tail = item;
            }
        }

        // Для добавления зашифрованных кодов, по аналогии с добавлением символов
        public void AddCode(string code)
        {
            bool flag = false;
            // Размер списка увеличивается
            // this.Count++;
            for (var pointer = this.Head; pointer != null && !flag; pointer = pointer.Next)
            {
                // Если повторяющийся
                if (pointer.Code == code)
                {
                    //pointer.Count++;
                    flag = true;
                }
            }
            if (!flag)
            {
                // Создаем новый элемент списка
                Node item = new Node(code, 2);

                // Если список пуст
                if (this.Head == null)
                    this.Head = item;
                else
                {
                    item.Prev = this.Tail;
                    this.Tail.Next = item;
                }
                // Новый элемент становится последним
                this.Tail = item;
            }
        }

        // Поиск записи в списке по имеющемуся символу
        public Node FindCode(string key)
        {
            for (var p = this.Head; p != null; p = p.Next)
            {
                if (p.Symbol == key)
                    return p;
            }
            return null;
        }

        // Поиск записи в списке по имеющемуся шифру
        public Node FindSymbol(string code)
        {
            for (var p = this.Head; p != null; p = p.Next)
            {
                if (p.Code == code)
                    return p;
            }
            return null;
        }
    }
}
