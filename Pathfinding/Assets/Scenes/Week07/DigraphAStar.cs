using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net.Mime;
using UnityEngine;


public enum AStarHeuristicStrategy
{
    StraightLineDistance,
    ManhattanDistance
}


public class DigraphAStar : MonoBehaviour
{
    //Nodes (or Vertices): Name [pos(x,y,z)]
    //Edges: Start Node, End Node [length]

    //Data structure to hold
    //Options:
    //Option1: Class for Node, class for Edge, List<Nodes>,List of Edges
    //Builder methods: add_ve   rtex, add_edge

    //Option2:
    //AdjancyMatrix[N][N] where N is number of nodes

    //Option3:dictionary
    //AdjancyList representation

    //Option3.1:dictionary
    Dictionary<char, Dictionary<char, int>> vertices = new Dictionary<char, Dictionary<char, int>>();
    Dictionary<char, int> distance = new Dictionary<char, int>();
    Dictionary<char, char> previous = new Dictionary<char, char>();
    
    Dictionary<char, Vector3> VertexPositions = new Dictionary<char, Vector3>();
    public AStarHeuristicStrategy aStarHeuristicStrategy = AStarHeuristicStrategy.StraightLineDistance;

    public float StarightLineDistance(BitVector32 p1, BitVector32 p2)
    {
        return BitVector32.Distance(p1, p2)
    }
    public float ManhattanDistance(BitVector32 p1, BitVector32 p2)
    {
        return Mathf.Abs(p1.x, p2.x) + Mathf.Abs(p1.y - p2.y) + Mathf.Abs(p1.z - p2.z)
    }


    public void add_vertex_Dijkstra(char vertex, Dictionary<char, int> neighbor_edges_dijkstra)
    {
        vertices[vertex] = neighbor_edges_dijkstra;
    }

    public void add_vertex_AStar(char vertex, Dictionary<char, int> neighbor_edges_dijkstra, Vector3)
    {
        vertices[vertex] = neighbor_edges_dijkstra;
        VertexPositions[vertex] = position;
    }


    /// <summary>
    /// Get all neighbours of a node
    /// </summary>
    /// <param name="node">node - the node to get neighbors from</param>
    /// <returns> a list of nodes that are adjacent the the given node above</returns>
    public char[] GetNeighbors(char node)
    {
        char[] result = null;
        if (vertices.ContainsKey(node))
        {
            var keys = vertices[node].Keys;
            result = new char[vertices[node].Count];
            int i = 0;
            foreach (var key in keys)
            {
                result[i] = key;
                i++;
            }
        }
        return result;
    }

    public void printArray<T>(T[] array)
    {
        foreach (T v in array)
        {
            UnityEngine.Debug.Log(v);
        }
    }

    public List<char> Find_Shortest_Path_via_Dijkstra_Algo(char start, char target)
    {
        List<char> path = new List<char>();
        initialize_single_source(start);
        List<char> Visited = new List<char>() { };
        List<char> Pending = new List<char>() { };
        foreach (char c in vertices.Keys)
        {
            Pending.Add(c);
        }

        //here starts the algorithm proper
        while (Pending.Count > 0)
        {
            char u = Extract_min(Pending);
            Visited.Add(u);
            //
            if (u == target)
            {
                //we have found the target,
                //construct the path and return it
                path.Add(u);
                while (previous[u] != '\0')
                {
                    u = previous[u];
                    path.Add(u);

                }
                path.Reverse();
                return path;
            }

            //check for no solution (no path)
            if (distance[u] == int.MaxValue)
            {
                return path; //Count 0  => indicates that no path exists between start and target
            }

            foreach (var v in vertices[u])
            {
                relax(u, v, v.Value);

            }
        }


        return path;
    }

    public List<char> Find_Shortest_Path_via_AStar_Algo(char start, char target)
    {
        
        
        List<char> path = new List<char>(); // return path


        //List<char> Visited = new List<char>() { }; // Closed
        //List<char> Pending = new List<char>() { }; // Open
        
        
        List<char> Closed = new List<char>() { };
        List<char> Open = new List<char>() { };

        //initialize_single_source(start);


        Dictionary<char, float> gScore = new Dictionary<char, float>();
        Dictionary<char, float> hScore = new Dictionary<char, float>();

        Dictionary<char, float> fScore = new Dictionary<char, float>();
        Dictionary<char, float> parentNode = new Dictionary<char, float>();


        gScore[start] = 0;
        hScore[start] = GoalDistanceEstimate(start, target);

        fScore[start] = gScore[start] + hScore[start];
        parentNode[start] = '-';

        Open.Add(start); 
      
        here starts the algorithm proper
        while (Pending.Count > 0)
        {
            char n = Extract_min_AStar(Open, fScore);

            if (n == target)
            {
                //we have found the target,
                //construct the path and return it
                path.Add(n);
                while (previous[n] != '-')
                {
                    n = previous[n];
                    path.Add(n);

                }
                path.Reverse();
                return path;
            }

            foreach (var nprime in vertices[n])
            {
                vertices[n][nprime]
                float newg = gScore[n] + vertices[n].Values[nprime.Key] ;

            }

        }


        //    Visited.Add(u);
        //    //


        //    //check for no solution (no path)
        //    if (distance[u] == int.MaxValue)
        //    {
        //        return path; //Count 0  => indicates that no path exists between start and target
        //    }

        //    foreach (var v in vertices[u])
        //    {
        //        relax(u, v, v.Value);

        //    }
        //}


        return path;
    }

   public float GoalDistanceEstimate (char start, target)
    {
        switch (aStarHeuristicStrategy)
        {
            case AStarHeuristicStrategy.StraightLineDistance:
                return StarightLineDistance(VertexPositions[start], VertexPositions[target]);
                
            case case AStarHeuristicStrategy.ManhattanDistance:
                return StarightLineDistance(VertexPositions[start], VertexPositions[target]);
                
            default:
                throw new NotImplementedException("Not good");
        }
    }



    private void relax(char u, KeyValuePair<char, int> v, int value)
    {
        if (distance[v.Key] > distance[u] + value)
        {
            distance[v.Key] = distance[u] + value;
            previous[v.Key] = u;
        }

    }

    private char Extract_min_AStar(List<char> open, Dictionary<char, float> fScore)
    {
        //we are simulating the behaviour of a Priority list with a regular list by sorting it first
        //Note: the O(...) is different

        open.Sort((x, y) => fScore[x].CompareTo(fScore[y]));
        char n = open[0];
        open.RemoveAt(0);
        return n;
    }

    private void initialize_single_source(char start)
    {
        foreach (char k in vertices.Keys)
        {
            distance[k] = int.MaxValue; //instead of infinity
            previous[k] = '\0';
        }
        distance[start] = 0;

    }
}
