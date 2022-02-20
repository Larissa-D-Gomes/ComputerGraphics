using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgorithmsImplementation1.Model
{
    public class MyPolygon
    {
        public List<MyLine> PolygonLines { get; set; }

        // Construtor Vazio
        public MyPolygon()
        {
            PolygonLines = new List<MyLine>();
        }
    }
}
