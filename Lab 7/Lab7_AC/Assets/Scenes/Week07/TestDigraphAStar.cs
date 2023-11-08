using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TestDigraphAStar : MonoBehaviour
{
    DigraphAStar mygraph;
    public char start='C', target='X';
    public AStarHeuristicStrategy aStarHeuristicStrategy = AStarHeuristicStrategy.StraightLineDistance;
    DigraphAStar CreateDigraphAStar1()
    {
        mygraph = new DigraphAStar();

        mygraph.add_vertex_AStar('A', new Dictionary<char, int>() { { 'B', 4 }, { 'X', 20 } }, new Vector3(0, 0, 4));
        mygraph.add_vertex_AStar('B', new Dictionary<char, int>() { { 'A', 4 }, { 'C', 4 } }, new Vector3(4, 0, 4));
        mygraph.add_vertex_AStar('C', new Dictionary<char, int>() { { 'B', 4 }, { 'Z', 4 } }, new Vector3(8, 0, 4));
        
        mygraph.add_vertex_AStar('D', new Dictionary<char, int>() { { 'C', 4 }, { 'Z', 4 } }, new Vector3(12, 0, 4));// ADDED NEW NODE
       
        mygraph.add_vertex_AStar('W', new Dictionary<char, int>() { { 'X', 4 } }            , new Vector3(-4, 0, 0));        
        mygraph.add_vertex_AStar('X', new Dictionary<char, int>() { { 'W', 4 }, { 'Y', 4 }, { 'A', 20 } }, new Vector3(0, 0, 0));
        mygraph.add_vertex_AStar('Y', new Dictionary<char, int>() { { 'X', 4 }, { 'Z', 4 } }, new Vector3(4, 0, 0));
        mygraph.add_vertex_AStar('Z', new Dictionary<char, int>() { { 'Y', 4 }, { 'C', 4 } }, new Vector3(8, 0, 0));
        return mygraph;

    }
    DigraphAStar CreateDigraphAStar_NotSuitableForManhattan()
    {
        mygraph = new DigraphAStar();

        mygraph.add_vertex_AStar('A', new Dictionary<char, int>() { { 'B', 4 }, { 'X', 20 } }, new Vector3(0, 0, 4));
        mygraph.add_vertex_AStar('B', new Dictionary<char, int>() { { 'A', 4 }, { 'C', 4 } }, new Vector3(4, 0, 4));
        mygraph.add_vertex_AStar('C', new Dictionary<char, int>() { { 'B', 4 }, { 'Z', 4 }, { 'D', 4} }, new Vector3(8, 0, 4));
       
        mygraph.add_vertex_AStar('D', new Dictionary<char, int>() { { 'C', 4 }, { 'Z', 4 } }, new Vector3(12, 0, 4));// ADDED NEW NODE

        mygraph.add_vertex_AStar('W', new Dictionary<char, int>() { { 'X', 4 } }, new Vector3(-4, 0, 0));
        mygraph.add_vertex_AStar('X', new Dictionary<char, int>() { { 'W', 4 }, { 'Y', 4 }, { 'A', 20 } }, new Vector3(0, 0, 0));
        mygraph.add_vertex_AStar('Y', new Dictionary<char, int>() { { 'X', 4 }, { 'Z', 4 } }, new Vector3(4, 0, 0));
        mygraph.add_vertex_AStar('Z', new Dictionary<char, int>() { { 'Y', 4 }, { 'C', 4 } }, new Vector3(8, 0, 0));
        mygraph.add_vertex_AStar('D', new Dictionary<char, int>() { { 'A', 4 }, { 'C', 4 }, { 'X', 6 } }, new Vector3(12, 0, 4));
        return mygraph;

    }
    DigraphAStar CreateGraph1()
    {
        mygraph = new DigraphAStar();

        mygraph.add_vertex_Dijkstra('A', new Dictionary<char, int>() { { 'B', 10 }, { 'C', 12 }, { 'D', 6 }, { 'E', 2 } });
        mygraph.add_vertex_Dijkstra('B', new Dictionary<char, int>() { { 'C', 2 }, { 'D', 4 }, { 'F', 5 } });
        mygraph.add_vertex_Dijkstra('C', new Dictionary<char, int>() { { 'B', 6 }, { 'F', 2 } });
        mygraph.add_vertex_Dijkstra('D', new Dictionary<char, int>() { { 'B', 3 }, { 'E', 3 } });
        mygraph.add_vertex_Dijkstra('E', new Dictionary<char, int>() { { 'D', 3 }, { 'F', 11 } });
        mygraph.add_vertex_Dijkstra('F', new Dictionary<char, int>() { });

        return mygraph;

    }
    DigraphAStar CreateGraph2()
    {
        mygraph = new DigraphAStar();
        //TODO: Create the graph in https://en.wikipedia.org/wiki/Dijkstra%27s_algorithm
        //Note: This is not a digraph (directed graph).
        //To make it a digraph, you have to add both edges;
        //f.e. from a -> 6 with length 14 and from 6 to a with also length 14


        return mygraph;

    }
    // Start is called before the first frame update
    void Start()
    {
        //mygraph = CreateGraph1();

        //char[] neighbors = mygraph.GetNeighbors('A');
        //print("Neighbors of A:"+neighbors);
        //mygraph.printArray<char>(neighbors);

        mygraph = CreateDigraphAStar1();
        mygraph.aStarHeuristicStrategy = aStarHeuristicStrategy;//DO THIS TO MAKE USE OF ManhattanDistance

        List<char> path = mygraph.Find_Shortest_Path_via_AStar_Algo(start, target);
        mygraph.printArray(path.ToArray());

        //DigraphAStar mygraph2 = CreateDigraphAStar_NotSuitableForManhattan();
        //List<char> path2 = mygraph2.Find_Shortest_Path_via_AStar_Algo(start, target);
        //mygraph2.printArray(path2.ToArray());
    }





    // Update is called once per frame
    void Update()
    {
        
    }
}



// Did you get the same results with Manhattan Distance strategy as with the default Euclidean Distance strategy?
//  If yes, what you need to change in the second graph to make it so Manhattan Distance does not work any more in at least one case? (Hint: Recall the 'admissibility' requirement for Manhattan Distance to work).
//  If no, what do you think it was the reason?


// Answer:::::::::::::::::::::::::::::::::::

// No they are different results because of the admissibility requirement of the Manhattan Distance heuristic. 
// Manhattan Distance doesn't find the shortest path and over estimates the cost. 