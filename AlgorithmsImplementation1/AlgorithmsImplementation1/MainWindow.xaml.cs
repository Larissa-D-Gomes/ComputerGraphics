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

        // Flag que usuário está definindo área de clipping
        public bool m_IsDefiningClipping = false;

        // Variáveis para marcar as coordenadas x e y
        // mínimas e máximas da área de recorte
        public MyPoint m_MaxCoordClipping = null;
        public MyPoint m_MinCoordClipping = null;

        // Verificação de input numerico
        private static readonly Regex _regex = new Regex("[^0-9,-]+");

        public MainWindow()
        {
            InitializeComponent();
            // Ponto sem reta vazio (não existe)
            // Cor selecionada default -> preto
            m_SelectedColor = (Color)ColorConverter.ConvertFromString("#000000");

            m_PointList = new List<MyPoint>();

            EventManager.RegisterClassHandler(typeof(RadioButton), RadioButton.ClickEvent, new RoutedEventHandler(ControlItens));
            EventManager.RegisterClassHandler(typeof(CheckBox), CheckBox.ClickEvent, new RoutedEventHandler(ControlItens));

        }

        /********** Métodos de desenho no canvas **********/

        /* Método para desenhar um ponto quando o usuário clica
         * em alguma área do canvas
         * @param object sender, MouseButtonEventArgs e
         */
        private void DrawPoint(object sender, MouseButtonEventArgs e)
        {
            if (m_IsDefiningClipping)
            {
                setClipping(e);
            } else if (!m_HasDrawing) // Desenhar nova figura apenas quando não houver outra já desenhada
            {
                // Criando ponto a partir das coordenadas do clique
                MyPoint v_NewPoint = new MyPoint((int)e.GetPosition(Canvas).X, (int)e.GetPosition(Canvas).Y);
                // Desenhando pixel nas coordenadas do clique
                DrawPixelByPoint(v_NewPoint);

                setPoint(v_NewPoint);
            }

        }

        /* Método para setar coordenadas mínimas e máximas da
         * área de recorte
         * @param MouseButtonEventArgs e
         */
        private void setClipping(MouseButtonEventArgs e)
        {
            if (m_MaxCoordClipping == null)
            {
                // Setando primeiro ponto do recorte
                m_MaxCoordClipping = new MyPoint((int)e.GetPosition(Canvas).X, (int)e.GetPosition(Canvas).Y);
            }
            else
            {
                // Setando segundo ponto do recorte
                m_MinCoordClipping = new MyPoint();

                // Se x do primeiro ponto < x do segundo, trocar x de m_MaxCoordClipping
                if (m_MaxCoordClipping.getIntX() < (int)e.GetPosition(Canvas).X)
                {
                    m_MinCoordClipping.setX(m_MaxCoordClipping.getIntX());
                    m_MaxCoordClipping.setX((int)e.GetPosition(Canvas).X);
                }
                else // Se não, apenas setar x de m_MinCoordClipping
                {
                    m_MinCoordClipping.setX((int)e.GetPosition(Canvas).X);
                }

                // Se y do primeiro ponto < y do segundo, trocar y de m_MaxCoordClipping
                if (m_MaxCoordClipping.getIntY() < (int)e.GetPosition(Canvas).Y)
                {
                    m_MinCoordClipping.setY(m_MaxCoordClipping.getIntY());
                    m_MaxCoordClipping.setY((int)e.GetPosition(Canvas).Y);
                }
                else // Se não, apenas setar x de m_MinCoordClipping
                {
                    m_MinCoordClipping.setY((int)e.GetPosition(Canvas).Y);
                }

                drawClippingArea();
                m_IsDefiningClipping = false;
            }
        }

        /* Método para desenhar área de clippling */
        private void drawClippingArea()
        {
            MyPolygon v_Polygon = new MyPolygon();

            List<MyPoint> v_Points = new List<MyPoint>();
            // Adicionando arestas do retângulo
            v_Points.Add(new MyPoint(m_MaxCoordClipping.getIntX(), m_MaxCoordClipping.getIntY()));
            v_Points.Add(new MyPoint(m_MaxCoordClipping.getIntX(), m_MinCoordClipping.getIntY()));
            v_Points.Add(new MyPoint(m_MinCoordClipping.getIntX(), m_MinCoordClipping.getIntY()));
            v_Points.Add(new MyPoint(m_MinCoordClipping.getIntX(), m_MaxCoordClipping.getIntY()));

            // Lingando arestas
            v_Polygon.createPolygonByListOfEdges(v_Points);
            // Calculando todos os pontos
            List<MyPoint> v_ListOfPoints = v_Polygon.getAllPoints(RadioButtonDDA.IsChecked == true);

            DrawListOfPoints(v_ListOfPoints);
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
            List<MyPoint> v_ListOfPoints = m_Polygon.getAllPoints(RadioButtonDDA.IsChecked == true);

            DrawListOfPoints(v_ListOfPoints);
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
            Canvas.SetLeft(v_Pixel.m_PixelValue, p_Point.getIntX());
            // Definindo posição Y do pixel
            Canvas.SetTop(v_Pixel.m_PixelValue, p_Point.getIntY());

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
            else if(RadioButtonScale.IsChecked == true)
                ApplyScale();
            else if(RadioButtonRot.IsChecked == true)
                ApplyRotation();
            else
                ApplyReflection();

        }

        /* Método para aplicar translação no 
         * objeto desenhado na tela
         */
        private void ApplyTranslation()
        {
            // Limpar tela
            Canvas.Children.Clear();


            double v_XVector = double.Parse(m_XInput.Text);

            // Invertendo coordenada de y, pois o eixo cresce 
            // de maneira invertida no canvas por ser
            // posição de uma matriz
            double v_YVector = double.Parse(m_YInput.Text) * -1;

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
                DrawListOfPoints(m_Polygon.getAllPoints(RadioButtonDDA.IsChecked == true));
            } else if (m_Circ != null) // Aplicar transformação em uma circunferência
            {
                m_Circ.Translation(v_XVector, v_YVector);
                DrawCirc();
            }

        }

        /* Método para aplicar escala no 
         * objeto desenhado na tela
         */
        private void ApplyScale()
        {
            // Limpar tela
            Canvas.Children.Clear();

            double v_XVector = double.Parse(m_XInput.Text);

            // Invertendo coordenada de y, pois o eixo cresce 
            // de maneira invertida no canvas por ser
            // posição de uma matriz
            double v_YVector = double.Parse(m_YInput.Text);

            // Aplicar transformação em um ponto
            if (m_PointList.Count == 1)
            {
                m_PointList[0].Scale(v_XVector, v_YVector);
                DrawPixelByPoint(m_PointList[0]);
            }
            else if (m_Line != null) // Aplicar transformação em uma reta
            {
                m_Line.Scale(v_XVector, v_YVector);
                DrawLine();
            }
            else if (m_Polygon != null) // Aplicar transformação em um poligono
            {
                m_Polygon.Scale(v_XVector, v_YVector);
                DrawPolygon();
            }
            else if (m_Circ != null) // Aplicar transformação em uma circunferência
            {
                m_Circ.Scale(v_YVector);
                DrawCirc();
            }

        }

        /* Método para aplicar rotação no 
         * objeto desenhado na tela
         */
        private void ApplyRotation()
        {
            // Limpar tela
            Canvas.Children.Clear();

            // Conversão de graus para radianos
            // Math -> funções de sen e cos calculam por radianos
            double v_Theta = -1 * double.Parse(m_YInput.Text) * Math.PI / 180.0;

            // Aplicar transformação em um ponto
            if (m_PointList.Count == 1)
            {
                m_PointList[0].Rotation(v_Theta);
                DrawPixelByPoint(m_PointList[0]);
            }
            else if (m_Line != null) // Aplicar transformação em uma reta
            {
                m_Line.Rotation(v_Theta);
                DrawLine();
            }
            else if (m_Polygon != null) // Aplicar transformação em um poligono
            {
                m_Polygon.Rotation(v_Theta);
                DrawPolygon();
            }
            else if (m_Circ != null) // Aplicar transformação em uma circunferência
            {
                m_Circ.Rotation(v_Theta);
                DrawCirc();
            }

        }

        /* Método para aplicar reflexão no 
         * objeto desenhado na tela
         */
        private void ApplyReflection()
        {
            // Limpar tela
            Canvas.Children.Clear();

            double v_XCanvas = (Canvas.ActualWidth) ;
            double v_YCanvas = (Canvas.ActualHeight);

            // Aplicar transformação em um ponto
            if (m_PointList.Count == 1)
            {
                m_PointList[0].Reflection(m_XCheck.IsChecked == true, m_YCheck.IsChecked == true, v_XCanvas, v_YCanvas);
                DrawPixelByPoint(m_PointList[0]);
            }
            else if (m_Line != null) // Aplicar transformação em uma reta
            {
                m_Line.Reflection(m_XCheck.IsChecked == true, m_YCheck.IsChecked == true, v_XCanvas, v_YCanvas);
                DrawLine();
            }
            else if (m_Polygon != null) // Aplicar transformação em um poligono
            {
                m_Polygon.Reflection(m_XCheck.IsChecked == true, m_YCheck.IsChecked == true, v_XCanvas, v_YCanvas);
                DrawListOfPoints(m_Polygon.getAllPoints(RadioButtonDDA.IsChecked == true));
            }
            else if (m_Circ != null) // Aplicar transformação em uma circunferência
            {
                m_Circ.Reflection(m_XCheck.IsChecked == true, m_YCheck.IsChecked == true, v_XCanvas, v_YCanvas);
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

        /* Método para clicks do usuário definirem área de recorte */
        private void DefineClippling(object sender, EventArgs e)
        {
            m_IsDefiningClipping = true;
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
            m_MinCoordClipping = null;
            m_MaxCoordClipping = null;
        }

        /* Método para controlar elementos mostrados na tela conforme radio buttons selecionados */
        private void ControlItens(object sender, EventArgs e)
        {
            m_XCheck.Visibility = Visibility.Hidden;
            m_YCheck.Visibility = Visibility.Hidden;

            if (RadioButtonScale.IsChecked == true && RadioButtonCirc.IsChecked == true)
            {
                m_YText.Text = "R * ";
                m_YText.Visibility = Visibility.Visible;
                m_YInput.Visibility = Visibility.Visible;
                m_XText.Visibility = Visibility.Hidden;
                m_XInput.Visibility = Visibility.Hidden;

            }
            else if (RadioButtonRot.IsChecked == true)
            {
                m_YText.Text = "θ: ";
                m_YText.Visibility = Visibility.Visible;
                m_YInput.Visibility = Visibility.Visible;
                m_XText.Visibility = Visibility.Hidden;
                m_XInput.Visibility = Visibility.Hidden;

            }
            else if (RadioButtonRefl.IsChecked == true)
            {
                m_XCheck.Visibility = Visibility.Visible;
                m_YCheck.Visibility = Visibility.Visible;

                m_XText.Visibility = Visibility.Visible;
                m_XInput.Visibility = Visibility.Hidden;
                m_YText.Visibility = Visibility.Visible;
                m_YInput.Visibility = Visibility.Hidden;
            }
            else
            {
                m_YText.Text = "Y: ";
                m_XText.Visibility = Visibility.Visible;
                m_YInput.Visibility = Visibility.Visible;
                m_XInput.Visibility = Visibility.Visible;
                m_XCheck.Visibility = Visibility.Hidden;
                m_YCheck.Visibility = Visibility.Hidden;
            }
        }

    }
}
