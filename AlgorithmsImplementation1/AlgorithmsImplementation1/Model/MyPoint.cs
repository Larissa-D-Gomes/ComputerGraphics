using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgorithmsImplementation1.Model
{
    public class MyPoint
    {
        public int m_X { get; set; }
        public int m_Y { get; set; }

        public MyPoint()
        {
            this.m_X = -1;
            this.m_Y = -1;
        }

        public MyPoint(int p_X, int p_Y)
        {
            this.m_X = p_X;
            this.m_Y = p_Y;
        }
    }
}
