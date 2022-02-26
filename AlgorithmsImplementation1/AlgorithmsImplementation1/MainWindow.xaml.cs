using AlgorithmsImplementation1.Model;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace AlgorithmsImplementation1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {        
        // Lista pontos desenhados
        public List<MyPoint> m_PointList;

        public MyLine m_Line;

        public MyPolygon m_Polygon;

        public MyCircumference m_Circ;

        public Color m_SelectedColor;
        // Variável para determinar se canvas possuem algum desenho 
        public bool m_HasDrawing = false;

        // Verificação de input numerico
        private static readonly Regex _regex = new Regex("[^0-9.-]+");

        public MainWindow()
        {
            InitializeComponent();
            // Ponto sem reta vazio (não existe)
            // Cor selecionada default -> preto
            m_SelectedColor = (Color)ColorConverter.ConvertFromString("#000000");

            m_PointList = new List<MyPoint>();
        }

        /********** Métodos de desenho no canvas **********/

        /* Método para desenhar um ponto quando o usuário clica
         * em alguma área do canvas
         * @param object sender, MouseButtonEventArgs e
         */
        private void DrawPoint(object sender, MouseButtonEventArgs e)
        {
            // Desenhar nova figura apenas quando não houver
            // outra já desenhada
            if (!m_HasDrawing)
            {
                // Criando ponto a partir das coordenadas do clique
                MyPoint v_NewPoint = new MyPoint((int)e.GetPosition(Canvas).X, (int)e.GetPosition(Canvas).Y);
                // Desenhando pixel nas coordenadas do clique
                DrawPixelByPoint(v_NewPoint);

                setPoint(v_NewPoint);
            }

        }

        /* Adicionar ponto a lista de pontos e 
         * desenhar estrutura de dados que ele pertence
         * @param MyPoint p_NewPoint
         */
        private void setPoint(MyPoint p_NewPoint)
        {
            m_PointList.Add(p_NewPoint);
            if (RadioButtonLine.IsChecked == true)
            {
                // Se dois pontos da reta já foram desenhados 
                if (m_PointList.Count == 2)
                {
                    // Desenhar reta a partir dos pontos desenhados
                    m_Line = new MyLine(m_PointList[0], m_PointList[1]);
                    DrawLine();
                }
            } else if (RadioButtonCirc.IsChecked == true)
            {
                // Se dois pontos da circunferência já foram desenhados 
                if (this.m_PointList.Count == 2)
                {
                    // Desenhar circunferência considerando o primeiro
                    // ponto desenhado com o centro, e o segundo como
                    // ponto tangente ao arco
                    int v_Radius = (int)Math.Round(MyPoint.GetDistanceBetweenTwoPoint(m_PointList[0], m_PointList[1]));
                    m_Circ = new MyCircumference(m_PointList[0], v_Radius);
                    DrawCirc();
                }

            } else {
                // Se todas arestas determinadas pelo usuário
                // foram desenhadas, desenhar poligono
                if ((InputVertice.Text != null || InputVertice.Text !="")
                     && m_PointList.Count == int.Parse(InputVertice.Text))
                    DrawPolygon();                
            }
                
        }

        private void DrawPolygon()
        {
            m_Polygon = new MyPolygon();

            // Lingando arestas
            m_Polygon.createPolygonByListOfEdges(m_PointList);
            // Calculando todos os pontos
            List<MyPoint> p_ListOfPoints = m_Polygon.getAllPoints(RadioButtonDDA.IsChecked == true);

            DrawListOfPoints(p_ListOfPoints);
        }

        /* Método para desenhar uma circunferência a partir de 
         * dois pontos desenhados no canvas
         * @param MyPoint p_Point1, MyPoint p_Point2
         */
        private void DrawLine()
        {
            
            List<MyPoint> v_PointsFromLine;

            if (RadioButtonBres.IsChecked == true)
                v_PointsFromLine = m_Line.BresenhamAlgorithm();// Definindo pontos a serem desenhados
                                                               // a partir do algoritmo de Bresenham 
            else
                v_PointsFromLine = m_Line.DDAAlgorithm();// Definindo pontos a serem desenhados
                                                         // a partir do algoritmo DDA
            DrawListOfPoints(v_PointsFromLine);

        }

        /* Método para desenhar uma reta a partir de 
         * dois pontos desenhados no canvas
         * @param MyPoint p_Point1, MyPoint p_Point2
         */
        private void DrawCirc()
        {
            DrawListOfPoints(m_Circ.BresenhamAlgorithm());
        }

        /* Método para desenhar lista de pontos de uma figura
         * @param List<MyPoint> p_PointsList
         */
        public void DrawListOfPoints(List<MyPoint> p_PointsList)
        {
            // Limpando pontos de referência
            Canvas.Children.Clear();
            m_HasDrawing = true;

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

        /********** Métodos de Transformação de pontos **********/

        /* OnClick botão de aplicar transformação
         * @param object sender, EventArgs e
         */
        private void ApplyTranformation(object sender, EventArgs e)
        {
            if (RadioButtonTransl.IsChecked == true)
                ApplyTranslation();

        }

        /* M[etodo para aplicar translação no 
         * objeto desenhado na tela
         */
        private void ApplyTranslation()
        {
            // Limpar tela
            Canvas.Children.Clear();

            
            int v_XVector = int.Parse(m_XInput.Text);

            // Invertendo coordenada de y, pois o eixo cresce 
            // de maneira invertida no canvas por ser
            // posição de uma matriz
            int v_YVector = int.Parse(m_YInput.Text) * -1;

            // Aplicar transformação em um ponto
            if (m_PointList.Count == 1)
            {
                m_PointList[0].TranslateSum(v_XVector, v_YVector);
                DrawPixelByPoint(m_PointList[0]);
            } else if (m_Line != null) // Aplicar transformação em uma reta
            {
                m_Line.Translation(v_XVector, v_YVector);
                DrawLine();
            } else if(m_Polygon != null) // Aplicar transformação em um poligono
            {
                m_Polygon.Translation(v_XVector, v_YVector);
                DrawPolygon();
            } else if (m_Circ != null) // Aplicar transformação em uma circunferência
            {
                m_Circ.Translation(v_XVector, v_YVector);
                DrawCirc();
            }

        }


        /********** Métodos de eventos da tela **********/

        /* Método para selecionar cor a partir de alterações da seleção do componente
         * de colorPicker
         * @param object sender, RoutedPropertyChangedEventArgs<Color?> e
         */
        private void SelectColor(object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {
            m_SelectedColor = (Color) e.NewValue;
        }

        /* Método para verificar se texto  é numerico
         * @param string text
         */
        private static bool IsTextAllowed(string text)
        {
            return _regex.IsMatch(text);
        }

        /* Método para verificar se novo caracter digitado no input
         * é numerico
         * @param object sender, TextCompositionEventArgs e
         */
        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            e.Handled = IsTextAllowed(e.Text);
        }

        /* Método para limpar tela */
        private void RemoveItensFromCanvas(object sender, EventArgs e)
        {
            Canvas.Children.Clear();
            // Canvas não possui mais desenhos
            m_HasDrawing = false;

            // Limpar atributos da tela
            m_PointList = new List<MyPoint> ();
            m_Line = null;
            m_Polygon =  null;
            m_Circ = null;
        }

    }
}
