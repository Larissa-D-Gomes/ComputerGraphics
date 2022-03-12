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
            int v_DeltaX = (int)Math.Round(this.m_Point2.getX() - this.m_Point1.getX());
            int v_DeltaY = (int)Math.Round(this.m_Point2.getY() - this.m_Point1.getY());

            // Definindo quantidade de passos do algoritmo a partir
            // do maior módulo delta de x ou y
            int v_Steps = Math.Abs(v_DeltaX) > Math.Abs(v_DeltaY) ?
                          Math.Abs(v_DeltaX) : Math.Abs(v_DeltaY);

            // Definindo incremento que será feito ao x e y
            // de cada coordenada calculado em cada passo 
            double v_IncrX = (double)v_DeltaX / (double)v_Steps;
            double v_IncrY = (double)v_DeltaY / (double)v_Steps;

            // X e Y do ponto anterior 
            double v_InitX = (double)this.m_Point1.getX();
            double v_InitY = (double)this.m_Point1.getY();

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

        /* Implementção do Algoritmo BresenhamAlgorithm para rasterização de retas em 
         * um canvas a partir dos dois pontos de extremidade
         * @return List<MyPoint> -> Lista de pontos a serem desenhados
         *  na Matriz (Canvas)
         */
        public List<MyPoint> BresenhamAlgorithm()
        {
            List<MyPoint> v_LinePoints = new List<MyPoint>();

            // Definindo Delta x e y da reta
            int v_DeltaX = this.m_Point2.getIntX() - this.m_Point1.getIntX();
            int v_DeltaY = this.m_Point2.getIntY() - this.m_Point1.getIntY();

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

            int v_InitX = this.m_Point1.getIntX();
            int v_InitY = this.m_Point1.getIntY();

            // Adicionando ponto à reta
            v_LinePoints.Add(new MyPoint(v_InitX, v_InitY));

            // Calculando pontos no Caso 1 
            if (v_DeltaX > v_DeltaY)
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

        /* Método para aplicar tranlação na reta
         * @param double p_X, double p_Y -> coordenadas do vetor de
         *                            translação
         */
        public void Translation(double p_VectorX, double p_VectorY)
        {
            // Aplicando no ponto inicial
            m_Point1.TranslateSum(p_VectorX, p_VectorY);
            // Aplicando no ponto final
            m_Point2.TranslateSum(p_VectorX, p_VectorY);
        }

        /* Método para aplicar escala na reta. Ponto de referêcia
         * será o ponto inicial (Primeiro desenhado).
         * @param double p_X, double p_Y -> coordenadas do vetor de
         *                                  escala
         */
        public void Scale(double p_VectorX, double p_VectorY)
        {
            Scale(p_VectorX, p_VectorY, m_Point1.getX(), m_Point1.getY());
        }

        /* Método para aplicar escala na reta conforme ponto de referêcia fixo
         * @param double p_X, double p_Y -> coordenadas do vetor de
         *                                  escala
         */
        public void Scale(double p_VectorX, double p_VectorY, double p_ReferenceX, double p_ReferenceY)
        {

            // Transladando pontos em relação a ponto de referencia
            m_Point1.setX(m_Point1.getX() - p_ReferenceX);
            m_Point1.setY(m_Point1.getY() - p_ReferenceY);
            m_Point2.setX(m_Point2.getX() - p_ReferenceX);
            m_Point2.setY(m_Point2.getY() - p_ReferenceY);

            // Aplicando no ponto inicial
            m_Point1.Scale(p_VectorX, p_VectorY);
            // Aplicando no ponto final
            m_Point2.Scale(p_VectorX, p_VectorY);

            // Transladando pontos para coordenadas finais
            m_Point1.setX(m_Point1.getX() + p_ReferenceX);
            m_Point1.setY(m_Point1.getY() + p_ReferenceY);
            m_Point2.setX(m_Point2.getX() + p_ReferenceX);
            m_Point2.setY(m_Point2.getY() + p_ReferenceY);

        }


        /* Método para aplicar rotação na reta. Ponto de referêcia
         * será o ponto inicial (Primeiro desenhado).
         * @param double p_X, double p_Y -> coordenadas do vetor de
         *                                  escala
         */
        public void Rotation(double p_Theta)
        {
            Rotation(p_Theta, m_Point1.getX(), m_Point1.getY());
        }

        /* Método para aplicar rotação na reta conforme ponto de referêcia fixo
         * @param double double p_Theta, double p_ReferenceX, double p_ReferenceY
         * -> coordenadas do vetor de escala
         */
        public void Rotation(double p_Theta, double p_ReferenceX, double p_ReferenceY)
        {

            // Transladando pontos em relação a ponto de referencia
            m_Point1.setX(m_Point1.getX() - p_ReferenceX);
            m_Point1.setY(m_Point1.getY() - p_ReferenceY);
            m_Point2.setX(m_Point2.getX() - p_ReferenceX);
            m_Point2.setY(m_Point2.getY() - p_ReferenceY);

            // Aplicando no ponto inicial
            m_Point1.Rotation(p_Theta);
            // Aplicando no ponto final
            m_Point2.Rotation(p_Theta);

            // Transladando pontos para coordenadas finais
            m_Point1.setX(m_Point1.getX() + p_ReferenceX);
            m_Point1.setY(m_Point1.getY() + p_ReferenceY);
            m_Point2.setX(m_Point2.getX() + p_ReferenceX);
            m_Point2.setY(m_Point2.getY() + p_ReferenceY);

        }

        /* Método para aplicar reflexão na reta, em relação aos eixos do meio do canvas.
         * O eixo Y será definido pela metade do width do canvas e X pela metade do heigth
         * @param bool p_ApplyX, bool p_ApplyY, double p_MiddleXCanvas, double p_MiddleYCanvas
         */
        public void Reflection(bool p_ApplyX, bool p_ApplyY, double p_MiddleXCanvas, double p_MiddleYCanvas)
        {
            // Aplicando no ponto inicial
            m_Point1.Reflection(p_ApplyX, p_ApplyY, p_MiddleXCanvas, p_MiddleYCanvas);
            // Aplicando no ponto final
            m_Point2.Reflection(p_ApplyX, p_ApplyY, p_MiddleXCanvas, p_MiddleYCanvas);
        }

        /* Método para calcular segmento de reta que será plotado na 
         * área de recorte, por Cohen-Sutherland, algoritmo que utiliza áreas codificadas
         * @param int p_XMax, int p_XMin, int p_YMax, int p_YMin -> coordenadas da área de recorte
         * @return MyLine -> segmento de reta a ser plotado
         */
        public MyLine CohenSutherland(int p_XMax, int p_XMin, int p_YMax, int p_YMin)
        {
            MyLine v_ClipplingLine = null;

            bool v_Done = false;
            bool v_Accepted = false;

            double v_X1 = this.m_Point1.getX();
            double v_Y1 = this.m_Point1.getY();
            double v_X2 = this.m_Point2.getX();
            double v_Y2 = this.m_Point2.getY();
            double v_X = 0 , v_Y = 0;

            // Enquanto cálculo não for finalizado
            while (!v_Done)
            {
                // Calculando códigos de pontos inicial e final da reta 
                int v_Code1 = getRegionCode((int)Math.Round(v_X1), (int)Math.Round(v_Y1), 
                                             p_XMax, p_XMin, p_YMax, p_YMin);
                int v_Code2 = getRegionCode((int)Math.Round(v_X2), (int)Math.Round(v_Y2), 
                                            p_XMax, p_XMin, p_YMax, p_YMin);

                int v_CodeOut;

                // Reta comepletamente dentro da área de corte
                if (v_Code1 == 0 && v_Code2 == 0)
                    v_Done = v_Accepted = true; // Nenhum recorte a fazer

                // Se algum bit mesma posição nos códigos forem iguais a 1
                // o segmento está completamente fora em alguma lateral.
                else if ((v_Code1 & v_Code2) != 0)
                    v_Done = true;
                // Se ponto 1 está fora
                else
                {
                    // Determinando ponto exterior
                    if (v_Code1 != 0)
                        v_CodeOut = v_Code1;
                    else
                        v_CodeOut = v_Code2;

                    // Se bit 1 diferente de 0, definir intercessão com
                    // limite esquerdo da área de recorte do ponto
                    if ((v_CodeOut & (1 << 0)) != 0)
                    {
                        v_X = p_XMin;
                        v_Y = v_Y1 + (v_Y2 - v_Y1) * (p_XMin - v_X1) / (v_X2 - v_X1);
                    }
                    // Se bit 2 diferente de 0, definir intercessão com
                    // limite direito da área de recorte do ponto 
                    else if ((v_CodeOut & (1 << 1)) != 0)
                    {
                        v_X = p_XMax;
                        v_Y = v_Y1 + (v_Y2 - v_Y1) * (p_XMax - v_X1) / (v_X2 - v_X1);
                    }
                    // Se bit 3 diferente de 0, definir intercessão com
                    // limite inferior da área de recorte do ponto 
                    else if ((v_CodeOut & (1 << 2)) != 0)
                    {
                        v_X = v_X1 + (v_X2 - v_X1) * (p_YMin - v_Y1) / (v_Y2 - v_Y1);
                        v_Y = p_YMin;
                    }
                    // Se bit 4 diferente de 0, definir intercessão com
                    // limite superior da área de recorte do ponto 
                    else if ((v_CodeOut & (1 << 3)) != 0)
                    {
                        v_X = v_X1 + (v_X2 - v_X1) * (p_YMax - v_Y1) / (v_Y2 - v_Y1);
                        v_Y = p_YMax;
                    }
                    if (v_CodeOut == v_Code1)
                    {
                        v_X1 = v_X;
                        v_Y1 = v_Y;
                    }
                    else
                    {
                        v_X2 = v_X;
                        v_Y2 = v_Y;
                    }
                }
            }

            if (v_Accepted)
            {
                MyPoint v_P1 = new MyPoint((int)Math.Round(v_X1), (int)Math.Round(v_Y1));
                MyPoint v_P2 = new MyPoint((int)Math.Round(v_X2), (int)Math.Round(v_Y2));
                v_ClipplingLine = new MyLine(v_P1, v_P2);
            }

            return v_ClipplingLine;

        }

        /* Método para definir código do ponto na área de recorte 
         * para o algoritmo de Cohen-Sutherland
         * @param int p_X, int p_Y, int p_XMax, int p_XMin, int p_YMax, int p_Ymin
         * @return int code
         */
        private int getRegionCode(int p_X, int p_Y, int p_XMax, int p_XMin, int p_YMax, int p_YMin)
        {
            int v_Code = 0;

            // Verificar se ponto está à esquerda da área de recorte
            if (p_X < p_XMin)
                v_Code += 1; // Setando primeiro bit

            // Verificar se ponto está à direita da área de recorte
            if (p_X > p_XMax)
                v_Code += 2; // Setando segundo bit

            // Verificar se ponto está abaixo da área de recorte
            if (p_Y < p_YMin)
                v_Code += 4; // Setando terceiro bit

            // Verificar se ponto está acima da área de recorte
            if (p_Y > p_YMax)
                v_Code += 8; // Setando quarto bit

            return v_Code;
        }


        /* Método para calcular segmento de reta que será plotado na 
         * área de recorte, por LiangBarsky, algoritmo que utiliza equação paramétrica da reta
         * @param int p_XMax, int p_XMin, int p_YMax, int p_YMin -> coordenadas da área de recorte
         * @return MyLine -> segmento de reta a ser plotado
         */
        public MyLine LiangBarsky(int p_XMax, int p_XMin, int p_YMax, int p_YMin)
        {
            MyLine v_ClipplingLine = null;

            //Variáveis u da equação paramétrica da reta
            double v_U1 = 0.0;
            double v_U2 = 1.0;

            double v_DeltaX = m_Point2.getX() - m_Point1.getX();
            double v_DeltaY = m_Point2.getY() - m_Point1.getY();

            // Chance de intercessão pela fronteira da esquerda
            if(ClipTest(v_DeltaX * -1, m_Point1.getX() - p_XMin, ref v_U1, ref v_U2))
                // Chance de intercessão pela fronteira da direita
                if (ClipTest(v_DeltaX, p_XMax - m_Point1.getX(), ref v_U1, ref v_U2))
                    // Chance de intercessão pela fronteira inferior
                    if (ClipTest(v_DeltaY * -1, m_Point1.getY() - p_YMin, ref v_U1, ref v_U2))
                        // Chance de intercessão pela fronteira superior 
                        if (ClipTest(v_DeltaY, p_YMax - m_Point1.getY(), ref v_U1, ref v_U2))
                        {
                            MyPoint v_P1 = new MyPoint(m_Point1.getIntX(), m_Point1.getIntY());
                            MyPoint v_P2 = new MyPoint(m_Point2.getIntX(), m_Point2.getIntY());
                            // Reta está dentro da área de recorte
                            v_ClipplingLine = new MyLine(v_P1, v_P2);

                            // Caso ponto final do segmento for diferente do
                            // ponto final original, atualizar ponto conforme u encontrado
                            if (v_U2 < 1.0)
                            {
                                v_ClipplingLine.m_Point2.setX((int)Math.Round(this.m_Point1.getX() + v_U2 * v_DeltaX));
                                v_ClipplingLine.m_Point2.setY((int)Math.Round(this.m_Point1.getY() + v_U2 * v_DeltaY));
                            }


                            // Caso ponto inicial do segmento for diferente do
                            // ponto final original, atualizar ponto conforme u encontrado
                            if (v_U1 > 0.0)
                            {
                                v_ClipplingLine.m_Point1.setX((int)Math.Round(this.m_Point1.getX() + v_U1 * v_DeltaX));
                                v_ClipplingLine.m_Point1.setY((int)Math.Round(this.m_Point1.getY() + v_U1 * v_DeltaY));
                            }
                        }

            return v_ClipplingLine;
        }


        /* Função que analisa chance da reta estar dentro da janela de visualização
         * a partir do u d equação paramétrica da reta, analisando sentido de 
         * crescimento. Variáveis U originais são passadas como referência 
         * para poderem ser atualizadas conforme u dos pontos candidatos
         * a intercessão com limites da janela.
         * @param double p_P, double p_Q, ref double p_U1, ref double p_U2
         */
        public bool ClipTest(double p_P, double p_Q, ref double p_U1, ref double p_U2)
        {
            bool v_Result = true;

            // Caso que reta cresce de fora para dentro
            if (p_P < 0.0)
            {
                // Calculando U de intercessão com a fronteira
                double v_UR = p_Q / p_P;

                // Se v_UR > p_U2, não há como haver intercessão com fronteira,
                // pois o valor de u é impossível, reta completamente depois da fronteira.
                if (v_UR > p_U2)
                    v_Result = false;
                else if (v_UR > p_U1) // Há como haver intercessão com a fronteira
                                      // A reta está depois da fronteira calculada
                    p_U1 = v_UR;
            }
            else if (p_P > 0.0)// Caso que reta cresce de dentro para fora 
            {
                // Calculando U de intercessão com a fronteira
                double v_UR = p_Q / p_P;

                // Se v_UR < p_U1, não há como haver intercessão com fronteira,
                // pois o valor de u é impossível, reta completamente antes da fronteira.
                if (v_UR < p_U1)
                    v_Result = false;
                else if (v_UR < p_U2) // Há como haver intercessão com a fronteira
                                      // A reta está depois da fronteira calculada
                    p_U2 = v_UR;
            }
            else if (p_Q < 0.0) // Reta paralela a fronteira sem chance de intercessão
            {
                v_Result = false;
            }

            return v_Result;
        }

    }
}
