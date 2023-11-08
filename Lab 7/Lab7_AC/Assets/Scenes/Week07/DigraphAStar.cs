using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
/// <summary>
/// Directed Graph Data Structure
/// </summary>
public enum AStarHeuristicStrategy
{
    StraightLineDistance, //or EuclideanDistance
    ManhattanDistance
}
public class DigraphAStar
{
    //Nodes (or Vertices): Name [pos(x,y,z)]
    //Edges: Start Node, End Node [length]

    //Data structure to hold
    //Options:
    //Option1: Class for Node, class for Edge, List<Nodes>,List of Edges
    //Builder methods: add_vertex, add_edge

    //Option2:
    //AdjancyMatrix[N][N] where N is number of nodes

    //Option3:dictionary
    //AdjancyList representation

    //Option3.1:dictionary
    Dictionary<char, Dictionary<char, int>> vertices = new Dictionary<char, Dictionary<char, int>>();
    Dictionary<char, int> distance=new Dictionary<char, int>(); //fScore
    Dictionary<char, char> previous=new Dictionary<char, char>();//nodeParent

    //A* related members
    Dictionary<char, Vector3> VertexPositions = new Dictionary<char, Vector3>();
    public AStarHeuristicStrategy aStarHeuristicStrategy=AStarHeuristicStrategy.StraightLineDistance;

    public float StraightLineDistance(Vector3 p1, Vector3 p2)
    {
        return Vector3.Distance(p1, p2);
    }

    public float ManhattanDistance(Vector3 p1, Vector3 p2)
    {
        return Mathf.Abs(p1.x - p2.x) + Mathf.Abs(p1.y - p2.y) + Mathf.Abs(p1.z - p2.z);
    }

    public void add_vertex_Dijkstra(char vertex, Dictionary<char, int> neighbor_edges_dijkstra)
    {
        vertices[vertex]= neighbor_edges_dijkstra;
    }
    public void add_vertex_AStar(char vertex, Dictionary<char, int> neighbor_edges_dijkstra, Vector3 position)
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
        foreach(T v in array)
        {
            Debug.Log(v);
        }

    }

    public List<char> Find_Shortest_Path_via_Dijkstra_Algo(char start, char target)
    {
        List<char> path = new List<char>();
        initialize_single_source(start);
        List<char> Visited=new List<char>() { };
        List<char> Pending=new List<char>() { };
        foreach(char c in vertices.Keys)
        {
            Pending.Add(c);
        }

        //here starts the algorithm proper
        while(Pending.Count > 0)
        {
            char u = Extract_min(Pending);
            Visited.Add(u);
            //
            if(u == target)
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

            foreach(var v in vertices[u])
            {
                relax(u, v, v.Value);

            }
        }


        return path;
    }
    public List<char> Find_Shortest_Path_via_AStar_Algo(char start, char target)
    {
        //NodeMark | g | h | 
        //         |-------|
        //         | f | p |
        
        List<char> path = new List<char>(); //return path

        //List<char> Visited = new List<char>() { }; //Closed
        //List<char> Pending = new List<char>() { }; //Open => should be Priority Queue


        List<char> Closed = new List<char>() { };  // 
        List<char> Open = new List<char>() { };

        //initialize_single_source(start);
        Dictionary<char, float> gScore = new Dictionary<char, float>();
        Dictionary<char, float> hScore = new Dictionary<char, float>();
        Dictionary<char, float> fScore = new Dictionary<char, float>();
        Dictionary<char, char> parentNode = new Dictionary<char, char>();

        gScore[start] = 0;
        hScore[start] = GoalDistEstimate(start,target); //need target as well
        fScore[start] = gScore[start]+ hScore[start];
        parentNode[start] = '-'; //'-' denotes the fact that start node has no parent

        Open.Add(start); //push s on Open

        //here starts the algorithm proper
        while (Open.Count > 0)
        {
            char n = Extract_min_AStar(Open,fScore); //pops the min node from open
            if (n == target)
            {
                //we have found the target,
                //construct the path backwards from target to start... 
                path.Add(n);
                while (parentNode[n] != '-')
                {
                    n = parentNode[n];
                    path.Add(n);

                }
                //...reverse it
                path.Reverse();
                //...and return it
                return path;  //return success
            }

            foreach (var nprimePair in vertices[n])
            {
                char nprime = nprimePair.Key;
                //vertices[n]
                float newg = gScore[n] + nprimePair.Value;
                if(Open.Contains(nprime) || Closed.Contains(nprime)){
                    if (gScore[nprime] <= newg)
                    {
                        continue; //skip
                    }
                }
                
                parentNode[nprime] = n; //n'.parent=n
                gScore[nprime] = newg;  //n'.g=newg
                hScore[nprime] = GoalDistEstimate(nprime, target); //n'.h = GoalDistEstimate(nprime)
                fScore[nprime] = gScore[nprime] + hScore[nprime];  //n'.f=n'.g+n'.h

                //Update the Open|Closed lists
                if (Closed.Contains(nprime))
                {
                    Closed.Remove(nprime); 
                }

                if (!Open.Contains(nprime))
                {
                    Open.Add(nprime);

                }
            }
            Closed.Add(n);


        } //While opem not empty

        //return failure is <=> return path and path.Count==0

        return path;
    }

    private float GoalDistEstimate(char start, char target)
    {
        //throw new NotImplementedException();
        switch (aStarHeuristicStrategy)
        {
            case AStarHeuristicStrategy.StraightLineDistance:
                return StraightLineDistance(VertexPositions[start], VertexPositions[target]);
            case AStarHeuristicStrategy.ManhattanDistance:
                return ManhattanDistance(VertexPositions[start], VertexPositions[target]);
            default:
                throw new NotImplementedException("The strategy {aStarHeuristicStrategy} is not implemented!");
        }
    }


    private void relax(char u, KeyValuePair<char, int> v, int value)
    {
        if (distance[v.Key]> distance[u] + value)
        {
            distance[v.Key] = distance[u] + value;
            previous[v.Key] = u;
        }
        
    }

    private char Extract_min(List<char> pending)
    {
        //we are simulating the behaviour of a Priority list with a regular list by sorting it first
        //Note: the O(...) is different

        pending.Sort((x, y) => distance[x].CompareTo(distance[y]));
        char u = pending[0];
        pending.RemoveAt(0);
        return u;
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
        foreach(char k in vertices.Keys)
        {
            distance[k] = int.MaxValue; //instead of infinity
            previous[k] = '\0';
        }
        distance[start] = 0;

    }
}
