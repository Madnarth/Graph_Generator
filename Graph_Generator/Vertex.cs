using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graph_Generator
{
    class Vertex
    {
        private int id;
        public int ID
        {
             get { return id; }
             set { id = value; }

        }
        private double posX;
        public double POSX
        {
            get { return posX; }
            set { posX = value; }

        }
        private double posY;
        public double POSY
        {
            get { return posY; }
            set { posY = value; }

        }
        private string color = "#FFFFFF";
        public string COLOR
        {
             get { return color; }
             set { color = value; }

        }
        private int degree = 0;
        public int DEGREE
        {
            get { return degree; }
            set { degree = value; }
        }
        private List<int> neighbors = new List<int>();
        public List<int> NEIGHBORS
        {
            get { return neighbors; }
            set { neighbors = value; }
        }
        public Vertex(int _id)
        {
            id = _id;
        }
    }
}
