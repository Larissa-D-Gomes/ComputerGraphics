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
         * @param int v_XVector, int v_YVector -> coordenadas do vetor de
         *                            translação
         */
        public void Translation(int p_XVector, int p_YVector)
        {
            foreach (MyLine v_Line in m_PolygonLines)
            {
                v_Line.Translation(p_XVector, p_YVector);
            }
        }
    }
}
