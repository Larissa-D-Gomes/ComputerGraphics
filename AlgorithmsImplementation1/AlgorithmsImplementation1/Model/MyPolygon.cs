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

        public void createPolygonByListOfEdges(List<MyPoint> p_Edges)
        {
            this.m_NumOfEdges = p_Edges.Count;

            
        }
    }
}
