namespace rsmith985.AOC.Y2023;

public class Day25 : Day
{
    private Graph<string> _graph;
    public override void Init()
    {
        _graph = new Graph<string>();

        foreach(var line in this.GetLines())
        {
            var key1 = line[..3];
            var items = line[4..].Trim().Split(' ');

            foreach(var item in items)
                _graph.Add(key1, item);
        }
    }

    public override object Part1()
    {
        var edgeCount = new Dictionary<string, int>();
        foreach(var e in _graph.Edges.Values)
            edgeCount.Add(e.Key, 0);   
            
        var rand = new Random();
        var allKeys = _graph.Nodes.Keys.ToList();

        var directed = _graph.ToDirectedGraph();
        for(int i = 0; i < 200; i++)
        {
            var start = allKeys[rand.Next(allKeys.Count)];
            var temp = new Dijkstra<string>(directed, start);
            var farthest = temp.GetFarthestNode();
            var path = temp.GetEdgePathTo(farthest);

            foreach(var e in path)
            {
                var key = _graph.GetEdgeKey((e.From.Data, e.To.Data));
                edgeCount[key]++;
            }
            /*
            if(i%50 == 0)
            {
                var max = edgeCount.OrderBy(i => i.Value).Reverse().Take(10).ToList();
                Console.WriteLine(max.Print());
            }
            */
        }
        foreach(var key in edgeCount.OrderBy(i => i.Value).Reverse().Take(3))
        {
            _graph.Remove(_graph.Edges[key.Key]);
        }
        
        var subGraphs = _graph.GetSubGraphs();

        if(subGraphs.Count != 2)
            return "Failed";

        return subGraphs[0].Nodes.Count*subGraphs[1].Nodes.Count;
    }

    public override object Part2()
    {
        return "Yay 2023";
    }
}
