using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graph_Generator
{
    class Vertices : IEnumerable<Vertex>
    {
        public List<Vertex> VertexList { get; set; }

        public IEnumerator<Vertex> GetEnumerator()
        {
            return VertexList.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return VertexList.GetEnumerator();
        }
    }
}
