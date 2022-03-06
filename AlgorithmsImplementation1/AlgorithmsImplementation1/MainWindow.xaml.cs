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
        // Lista pontos desenhados auxiliar para desenhos em estruturas
        // de retas, poligons e circunferência
        public List<MyPoint> m_PointListAux;

        public List<MyPoint> m_PointList = new List<MyPoint>();

        public List<MyLine> m_LineList = new List<MyLine>();

        public List<MyPolygon> m_PolygonList = new List<MyPolygon>();

        public List<MyCircumference> m_CircList = new List<MyCircumference>();

        public Color m_SelectedColor;
        // Variável para determinar se canvas possuem algum desenho 
        public bool m_HasDrawing = false;

        // Flag que usuário está definindo área de clipping
        public bool m_IsDefiningClipping = false;

        // Flag existe uma área de clipping definida
        public bool m_HasClipping = false;


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

            m_PointListAux = new List<MyPoint>();

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
            } else 
            {
                // Criando ponto a partir das coordenadas do clique
                MyPoint v_NewPoint = new MyPoint((int)e.GetPosition(Canvas).X, (int)e.GetPosition(Canvas).Y);
                // Desenhando pixel nas coordenadas do clique
                DrawPixelByPoint(v_NewPoint);

                if (RadioButtonPoint.IsChecked == false)
                {
                    setPoint(v_NewPoint);
                }
                else
                {
                    m_PointList.Add(v_NewPoint);
                }
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
                m_HasClipping = true;
                drawClippingArea();

                // Definindo retas plotadas pelo recorte
                foreach (MyLine p_Line in this.m_LineList)
                {
                    MyLine v_ClipplingLine = p_Line.CohenSutherland(m_MaxCoordClipping.getIntX(),
                    m_MinCoordClipping.getIntX(), m_MaxCoordClipping.getIntY(), m_MinCoordClipping.getIntY());

                    if (v_ClipplingLine != null)
                    {
                       DrawLine(v_ClipplingLine, false);
                    }
                    
                }

                // Definindo retas de poligonos plotadas pelo recorte
                foreach (MyPolygon p_Polygon in m_PolygonList)
                {
                    List<MyLine> v_ClipplingLineList = p_Polygon.CohenSutherland(m_MaxCoordClipping.getIntX(),
                    m_MinCoordClipping.getIntX(), m_MaxCoordClipping.getIntY(), m_MinCoordClipping.getIntY());

                    foreach (MyLine v_Line in v_ClipplingLineList)
                    {
                        DrawLine(v_Line, false);
                    }

                }

                // Definindo pontos dentro do recorte
                foreach (MyPoint v_Point in m_PointList)
                {
                    if(v_Point.getIntX() >= m_MinCoordClipping.getIntX() &&
                       v_Point.getIntX() <= m_MaxCoordClipping.getIntX() &&
                       v_Point.getIntY() >= m_MinCoordClipping.getIntY() &&
                       v_Point.getIntY() <= m_MaxCoordClipping.getIntY())
                        DrawPixelByPoint(v_Point);

                }

                // Definindo pontos da circumferência dentro do recorte
                // A análise erá feita ponto a ponto calculado pelo algoritmo
                // de Bresenham
                foreach (MyCircumference p_Circ in m_CircList)
                {
                    List<MyPoint> v_Points = p_Circ.BresenhamAlgorithm();
                    foreach (MyPoint v_Point in v_Points)
                    {
                        if (v_Point.getIntX() >= m_MinCoordClipping.getIntX() &&
                           v_Point.getIntX() <= m_MaxCoordClipping.getIntX() &&
                           v_Point.getIntY() >= m_MinCoordClipping.getIntY() &&
                           v_Point.getIntY() <= m_MaxCoordClipping.getIntY())
                            DrawPixelByPoint(v_Point);

                    }
                }

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

            DrawListOfPoints(v_ListOfPoints, true);
        }

        /* Adicionar ponto a lista de pontos e 
         * desenhar estrutura de dados que ele pertence
         * @param MyPoint p_NewPoint
         */
        private void setPoint(MyPoint p_NewPoint)
        {
            m_PointListAux.Add(p_NewPoint);
            if (RadioButtonLine.IsChecked == true)
            {
                // Se dois pontos da reta já foram desenhados 
                if (m_PointListAux.Count == 2)
                {
                    // Desenhar reta a partir dos pontos desenhados
                    m_LineList.Add(new MyLine(m_PointListAux[0], m_PointListAux[1]));

                    foreach (MyLine p_Line in m_LineList)
                        DrawLine(p_Line, false);
                    m_PointListAux = new List<MyPoint>();
                }
            } else if (RadioButtonCirc.IsChecked == true)
            {
                // Se dois pontos da circunferência já foram desenhados 
                if (this.m_PointListAux.Count == 2)
                {
                    // Desenhar circunferência considerando o primeiro
                    // ponto desenhado com o centro, e o segundo como
                    // ponto tangente ao arco
                    int v_Radius = (int)Math.Round(MyPoint.GetDistanceBetweenTwoPoint(m_PointListAux[0], m_PointListAux[1]));

                    MyCircumference v_NewCirc = new MyCircumference(m_PointListAux[0], v_Radius);
                    m_CircList.Add(v_NewCirc);
                    DrawCirc(v_NewCirc, true);
                    m_PointListAux = new List<MyPoint>();
                }

            } else {
                // Se todas arestas determinadas pelo usuário
                // foram desenhadas, desenhar poligono
                if ((InputVertice.Text != null || InputVertice.Text != "")
                     && m_PointListAux.Count == int.Parse(InputVertice.Text))
                {
                    MyPolygon v_NewPolygon = new MyPolygon();
                    // Lingando arestas
                    v_NewPolygon.createPolygonByListOfEdges(m_PointListAux);

                    m_PolygonList.Add(v_NewPolygon);
                    DrawPolygon(v_NewPolygon);
                    m_PointListAux = new List<MyPoint>();
                }
            }

        }

        /* Método para desenhar poligono
         * @param MyPolygon p_Polygon
         */
        private void DrawPolygon(MyPolygon p_Polygon)
        {
            
            // Calculando todos os pontos
            List<MyPoint> v_ListOfPoints = p_Polygon.getAllPoints(RadioButtonDDA.IsChecked == true);

            DrawListOfPoints(v_ListOfPoints, false);
        }

        /* Método para desenhar uma circunferência a partir de 
         * dois pontos desenhados no canvas
         * @param MyLine p_Line, bool p_IsToCleanCavas
         */
        private void DrawLine(MyLine p_Line, bool p_IsToCleanCavas)
        {
            
            List<MyPoint> v_PointsFromLine;

            if (RadioButtonBres.IsChecked == true)
                v_PointsFromLine = p_Line.BresenhamAlgorithm();// Definindo pontos a serem desenhados
                                                               // a partir do algoritmo de Bresenham 
            else
                v_PointsFromLine = p_Line.DDAAlgorithm();// Definindo pontos a serem desenhados
                                                         // a partir do algoritmo DDA
            DrawListOfPoints(v_PointsFromLine, p_IsToCleanCavas);

        }

        /* Método para desenhar uma reta a partir de 
         * dois pontos desenhados no canvas
         * @param MyCircumference p_Circ, bool p_DeleteCenter
         */
        private void DrawCirc(MyCircumference p_Circ, bool p_DeleteCenter)
        {
            // Apagando ponto de referência do centro
            if (p_DeleteCenter)
            {
                UIElement v_Center = Canvas.Children[Canvas.Children.Count - 2];
                Canvas.Children.Remove(v_Center);
            }

            DrawListOfPoints(p_Circ.BresenhamAlgorithm(), false);
        }

        /* Método para desenhar lista de pontos de uma figura
         * @param List<MyPoint> p_PointsList, bool p_IsToCleanCavas
         */
        public void DrawListOfPoints(List<MyPoint> p_PointsList, bool p_IsToCleanCavas)
        {
            if(p_IsToCleanCavas)
            // Limpando pontos de referência
                Canvas.Children.Clear();

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
            Pixel v_Pixel = new Pixel(m_SelectedColor);

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
            if (m_PointList != null)
            {
                foreach (MyPoint p_Point in m_PointList)
                {
                    p_Point.TranslateSum(v_XVector, v_YVector);
                    DrawPixelByPoint(p_Point);
                }
            } 
            if (m_LineList != null) // Aplicar transformação em uma reta
            {
                foreach (MyLine p_Line in m_LineList)
                {
                    p_Line.Translation(v_XVector, v_YVector);
                    DrawLine(p_Line, false);
                }
            } 
            if(m_PolygonList != null) // Aplicar transformação em um poligono
            {
                foreach (MyPolygon p_Polygon in m_PolygonList) {
                    p_Polygon.Translation(v_XVector, v_YVector);
                    DrawListOfPoints(p_Polygon.getAllPoints(RadioButtonDDA.IsChecked == true), false);
                }
            } 
            if (m_CircList != null) // Aplicar transformação em uma circunferência
            {
                foreach (MyCircumference p_Circ in m_CircList)
                {
                    p_Circ.Translation(v_XVector, v_YVector);
                    DrawCirc(p_Circ, false);
                }
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
            if (m_PointList != null)
            {
                foreach (MyPoint p_Point in m_PointList)
                {
                    p_Point.Scale(v_XVector, v_YVector);
                    DrawPixelByPoint(p_Point);
                }
            }
            if (m_LineList != null) // Aplicar transformação em uma reta
            {
                foreach (MyLine p_Line in m_LineList)
                {
                    p_Line.Scale(v_XVector, v_YVector);
                    DrawLine(p_Line, false);
                }
            }
            if (m_PolygonList != null) // Aplicar transformação em um poligono
            {
                foreach (MyPolygon p_Polygon in m_PolygonList)
                {
                    p_Polygon.Scale(v_XVector, v_YVector);
                    DrawPolygon(p_Polygon);
                }
            }
            if (m_CircList != null) // Aplicar transformação em uma circunferência
            {
                foreach (MyCircumference p_Circ in m_CircList)
                {
                    p_Circ.Scale(v_YVector);
                    DrawCirc(p_Circ, false);
                }
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
            if (m_PointList != null)
            {
                foreach (MyPoint p_Point in m_PointList)
                {
                    p_Point.Rotation(v_Theta);
                    DrawPixelByPoint(p_Point);
                }
            }
            if (m_LineList != null) // Aplicar transformação em uma reta
            {
                foreach (MyLine p_Line in m_LineList)
                {
                    p_Line.Rotation(v_Theta);
                    DrawLine(p_Line, false);
                }
            }
            if (m_PolygonList != null) // Aplicar transformação em um poligono
            {
                foreach (MyPolygon p_Polygon in m_PolygonList)
                {
                    p_Polygon.Rotation(v_Theta);
                    DrawPolygon(p_Polygon);
                }
            }
            if (m_CircList != null) // Aplicar transformação em uma circunferência
            {
                foreach (MyCircumference p_Circ in m_CircList)
                {
                    p_Circ.Rotation(v_Theta);
                    DrawCirc(p_Circ, false);
                }
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
            if (m_PointListAux != null)
            {
                foreach (MyPoint p_Point in m_PointList)
                {
                    p_Point.Reflection(m_XCheck.IsChecked == true, m_YCheck.IsChecked == true, v_XCanvas, v_YCanvas);
                    DrawPixelByPoint(p_Point);
                }
            }
            if (m_LineList != null) // Aplicar transformação em uma reta
            {
                foreach (MyLine p_Line in m_LineList)
                {
                    p_Line.Reflection(m_XCheck.IsChecked == true, m_YCheck.IsChecked == true, v_XCanvas, v_YCanvas);
                    DrawLine(p_Line, true);
                }
            }
            if (m_PolygonList != null) // Aplicar transformação em um poligono
            {
                foreach (MyPolygon p_Polygon in m_PolygonList)
                {
                    p_Polygon.Reflection(m_XCheck.IsChecked == true, m_YCheck.IsChecked == true, v_XCanvas, v_YCanvas);
                    DrawListOfPoints(p_Polygon.getAllPoints(RadioButtonDDA.IsChecked == true), true);
                }
            }
            if (m_CircList != null) // Aplicar transformação em uma circunferência
            {
                foreach (MyCircumference p_Circ in m_CircList)
                {
                    p_Circ.Reflection(m_XCheck.IsChecked == true, m_YCheck.IsChecked == true, v_XCanvas, v_YCanvas);
                    DrawCirc(p_Circ, false);
                }
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
            if (!m_HasClipping) {
                m_IsDefiningClipping = true;
                m_ClippingButton.Content = "Limpar Recorte";
            }
            else
            {
                m_HasClipping = false;
                m_ClippingButton.Content = "Definir Recorte";
                Canvas.Children.Clear();

                // Redesenhando figuras originais
                foreach (MyLine p_Line in m_LineList)
                {
                    DrawLine(p_Line, false);
                }

                foreach (MyPolygon p_Polygon in m_PolygonList)
                {
                    DrawPolygon(p_Polygon);
                }

                foreach (MyPoint p_Point in m_PointList)
                {
                    DrawPixelByPoint(p_Point);
                }

                foreach (MyCircumference p_Circ in m_CircList)
                {
                    DrawCirc(p_Circ, false);
                }

                m_MaxCoordClipping = m_MinCoordClipping = null;
            }
        }

        /* Método para limpar tela */
        private void RemoveItensFromCanvas(object sender, EventArgs e)
        {

            // Canvas não possui mais desenhos
            m_HasDrawing = false;
            Canvas.Children.Clear();
            // Limpar atributos da tela
            m_PointListAux = new List<MyPoint> ();
            m_PointList = new List<MyPoint>();
            m_LineList = new List<MyLine>();
            m_PolygonList = new List<MyPolygon>();
            m_CircList = new List<MyCircumference>();
            m_MinCoordClipping = null;
            m_MaxCoordClipping = null;
            m_HasClipping = false;
            m_IsDefiningClipping = false;
            m_ClippingButton.Content = "Definir Recorte";
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
