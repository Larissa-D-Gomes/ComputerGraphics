using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgorithmsImplementation1.Model
{
    public class MyPoint
    {
        public double m_X { get; set; }
        public double m_Y { get; set; }

        public MyPoint()
        {
            this.m_X = -1.0;
            this.m_Y = -1.0;
        }

        public MyPoint(double p_X, double p_Y)
        {
            this.m_X = p_X;
            this.m_Y = p_Y;
        }
    }
}
