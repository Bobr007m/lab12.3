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
            int size = 5;
            Point idTree = null;
            idTree = ldeaITree(size,
            idTree);
            ShowTree(idTree, 3);
        }
    }
}
