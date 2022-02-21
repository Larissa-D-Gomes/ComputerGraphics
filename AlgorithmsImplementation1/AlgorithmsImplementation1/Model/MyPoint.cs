using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgorithmsImplementation1.Model
{
    public class MyPoint
    {
        public int m_X { get; set; }
        public int m_Y { get; set; }

        // Construtor vazio
        public MyPoint()
        {
            this.m_X = -1;
            this.m_Y = -1;
        }

        // Contrutor 
        public MyPoint(int p_X, int p_Y)
        {
            this.m_X = p_X;
            this.m_Y = p_Y;
        }

        /* Translação de ponto por multiplicação de matrix
         * -> fórmula genérica
         * @param int p_X, int p_Y -> coordenadas do vetor de
         *                            translação
         */
        public void TranslateMult(int p_X, int p_Y)
        {
            Matrix v_MatrixVetor = new Matrix(3);
            v_MatrixVetor.SetIdentity();
            // Setando vetor de translação
            v_MatrixVetor.setValuePosition(0, 2, p_X);
            v_MatrixVetor.setValuePosition(1, 2, p_Y);

            Matrix v_PointMatrix = this.getPointMatrix();

            // MatrizVetor * Matriz Ponto
            Matrix m_MultMatrix = Matrix.MultiplyMatrix(v_MatrixVetor, v_PointMatrix);

            this.m_X = m_MultMatrix.getValuePosition(0, 0);
            this.m_Y = m_MultMatrix.getValuePosition(0, 1);

        }

        /* Translação de ponto por soma 
         * @param int p_X, int p_Y -> coordenadas do vetor de
         *                            translação
         */
        public void TranslateSum(int p_X, int p_Y)
        {
            this.m_X += p_X;
            this.m_Y += p_Y;
        }

        /* Retorna uma matriz do ponto com coordenadas homogêneas
         * | x |
         * | y |
         * | 1 |
         * @return Matrix
         */
        public Matrix getPointMatrix()
        {
            Matrix v_PointMatrix = new Matrix(3, 1);
            v_PointMatrix.setValuePosition(0, 0, this.m_X);
            v_PointMatrix.setValuePosition(0, 1, this.m_Y);
            v_PointMatrix.setValuePosition(0, 2, 1);

            return v_PointMatrix;
        }
    }
}
