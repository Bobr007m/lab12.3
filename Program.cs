using System;
using System.Collections.Generic;
using Geometryclass;

namespace lab12._3
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BinaryTree tree = new BinaryTree();
            bool exit = false;

            while (!exit)
            {
                Console.WriteLine("\nМеню:");
                Console.WriteLine("1. Создать идеально сбалансированное дерево");
                Console.WriteLine("2. Показать исходное дерево (по уровням)");
                Console.WriteLine("3. Показать преобразованное дерево (по уровням)");
                Console.WriteLine("4. Найти количество листьев в текущем дереве");
                Console.WriteLine("5. Преобразовать в АВЛ-дерево поиска");
                Console.WriteLine("6. Добавить элемент в АВЛ-дерево");
                Console.WriteLine("7. Удалить элемент из АВЛ-дерева");
                Console.WriteLine("0. Выход");
                Console.Write("Выберите действие: ");

                string input = Console.ReadLine();
                switch (input)
                {
                    case "1":
                        Console.Write("Введите размер дерева (положительное целое число): ");
                        if (int.TryParse(Console.ReadLine(), out int size) && size > 0)
                        {
                            tree.CreateBalancedTree(size);
                            Console.WriteLine("\nИсходное дерево:");
                            tree.PrintTree(original: true);
                        }
                        else
                        {
                            Console.WriteLine("Ошибка: введите положительное целое число");
                        }
                        break;

                    case "2":
                        Console.WriteLine("\nИсходное дерево:");
                        tree.PrintTree(original: true);
                        break;

                    case "3":
                        Console.WriteLine("\nПреобразованное дерево:");
                        tree.PrintTree();
                        break;

                    case "4":
                        Console.WriteLine($"\nКоличество листьев: {tree.CountLeaves()}");
                        break;

                    case "5":
                        if (tree.CountLeaves() > 0)
                        {
                            tree.ConvertToAVLTree();
                            Console.WriteLine("\nДерево преобразовано в АВЛ-дерево поиска");
                            Console.WriteLine("\nИсходное дерево:");
                            tree.PrintTree(original: true);
                            Console.WriteLine("\nАВЛ-дерево:");
                            tree.PrintTree();
                        }
                        else
                        {
                            Console.WriteLine("Ошибка: дерево пустое");
                        }
                        break;

                    case "6":
                        Console.WriteLine("\nВыберите тип фигуры для добавления:");
                        Console.WriteLine("1 - Прямоугольник");
                        Console.WriteLine("2 - Окружность");
                        Console.WriteLine("3 - Параллелепипед");
                        Console.Write("Ваш выбор: ");
                        string addType = Console.ReadLine();
                        Geometryfigure1 addFigure = CreateFigure(addType);
                        if (addFigure != null)
                        {
                            tree.Insert(addFigure);
                            Console.WriteLine("\nЭлемент добавлен. Текущее дерево:");
                            tree.PrintTree();
                        }
                        break;

                    case "7":
                        if (tree.CountLeaves() > 0)
                        {
                            Console.WriteLine("\nВыберите тип фигуры для удаления:");
                            Console.WriteLine("1 - Прямоугольник");
                            Console.WriteLine("2 - Окружность");
                            Console.WriteLine("3 - Параллелепипед");
                            Console.Write("Ваш выбор: ");
                            string delType = Console.ReadLine();
                            Geometryfigure1 delFigure = CreateFigure(delType);
                            if (delFigure != null)
                            {
                                if (tree.DeleteNode(delFigure))
                                {
                                    Console.WriteLine("\nЭлемент удален. Текущее дерево:");
                                    tree.PrintTree();
                                }
                                else
                                {
                                    Console.WriteLine("Элемент не найден");
                                }
                            }
                        }
                        else
                        {
                            Console.WriteLine("Ошибка: дерево пустое");
                        }
                        break;

                    case "0":
                        exit = true;
                        break;

                    default:
                        Console.WriteLine("Неверный ввод. Пожалуйста, выберите пункт меню от 0 до 7");
                        break;
                }
            }
        }

        public static Geometryfigure1 CreateFigure(string type)
        {
            try
            {
                switch (type)
                {
                    case "1":
                        Console.Write("Ширина (положительное число): ");
                        if (!double.TryParse(Console.ReadLine(), out double w) || w <= 0)
                            throw new ArgumentException("Некорректная ширина");
                        Console.Write("Длина (положительное число): ");
                        if (!double.TryParse(Console.ReadLine(), out double l) || l <= 0)
                            throw new ArgumentException("Некорректная длина");
                        return new Rectangle1(w, l);

                    case "2":
                        Console.Write("Радиус (положительное число): ");
                        if (!double.TryParse(Console.ReadLine(), out double r) || r <= 0)
                            throw new ArgumentException("Некорректный радиус");
                        return new Circle1(r);

                    case "3":
                        Console.Write("Высота (положительное число): ");
                        if (!double.TryParse(Console.ReadLine(), out double hp) || hp <= 0)
                            throw new ArgumentException("Некорректная высота");
                        Console.Write("Ширина (положительное число): ");
                        if (!double.TryParse(Console.ReadLine(), out double wp) || wp <= 0)
                            throw new ArgumentException("Некорректная ширина");
                        Console.Write("Длина (положительное число): ");
                        if (!double.TryParse(Console.ReadLine(), out double lp) || lp <= 0)
                            throw new ArgumentException("Некорректная длина");
                        return new Parallelepiped1(hp, wp, lp);

                    default:
                        Console.WriteLine("Неверный выбор типа фигуры");
                        return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
                return null;
            }
        }
    }
}