using AlgorithmsImplementation1.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;

namespace AlgorithmsImplementation1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {        
        // Variável para marcar ponto desenhado 
        // que não pertence a uma reta
        public MyPoint m_FirstPoint;

        // Lista para armazenar arestas de um poligono
        public List<MyPoint> m_PolygonEdges;

        public MyLine m_Line;

        public MyPolygon m_Polygon;

        public Color m_SelectedColor;

        private static readonly Regex _regex = new Regex("[^0-9.-]+");

        public MainWindow()
        {
            InitializeComponent();
            // Ponto sem reta vazio (não existe)
            m_FirstPoint = null;
            // Cor selecionada default -> preto
            this.m_SelectedColor = (Color)ColorConverter.ConvertFromString("#000000");

            m_Polygon = new MyPolygon();
        }

        /* Método para desenhar um ponto quando o usuário clica
         * em alguma área do canvas
         * @param object sender, MouseButtonEventArgs e
         */
        private void DrawPoint(object sender, MouseButtonEventArgs e)
        {
            // Criando ponto a partir das coordenadas do clique
            MyPoint v_NewPoint = new MyPoint((int)e.GetPosition(Canvas).X, (int)e.GetPosition(Canvas).Y);
            // Desenhando pixel nas coordenadas do clique
            DrawPixelByPoint(v_NewPoint);

            setPointToLineOrPolygon(v_NewPoint);

        }

        private void setPointToLineOrPolygon(MyPoint p_NewPoint)
        {
            if (RadioButtonLine.IsChecked == true)
            {
                // Se existe ponto fora de uma reta 
                if (this.m_FirstPoint != null)
                {
                    // Desenhar reta a partir do ponto fora de uma reta
                    // e novo ponto desenhado
                    DrawLine(this.m_FirstPoint, p_NewPoint);

                    // Setar flag para informar que não há mais 
                    // um ponto fora de uma reta
                    m_FirstPoint = null;
                }
                else // Se não existir ponto sem reta
                {
                    // Setar novo ponto sem reta
                    this.m_FirstPoint = p_NewPoint;
                }
            } else if (RadioButtonCirc.IsChecked == true)
            {
                if (m_FirstPoint != null)
                {
                    DrawCirc(m_FirstPoint, p_NewPoint);
                }
                else
                {
                    m_FirstPoint = p_NewPoint;
                }  
            } else{
                if (m_PolygonEdges.Count < 3) {
                    m_PolygonEdges.Add(p_NewPoint);
                    if (m_PolygonEdges.Count == 3)
                        DrawPolygon();
                }
            }
                
        }

        private void DrawPolygon()
        {

        }

        /* Método para desenhar uma circunferência a partir de 
         * dois pontos desenhados no canvas
         * @param MyPoint p_Point1, MyPoint p_Point2
         */
        private void DrawLine(MyPoint p_Point1, MyPoint p_Point2)
        {
            MyLine v_Line = new MyLine(p_Point1, p_Point2);


            List<MyPoint> v_PointsFromLine;

            if (RadioButtonBres.IsChecked == true)
                v_PointsFromLine = v_Line.BresenhamAlgorithm();// Definindo pontos a serem desenhados
                                                               // a partir do algoritmo de Bresenham 
            else
                v_PointsFromLine = v_Line.DDAAlgorithm();// Definindo pontos a serem desenhados
                                                         // a partir do algoritmo DDA
            DrawListOfPoints(v_PointsFromLine);

        }


        /* Método para desenhar uma reta a partir de 
         * dois pontos desenhados no canvas
         * @param MyPoint p_Point1, MyPoint p_Point2
         */
        private void DrawCirc(MyPoint p_Point1, MyPoint p_Point2)
        {
            int v_Radius = (int)Math.Round(MyPoint.GetDistanceBetweenTwoPoint(p_Point1, p_Point2));
            MyCircumference m_Circumference = new MyCircumference(p_Point1, v_Radius);         

            DrawListOfPoints(m_Circumference.BresenhamAlgorithm());
        }

        public void DrawListOfPoints(List<MyPoint> p_PointsList)
        {
            foreach (MyPoint v_Point in p_PointsList)
            {
                // Desenhar pixel referente a pontos calculados
                DrawPixelByPoint(v_Point);
            }
        }

        /* Método para desenhar pixels na tela
         * @param MyPoint p_Point
         */
        private void DrawPixelByPoint(MyPoint p_Point)
        {
            // Definindo cor do pixel
            Pixel v_Pixel = new Pixel(this.m_SelectedColor);

            // Definindo posição X do pixel
            Canvas.SetLeft(v_Pixel.m_PixelValue, p_Point.getX());
            // Definindo posição Y do pixel
            Canvas.SetTop(v_Pixel.m_PixelValue, p_Point.getY());

            // Desenhando pixel
            Canvas.Children.Add(v_Pixel.m_PixelValue);
        }

        /* Método para selecionar cor a partir de alterações da seleção do componente
         * de colorPicker
         * @param object sender, RoutedPropertyChangedEventArgs<Color?> e
         */
        public void SelectColor(object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {
            m_SelectedColor = (Color) e.NewValue;
        }

        private static bool IsTextAllowed(string text)
        {
            return _regex.IsMatch(text);
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            e.Handled = IsTextAllowed(e.Text);
        }

        private void RemoveItensFromCanvas(object sender, EventArgs e)
        {
            Canvas.Children.Clear();
        }

    }
}
