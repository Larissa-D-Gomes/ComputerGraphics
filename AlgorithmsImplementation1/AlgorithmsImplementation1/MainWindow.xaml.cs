using AlgorithmsImplementation1.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AlgorithmsImplementation1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public DDALine m_Line;

        public Point m_OutOfLinePoint; 

        public MainWindow()
        {
            InitializeComponent();

            m_OutOfLinePoint = new Point(-1, -1);
        }

        private void DrawPoint(object sender, MouseButtonEventArgs e)
        {
            Point v_NewPoint = e.GetPosition(Canvas);
            DrawPixelByPoint(v_NewPoint);

            if (this.m_OutOfLinePoint.X != -1)
            {
                DrawLine(this.m_OutOfLinePoint, v_NewPoint);
                m_OutOfLinePoint.X = -1;
            }
            else
            {
                this.m_OutOfLinePoint = v_NewPoint;
            }

        }

        private void DrawLine(Point p_OutOfLinePoint, Point p_NewPoint)
        {
            DDALine v_Line = new DDALine(p_OutOfLinePoint, p_NewPoint);

            List<Point> v_PointsFromLine = v_Line.DDAAlgorithm();

            foreach (Point v_Point in v_PointsFromLine)
            {
                DrawPixelByPoint(v_Point);
            }
        }

        private void DrawPixelByPoint(Point p_Point)
        {
            Pixel v_Pixel = new Pixel();

            Canvas.SetLeft(v_Pixel.m_PixelValue, p_Point.X);
            Canvas.SetTop(v_Pixel.m_PixelValue, p_Point.Y);

            Canvas.Children.Add(v_Pixel.m_PixelValue);
        }
    }
}
