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
            int v_DeltaX = this.m_Point2.getX() - this.m_Point1.getX();
            int v_DeltaY = this.m_Point2.getY()- this.m_Point1.getY();

            // Definindo quantidade de passos do algoritmo a partir
            // do maior módulo delta de x ou y
            int v_Steps = Math.Abs(v_DeltaX) > Math.Abs(v_DeltaY) ?
                          Math.Abs(v_DeltaX) : Math.Abs(v_DeltaY);

            // Definindo incremento que será feito ao x e y
            // de cada coordenada calculado em cada passo 
            double v_IncrX = (double) v_DeltaX / (double) v_Steps;
            double v_IncrY = (double) v_DeltaY / (double) v_Steps;

            // X e Y do ponto anterior 
            double v_InitX = (double) this.m_Point1.getX();
            double v_InitY = (double) this.m_Point1.getY();

            v_LinePoints.Add(new MyPoint((int)Math.Round(v_InitX),
                                             (int)Math.Round(v_InitY)));

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


        public List<MyPoint> BresenhamAlgorithm()
        {
            List<MyPoint> v_LinePoints = new List<MyPoint>();

            // Definindo Delta x e y da reta
            int v_DeltaX = this.m_Point2.getX() - this.m_Point1.getX();
            int v_DeltaY = this.m_Point2.getY() - this.m_Point1.getY();

            // Definindo incremento de X
            int v_IncrX;

            // Se delta x for positivo, incremento será positivo
            if (v_DeltaX >= 0)
                v_IncrX = 1;
            else // Se delta x for negativo, incremento será negativo
            {
                v_DeltaX = Math.Abs(v_DeltaX);
                v_IncrX = -1;
            }


            // Definindo incremento de Y
            int v_IncrY;

            // Se delta y for positivo, incremento será positivo
            if (v_DeltaY >= 0)
                v_IncrY = 1;
            else // Se delta y for negativo, incremento será negativo
            {
                v_DeltaY = Math.Abs(v_DeltaY);
                v_IncrY = -1;
            }

            int v_InitX = this.m_Point1.getX();
            int v_InitY = this.m_Point1.getY();

            // Adicionando ponto à reta
            v_LinePoints.Add(new MyPoint(v_InitX, v_InitY));

            // Calculando pontos no Caso 1 
            if(v_DeltaX > v_DeltaY)
            {
                // Variável para decidir de Y será incrementado ou não
                // Cálculo de P0
                int v_P = 2 * v_DeltaY - v_DeltaX;

                // Calculo de constantes
                int v_Const1 = 2 * v_DeltaY;
                int v_Const2 = 2 * (v_DeltaY - v_DeltaX);

                // Iteração para calcular os pontos da reta
                for (int i = 0; i < v_DeltaX; i++)
                {
                    // x será incrementado em todos os passos
                    v_InitX += v_IncrX;

                    // y só será incrementado se na iteração anterior
                    // o P calculado for >= 0
                    if (v_P >= 0)
                    {
                        // Incrementando Y
                        v_InitY += v_IncrY;
                        // Calculando P
                        v_P += v_Const2;
                    }                        
                    else
                    {
                        // Calculando P
                        v_P += v_Const1;
                    }
                    // Adicionando ponto calculado à reta
                    v_LinePoints.Add(new MyPoint(v_InitX, v_InitY));
                }
            }
            else
            {
                // Variável para decidir de X será incrementado ou não
                // Cálculo de P0
                int v_P = 2 * v_DeltaX - v_DeltaY;

                // Calculo de constantes
                int v_Const1 = 2 * v_DeltaX;
                int v_Const2 = 2 * (v_DeltaX - v_DeltaY);

                // Iteração para calcular os pontos da reta
                for (int i = 0; i < v_DeltaY; i++)
                {
                    // y será incrementado em todos os passos
                    v_InitY += v_IncrY;

                    // x só será incrementado se na iteração anterior
                    // o P calculado for >= 0
                    if (v_P >= 0)
                    {
                        // Incrementando X
                        v_InitX += v_IncrX;
                        // Calculando P
                        v_P += v_Const2;
                    }
                    else
                    {
                        // Calculando P
                        v_P += v_Const1;
                    }
                    // Adicionando ponto calculado à reta
                    v_LinePoints.Add(new MyPoint(v_InitX, v_InitY));
                }
            }

            return v_LinePoints;
        }

    }
}
