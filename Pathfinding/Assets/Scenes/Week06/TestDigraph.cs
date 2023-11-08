using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestDigraph : MonoBehaviour
{  
    Digraph CreateGraph1()
    {
        Digraph mygraph = new Digraph();

        mygraph.add_vertex_Dijkstra('A', new Dictionary<char, int>() { { 'B', 10 }, { 'C', 12 }, { 'D', 6 }, { 'E', 2 } });
        mygraph.add_vertex_Dijkstra('B', new Dictionary<char, int>() { { 'C', 2 }, { 'D', 4 }, { 'F', 5 } });
        mygraph.add_vertex_Dijkstra('C', new Dictionary<char, int>() { { 'B', 6 }, { 'F', 2 } });
        mygraph.add_vertex_Dijkstra('D', new Dictionary<char, int>() { { 'B', 3 }, { 'E', 3 } });
        mygraph.add_vertex_Dijkstra('E', new Dictionary<char, int>() { { 'D', 3 }, { 'F', 11 } });
        mygraph.add_vertex_Dijkstra('F', new Dictionary<char, int>() { });
        return mygraph;

    }

    Digraph CreateGraph2()
    {
        Digraph mygraph = new Digraph();
        //TODO: Create the graph in https://en.wikipedia.org/wiki/Dijkstra%27s_algorithm

        mygraph.add_vertex_Dijkstra('A', new Dictionary<char, int>() { { 'B', 7 }, { 'C', 9}, { 'F', 14} });
        mygraph.add_vertex_Dijkstra('B', new Dictionary<char, int>() { { 'C', 10 }, { 'D', 15 } });
        mygraph.add_vertex_Dijkstra('C', new Dictionary<char, int>() { { 'D', 11 }, { 'F', 2 } });
        mygraph.add_vertex_Dijkstra('D', new Dictionary<char, int>() { { 'E', 6 } });
        mygraph.add_vertex_Dijkstra('E', new Dictionary<char, int>() { });
        mygraph.add_vertex_Dijkstra('F', new Dictionary<char, int>() { { 'E', 9 } });
        return mygraph;

        //Note: This is not a digraph (directed graph).
        //To make it a digraph, you have to add both edges;
        //f.e. from a -> 6 with length 14 and from 6 to a with also length 14
    }
    // Start is called before the first frame update
    void Start()
    {
        Digraph mygraph1 = CreateGraph1();
        Digraph mygraph2 = CreateGraph2();

        char[] neighbors = mygraph1.GetNeighbors('A');
        print("Neighbors of A:" + neighbors);
        mygraph1.printArray<char>(neighbors);

        List<char> path = mygraph1.Find_Shortest_Path_via_Dijkstra_Algo('A', 'F');
        mygraph1.printArray(path.ToArray());

// graph2

        char[] neighbors2 = mygraph2.GetNeighbors('A');
        print("Neighbors of A:" + neighbors2);
        mygraph2.printArray<char>(neighbors2);

        List<char> path2 = mygraph2.Find_Shortest_Path_via_Dijkstra_Algo('A', 'E');
        mygraph2.printArray(path2.ToArray());

    }
}
