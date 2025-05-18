using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Geometryclass;

namespace lab12._3
{
    class Program
    {
        static void Main(string[] args)
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

                switch (Console.ReadLine())
                {
                    case "1":
                        Console.Write("Введите размер дерева: ");
                        int size = int.Parse(Console.ReadLine());
                        tree.CreateBalancedTree(size);
                        Console.WriteLine("\nИсходное дерево:");
                        tree.PrintTree(original: true);
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
                        tree.ConvertToAVLTree();
                        Console.WriteLine("\nДерево преобразовано в АВЛ-дерево поиска");
                        Console.WriteLine("\nИсходное дерево:");
                        tree.PrintTree(original: true);
                        Console.WriteLine("\nАВЛ-дерево:");
                        tree.PrintTree();
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
                        break;

                    case "0":
                        exit = true;
                        break;

                    default:
                        Console.WriteLine("Неверный ввод");
                        break;
                }
            }
        }

        static Geometryfigure1 CreateFigure(string type)
        {
            switch (type)
            {
                case "1":
                    Console.Write("Ширина: ");
                    double w = double.Parse(Console.ReadLine());
                    Console.Write("Длина: ");
                    double l = double.Parse(Console.ReadLine());
                    return new Rectangle1(w, l);
                case "2":
                    Console.Write("Радиус: ");
                    double r = double.Parse(Console.ReadLine());
                    return new Circle1(r);
                case "3":
                    Console.Write("Высота: ");
                    double hp = double.Parse(Console.ReadLine());
                    Console.Write("Ширина: ");
                    double wp = double.Parse(Console.ReadLine());
                    Console.Write("Длина: ");
                    double lp = double.Parse(Console.ReadLine());
                    return new Parallelepiped1(hp, wp, lp);
                default:
                    Console.WriteLine("Неверный выбор");
                    return null;
            }
        }
    }
}