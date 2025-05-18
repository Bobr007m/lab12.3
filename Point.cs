using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Geometryclass;

namespace lab12._3
{
    class Point
    {
        public int data;
        public Point left, right;
        public Point(int d = 0)
        {
            data = d;
            left = null;
            right = null;
        }
        public override string ToString()
        {
            return data + " ";
        }
    }
}
