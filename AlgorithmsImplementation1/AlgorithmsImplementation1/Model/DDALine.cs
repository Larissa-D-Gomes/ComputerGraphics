using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AlgorithmsImplementation1.Model
{
    public class DDALine
    {
        public Point m_Point1 { get; set; }

        public Point m_Point2 { get; set; }

        // Empty constructor 
        public DDALine() { }

        public DDALine(Point p_Point1, Point p_Point2)
        { 
            this.m_Point1 = p_Point1;
            this.m_Point2 = p_Point2;
        }

        public List<Point> DDAAlgorithm()
        {
            List<Point> v_LinePoints = new List<Point>();

            double v_DeltaX = this.m_Point2.X - this.m_Point1.X;
            double v_DeltaY = this.m_Point2.Y - this.m_Point1.Y;

            int v_Steps = Math.Abs(v_DeltaX) > Math.Abs(v_DeltaY) ?
                                (int) Math.Round(Math.Abs(v_DeltaX)) :
                                (int) Math.Round(Math.Abs(v_DeltaY));

            double v_IncrX = v_DeltaX / v_Steps;
            double v_IncrY = v_DeltaY / v_Steps;

            double v_InitX = this.m_Point1.X;
            double v_InitY = this.m_Point1.Y;
           
            for (int i = 0; i < v_Steps; i++)
            {
                v_InitX += v_IncrX;
                v_InitY += v_IncrY;
                v_LinePoints.Add(new Point(v_InitX, v_InitY));
            }


            return v_LinePoints;
        }
    }
}
