using System.Drawing;
using Emgu.CV;
using Emgu.CV.Structure;

namespace rsmith985.AOC.Y2023;

public class Day10 : Day
{
    //protected override string _testFile => @"D:\Google Drive\_Ryan\Code\C#\AOC\test1.txt";

    public override object Part1()
    {
        var points = getLoop(this.GetLines());
        return Math.Ceiling(points.Count/2.0);
    }

    public override object Part2()
    {
        var input = this.GetLines();
        (var start, var connCount) = getConnCount(input);

        var validConn = connCount.Where(kv => kv.Value == 2 || kv.Key.Item1 == start || kv.Key.Item2 == start).Select(kv => kv.Key);

        var graph = new Graph<Point>();
        foreach(var conn in validConn)
            graph.Add(conn);

        var points = getLoop(graph, start);
        
        var set = new HashSet<Point>(points);
        //var pointsF = points.Select(i => i.ToFloat()).ToList();
        var bounds = points.GetBoundingRect();

        var count = 0;
        for(int y = 0; y < input.Length; y++)
        {
            for(int x = 0; x < input[y].Length; x++)
            {
                var p = new Point(x,y);
                if(!set.Contains(p))
                {
                    if(bounds.Contains(p) && points.PolygonContains(p))
                    {
                        count++;
                    }
                }
            }
        }
        return count;
    }

    private List<Point> getLoop(string[] input)
    {
        (var start, var connCount) = getConnCount(input);

        var validConn = connCount.Where(kv => kv.Value == 2 || kv.Key.Item1 == start || kv.Key.Item2 == start).Select(kv => kv.Key);

        var graph = new Graph<Point>();
        foreach(var conn in validConn)
            graph.Add(conn);
            
        return getLoop(graph, start);
    }

    private List<Point> getLoop(Graph<Point> graph, Point start)
    {
        var startNode = graph.Nodes[start];
        var curr = startNode.Edges.First();
        var currNode = curr.GetOpposite(startNode);

        var points = new List<Point>();
        points.Add(start);
        while(true)
        {
            points.Add(currNode.Data);
            
            curr = currNode.GetNext(curr);
            currNode = curr.GetOpposite(currNode);

            if(currNode == startNode)
                break;
        }
        return points;
    }

    private (Point, Dictionary<(Point, Point), int>) getConnCount(string[] input)
    {
        var connCount = new Dictionary<(Point, Point), int>();

        Point start = Point.Empty;
        for(int y = 0; y < input.Length; y++)
        {
            for(int x = 0; x < input[y].Length; x++)
            {
                var c = input[y][x];
                if(c == '.') continue;

                if(c == 'S')
                {
                    start = new Point(x,y);
                }
                else if(c == '|')
                {
                    addConn((new Point(x, y), new Point(x, y+1)), connCount);
                    addConn((new Point(x, y), new Point(x, y-1)), connCount);
                }
                else if(c == '-')
                {
                    addConn((new Point(x, y), new Point(x+1, y)), connCount);
                    addConn((new Point(x, y), new Point(x-1, y)), connCount);
                }
                else if(c == 'L')
                {
                    addConn((new Point(x, y), new Point(x, y-1)), connCount);
                    addConn((new Point(x, y), new Point(x+1, y)), connCount);
                }
                else if(c == 'J')
                {
                    addConn((new Point(x, y), new Point(x, y-1)), connCount);
                    addConn((new Point(x, y), new Point(x-1, y)), connCount);
                }
                else if(c == '7')
                {
                    addConn((new Point(x, y), new Point(x, y+1)), connCount);
                    addConn((new Point(x, y), new Point(x-1, y)), connCount);
                }
                else if(c == 'F')
                {
                    addConn((new Point(x, y), new Point(x, y+1)), connCount);
                    addConn((new Point(x, y), new Point(x+1, y)), connCount);
                }
                else
                {
                    throw new Exception();
                }
            }
        }
        return (start, connCount);
    }

    private void addConn((Point, Point) conn, Dictionary<(Point, Point), int> dict)
    {
        var order = conn.Item1.ToString().CompareTo(conn.Item2.ToString());

        var n1 = order < 0 ? conn.Item1 : conn.Item2;
        var n2 = order < 0  ? conn.Item2 : conn.Item1;

        var c = (n1, n2);

        if(!dict.ContainsKey(c))
            dict.Add(c, 0);
        dict[c]++;
    }

}


