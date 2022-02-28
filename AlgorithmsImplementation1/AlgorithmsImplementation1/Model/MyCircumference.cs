using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgorithmsImplementation1.Model
{
    public class MyCircumference
    {
        MyPoint m_Center { get; set; }
        int m_Radius { get; set; }

        // Ponto fixo rotação -> variável auxiliar
        MyPoint m_FixedPointRotation { get; set; }
        

        // Construtor vazio
        public MyCircumference()
        {
            m_Center = null;
            m_Radius = 0;
        }

        // Construtor
        public MyCircumference(MyPoint p_Center, int p_Radius)
        {
            m_Center = p_Center;
            m_Radius = p_Radius;
            m_FixedPointRotation = new MyPoint();
            this.DefineRotationFixedPoint();

        }

        /* Método para que retorna todos pontos simétricos 
         * da circunferência a partir de um ponto de referência
         * @param int p_X, int p_Y
         * @return List<MyPoint> Lista de pontos simétricos
         */
        public List<MyPoint> GetSimetricPoints(int p_X, int p_Y)
        {
            List<MyPoint> v_Points = new List<MyPoint>();

            v_Points.Add(new MyPoint(this.m_Center.getIntX() + p_X, this.m_Center.getIntY() + p_Y));
            v_Points.Add(new MyPoint(this.m_Center.getIntX() - p_X, this.m_Center.getIntY() + p_Y));
            v_Points.Add(new MyPoint(this.m_Center.getIntX() + p_X, this.m_Center.getIntY() - p_Y));
            v_Points.Add(new MyPoint(this.m_Center.getIntX() - p_X, this.m_Center.getIntY() - p_Y));
            v_Points.Add(new MyPoint(this.m_Center.getIntX() + p_Y, this.m_Center.getIntY() + p_X));
            v_Points.Add(new MyPoint(this.m_Center.getIntX() - p_Y, this.m_Center.getIntY() + p_X));
            v_Points.Add(new MyPoint(this.m_Center.getIntX() + p_Y, this.m_Center.getIntY() - p_X));
            v_Points.Add(new MyPoint(this.m_Center.getIntX() - p_Y, this.m_Center.getIntY() - p_X));

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

        /* Método para aplicar tranlação na circunferência
         * @param double p_X, double p_Y -> coordenadas do vetor de
         *                            translação
         */
        public void Translation(double p_VectorX, double p_VectorY)
        {
            // Aplicando no centro da circunferência
            m_Center.TranslateSum(p_VectorX, p_VectorY);

            // Redefinindo ponto fixo para tranformação de rotação
            DefineRotationFixedPoint();
        }

        /* Método para aplicar escala na circunferência -> centro fixo
         * @param double p_X, double p_Y -> coordenadas do vetor de
         *                            translação
         */
        public void Scale(double p_Vector)
        {
            // Aplicando escala no raio da circunferência
            this.m_Radius = (int)Math.Round((double)this.m_Radius * p_Vector);

            // Redefinindo ponto fixo para tranformação de rotação
            DefineRotationFixedPoint();
        }

        /* Método para aplicar rotação na circunferência -> em relação 
         * ao ponto menor coordenada de y da circunferência original
         * @param double p_X, double p_Y -> coordenadas do vetor de
         *                            translação
         */
        public void Rotation(double p_Theta)
        {
            double v_TransX = m_FixedPointRotation.getX();
            double v_TransY = m_FixedPointRotation.getY();
            // Posicionado ponto com menor y na origem, ou seja
            // centro com x = 0 e y = raio
            this.m_Center.TranslateSum(v_TransX * -1, v_TransY * -1);

            // Aplicando rotação no centro
            this.m_Center.Rotation(p_Theta);
            // Desfazendo translação
            this.m_Center.TranslateSum(v_TransX, v_TransY);

        }


        /* Método para setar ponto com menor Y da circunferência
         * como ponto fixo para rotação
         */
        public void DefineRotationFixedPoint()
        {
            // Setando ponto fixo auxiliar para rotação
            // da circunferência. O ponto será o ponto com
            // menor y da circunferência original
            m_FixedPointRotation.setX(m_Center.getX());
            m_FixedPointRotation.setY(m_Center.getY() + m_Radius);
        }
    }
}
