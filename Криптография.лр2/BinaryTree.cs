using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace encryption
{
    // Класс, реализующий узел бинарного дерева
    // Бинарное дерево используется для декодирования
    class Elem
    {
        public int Key;     // Ключ, имеет значения: -1 для корня, 0 и 1 для остальных
        public Elem Left;   // Укзаатель на левого потомка
        public Elem Right;  // Указатель на правого потомка
        public Elem Parent; // Указатель на родителя

        // конструктор класса
        public Elem(int k)
        {
            this.Key = k;
            this.Left = null;
            this.Right = null;
            this.Parent = null;
        }
    }


    // Класс, реализующий бинарное дерево, 
    // нужное для декодирования
    class BinaryTree
    {
        // Корень дерева
        public Elem Root; 
        // Конструктор
        public BinaryTree()
        {
            Root = null;
        }

        // Добавление нового элемента
        public void Insert(string code)
        {
            // Получаем первый элемент шифра
            int k = Convert.ToInt32(code[0]) - 48; 

            Elem pointer = null;

            // Создаем элемент дерева для вставки перовго символа
            var T = new Elem(k);

            if (Root == null) // Если дерево еще не инициализованно
            {
                Root = new Elem(-1);
                if (T.Key == 0)
                {
                    T.Parent = Root;
                    Root.Left = T;
                    pointer = Root.Left;
                }
                else
                {
                    T.Parent = Root;
                    Root.Right = T;
                    pointer = Root.Right;
                }
            }
            // Если в дереве уже есть один или более элементов
            else 
            {
                pointer = this.Root;

                if (T.Key == 0)
                {
                    if (pointer.Left != null)
                        pointer = pointer.Left;
                    else
                    {
                        T.Parent = Root;
                        Root.Left = T;
                        pointer = Root.Left;
                    }
                }
                else
                {
                    if (pointer.Right != null)
                        pointer = pointer.Right;
                    else
                    {
                        T.Parent = Root;
                        Root.Right = T;
                        pointer = Root.Right;
                    }
                }

            }

            // Вставляем остальные символы шифра в дерево
            for (int i = 1; i < code.Length; i++)
            {
                k = Convert.ToInt32(code[i]) - 48;
                var q = new Elem(k);

                if (q.Key == 0)
                {
                    if (pointer.Left != null)
                        pointer = pointer.Left;
                    else
                    {
                        q.Parent = pointer;
                        pointer.Left = q;
                        pointer = pointer.Left;
                    }
                }
                else
                {
                    if (pointer.Right != null)
                        pointer = pointer.Right;
                    else
                    {
                        q.Parent = pointer;
                        pointer.Right = q;
                        pointer = pointer.Right;
                    }
                }
            }
        }

    }
}
