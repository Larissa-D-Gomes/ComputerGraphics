using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AlgorithmsImplementation1.Model
{
    public class MyLine
    {
        public MyPoint m_Point1 { get; set; }
        public MyPoint m_Point2 { get; set; }

        // Construtor vazio
        public MyLine() { }

        // Construtor 
        public MyLine(MyPoint p_Point1, MyPoint p_Point2)
        {
            this.m_Point1 = p_Point1;
            this.m_Point2 = p_Point2;
        }

        /* Implementção do Algoritmo DDA para rasterização de retas em 
         * um canvas a partir dos dois pontos de extremidade
         * @return List<MyPoint> -> Lista de pontos a serem desenhados
         *  na Matriz (Canvas)
         */
        public List<MyPoint> DDAAlgorithm()
        {
            List<MyPoint> v_LinePoints = new List<MyPoint>();

            // Definindo Delta x e y da reta
            double v_DeltaX = this.m_Point2.m_X - this.m_Point1.m_X;
            double v_DeltaY = this.m_Point2.m_Y - this.m_Point1.m_Y;

            // Definindo quantidade de passos do algoritmo a partir
            // do maior módulo delta de x ou y
            int v_Steps = Math.Abs(v_DeltaX) > Math.Abs(v_DeltaY) ?
                                (int) Math.Round(Math.Abs(v_DeltaX)) :
                                (int) Math.Round(Math.Abs(v_DeltaY));

            // Definindo incremento que será feito ao x e y
            // de cada coordenada calculado em cada passo 
            double v_IncrX = v_DeltaX / v_Steps;
            double v_IncrY = v_DeltaY / v_Steps;

            // X e Y do pontos inicial 
            double v_InitX = this.m_Point1.m_X;
            double v_InitY = this.m_Point1.m_Y;

            // Iterção para calcular todos os pontos da reta
            // que serão desenhados no canvas
            for (int i = 0; i < v_Steps; i++)
            {
                v_InitX += v_IncrX;
                v_InitY += v_IncrY;

                // Adicionando novo ponto calculado, com valores de
                // x e y arredondados
                v_LinePoints.Add(new MyPoint((int)Math.Round(v_InitX), 
                                             (int)Math.Round(v_InitY)));
            }

            return v_LinePoints;
        }
    }
}
