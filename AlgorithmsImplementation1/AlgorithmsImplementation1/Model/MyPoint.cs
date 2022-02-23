using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgorithmsImplementation1.Model
{
    public class MyPoint
    {
        /* Método para calcular distância entre dois pontos
         * @param MyPoint p_Point1, MyPoint p_Point2
         * @return double distância 
         */
        public static double GetDistanceBetweenTwoPoint(MyPoint p_Point1, MyPoint p_Point2)
        {
            return Math.Sqrt(Math.Pow(p_Point2.getX() - p_Point1.getX(), 2)
                + Math.Pow(p_Point2.getY() - p_Point1.getY(), 2));
        }

        /* Ponto = |x|
         *         |y|
         */
        public Matrix m_Point = null; 

        // Construtor vazio
        public MyPoint()
        {
            
        }

        // Contrutor 
        public MyPoint(int p_X, int p_Y)
        {
            m_Point = new Matrix(2, 1); 
            this.setX(p_X);
            this.setY(p_Y);
        }

        public void setX(int p_X)
        {
            this.m_Point.setValuePosition(0, 0, p_X);
        }

        public void setY(int p_Y)
        {
            this.m_Point.setValuePosition(1, 0, p_Y);
        }

        public int getX()
        {
            return this.m_Point.getValuePosition(0, 0);
        }

        public int getY()
        {
            return this.m_Point.getValuePosition(1, 0);
        }

        /* Translação de ponto por soma 
         * @param int p_X, int p_Y -> coordenadas do vetor de
         *                            translação
         */
        public void TranslateSum(int p_VectorX, int p_VectorY)
        {
            // x += p_VectorX
            this.setX(this.getX() + p_VectorX);


            // y += p_VectorY
            this.setY(this.getY() + p_VectorY);
        }

        
    }
}
