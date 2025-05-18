using Geometryclass;
using System.Collections.Generic;
using System;

namespace lab12._3
{
    /// <summary>
    /// Класс для работы с бинарным деревом поиска с балансировкой AVL
    /// </summary>
    public class BinaryTree
    {
        /// <summary>
        /// Внутренний класс узла дерева
        /// </summary>
        private class TreeNode
        {
            public Geometryfigure1 Data;  // Хранимые данные (геометрическая фигура)
            public TreeNode Left;        // Указатель на левое поддерево
            public TreeNode Right;       // Указатель на правое поддерево
            public int Height;           // Высота поддерева с корнем в данном узле

            /// <summary>
            /// Конструктор узла дерева
            /// </summary>
            public TreeNode(Geometryfigure1 data)
            {
                Data = data;
                Left = Right = null;     // Инициализация потомков как null
                Height = 1;              // Высота нового узла всегда 1
            }
        }

        // Приватные поля класса
        private TreeNode root;           // Корень основного дерева
        private TreeNode originalRoot;    // Корень копии исходного дерева
        private Random rnd = new Random(); // Генератор случайных чисел

        #region Вспомогательные методы AVL-дерева

        /// <summary>
        /// Получение высоты узла (с обработкой null-значений)
        /// </summary>
        private int GetHeight(TreeNode node)
        {
            return node?.Height ?? 0;    // Для null возвращаем 0
        }

        /// <summary>
        /// Обновление высоты узла на основе высот потомков
        /// </summary>
        private void UpdateHeight(TreeNode node)
        {
            // Высота = 1 + максимальная из высот поддеревьев
            node.Height = 1 + Math.Max(GetHeight(node.Left), GetHeight(node.Right));
        }

        /// <summary>
        /// Вычисление баланс-фактора узла
        /// </summary>
        private int GetBalanceFactor(TreeNode node)
        {
            // Разница высот левого и правого поддеревьев
            return GetHeight(node.Left) - GetHeight(node.Right);
        }

        #endregion

        #region Основные операции AVL-дерева

        /// <summary>
        /// Правый поворот (для случая LL)
        /// </summary>
        private TreeNode RightRotate(TreeNode y)
        {
            // Фиксируем указатели
            TreeNode x = y.Left;
            TreeNode T2 = x.Right;

            // Выполняем поворот
            x.Right = y;
            y.Left = T2;

            // Обновляем высоты
            UpdateHeight(y);
            UpdateHeight(x);

            return x; // Новый корень поддерева
        }

        /// <summary>
        /// Левый поворот (для случая RR)
        /// </summary>
        private TreeNode LeftRotate(TreeNode x)
        {
            // Фиксируем указатели
            TreeNode y = x.Right;
            TreeNode T2 = y.Left;

            // Выполняем поворот
            y.Left = x;
            x.Right = T2;

            // Обновляем высоты
            UpdateHeight(x);
            UpdateHeight(y);

            return y; // Новый корень поддерева
        }

        /// <summary>
        /// Балансировка узла
        /// </summary>
        private TreeNode Balance(TreeNode node)
        {
            if (node == null) return null;

            // Обновляем высоту текущего узла
            UpdateHeight(node);
            int balanceFactor = GetBalanceFactor(node);

            // Определяем тип дисбаланса
            if (balanceFactor > 1) // Левое поддерево выше
            {
                if (GetBalanceFactor(node.Left) >= 0) // LL-случай
                    return RightRotate(node);
                else // LR-случай
                {
                    node.Left = LeftRotate(node.Left);
                    return RightRotate(node);
                }
            }

            if (balanceFactor < -1) // Правое поддерево выше
            {
                if (GetBalanceFactor(node.Right) <= 0) // RR-случай
                    return LeftRotate(node);
                else // RL-случай
                {
                    node.Right = RightRotate(node.Right);
                    return LeftRotate(node);
                }
            }

            return node; // Балансировка не требуется
        }

        #endregion

        #region Базовые операции с деревом

        /// <summary>
        /// Создание идеально сбалансированного дерева
        /// </summary>
        public void CreateBalancedTree(int size)
        {
            root = BuildBalancedTree(size);
            originalRoot = CloneTree(root); // Сохраняем копию
        }

        /// <summary>
        /// Рекурсивное построение сбалансированного дерева
        /// </summary>
        private TreeNode BuildBalancedTree(int size)
        {
            if (size == 0) return null;

            // Распределение элементов между поддеревьями
            int leftSize = size / 2;
            int rightSize = size - leftSize - 1;

            // Создание нового узла со случайной фигурой
            Geometryfigure1 figure = GetRandomFigure();
            TreeNode node = new TreeNode(figure);

            // Рекурсивное построение поддеревьев
            node.Left = BuildBalancedTree(leftSize);
            node.Right = BuildBalancedTree(rightSize);

            return node;
        }

        /// <summary>
        /// Глубокое копирование дерева
        /// </summary>
        private TreeNode CloneTree(TreeNode node)
        {
            if (node == null) return null;

            // Создаем новый узел с копией данных
            TreeNode newNode = new TreeNode((Geometryfigure1)node.Data.Clone());

            // Рекурсивно копируем поддеревья
            newNode.Left = CloneTree(node.Left);
            newNode.Right = CloneTree(node.Right);

            return newNode;
        }

        /// <summary>
        /// Генерация случайной геометрической фигуры
        /// </summary>
        private Geometryfigure1 GetRandomFigure()
        {
            int type = rnd.Next(0, 3); // Случайный выбор типа фигуры
            switch (type)
            {
                case 0: return new Rectangle1(rnd.Next(1, 10), rnd.Next(1, 10));
                case 1: return new Circle1(rnd.Next(1, 10));
                case 2: return new Parallelepiped1(rnd.Next(1, 10), rnd.Next(1, 10), rnd.Next(1, 10));
                default: return new Rectangle1(1, 1);
            }
        }

        #endregion

        #region Методы работы с деревом

        /// <summary>
        /// Печать дерева по уровням
        /// </summary>
        public void PrintTree(bool original = false)
        {
            TreeNode currentRoot = original ? originalRoot : root;
            if (currentRoot == null)
            {
                Console.WriteLine("Дерево пустое");
                return;
            }

            // Обход в ширину с использованием очереди
            Queue<TreeNode> queue = new Queue<TreeNode>();
            queue.Enqueue(currentRoot);

            int level = 0;
            while (queue.Count > 0)
            {
                Console.Write($"Уровень {level}: ");
                int levelSize = queue.Count;

                // Обработка всех узлов текущего уровня
                for (int i = 0; i < levelSize; i++)
                {
                    TreeNode current = queue.Dequeue();
                    Console.Write(current.Data + " | ");

                    // Добавление потомков в очередь
                    if (current.Left != null) queue.Enqueue(current.Left);
                    if (current.Right != null) queue.Enqueue(current.Right);
                }

                Console.WriteLine();
                level++;
            }
        }

        /// <summary>
        /// Подсчет количества листьев
        /// </summary>
        public int CountLeaves(bool original = false)
        {
            return CountLeaves(original ? originalRoot : root);
        }

        private int CountLeaves(TreeNode node)
        {
            if (node == null) return 0;
            if (node.Left == null && node.Right == null) return 1; // Найден лист
            return CountLeaves(node.Left) + CountLeaves(node.Right);
        }

        #endregion

        #region Преобразования дерева

        /// <summary>
        /// Преобразование в AVL-дерево
        /// </summary>
        public void ConvertToAVLTree()
        {
            // Сбор всех элементов в отсортированный список
            List<Geometryfigure1> elements = new List<Geometryfigure1>();
            InOrderTraversal(root, elements);

            // Построение нового сбалансированного дерева
            root = null;
            foreach (var item in elements)
            {
                root = Insert(root, item); // Вставка с балансировкой
            }
        }

        /// <summary>
        /// Преобразование в обычное дерево поиска
        /// </summary>
        public void ConvertToSearchTree()
        {
            List<Geometryfigure1> elements = new List<Geometryfigure1>();
            InOrderTraversal(root, elements);
            elements.Sort((a, b) => a.CompareTo(b));
            root = BuildSearchTree(elements, 0, elements.Count - 1);
        }

        /// <summary>
        /// Симметричный обход дерева (LNR)
        /// </summary>
        private void InOrderTraversal(TreeNode node, List<Geometryfigure1> elements)
        {
            if (node == null) return;
            InOrderTraversal(node.Left, elements);
            elements.Add(node.Data);
            InOrderTraversal(node.Right, elements);
        }

        /// <summary>
        /// Построение сбалансированного дерева поиска из отсортированного списка
        /// </summary>
        private TreeNode BuildSearchTree(List<Geometryfigure1> elements, int start, int end)
        {
            if (start > end) return null;

            // Средний элемент становится корнем
            int mid = (start + end) / 2;
            TreeNode node = new TreeNode(elements[mid]);

            // Рекурсивное построение поддеревьев
            node.Left = BuildSearchTree(elements, start, mid - 1);
            node.Right = BuildSearchTree(elements, mid + 1, end);

            return node;
        }

        #endregion

        #region Основные операции модификации

        /// <summary>
        /// Вставка элемента с автоматической балансировкой
        /// </summary>
        public void Insert(Geometryfigure1 data)
        {
            root = Insert(root, data);
        }

        private TreeNode Insert(TreeNode node, Geometryfigure1 data)
        {
            if (node == null) return new TreeNode(data);

            // Поиск места для вставки
            int cmp = data.CompareTo(node.Data);
            if (cmp < 0)
                node.Left = Insert(node.Left, data);
            else if (cmp > 0)
                node.Right = Insert(node.Right, data);
            else
                return node; // Дубликаты не допускаются

            return Balance(node); // Балансировка после вставки
        }

        /// <summary>
        /// Удаление элемента с балансировкой
        /// </summary>
        public bool DeleteNode(Geometryfigure1 key)
        {
            int initialCount = CountLeaves();
            root = DeleteNode(root, key);
            return initialCount != CountLeaves(); // Возвращает true, если дерево изменилось
        }

        private TreeNode DeleteNode(TreeNode node, Geometryfigure1 key)
        {
            if (node == null) return null;

            // Поиск удаляемого узла
            int cmp = key.CompareTo(node.Data);
            if (cmp < 0)
                node.Left = DeleteNode(node.Left, key);
            else if (cmp > 0)
                node.Right = DeleteNode(node.Right, key);
            else
            {
                // Узел найден - обработка случаев
                if (node.Left == null) return node.Right;
                if (node.Right == null) return node.Left;

                // У узла есть оба потомка
                TreeNode temp = FindMin(node.Right);
                node.Data = temp.Data;
                node.Right = DeleteNode(node.Right, temp.Data);
            }
            return Balance(node); // Балансировка после удаления
        }

        /// <summary>
        /// Поиск узла с минимальным значением в поддереве
        /// </summary>
        private TreeNode FindMin(TreeNode node)
        {
            while (node.Left != null) node = node.Left;
            return node;
        }

        #endregion
    }
}