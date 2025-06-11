using Geometryclass; 
using System.Collections.Generic;  
using System;  

namespace lab12._3  
{
    /// <summary>
    /// Реализация самобалансирующегося AVL-дерева для хранения геометрических фигур
    /// </summary>
    public class BinaryTree<T> where T : Geometryfigure1, ICloneable, IIni, new()
    {
        // Приватные поля класса
        private TreeNode<T> root;  // Корневой узел дерева 
        private Random rnd = new Random();  

        #region Вспомогательные методы AVL-дерева

        /// <summary>
        /// Получает высоту узла с проверкой на null
        /// </summary>
        /// <param name="node">Узел для проверки</param>
        /// <returns>Высота узла (0 если узел null)</returns>
        private int GetHeight(TreeNode<T> node)
        {
            // Используем null-условный оператор (?.) и оператор объединения null (??)
            return node?.Height ?? 0;  // Если node == null, вернет 0
        }

        /// <summary>
        /// Обновляет высоту узла на основе высот его поддеревьев
        /// </summary>
        /// <param name="node">Узел для обновления</param>
        private void UpdateHeight(TreeNode<T> node)
        {
            // Высота узла = 1 (текущий уровень) + максимальная из высот поддеревьев
            node.Height = 1 + Math.Max(GetHeight(node.Left), GetHeight(node.Right));
        }

        /// <summary>
        /// Вычисляет коэффициент баланса узла
        /// </summary>
        /// <param name="node">Узел для расчета</param>
        /// <returns>Разница высот левого и правого поддеревьев</returns>
        private int GetBalanceFactor(TreeNode<T> node)
        {
            // Положительное значение - левое поддерево выше
            // Отрицательное - правое поддерево выше
            // 0 - поддеревья сбалансированы
            return GetHeight(node.Left) - GetHeight(node.Right);
        }

        #endregion

        #region Основные операции балансировки

        /// <summary>
        /// Выполняет правый поворот вокруг указанного узла (LL-случай)
        /// </summary>
        /// <param name="y">Корень дисбалансированного поддерева</param>
        /// <returns>Новый корень поддерева после поворота</returns>
        private TreeNode<T> RightRotate(TreeNode<T> y)
        {
            // Фиксируем необходимые узлы:
            TreeNode<T> x = y.Left;    // Левый потомок станет новым корнем
            TreeNode<T> T2 = x.Right;  // Правое поддерево x (станет левым поддеревом y)

            // Выполняем поворот:
            x.Right = y;      // Перемещаем y вправо от x
            y.Left = T2;      // Перемещаем T2 влево от y

            // Обновляем высоты (сначала y, потом x, так как x теперь выше)
            UpdateHeight(y);  // Высота y могла измениться
            UpdateHeight(x);  // Высота x точно изменилась

            return x;  // Возвращаем новый корень поддерева
        }

        /// <summary>
        /// Выполняет левый поворот вокруг указанного узла (RR-случай)
        /// </summary>
        /// <param name="x">Корень дисбалансированного поддерева</param>
        /// <returns>Новый корень поддерева после поворота</returns>
        private TreeNode<T> LeftRotate(TreeNode<T> x)
        {
            // Фиксируем необходимые узлы:
            TreeNode<T> y = x.Right;  // Правый потомок станет новым корнем
            TreeNode<T> T2 = y.Left;  // Левое поддерево y (станет правым поддеревом x)

            // Выполняем поворот:
            y.Left = x;      // Перемещаем x влево от y
            x.Right = T2;    // Перемещаем T2 вправо от x

            // Обновляем высоты (сначала x, потом y, так как y теперь выше)
            UpdateHeight(x);  // Высота x могла измениться
            UpdateHeight(y);  // Высота y точно изменилась

            return y;  // Возвращаем новый корень поддерева
        }

        /// <summary>
        /// Балансирует узел при необходимости
        /// </summary>
        /// <param name="node">Узел для балансировки</param>
        /// <returns>Сбалансированный узел</returns>
        private TreeNode<T> Balance(TreeNode<T> node)
        {
            if (node == null) return null;  // Если узел пустой, возвращаем null

            // Обновляем высоту текущего узла
            UpdateHeight(node);

            // Получаем текущий баланс
            int balanceFactor = GetBalanceFactor(node);

            // Проверяем возможные случаи дисбаланса:

            // Случай 1: LL (левое поддерево слишком высокое)
            if (balanceFactor > 1 && GetBalanceFactor(node.Left) >= 0)
                return RightRotate(node);  // Простой правый поворот

            // Случай 2: LR (двойной поворот)
            if (balanceFactor > 1 && GetBalanceFactor(node.Left) < 0)
            {
                node.Left = LeftRotate(node.Left);  // Сначала левый поворот для левого потомка
                return RightRotate(node);           // Затем правый поворот для текущего узла
            }

            // Случай 3: RR (правое поддерево слишком высокое)
            if (balanceFactor < -1 && GetBalanceFactor(node.Right) <= 0)
                return LeftRotate(node);  // Простой левый поворот

            // Случай 4: RL (двойной поворот)
            if (balanceFactor < -1 && GetBalanceFactor(node.Right) > 0)
            {
                node.Right = RightRotate(node.Right);  // Сначала правый поворот для правого потомка
                return LeftRotate(node);               // Затем левый поворот для текущего узла
            }

            // Если баланс в пределах -1..1, возвращаем узел без изменений
            return node;
        }

        #endregion

        #region Базовые операции с деревом

        /// <summary>
        /// Создает идеально сбалансированное дерево указанного размера
        /// </summary>
        /// <param name="size">Количество элементов в дереве</param>
        public void CreateBalancedTree(int size)
        {
            root = BuildBalancedTree(size);  // Запускаем рекурсивное построение
        }

        /// <summary>
        /// Рекурсивно строит сбалансированное дерево
        /// </summary>
        /// <param name="size">Текущий размер поддерева</param>
        /// <returns>Корень построенного поддерева</returns>
        private TreeNode<T> BuildBalancedTree(int size)
        {
            if (size == 0) return null;  // Базовый случай рекурсии

            // Распределяем элементы между поддеревьями:
            int leftSize = size / 2;     // Размер левого поддерева
            int rightSize = size - leftSize - 1;  // Размер правого поддерева

            // Создаем новый узел со случайной фигурой:
            T figure = GetRandomTypedFigure();
            TreeNode<T> node = new TreeNode<T>(figure);

            // Рекурсивно строим поддеревья:
            node.Left = BuildBalancedTree(leftSize);   // Строим левое поддерево
            node.Right = BuildBalancedTree(rightSize); // Строим правое поддерево

            return node;  // Возвращаем построенный узел
        }

        /// <summary>
        /// Генерирует случайную фигуру заданного типа T
        /// </summary>
        /// <returns>Случайная фигура типа T</returns>
        private T GetRandomTypedFigure()
        {
            int type = rnd.Next(0, 3);  // Случайный выбор типа фигуры (0-2)
            Geometryfigure1 figure = null;  // Инициализация фигуры

            // Создаем фигуру в соответствии с выбранным типом:
            while (figure == null)  // Гарантируем создание фигуры
            {
                if (type == 0)
                    figure = new Rectangle1(rnd.Next(1, 10), rnd.Next(1, 10));  // Прямоугольник
                else if (type == 1)
                    figure = new Circle1(rnd.Next(1, 10));  // Круг
                else if (type == 2)
                    figure = new Parallelepiped1(rnd.Next(1, 10), rnd.Next(1, 10), rnd.Next(1, 10));  // Параллелепипед
            }

            return (T)(object)figure;  // Приведение типа к T
        }

        /// <summary>
        /// Генерирует случайную геометрическую фигуру (базовый тип)
        /// </summary>
        /// <returns>Случайная фигура типа Geometryfigure1</returns>
        private Geometryfigure1 GetRandomFigure()
        {
            int type = rnd.Next(0, 3);  // Случайный выбор типа фигуры
            switch (type)  // Создаем фигуру в зависимости от типа
            {
                case 0: return new Rectangle1(rnd.Next(1, 10), rnd.Next(1, 10));  // Прямоугольник
                case 1: return new Circle1(rnd.Next(1, 10));  // Круг
                case 2: return new Parallelepiped1(rnd.Next(1, 10), rnd.Next(1, 10), rnd.Next(1, 10));  // Параллелепипед
                default: return new Rectangle1(1, 1);  // Запасной вариант
            }
        }

        #endregion

        #region Методы работы с деревом

        /// <summary>
        /// Выводит дерево в консоль в виде иерархической структуры
        /// </summary>
        /// <param name="original">Флаг для отображения оригинального дерева</param>
        public void ShowTree(bool original = false)
        {
            if (root == null)  // Проверка на пустое дерево
            {
                Console.WriteLine("Дерево пустое");
                return;
            }
            ShowTreeRecursive(root, 0);  // Запуск рекурсивного вывода
        }

        /// <summary>
        /// Рекурсивно выводит дерево с отступами по уровням
        /// </summary>
        /// <param name="node">Текущий узел</param>
        /// <param name="level">Текущий уровень вложенности</param>
        private void ShowTreeRecursive(TreeNode<T> node, int level)
        {
            if (node == null) return;  // Базовый случай рекурсии

            // Сначала выводим правое поддерево (будет выше в выводе)
            ShowTreeRecursive(node.Right, level + 1);

            // Вывод текущего узла с отступами:
            for (int i = 0; i < level; i++)
                Console.Write("   ");  // По 3 пробела на каждый уровень
            Console.WriteLine(node.Data);  // Вывод данных узла

            // Затем выводим левое поддерево (будет ниже в выводе)
            ShowTreeRecursive(node.Left, level + 1);
        }

        /// <summary>
        /// Подсчитывает количество листьев в дереве
        /// </summary>
        /// <returns>Количество листьев</returns>
        public int CountLeaves()
        {
            return CountLeaves(root);  // Запуск рекурсивного подсчета
        }

        /// <summary>
        /// Рекурсивно подсчитывает листья в поддереве
        /// </summary>
        /// <param name="node">Корень поддерева</param>
        /// <returns>Количество листьев в поддереве</returns>
        private int CountLeaves(TreeNode<T> node)
        {
            if (node == null) return 0;  // Пустое поддерево не содержит листьев
            if (node.Left == null && node.Right == null) return 1;  // Узел без потомков - лист
            return CountLeaves(node.Left) + CountLeaves(node.Right);  // Сумма листьев поддеревьев
        }

        #endregion

        #region Преобразования дерева

        /// <summary>
        /// Преобразует дерево в сбалансированное AVL-дерево
        /// </summary>
        public void ConvertToAVLTree()
        {
            // Собираем все элементы в отсортированный список:
            List<T> elements = new List<T>();
            InOrderTraversal(root, elements);  // Симметричный обход дает сортированный список

            // Перестраиваем дерево:
            root = null;  // Очищаем текущее дерево
            foreach (var item in elements)
            {
                root = Insert(root, item);  // Вставляем элементы с балансировкой
            }
        }

        /// <summary>
        /// Выполняет симметричный обход дерева (LNR)
        /// </summary>
        /// <param name="node">Текущий узел</param>
        /// <param name="elements">Список для сохранения элементов</param>
        private void InOrderTraversal(TreeNode<T> node, List<T> elements)
        {
            if (node == null) return;  // Базовый случай рекурсии
            InOrderTraversal(node.Left, elements);  // Рекурсивный обход левого поддерева
            elements.Add(node.Data);                // Обработка текущего узла
            InOrderTraversal(node.Right, elements); // Рекурсивный обход правого поддерева
        }

        #endregion

        #region Основные операции модификации

        /// <summary>
        /// Вставляет элемент в дерево с автоматической балансировкой
        /// </summary>
        /// <param name="data">Данные для вставки</param>
        public void Insert(T data)
        {
            root = Insert(root, data);  // Запуск рекурсивной вставки
        }

        /// <summary>
        /// Рекурсивно вставляет элемент в поддерево
        /// </summary>
        /// <param name="node">Корень поддерева</param>
        /// <param name="data">Данные для вставки</param>
        /// <returns>Новый корень поддерева</returns>
        private TreeNode<T> Insert(TreeNode<T> node, T data)
        {
            if (node == null) return new TreeNode<T>(data);  // Базовый случай - создаем новый узел

            // Сравниваем элементы для определения направления вставки:
            int cmp = data.CompareTo(node.Data);
            if (cmp < 0)  // Если элемент меньше, вставляем в левое поддерево
                node.Left = Insert(node.Left, data);
            else if (cmp > 0)  // Если элемент больше, вставляем в правое поддерево
                node.Right = Insert(node.Right, data);
            // Если элемент равен, не вставляем (дубликаты не допускаются)

            return Balance(node);  // Балансируем узел после вставки
        }

        /// <summary>
        /// Вставляет элемент типа Geometryfigure1 с проверкой типа
        /// </summary>
        /// <param name="figure">Фигура для вставки</param>
        public void Insert(Geometryfigure1 figure)
        {
            if (figure is T typedFigure)  // Проверяем совместимость типов
                root = Insert(root, typedFigure);  // Вставляем если тип подходит
            else
                throw new ArgumentException($"Тип {figure.GetType()} не совместим с {typeof(T)}");
        }

        /// <summary>
        /// Удаляет элемент из дерева с балансировкой
        /// </summary>
        /// <param name="key">Элемент для удаления</param>
        /// <returns>True, если дерево было изменено</returns>
        public bool DeleteNode(Geometryfigure1 key)
        {
            int initialCount = CountLeaves();  // Запоминаем количество листьев
            root = DeleteNode(root, key);      // Выполняем удаление
            return initialCount != CountLeaves();  // Проверяем, изменилось ли дерево
        }

        /// <summary>
        /// Рекурсивно удаляет элемент из поддерева
        /// </summary>
        /// <param name="node">Корень поддерева</param>
        /// <param name="key">Элемент для удаления</param>
        /// <returns>Новый корень поддерева</returns>
        private TreeNode<T> DeleteNode(TreeNode<T> node, Geometryfigure1 key)
        {
            if (node == null) return null;  // Элемент не найден

            // Поиск удаляемого узла:
            int cmp = key.CompareTo(node.Data);
            if (cmp < 0)  // Ищем в левом поддереве
                node.Left = DeleteNode(node.Left, key);
            else if (cmp > 0)  // Ищем в правом поддереве
                node.Right = DeleteNode(node.Right, key);
            else  // Узел найден
            {
                // Узел с одним или без поддеревьев:
                if (node.Left == null) return node.Right;  // Возвращаем правое поддерево
                if (node.Right == null) return node.Left;  // Возвращаем левое поддерево

                // Узел с двумя поддеревьями:
                TreeNode<T> temp = FindMin(node.Right);  // Находим минимальный в правом поддереве
                node.Data = temp.Data;                   // Копируем данные
                node.Right = DeleteNode(node.Right, temp.Data);  // Удаляем дубликат
            }

            return Balance(node);  // Балансируем узел после удаления
        }

        /// <summary>
        /// Находит узел с минимальным значением в поддереве
        /// </summary>
        /// <param name="node">Корень поддерева</param>
        /// <returns>Узел с минимальным значением</returns>
        private TreeNode<T> FindMin(TreeNode<T> node)
        {
            // Идем по левым потомкам до конца
            while (node.Left != null) node = node.Left;
            return node;  // Возвращаем крайний левый узел
        }

        #endregion
    }
}