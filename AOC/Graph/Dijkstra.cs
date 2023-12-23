using System.Collections;

namespace rsmith985.AOC.Y2023;


internal class Dijkstra<K>
{
    private Dictionary<K, double> _distTo;
    private Dictionary<K, DirectedEdge<K>> _edgeTo;

    private PriorityQueue<K, double> _queue;

    private DirectedGraph<K> _graph;
    private DirectedNode<K> _start;

    private bool _hopsNotWeight;

    public Dijkstra(DirectedGraph<K> graph, K start, bool hopsNotWeight = false)
    {
        _graph = graph;
        _start = graph.Nodes[start];
        _hopsNotWeight = hopsNotWeight;
        compute();
    }

    public double DistanceTo(K key) => _distTo[key];
    public bool PathExistsTo(K key) => _distTo[key] < double.PositiveInfinity;

    private void compute()
    {
        _distTo = new Dictionary<K, double>();
        _edgeTo = new Dictionary<K, DirectedEdge<K>>();
        _queue = new PriorityQueue<K, double>();

        foreach(var key in _graph.Nodes.Keys)
        {
            _distTo.Add(key, double.PositiveInfinity);
            _edgeTo.Add(key, null);
        }
        _distTo[_start.Data] = 0.0;

        _queue.Add(_start.Data, _distTo[_start.Data]);

        while(_queue.Count > 0)
        {
            var key = _queue.PopMin();
            foreach(var e in _graph.Nodes[key].EdgesTo)
                relax(e);
        }
    }

    private void relax(DirectedEdge<K> edge)
    {
        var weight = _hopsNotWeight ? 1 : edge.Weight;
        var v = edge.From.Data;
        var w = edge.To.Data;

        if(_distTo[v] + weight < _distTo[w])
        {
            _distTo[w] = _distTo[v] + weight;
            _edgeTo[w] = edge;

            if(_queue.Contains(w))
                _queue.ChangePriority(w, _distTo[w]);
            else
                _queue.Insert(w, _distTo[w]);
        }
    }
}