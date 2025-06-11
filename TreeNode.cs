using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Geometryclass;

namespace lab12._3
{
   
        /// <summary>
        /// Обобщённый класс узла дерева
        /// </summary>
        public class TreeNode<T> where T: Geometryfigure1, ICloneable, IIni, new()
    {
            public T Data;               // Хранимые данные любого типа
            public TreeNode<T> Left;     // Левое поддерево
            public TreeNode<T> Right;    // Правое поддерево
            public int Height;           // Высота узла

            public TreeNode(T data)
            {
                Data = data;
                Left = null;
                Right = null;
                Height = 1;
            }
        }
    }
