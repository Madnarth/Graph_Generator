using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graph_Generator
{
    public class Adjacency_Matrix
    {
        private static int n = 0;
        public static int N
        {
            get { return n; }
            set { n = value; }

        }

        private static double p = 0;
        public static double P
        {
            get { return p; }
            set { p = value; }

        }

        private static int[,] adjMatrix;
        public static int [,] AdjMatrix
        {
            get { return adjMatrix; }
            set { adjMatrix = value; }
        }

        //Zerowanie macierzy sąsiedztwa
        public void Zero_Adjacency_Matrix()
        {
            for (int i = 0; i < adjMatrix.GetLength(0); i++)
            {
                for (int j = 0; j < adjMatrix.GetLength(1); j++)
                {
                    adjMatrix[i, j] = 0;
                }
            }
        }
        //Losowanie macierzy sąsiedztwa
        public void Fill_Half_Adjacency_Matrix()
        {
            Random rand = new Random();
            adjMatrix = new int[N, N];
            for (int i = 0; i < adjMatrix.GetLength(0); i++)
            {
                for (int j = 0; j < adjMatrix.GetLength(1); j++)
                {
                    if (i < j)
                    {
                        if (rand.NextDouble() < P)
                        {
                            adjMatrix[i, j] = 1;
                        }
                    }
                    else
                        adjMatrix[i, j] = 0;
                }
            }
        }
        //Odbicie macierzy sąsiedztwa
        public void Reflect_Adjacency_Matrix()
        {
            for (int i = 0; i < adjMatrix.GetLength(0); i++)
            {
                for (int j = 0; j < adjMatrix.GetLength(1); j++)
                {
                        adjMatrix[j, i] = adjMatrix[i, j];                        
                }
            }
        }
    }
}
