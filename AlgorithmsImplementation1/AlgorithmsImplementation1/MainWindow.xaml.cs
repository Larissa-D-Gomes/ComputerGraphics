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
        // Variável para marcar ponto desenhado 
        // que não pertence a uma reta
        public MyPoint m_OutOfLinePoint;
        
        public Color m_SelectedColor;
        
        public MainWindow()
        {
            InitializeComponent();
            // Ponto sem reta vazio (não existe)
            m_OutOfLinePoint = new MyPoint();
            // Cor selecionada default -> preto
            this.m_SelectedColor = (Color)ColorConverter.ConvertFromString("#000000");
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

            // Se existe ponto fora de uma reta 
            if (this.m_OutOfLinePoint.m_X != -1)
            {
                // Desenhar reta a partir do ponto fora de uma reta
                // e novo ponto desenhado
                DrawLine(this.m_OutOfLinePoint, v_NewPoint);

                // Setar flag para informar que não há mais 
                // um ponto fora de uma reta
                m_OutOfLinePoint.m_X = -1;
            }
            else // Se não existir ponto sem reta
            {
                // Setar novo ponto sem reta
                this.m_OutOfLinePoint = v_NewPoint;
            }

        }

        /* Método para desenhar uma reta a partir de 
         * dois pontos desenhados no canvas
         * @param MyPoint p_Point1, MyPoint p_Point2
         */
        private void DrawLine(MyPoint p_Point1, MyPoint p_Point2)
        {
            MyLine v_Line = new MyLine(p_Point1, p_Point2);

            
            List<MyPoint> v_PointsFromLine;
             
            if(RadioButtonBres.IsChecked == true)
                v_PointsFromLine = v_Line.BresenhamAlgorithm();// Definindo pontos a serem desenhados
                                                               // a partir do algoritmo de Bresenham 
            else
                v_PointsFromLine = v_Line.DDAAlgorithm();// Definindo pontos a serem desenhados
                                                         // a partir do algoritmo DDA

            foreach (MyPoint v_Point in v_PointsFromLine)
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
            Canvas.SetLeft(v_Pixel.m_PixelValue, p_Point.m_X);
            // Definindo posição Y do pixel
            Canvas.SetTop(v_Pixel.m_PixelValue, p_Point.m_Y);

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

         
    }
}
