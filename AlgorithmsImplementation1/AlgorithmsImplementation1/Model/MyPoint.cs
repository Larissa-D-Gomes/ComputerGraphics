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
            m_Point = new Matrix(2, 1);
        }

        // Construtor 
        public MyPoint(int p_X, int p_Y)
        {
            m_Point = new Matrix(2, 1); 
            this.setX(p_X);
            this.setY(p_Y);
        }

        public void setX(double p_X)
        {
            this.m_Point.setValuePosition(0, 0, p_X);
        }

        public void setY(double p_Y)
        {
            this.m_Point.setValuePosition(1, 0, p_Y);
        }

        public double getX()
        {
            return this.m_Point.getValuePosition(0, 0);
        }

        public double getY()
        {
            return this.m_Point.getValuePosition(1, 0);
        }

        public int getIntX()
        {
            return (int)Math.Round(this.m_Point.getValuePosition(0, 0));
        }


        public int getIntY()
        {
            return (int)Math.Round(this.m_Point.getValuePosition(1, 0));
        }

        /* Translação de ponto por soma 
         * @param double p_X, double p_Y -> coordenadas do vetor de
         *                            translação
         */
        public void TranslateSum(double p_VectorX, double p_VectorY)
        {
            // x += p_VectorX
            this.setX(this.getX() + p_VectorX);


            // y += p_VectorY
            this.setY(this.getY() + p_VectorY);
        }

        /* Escala de ponto 
         * @param double p_X, double p_Y -> coordenadas do vetor de
         *                            translação
         */
        public void Scale(double p_VectorX, double p_VectorY)
        {
            // Instanciando matriz de fatores da escala
            Matrix v_MatrixScale = new Matrix(2, 2);

            // Setando identidade
            v_MatrixScale.SetIdentity();

            // Posição 0,0 = x do vetor de escala
            v_MatrixScale.setValuePosition(0, 0, p_VectorX);
            
            // Posição 1,1 = y do vetor de escala
            v_MatrixScale.setValuePosition(1, 1, p_VectorY);

            // Ponto = Matriz Escala * Matriz Ponto
            this.m_Point = Matrix.MultiplyMatrix(v_MatrixScale, this.m_Point);
        }


        /* Rotação de ponto 
         * @param double p_Theta
         */
        public void Rotation(double p_Theta)
        {
            // Instanciando matriz de fatores da escala
            Matrix v_MatrixRotation = new Matrix(2, 2);

            // Posição 0,0 = cos(θ) do vetor de escala
            v_MatrixRotation.setValuePosition(0, 0, Math.Cos(p_Theta));

            // Posição 0,1 = -sen(θ) do vetor de escala
            v_MatrixRotation.setValuePosition(0, 1, Math.Sin(p_Theta) * -1);

            // Posição 1,0 = sen(θ) do vetor de escala
            v_MatrixRotation.setValuePosition(1, 0, Math.Sin(p_Theta));

            // Posição 1,1 = cos(θ) do vetor de escala
            v_MatrixRotation.setValuePosition(1, 1, Math.Cos(p_Theta));

            // Ponto = Matriz Escala * Matriz Ponto
            this.m_Point = Matrix.MultiplyMatrix(v_MatrixRotation, this.m_Point);
        }

        /* Método para aplicar reflexão no ponto, em relação aos eixos do meio do canvas.
         * O eixo Y será definido pela metade do width do canvas e X pela metade do heigth
         * @param bool p_ApplyX, bool p_ApplyY, double p_MiddleXCanvas, double p_MiddleYCanvas
         */
        public void Reflection(bool p_ApplyX, bool p_ApplyY, double p_MiddleXCanvas, double p_MiddleYCanvas)
        {
            // Instanciando matriz de fatores da escala
            Matrix v_MatrixReflection = new Matrix(2, 2);

            // Setando identidade
            v_MatrixReflection.SetIdentity();

            // Aplicar reflexão em relação ao eixo x
            if (p_ApplyX)
                // Posição 0,0 = x do vetor de escala
                v_MatrixReflection.setValuePosition(0, 0, -1);
            else
                p_MiddleXCanvas = 0;

            // Aplicar reflexão em relação ao eixo x
            if (p_ApplyY)
                // Posição 1,1 = y do vetor de escala
                v_MatrixReflection.setValuePosition(1, 1, -1);
            else
                p_MiddleYCanvas = 0;

            // Ponto = Matriz Reflexão * Matriz Ponto
            this.m_Point = Matrix.MultiplyMatrix(v_MatrixReflection, this.m_Point);
            // Rotacionar em relação aos eixos do meio do canvas
            TranslateSum(p_MiddleXCanvas, p_MiddleYCanvas);
        }
    }
}
