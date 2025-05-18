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
                Console.WriteLine("5. Преобразовать в дерево поиска");
                Console.WriteLine("6. Удалить элемент");
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
                        tree.ConvertToSearchTree();
                        Console.WriteLine("\nДерево преобразовано в дерево поиска");
                        Console.WriteLine("\nИсходное дерево:");
                        tree.PrintTree(original: true);
                        Console.WriteLine("\nПреобразованное дерево:");
                        tree.PrintTree();
                        break;

                    case "6":
                        Console.WriteLine("\nВыберите тип фигуры для удаления:");
                        Console.WriteLine("1 - Прямоугольник");
                        Console.WriteLine("2 - Окружность");
                        Console.WriteLine("3 - Параллелепипед");
                        Console.Write("Ваш выбор: ");
                        string type = Console.ReadLine();

                        Geometryfigure1 figure;
                        switch (type)
                        {
                            case "1":
                                Console.Write("Ширина: ");
                                double w = double.Parse(Console.ReadLine());
                                Console.Write("Длина: ");
                                double l = double.Parse(Console.ReadLine());
                                figure = new Rectangle1(w, l);
                                break;
                            case "2":
                                Console.Write("Радиус: ");
                                double r = double.Parse(Console.ReadLine());
                                figure = new Circle1(r);
                                break;
                            case "3":
                                Console.Write("Высота: ");
                                double hp = double.Parse(Console.ReadLine());
                                Console.Write("Ширина: ");
                                double wp = double.Parse(Console.ReadLine());
                                Console.Write("Длина: ");
                                double lp = double.Parse(Console.ReadLine());
                                figure = new Parallelepiped1(hp, wp, lp);
                                break;
                            default:
                                Console.WriteLine("Неверный выбор");
                                continue;
                        }

                        if (tree.DeleteNode(figure))
                        {
                            Console.WriteLine("\nЭлемент удален. Текущее дерево:");
                            tree.PrintTree();
                        }
                        else
                        {
                            Console.WriteLine("Элемент не найден");
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
    }
}
