using System.Drawing;
using rsmith985.AOC.Y2023;

namespace rsmith985.AOC;

public static class LongestPath
{
    public static double GetLongestPathWithCycles<T>(Graph<T> graph, T start, T end)
    {
        var path = new LongestPathCycles<T>(graph, start);
        return path.GetLongestTo(end);
    }

    public static double GetLongestPathNoCycles<T>(Graph<T> graph, T start, T end)
    {
        foreach(var e in graph.Edges.Values)
            e.Weight = -e.Weight;
        
        var shortest = new Dijkstra<T>(graph.ToDirectedGraph(), start);
        var dist = -shortest.DistanceTo(end);

        foreach(var e in graph.Edges.Values)
            e.Weight = -e.Weight;

        return dist;
    }
}
class LongestPathCycles<T>
{
    private Graph<T> _graph;
    private HashSet<T> _visited;

    private Dictionary<T, double> _dist;

    public LongestPathCycles(Graph<T> graph, T start)
    {  
        _graph = graph;
        
        _dist = new Dictionary<T, double>();
        _visited = new HashSet<T>();
        foreach(var node in graph.Nodes.Values)
            _dist.Add(node.Data, 0);
        
        getLongestPath(graph.Nodes[start], 0);
    }

    public double GetLongestTo(T end)
    {
        return _dist[end];
    }

    private void getLongestPath(Node<T> node, double currSum)
    {
        if(_visited.Contains(node.Data))
            return;
        
        _visited.Add(node.Data);

        if(_dist[node.Data] < currSum)
            _dist[node.Data] = currSum;
        
        foreach(var conn in node.Edges)
            getLongestPath(conn.GetOpposite(node), currSum + conn.Weight);
        
        _visited.Remove(node.Data);
    }
}
