using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgorithmsImplementation1.Model
{
    public class Matrix
    {
        /* Método estático para multiplicar 2 matrizes
         * @param Matrix m_Matrix1, Matrix m_Matrix2
         * @return Matrix resultado da multiplicação
         */
        public static Matrix MultiplyMatrix(Matrix m_Matrix1, Matrix m_Matrix2)
        {
            // Criando Matriz resultado
            Matrix v_Result = new Matrix(m_Matrix1.m_NumRow, m_Matrix2.m_NumColumn);

            for (int i = 0; i < v_Result.m_NumRow; i++)
            {
                for (int j = 0; j < v_Result.m_NumColumn; j++)
                {
                    // Somatório da multiplicação de elementos da matriz 1 e 2
                    double v_Sum = 0;

                    for (int k = 0; k < m_Matrix1.m_NumColumn; k++)
                    {
                        v_Sum += m_Matrix1.getValuePosition(i, k) * m_Matrix2.getValuePosition(k, j);
                    }
                    // Setando valor do somatório das multiplicações
                    // na posiçãoo i, j
                    v_Result.setValuePosition(i, j, v_Sum);
                }
            }

            return v_Result;
        }

        // Atributos
        public double[,] m_Matrix { get; }
        public int m_NumRow { get; set; }
        public int m_NumColumn { get; set; }

        // Construtor vazio
        public Matrix() : this(2) { }
        // Construtor
        public Matrix(int p_NumRowColumn) : this(p_NumRowColumn, p_NumRowColumn) { }

        // Construtor
        public Matrix(int p_NumRow, int p_NumColumn)
        {
            this.m_NumRow = p_NumRow;
            this.m_NumColumn = p_NumColumn;
            
            // Alocando Matrix
            this.m_Matrix = new double[this.m_NumRow, this.m_NumColumn];
        }

        /* Método para setar valor em uma posição da matriz
         * @param int p_Row, int p_Column, int p_Value
         */
        public void setValuePosition(int p_Row, int p_Column, double p_Value)
        {
            this.m_Matrix[p_Row, p_Column] = p_Value;
        }

        /* Método para retornar valor em uma posição da matriz
         * @param int p_Row, int p_Column
         */
        public double getValuePosition(int p_Row, int p_Column)
        {
            return this.m_Matrix[p_Row, p_Column];
        }

        /* Método para setar matriz como identidade */
        public void SetIdentity()
        {
            for (int i = 0; i < this.m_NumRow; i++)
                for (int j = 0; j < this.m_NumRow; j++)
                    // Valores na diagonal = 1, fora da diagonal = 0 
                    this.m_Matrix[i, j] = i == j ? 1 : 0;
        }
        

    }
}
