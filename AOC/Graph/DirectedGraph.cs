using System.Collections;

namespace rsmith985.AOC.Y2023;

class DirectedGraph<T>
{
    public Dictionary<T, DGraphNode<T>> Nodes{get;}
    public Dictionary<string, DirectedEdge<T>> Edges{get;}

    public DirectedGraph()
    {
        this.Nodes = new();
        this.Edges = new();

    }

    public DGraphNode<T> GetOrAddNode(T data)
    {
        if(this.Nodes.ContainsKey(data))
            return this.Nodes[data];
        var node = new DGraphNode<T>(data);
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
        var e = new DirectedEdge<T>(key, n1, n2, weight);
        this.Edges.Add(key, e);
    }

    public static string GetEdgeKey((T, T) items)
    {
        var str1 = items.Item1.ToString();
        var str2 = items.Item2.ToString();
        return str1 + "_" + str2;
    }
}

class DGraphNode<T>
{
    public T Data{ get; }

    public List<DirectedEdge<T>> EdgesTo{get;}
    public List<DirectedEdge<T>> EdgesFrom{get;}

    public DGraphNode(T data)
    {
        this.Data = data;
        this.EdgesTo = new List<DirectedEdge<T>>();
        this.EdgesFrom = new List<DirectedEdge<T>>();
    }
}


internal class DirectedEdge<T>
{
    public string Key{get;}
    public DGraphNode<T> From{get;}
    public DGraphNode<T> To{get;}
    public double Weight {get;}

    public DirectedEdge(string key, DGraphNode<T> from, DGraphNode<T> to, double weight = 1.0)
    {
        this.Key = key;
        this.From = from;
        this.To = to;
        this.From.EdgesTo.Add(this);
        this.To.EdgesFrom.Add(this);
        this.Weight = weight;
    }

    public DGraphNode<T> GetOpposite(DGraphNode<T> node)
    {
        if(node == this.From) return this.To;
        if(node == this.To) return this.From;
        throw new Exception();
    }
}
