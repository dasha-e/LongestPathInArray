using System;
using System.Collections.Generic;
using System.Linq;

namespace TheLongestPath
{
    class Program
    {
        struct Vertex // vertex of the graph
        {
            public int i;
            public int j;
        }

        static void IsItBE(double[,] arr, int i, int j, out bool begin, out bool end) // checking whether the vertex is it a sink or source vertex
        {
            byte b = 0, e = 0;
            begin = false; end = false;
            if (i > 0)
            {
                if (arr[i, j] < arr[i - 1, j]) b++;
                else if (arr[i, j] > arr[i - 1, j]) e++;
            }
            if (j > 0)
            {
                if (arr[i, j] < arr[i, j - 1]) b++;
                else if (arr[i, j] > arr[i, j - 1]) e++;
            }
            if (i < arr.GetLength(0) - 1)
            {
                if (arr[i, j] < arr[i + 1, j]) b++;
                else if (arr[i, j] > arr[i + 1, j]) e++;
            }
            if (j < arr.GetLength(1) - 1)
            {
                if (arr[i, j] < arr[i, j + 1]) b++;
                else if (arr[i, j] > arr[i, j + 1]) e++;
            }
            if (b == 0 && e != 0) end = true;
            if (e == 0 && b != 0) begin = true;
        }


        static bool BEs(double[,] arr, out List<Vertex> begins, out List<Vertex> ends) // making list of source and sink vertices
        {
            List<Vertex> b = new List<Vertex>(); //list of source vertices
            List<Vertex> e = new List<Vertex>(); //list of sink vertices
            Vertex v = new Vertex();
            for (int i = 0; i < arr.GetLength(0); i++)
                for (int j = 0; j < arr.GetLength(1); j++)
                {
                    IsItBE(arr, i, j, out bool begin, out bool end);
                    if (begin)
                    {
                        v.i = i; v.j = j;
                        b.Add(v);
                    }
                    else if (end)
                    {
                        v.i = i; v.j = j;
                        e.Add(v);
                    }
                }
            begins = b;
            ends = e;
            if (b.Any()) return true;
            return false;

        }

        static List<Vertex> LongestWay(double[,] arr)
        {
            List<Vertex> MaxWay = new List<Vertex>();
            if (!(BEs(arr, out List<Vertex> begin, out List<Vertex> end))) // if there are no sink or source vertices, all vertices of the graph are isolated, and any path in the graph is 0
            {
                Vertex v; v.i = 0; v.j = 0;
                MaxWay.Add(v);
                return MaxWay;
            }
            List<Vertex> Way = new List<Vertex>();
            // the longest path begins at one of the sources, and ends at one of the sinks 
            if (begin.Count() <= end.Count()) // therefore, the search will be conducted from sinks or sources (depending on what is less)
            {
                foreach (Vertex v in begin)
                {
                    Way = MaxWayFromThisVertex(arr, v.i, v.j, 1);
                    if (Way.Count() > MaxWay.Count())
                    {
                        MaxWay.Clear();
                        MaxWay.AddRange(Way);
                    }
                    Way.Clear();
                }
            }
            else
            {
                foreach (Vertex v in end)
                {
                    Way = MaxWayFromThisVertex(arr, v.i, v.j, -1);
                    if (Way.Count() > MaxWay.Count())
                    {
                        MaxWay.Clear();
                        MaxWay.AddRange(Way);
                    }
                    Way.Clear();
                }
                MaxWay.Reverse();
            }

            return MaxWay;

        }

        static List<Vertex> MaxWayFromThisVertex(double[,] arr, int i, int j, int sign) // finding the maximum path from the given vertex
        {

            List<Vertex> Way = new List<Vertex>();
            List<Vertex> LocalWay = new List<Vertex>();
            List<Vertex> MaxLocalWay = new List<Vertex>();
            Vertex v; v.i = i; v.j = j;
            Way.Add(v);
            if (i > 0 && (arr[i - 1, j] - arr[i, j]) * sign > 0)
            {
                MaxLocalWay = MaxWayFromThisVertex(arr, i - 1, j, sign);
            }
            if (j > 0 && (arr[i, j - 1] - arr[i, j]) * sign > 0)
            {
                LocalWay = MaxWayFromThisVertex(arr, i, j - 1, sign);
                if (MaxLocalWay.Count() < LocalWay.Count())
                    MaxLocalWay = LocalWay.GetRange(0, LocalWay.Count());
                LocalWay.Clear();
            }
            if (i < arr.GetLength(0) - 1 && (arr[i + 1, j] - arr[i, j]) * sign > 0)
            {
                LocalWay = MaxWayFromThisVertex(arr, i + 1, j, sign);
                if (MaxLocalWay.Count() < LocalWay.Count())
                    MaxLocalWay = LocalWay.GetRange(0, LocalWay.Count());
                LocalWay.Clear();
            }
            if (j < arr.GetLength(1) - 1 && (arr[i, j + 1] - arr[i, j]) * sign > 0)
            {
                LocalWay = MaxWayFromThisVertex(arr, i, j + 1, sign);
                if (MaxLocalWay.Count() < LocalWay.Count())
                    MaxLocalWay = LocalWay.GetRange(0, LocalWay.Count());
                LocalWay.Clear();
            }
            Way.AddRange(MaxLocalWay);
            return Way;
        }

        static void displayList(List<Vertex> List)
        {
            foreach (Vertex el in List)
                Console.Write("({0}, {1}) ", el.i, el.j);
            Console.WriteLine();
        }

        static void Main(string[] args)
        {
            // several examples are using as a test
            double[,] arr1 = { { 2, 5, 1, 0 },
                               { 3, 3, 1, 9 },
                               { 4, 4, 7, 8 } };

            double[,] arr2 = { { 1, 2, 3, 4 },
                               { 5, 6, 7, 8 },
                               { 9, 10, 11, 12 } };

            double[,] arr3 = { { 1, 1, 2, 3 },
                               { 1, 1, 2, 3 },
                               { 1, 1, 2, 3 } };

            double[,] arr4 = { { 2, 2, 2 },
                               { 2, 2, 2 },
                               { 2, 2, 2} };
            double[,] arr5 = { { 1, 1, 1, 1 },
                               { 2, 3, 4, 5 } };

            Console.Write("Example 1: "); displayList(LongestWay(arr1));
            Console.Write("Example 2: "); displayList(LongestWay(arr2));
            Console.Write("Example 3: "); displayList(LongestWay(arr3));
            Console.Write("Example 4: "); displayList(LongestWay(arr4));
            Console.Write("Example 5: "); displayList(LongestWay(arr5));

            Console.Write("Press any key to finish.");
            Console.ReadKey();
        }
    }
}
