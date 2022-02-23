using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgorithmsImplementation1.Model
{
    internal class MyCircumference
    {
        MyPoint m_Center { get; set; }
        int m_Radius { get; set; }

        // Constructor
        public MyCircumference(MyPoint p_Center, int p_Radius)
        {
            m_Center = p_Center;
            m_Radius = p_Radius;
        }

        /* Método para que retorna todos pontos simétricos 
         * da circunferência a partir de um ponto de referência
         * @param int p_X, int p_Y
         * @return List<MyPoint> Lista de pontos simétricos
         */
        public List<MyPoint> GetSimetricPoints(int p_X, int p_Y)
        {
            List<MyPoint> v_Points = new List<MyPoint>();

            v_Points.Add(new MyPoint(this.m_Center.getX() + p_X, this.m_Center.getY() + p_Y));
            v_Points.Add(new MyPoint(this.m_Center.getX() - p_X, this.m_Center.getY() + p_Y));
            v_Points.Add(new MyPoint(this.m_Center.getX() + p_X, this.m_Center.getY() - p_Y));
            v_Points.Add(new MyPoint(this.m_Center.getX() - p_X, this.m_Center.getY() - p_Y));
            v_Points.Add(new MyPoint(this.m_Center.getX() + p_Y, this.m_Center.getY() + p_X));
            v_Points.Add(new MyPoint(this.m_Center.getX() - p_Y, this.m_Center.getY() + p_X));
            v_Points.Add(new MyPoint(this.m_Center.getX() + p_Y, this.m_Center.getY() - p_X));
            v_Points.Add(new MyPoint(this.m_Center.getX() - p_Y, this.m_Center.getY() - p_X));

            return v_Points;
        }

        /* Método para calcilar pontos a serem plotados
         * pelo algoritmo de Bresenham
         * @return List<MyPoint> pontos da circunferência a serem plotados
         */
        public List<MyPoint> BresenhamAlgorithm()
        {
            List<MyPoint> v_Points = new List<MyPoint>();

            // Setando x = 0 e Y = raio
            // Considerar que centro da circunferência está
            // no centro com a finalidade de facilitar calculos
            int v_X = 0;
            int v_Y = this.m_Radius;

            // Variável de decisão se y irá variar em cada iteraçãp
            int v_P = 3 - 2 * this.m_Radius;

            // Adicionando pontos simetricos de (0, raio)
            v_Points.AddRange(GetSimetricPoints(v_X, v_Y));

            // Calcular apenas pontos do segundo octante
            while (v_X < v_Y)
            {
                // Se variável de decisão for negativa,
                // y não irá variar
                if (v_P < 0)
                {
                    v_P += 4 * v_X + 6;
                }
                else// Se variável de decisão for positiva, y irá variar
                {
                    v_P += 4 * (v_X - v_Y) + 10;
                    v_Y--;
                }

                // X sempre irá variar
                v_X++;

                // Adicionando pontos simetricos de (x, y)
                v_Points.AddRange(GetSimetricPoints(v_X, v_Y));
            }

            return v_Points;
        }
    }
}
