using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace encryption
{
    // Элемент бинарного дерева
    class Elem
    {
        public int Key;     // Значение узлаЖ -1, 1, 0
        public Elem Left;   // Укзаатель на левого потомка
        public Elem Right;  // Указатель на правого потомка
        public Elem Parent; // Указатель на родителя

        // Конструктор
        public Elem(int k)
        {
            this.Key = k;
            this.Left = null;
            this.Right = null;
            this.Parent = null;
        }
    }

    // Класс, реализующий бинарное дерево,
    // с помощью которого декодируется сообщение
    class BinaryTree
    {
        public Elem Root; // Корень дерева

        // Конструктор
        public BinaryTree()
        {
            Root = null;
        }

        // Вставка нового элемента в дерево
        public void Insert(string code)
        {
            int k = Convert.ToInt32(code[0]) - 48; // Получаем первый элемент
            Elem pointer = null;

            // Создаем элемент дерева для вставки перовго символа
            var T = new Elem(k);

            if (Root == null)
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

            // Вставляем остальные символы в дерево
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