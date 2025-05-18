using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Geometryclass;

namespace lab12._3
{
    public class BinaryTree
    {
        private class TreeNode
        {
            public Geometryfigure1 Data;
            public TreeNode Left;
            public TreeNode Right;
            public int Height; // Высота поддерева

            public TreeNode(Geometryfigure1 data)
            {
                Data = data;
                Left = null;
                Right = null;
                Height = 1; // Начальная высота нового узла
            }
        }
        private int GetHeight(TreeNode node)
        {
            return node?.Height ?? 0;
        }

        private void UpdateHeight(TreeNode node)
        {
            if (node != null)
                node.Height = 1 + Math.Max(GetHeight(node.Left), GetHeight(node.Right));
        }
        private int GetBalanceFactor(TreeNode node)
        {
            if (node == null) return 0;
            return GetHeight(node.Left) - GetHeight(node.Right);
        }
        public void ConvertToAVLTree()
        {
            // Собираем все элементы дерева в отсортированный список
            List<Geometryfigure1> elements = new List<Geometryfigure1>();
            InOrderTraversal(root, elements);
            elements.Sort((a, b) => a.CompareTo(b));

            // Строим сбалансированное АВЛ-дерево из отсортированного списка
            root = BuildAVLTree(elements, 0, elements.Count - 1);
        }

        private TreeNode BuildAVLTree(List<Geometryfigure1> elements, int start, int end)
        {
            if (start > end) return null;

            int mid = (start + end) / 2;
            TreeNode node = new TreeNode(elements[mid]);

            node.Left = BuildAVLTree(elements, start, mid - 1);
            node.Right = BuildAVLTree(elements, mid + 1, end);

            // Обновляем высоту и балансируем узел
            UpdateHeight(node);
            return Balance(node);
        }

        // Правый поворот (LL)
        private TreeNode RightRotate(TreeNode y)
        {
            TreeNode x = y.Left;
            TreeNode T2 = x.Right;

            x.Right = y;
            y.Left = T2;

            UpdateHeight(y);
            UpdateHeight(x);

            return x;
        }

        // Левый поворот (RR)
        private TreeNode LeftRotate(TreeNode x)
        {
            TreeNode y = x.Right;
            TreeNode T2 = y.Left;

            y.Left = x;
            x.Right = T2;

            UpdateHeight(x);
            UpdateHeight(y);

            return y;
        }
        private TreeNode Balance(TreeNode node)
        {
            if (node == null) return null;

            UpdateHeight(node);
            int balanceFactor = GetBalanceFactor(node);

            // LL-случай
            if (balanceFactor > 1 && GetBalanceFactor(node.Left) >= 0)
                return RightRotate(node);

            // RR-случай
            if (balanceFactor < -1 && GetBalanceFactor(node.Right) <= 0)
                return LeftRotate(node);

            // LR-случай
            if (balanceFactor > 1 && GetBalanceFactor(node.Left) < 0)
            {
                node.Left = LeftRotate(node.Left);
                return RightRotate(node);
            }

            // RL-случай
            if (balanceFactor < -1 && GetBalanceFactor(node.Right) > 0)
            {
                node.Right = RightRotate(node.Right);
                return LeftRotate(node);
            }

            return node;
        }

        private TreeNode root;
        private TreeNode originalRoot; // Сохраняем исходное дерево
        private Random rnd = new Random();

        public void CreateBalancedTree(int size)
        {
            root = BuildBalancedTree(size);
            originalRoot = CloneTree(root); // Сохраняем копию исходного дерева
        }

        private TreeNode BuildBalancedTree(int size)
        {
            if (size == 0) return null;

            int leftSize = size / 2;
            int rightSize = size - leftSize - 1;

            Geometryfigure1 figure = GetRandomFigure();
            Console.WriteLine($"Добавлена фигура: {figure}"); 

            TreeNode node = new TreeNode(figure);
            node.Left = BuildBalancedTree(leftSize);
            node.Right = BuildBalancedTree(rightSize);

            return node;
        }

        private TreeNode CloneTree(TreeNode node)
        {
            if (node == null) return null;

            TreeNode newNode = new TreeNode((Geometryfigure1)node.Data.Clone());
            newNode.Left = CloneTree(node.Left);
            newNode.Right = CloneTree(node.Right);

            return newNode;
        }

        private Geometryfigure1 GetRandomFigure()
        {
            int type = rnd.Next(0, 3);
            switch (type)
            {
                case 0:
                    var rect = new Rectangle1(rnd.Next(1, 10), rnd.Next(1, 10));
                    Console.WriteLine($"Добавлен прямоугольник: Ширина={rect.Width}, Высота={rect.Length}");
                    return rect;

                case 1:
                    var circle = new Circle1(rnd.Next(1, 10));
                    Console.WriteLine($"Добавлена окружность: Радиус={circle.Radius}");
                    return circle;

                case 2:
                    var paral = new Parallelepiped1(rnd.Next(1, 10), rnd.Next(1, 10), rnd.Next(1, 10));
                    Console.WriteLine($"Добавлен параллелепипед: Высота={paral.Height}, Ширина={paral.Width}, Длина={paral.Length}");
                    return paral;

                default:
                    return new Rectangle1(1, 1);
            }
        }
        public void PrintTree(bool original = false)
        {
            TreeNode currentRoot = original ? originalRoot : root;
            if (currentRoot == null)
            {
                Console.WriteLine("Дерево пустое");
                return;
            }

            Queue<TreeNode> queue = new Queue<TreeNode>();
            queue.Enqueue(currentRoot);

            int level = 0;
            while (queue.Count > 0)
            {
                Console.Write($"Уровень {level}: ");
                int levelSize = queue.Count;

                for (int i = 0; i < levelSize; i++)
                {
                    TreeNode current = queue.Dequeue();
                    Console.Write(current.Data + " | ");

                    if (current.Left != null) queue.Enqueue(current.Left);
                    if (current.Right != null) queue.Enqueue(current.Right);
                }

                Console.WriteLine();
                level++;
            }
        }

        public int CountLeaves(bool original = false)
        {
            return CountLeaves(original ? originalRoot : root);
        }

        private int CountLeaves(TreeNode node)
        {
            if (node == null) return 0;
            if (node.Left == null && node.Right == null) return 1;
            return CountLeaves(node.Left) + CountLeaves(node.Right);
        }

        public void ConvertToSearchTree()
        {
            List<Geometryfigure1> elements = new List<Geometryfigure1>();
            InOrderTraversal(root, elements);
            elements.Sort((a, b) => a.CompareTo(b));
            root = BuildSearchTree(elements, 0, elements.Count - 1);
        }

        private TreeNode BuildSearchTree(List<Geometryfigure1> elements, int start, int end)
        {
            if (start > end) return null;

            int mid = (start + end) / 2;
            TreeNode node = new TreeNode(elements[mid]);

            node.Left = BuildSearchTree(elements, start, mid - 1);
            node.Right = BuildSearchTree(elements, mid + 1, end);

            return node;
        }

        private void InOrderTraversal(TreeNode node, List<Geometryfigure1> elements)
        {
            if (node == null) return;
            InOrderTraversal(node.Left, elements);
            elements.Add(node.Data);
            InOrderTraversal(node.Right, elements);
        }

        public bool DeleteNode(Geometryfigure1 key)
        {
            int initialCount = CountLeaves();
            root = DeleteNode(root, key);
            return initialCount != CountLeaves();
            
        }

        private TreeNode DeleteNode(TreeNode node, Geometryfigure1 key)
        {
            if (node == null) return null;

            int cmp = key.CompareTo(node.Data);
            if (cmp < 0)
                node.Left = DeleteNode(node.Left, key);
            else if (cmp > 0)
                node.Right = DeleteNode(node.Right, key);
            else
            {
                if (node.Left == null) return node.Right;
                if (node.Right == null) return node.Left;

                TreeNode temp = FindMin(node.Right);
                node.Data = temp.Data;
                node.Right = DeleteNode(node.Right, temp.Data);
            }
            return Balance(node); // Балансировка после удаления
        }

        private TreeNode FindMin(TreeNode node)
        {
            while (node.Left != null) node = node.Left;
            return node;
        }
        public void Insert(Geometryfigure1 data)
        {
            root = Insert(root, data);
        }

        private TreeNode Insert(TreeNode node, Geometryfigure1 data)
        {
            if (node == null) return new TreeNode(data);

            int cmp = data.CompareTo(node.Data);
            if (cmp < 0)
                node.Left = Insert(node.Left, data);
            else if (cmp > 0)
                node.Right = Insert(node.Right, data);
            else
                return node; // Дубликаты не допускаются

            return Balance(node); // Балансировка после вставки
        }

    }
}


