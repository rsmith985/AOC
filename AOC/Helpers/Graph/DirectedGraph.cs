using System.Collections;

namespace rsmith985.AOC;

public class DirectedGraph<T>
{
    public Dictionary<T, DirectedNode<T>> Nodes{get;}
    public Dictionary<string, DirectedEdge<T>> Edges{get;}

    public DirectedGraph()
    {
        this.Nodes = new();
        this.Edges = new();

    }

    public DirectedNode<T> GetOrAddNode(T data)
    {
        if(this.Nodes.ContainsKey(data))
            return this.Nodes[data];
        var node = new DirectedNode<T>(data);
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

    public bool PathExists(T start, T end)
    {
        var sNode = this.Nodes[start];
        var eNode = this.Nodes[end];
        return PathExists(sNode, eNode);
    }
    public bool PathExists(DirectedNode<T> start, DirectedNode<T> end)
    {
        var queue = new Queue<DirectedNode<T>>();
        var visited = new HashSet<T>();

        queue.Enqueue(start);
        visited.Add(start.Data);

        while(queue.Any())
        {
            var curr = queue.Dequeue();
            foreach(var conn in curr.ConnectedTo())
            {
                if(conn == end) 
                    return true;

                if(visited.Contains(conn.Data)) continue;

                queue.Enqueue(conn);
                visited.Add(conn.Data);
            }
        }
        return false;
    }
}

public class DirectedNode<T>
{
    public T Data{ get; }

    public List<DirectedEdge<T>> EdgesTo{get;}
    public List<DirectedEdge<T>> EdgesFrom{get;}

    public DirectedNode(T data)
    {
        this.Data = data;
        this.EdgesTo = new List<DirectedEdge<T>>();
        this.EdgesFrom = new List<DirectedEdge<T>>();
    }

    public IEnumerable<DirectedNode<T>> ConnectedTo()
    {
        foreach(var edge in this.EdgesTo)
            yield return edge.GetOpposite(this);
    }
}


public class DirectedEdge<T>
{
    public string Key{get;}
    public DirectedNode<T> From{get;}
    public DirectedNode<T> To{get;}
    public double Weight {get;}

    public DirectedEdge(string key, DirectedNode<T> from, DirectedNode<T> to, double weight = 1.0)
    {
        this.Key = key;
        this.From = from;
        this.To = to;
        this.From.EdgesTo.Add(this);
        this.To.EdgesFrom.Add(this);
        this.Weight = weight;
    }

    public DirectedNode<T> GetOpposite(DirectedNode<T> node)
    {
        if(node == this.From) return this.To;
        if(node == this.To) return this.From;
        throw new Exception();
    }
}
