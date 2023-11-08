using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;

public class TestDigraphAStar : MonoBehaviour
{
    DigraphAStar mygraph()
    {
        DigraphAStar mygraph = new DigraphAStar();

        mygraph.add_vertex_AStar('A', new Dictionary<char, int>() { { 'B', 4 }, { 'X', 20 }}, new Vector3(0,0,4));
        mygraph.add_vertex_AStar('B', new Dictionary<char, int>() { { 'A', 4 }, { 'C', 4 } }, new Vector3(4, 0, 4));
        mygraph.add_vertex_AStar('C', new Dictionary<char, int>() { { 'B', 6 }, { 'Z', 4 } }, new Vector3(8, 0, 4));
       
        mygraph.add_vertex_AStar('W', new Dictionary<char, int>() { { 'X', 4 } }, new Vector3(-4, 0, 0));

        mygraph.add_vertex_AStar('X', new Dictionary<char, int>() { { 'A', 4 }, { 'W', 4 }, { 'Y', 4 } }, new Vector3(0, 0, 0));
        mygraph.add_vertex_AStar('Y', new Dictionary<char, int>() { { 'X', 4 }, { 'Z', 4 } }, new Vector3(4, 0, 4));
        mygraph.add_vertex_AStar('Z', new Dictionary<char, int>() { { 'Y', 4 }, { 'C', 4 } }, new Vector3(8, 0, 0));

        return mygraph;

    }

    DigraphAStar CreateGraph2()
    {
        Digraph mygraph = new DigraphAStar();
        //TODO: Create the graph in https://en.wikipedia.org/wiki/Dijkstra%27s_algorithm

        mygraph.add_vertex_Dijkstra('A', new Dictionary<char, int>() { { 'B', 7 }, { 'C', 9 }, { 'F', 14 } });
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
        //Digraph mygraph1 = CreateGraph1();
        //Digraph mygraph2 = CreateGraph2();

        //char[] neighbors = mygraph1.GetNeighbors('A');
        //print("Neighbors of A:" + neighbors);
        //mygraph1.printArray<char>(neighbors);

        //List<char> path = mygraph1.Find_Shortest_Path_via_Dijkstra_Algo('A', 'F');
        //mygraph1.printArray(path.ToArray());

        //// graph2

        //char[] neighbors2 = mygraph2.GetNeighbors('A');
        //print("Neighbors of A:" + neighbors2);
        //mygraph2.printArray<char>(neighbors2);

        //List<char> path2 = mygraph2.Find_Shortest_Path_via_Dijkstra_Algo('A', 'E');
        //mygraph2.printArray(path2.ToArray());


        mygraph = CreateDigraphAStar();


    }
}

