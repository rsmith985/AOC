﻿using System.Collections;

namespace rsmith985.AOC;

public class Graph<T>
{
    public Dictionary<T, Node<T>> Nodes{get;}
    public Dictionary<string, Edge<T>> Edges{get;}

    public Graph()
    {
        this.Nodes = new();
        this.Edges = new();
    }

    public Node<T> GetOrAddNode(T data)
    {
        if(this.Nodes.ContainsKey(data))
            return this.Nodes[data];
        var node = new Node<T>(data);
        this.Nodes.Add(data, node);
        return node;
    }

    public void Add(T n1, T n2, double weight = 1.0)
    {
        this.Add((n1, n2), weight);
    }
    public void Add((T, T) edge, double weight = 1.0)
    {
        var key = GetEdgeKey(edge);
        if(this.Edges.ContainsKey(key)) return;

        var n1 = this.GetOrAddNode(edge.Item1);
        var n2 = this.GetOrAddNode(edge.Item2);
        var e = new Edge<T>(key, n1, n2, weight);
        this.Edges.Add(key, e);
    }

    public void Add(Edge<T> edge)
    {
        this.Edges.Add(edge.Key, edge);
    }

    public void Remove(Edge<T> edge)
    {
        if(!this.Edges.ContainsKey(edge.Key))
            throw new Exception();
        
        edge.Node1.Remove(edge);
        edge.Node2.Remove(edge);
        this.Edges.Remove(edge.Key);
    }

    public static string GetEdgeKey((T, T) items)
    {
        var str1 = items.Item1.ToString();
        var str2 = items.Item2.ToString();
        return str1.CompareTo(str2) < 0 ? str1 + "_" + str2 : str2 + "_" + str1;
    }

    public DirectedGraph<T> ToDirectedGraph()
    {
        var graph = new DirectedGraph<T>();
        foreach(var e in this.Edges.Values)
        {
            graph.Add(e.Node1.Data, e.Node2.Data, e.Weight);
            graph.Add(e.Node2.Data, e.Node1.Data, e.Weight);
        }
        return graph;
    }

    public bool IsFullyConnected()
    {
        var queue = new Queue<Node<T>>();
        var visited = new HashSet<Node<T>>();

        var first = this.Nodes.Values.First();
        queue.Enqueue(first); // Start from any vertex
        visited.Add(first);

        while (queue.Any())
        {
            var curr = queue.Dequeue();
            foreach (var node in curr.GetConnected())
            {
                if (!visited.Contains(node))
                {
                    queue.Enqueue(node);
                    visited.Add(node);
                }
            }
        }

        return visited.Count == this.Nodes.Count;
    }
}

public class Node<T>
{
    public T Data{ get; }

    public List<Edge<T>> Edges{get;}

    public int Degree => this.Edges.Count;

    public Node(T data)
    {
        this.Data = data;
        this.Edges = new List<Edge<T>>();
    }

    public Edge<T> GetNext(Edge<T> edge)
    {
        if(this.Edges.Count != 2) throw new Exception();

        return  this.Edges[0] == edge ? this.Edges[1] : 
                this.Edges[1] == edge ? this.Edges[0] :
                throw new Exception();
    }

    public IEnumerable<Node<T>> GetConnected()
    {
        foreach(var edge in this.Edges)
            yield return edge.GetOpposite(this);
    }

    public void Remove(Edge<T> edge)
    {
        this.Edges.Remove(edge);
    }
}

public class Edge<T>
{
    public string Key{get;}
    public Node<T> Node1{get;}
    public Node<T> Node2{get;}
    public double Weight {get; set;}

    public Edge(string key, Node<T> node1, Node<T> node2, double weight = 1.0)
    {
        this.Key = key;
        this.Node1 = node1;
        this.Node2 = node2;
        this.Node1.Edges.Add(this);
        this.Node2.Edges.Add(this);
        this.Weight = weight;
    }

    public Node<T> GetOpposite(Node<T> node)
    {
        if(node == this.Node1) return this.Node2;
        if(node == this.Node2) return this.Node1;
        throw new Exception();
    }
}