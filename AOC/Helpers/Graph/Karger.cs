namespace rsmith985.AOC.Y2023;

public class Karger<T>
{
    private Graph<T> _graph;
    public Karger(Graph<T> graph)
    {
        _graph = graph;
    }

    public List<Edge<T>> Run()
    {
        var rand = new Random();

        var graph = _graph.Copy();
        while(graph.Nodes.Count > 2)
        {
            var edges = graph.Edges.Values.ToList();
            var edge = edges[rand.Next(edges.Count)];

            var n1 = edge.Node1;
            var n2 = edge.Node2;

            graph.Remove(edge);
            
            if(n1 == n2) continue;

            foreach(var conn in n2.Edges.ToList())
                conn._SwapNode(n2, n1);

            graph.Remove(n2);
        }

        var rv = new List<Edge<T>>();
        foreach(var e in graph.Edges)
        {
            rv.Add(_graph.Edges[e.Key]);
        }
        return rv;
    }

}
