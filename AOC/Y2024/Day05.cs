using System;

namespace rsmith985.AOC.Y2024;

public class Day05 : Day
{
    //protected override bool _useDefaultTestFile => true;

    public override object Part1()
    {
        (var graph, var lists) = parseInput();

        var valid = new List<List<int>>();
        foreach(var list in lists)
        {
            var subgraph = graph.GetSubGraph(new HashSet<int>(list));

            if(isValid(subgraph, list))
                valid.Add(list);
        }

        var sum = 0;
        foreach(var list in valid)
        {
            //Console.WriteLine(list[(list.Count-1)/2] + " " + "Valid: " + list.Print());
            sum += list[(list.Count-1)/2];
        }
        return sum;
    }

    public override object Part2()
    {
        (var graph, var lists) = parseInput();

        var invalid = new List<List<int>>();
        foreach(var list in lists)
        {
            var subgraph = graph.GetSubGraph(new HashSet<int>(list));

            if(!isValid(subgraph, list))
                invalid.Add(list);
        }

        var valid = new List<List<int>>();
        foreach(var list in invalid)
        {
            var subgraph = graph.GetSubGraph(new HashSet<int>(list));
            var order = getOrdering(subgraph);
            valid.Add(order.OrderBy(kv => kv.Value).Select(kv => kv.Key).ToList());
        }

        var sum = 0;
        foreach(var list in valid)
        {
            //Console.WriteLine(list[(list.Count-1)/2] + " " + "Valid: " + list.Print());
            sum += list[(list.Count-1)/2];
        }
        return sum;
    }


    private (DirectedGraph<int> graph, List<List<int>> lists) parseInput()
    {
        var graph = new DirectedGraph<int>();
        var lists = new List<List<int>>();
        foreach(var line in this.GetLines())
        {
            if(line.Contains("|"))
            {
                (var i1, var i2)= line.Split2(int.Parse, "|");
                graph.Add((i1, i2));
            }
            else if(line.Contains(","))
            {
                lists.Add(line.Split(int.Parse, ",").ToList());
            }
        }
        return (graph, lists);
    }

    private Dictionary<int, int> getOrdering(DirectedGraph<int> graph)
    {
        var copy = graph.Copy();

        var rv = new Dictionary<int, int>();
        var idx = 0;
        while(copy.Nodes.Any())
        {
            var nodes = copy.Nodes.Values.Where(i => i.EdgesFrom.Count == 0).ToList();

            foreach(var node in nodes)
            {
                rv.Add(node.Data, idx);
                copy.Remove(node);
            }
            idx++;
        }
        //Console.WriteLine("Ordering: " + rv.OrderBy(kv => kv.Value).Select(kv => $"{kv.Value}.{kv.Key}").Print());
        return rv;
    }

    private bool isValid(DirectedGraph<int> graph, List<int> list)
    {
        var order = getOrdering(graph);
        for(int i = 1; i < order.Count; i++)
        {
            if(order[list[i-1]] > order[list[i]])
                return false;
        }
        return true;
    }
}
