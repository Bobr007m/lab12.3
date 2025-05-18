using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Geometryclass;

namespace lab12._3
{
    public class BinaryTree
    {
        Random rnd = new Random();
        // Вспомогательная функция для генерации случайных данных
        static int GetInfo()
        {
            int info = rnd.Next(0, 100);  // Генерация случайного числа от 0 до 99
            Console.WriteLine("The element {0} is adding...", info);
            return info;
        }

        // Рекурсивная функция создания идеально сбалансированного бинарного дерева
        static Point IdealTree(int size, Point p)
        {
            Point r;
            int nl, nr;  // Количество элементов в левом и правом поддеревьях

            // Базовый случай рекурсии - если размер дерева = 0
            if (size == 0)
            {
                p = null;
                return p;
            }

            // Вычисляем размеры поддеревьев
            nl = size / 2;      // Левое поддерево получает половину элементов
            nr = size - nl - 1; // Правое поддерево - оставшиеся (минус текущий узел)

            // Создаем новый узел со случайным значением
            int d = GetInfo();
            r = new Point(d);

            // Рекурсивно строим левое и правое поддеревья
            r.left = IdealTree(nl, r.left);
            r.right = IdealTree(nr, r.right);

            return r;
        }
        // Выводит бинарное дерево с отступами
        static void ShowTree(Point p, int l)
        {
            if (p != null)
            {
                // Рекурсивно выводим левое поддерево с увеличенным отступом
                ShowTree(p.left, l + 3);

                // Вывод отступов 
                for (int i = 0; i < l; i++)
                    Console.Write(" ");

                // Вывод значения текущего узла
                Console.WriteLine(p.data);

                // Рекурсивно выводим правое поддерево с увеличенным отступом
                ShowTree(p.right, l + 3);
            }
        }

        // Создает новый узел дерева с заданным значением
        static Point MakePoint(int d)
        {
            Point p = new Point(d);
            return p;
        }

        // Добавляет новый узел в бинарное дерево поиска
        static Point Add(Point root, int d)
        {
            Point p = root;  // Текущий узел при обходе
            Point r = null;  // Родительский узел
            bool ok = false; // Флаг обнаружения дубликата

            // Поиск места для вставки 
            while (p != null && !ok)
            {
                r = p; // Запоминаем предыдущий узел

                // Проверка на совпадение значений
                if (d == p.data)
                    ok = true;
                else if (d < p.data)
                    p = p.left;
                else
                    p = p.right;
            }

            // Если значение уже есть в дереве
            if (ok)
                return p; // Возвращаем существующий узел

            // Создаем новый узел
            Point NewPoint = MakePoint(d);

            // Вставляем новый узел в нужное место
            if (d < r.data)
                r.left = NewPoint;
            else
                r.right = NewPoint;

            return NewPoint; // Возвращаем новый узел
        }

        // Формирование первого элемента дерева
        static Point First(int d)
        {
            Point p = new Point(d);
            return p;
        }
        int size = 5;
        Point idTree = null;
        idTree = IdealTree(size, idTree);
        ShowTree(idTree, 3);


    }
}
