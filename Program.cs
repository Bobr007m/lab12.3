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
            // 1. Создаем дерево поиска
            Point searchTree = null;
            searchTree = Add(searchTree, 5);
            Add(searchTree, 3);
            Add(searchTree, 7);
            Add(searchTree, 2);
            Add(searchTree, 4);
            Add(searchTree, 6);
            Add(searchTree, 8);

            Console.WriteLine("Дерево поиска:");
            ShowTree(searchTree, 0);

            // 2. Создаем идеально сбалансированное дерево
            Console.WriteLine("\nВведите размер идеально сбалансированного дерева:");
            int size = int.Parse(Console.ReadLine());

            Point balancedTree = IdealTree(size, null);

            Console.WriteLine("\nИдеально сбалансированное дерево:");
            ShowTree(balancedTree, 0);
        }
    }
}
