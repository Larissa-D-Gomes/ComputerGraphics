using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgorithmsImplementation1.Model
{
    public class MyPolygon
    {
        public int m_NumOfEdges { get; set; }
        public List<MyLine> m_PolygonLines { get; set; }

        // Construtor Vazio
        public MyPolygon()
        {
            m_PolygonLines = new List<MyLine>();
        }

        /* Método para ligar arestas (pontos) de um poligono 
         * por vértices (retas)
         * @param List<MyPoint> p_Edges
         */
        public void createPolygonByListOfEdges(List<MyPoint> p_Edges)
        {
            this.m_NumOfEdges = p_Edges.Count;

            // Ligar arestas com arestas vizinhas da lista
            for (int i = 0; i < this.m_NumOfEdges - 1; i++)
            {
                this.m_PolygonLines.Add(new MyLine(p_Edges[i], p_Edges[i + 1]));
            }

            // Ligando última aresta com a primeira, para fechar o poligono
            this.m_PolygonLines.Add(new MyLine(p_Edges[this.m_NumOfEdges - 1], p_Edges[0]));
        }

        /* Método para retornar todos os pontos de um poligono
         * a partir da rasterização das retas que o pertencem por
         * DDA ou Bresenham
         * @param bool p_IsDDA -> true utiliza algoritmo de DDA
         *                     -> true utiliza algoritmo de Bresenham
         * @return List<MyPoint> p_PointsOfPolygon -> lista de pontos do poligono
         */
        public List<MyPoint> getAllPoints(bool p_IsDDA)
        {
            List<MyPoint> p_PointsOfPolygon = new List<MyPoint>();

            // Calcular pontos de todas as retas do poligono por DDA

            if(p_IsDDA)
                foreach (MyLine p_Line in this.m_PolygonLines)
                {
                    p_PointsOfPolygon.AddRange(p_Line.DDAAlgorithm());
                }
            else // Calcular pontos de todas as retas do poligono por Bresenham
                foreach (MyLine p_Line in this.m_PolygonLines)
                {
                    p_PointsOfPolygon.AddRange(p_Line.BresenhamAlgorithm());
                }

            return p_PointsOfPolygon;
        }

        /* Método para aplicar tranlação no poligono
         * @param double v_XVector, double v_YVector -> coordenadas do vetor de
         *                            translação
         */
        public void Translation(double p_XVector, double p_YVector)
        {
            foreach (MyLine v_Line in m_PolygonLines)
            {
                v_Line.Translation(p_XVector, p_YVector);
            }
        }

        /* Método para aplicar escala no poligono. Ponto de referêcia
         * será o ponto inicial (Primeiro desenhado).
         * @param double p_X, double p_Y -> coordenadas do vetor de
         *                                  escala
         */
        public void Scale(double p_VectorX, double p_VectorY)
        {
            double v_ReferenceX = this.m_PolygonLines[0].m_Point1.getX();
            double v_ReferenceY = this.m_PolygonLines[0].m_Point1.getY();

            foreach(MyLine v_Line in this.m_PolygonLines)
            {
                v_Line.Scale(p_VectorX, p_VectorY, v_ReferenceX, v_ReferenceY);
            }
        }


        /* Método para aplicar rotação no poligono. Ponto de referêcia
         * será o ponto inicial (Primeiro desenhado).
         * @param double p_X, double p_Y -> coordenadas do vetor de
         *                                  escala
         */
        public void Rotation(double v_Theta)
        {
            double v_ReferenceX = this.m_PolygonLines[0].m_Point1.getX();
            double v_ReferenceY = this.m_PolygonLines[0].m_Point1.getY();

            // Aplicando rotação para cada primeiro ponto de cada reta para
            // evitar rotacionar o ponto duas vezes 
            foreach (MyLine v_Line in this.m_PolygonLines)
            {
                v_Line.m_Point1.TranslateSum(-1 * v_ReferenceX, -1 * v_ReferenceY);
                v_Line.m_Point1.Rotation(v_Theta);
                v_Line.m_Point1.TranslateSum(v_ReferenceX, v_ReferenceY);
            }
        }

        /* Método para aplicar reflexão na reta, em relação aos eixos do meio do canvas.
         * O eixo Y será definido pela metade do width do canvas e X pela metade do heigth
         * @param bool p_ApplyX, bool p_ApplyY, double p_MiddleXCanvas, double p_MiddleYCanvas
         */
        public void Reflection(bool p_ApplyX, bool p_ApplyY, double p_MiddleXCanvas, double p_MiddleYCanvas)
        {
            foreach (MyLine v_Line in this.m_PolygonLines)
            {
                // Aplicar reflexão apenas no primeiro ponto para evitar que a
                // operação seja feita duas vezes no mesmo ponto
                v_Line.m_Point1.Reflection(p_ApplyX, p_ApplyY, p_MiddleXCanvas, p_MiddleYCanvas);
            }
        }

        /* Método para calcular segmentos de retas de poligonos que serão plotados pela
         * área de recorte Cohen-Sutherland
         * @param int p_XMax, int p_XMin, int p_YMax, int p_YMin -> coordenadas da área de recorte
         * @return List<MyLine> -> lista de segmentos de reta a serem plotados
         */
        public List<MyLine> CohenSutherland(int p_XMax, int p_XMin, int p_YMax, int p_YMin)
        {
            List<MyLine> p_List = new List<MyLine>();

            foreach(MyLine p_Line in this.m_PolygonLines)
            {
                MyLine v_NewLine = p_Line.CohenSutherland(p_XMax, p_XMin, p_YMax, p_YMin);

                if(v_NewLine != null)
                    p_List.Add(v_NewLine);
            }

            return p_List;
        }

        /* Método para calcular segmentos de retas de poligonos que serão plotados pela
         * área de recorte por Liang-Barsky
         * @param int p_XMax, int p_XMin, int p_YMax, int p_YMin -> coordenadas da área de recorte
         * @return List<MyLine> -> lista de segmentos de reta a serem plotados
         */
        public List<MyLine> LiangBarsky(int p_XMax, int p_XMin, int p_YMax, int p_YMin)
        {
            List<MyLine> p_List = new List<MyLine>();

            foreach (MyLine p_Line in this.m_PolygonLines)
            {
                MyLine v_NewLine = p_Line.LiangBarsky(p_XMax, p_XMin, p_YMax, p_YMin);

                if (v_NewLine != null)
                    p_List.Add(v_NewLine);
            }

            return p_List;
        }
    }
}
