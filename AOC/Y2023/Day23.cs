using System.Drawing;

namespace rsmith985.AOC.Y2023;

public class Day23 : Day
{
    private char[,] _map;
    private Point _start;
    private Point _end;
    public override void Init()
    {
        _map = this.GetLines().To2DArray();
        
        var last = _map.GetLength(1) - 1;
        for(int x = 0; x < _map.GetLength(0); x++)
        {
            if(_map[x,0] == '.'){
                _start = new Point(x, 1);
                _map[x,0] = '#';
            }
            if(_map[x,last] == '.'){
                _end = new Point(x,last - 1);
                _map[x,last] = '#';
            }
        }
    }

    public override object Part1()
    {
        var queue = new Queue<Path>();
        queue.Enqueue(new Path(_start));

        var finishedPaths = new List<Path>();
        while(queue.Any())
        {
            var path = queue.Dequeue();
            if(path.CurrLoc == _end)
            {
                finishedPaths.Add(path);
                continue;
            }

            var next = path.NextLocations(_map);
            if(!next.Any()) 
                continue;

            if(next.Count == 1)
            {
                path.ContinuePath(next[0]);
                queue.Enqueue(path);
            }
            else
            {
                for(int i = 0; i < next.Count - 1; i++)
                {
                    var copy = path.Copy();
                    copy.ContinuePath(next[i]);
                    queue.Enqueue(copy);
                }
                path.ContinuePath(next[^1]);
                queue.Enqueue(path);
            }
        }

        return finishedPaths.Max(i => i.Visited.Count + 2);
    }

    public override object Part2()
    {
        var map = new char[_map.GetLength(0),_map.GetLength(1)];
        for(var x = 0; x < _map.GetLength(0); x++)
        {
            for(var y = 0; y < _map.GetLength(1); y++)
            {
                if(_map[x,y] == '.' || _map[x,y] == '#') 
                    map[x,y] = _map[x,y];
                else
                    map[x,y] = '.';
            }
        }

        var graph = new Graph<Point>();

        var existingStarts = new HashSet<(int, int)>();
        var queue = new Queue<Path>();
        queue.Enqueue(new Path(_start));

        while(queue.Any())
        {
            var path = queue.Dequeue();
            if(path.CurrLoc == _end)
            {
                graph.Add((path.Start, _end), path.Visited.Count);
                continue;
            }

            var next = path.NextLocations(map);
            if(!next.Any()) 
                continue;

            if(next.Count == 1)
            {
                path.ContinuePath(next[0]);
                queue.Enqueue(path);
            }
            else
            {
                graph.Add((path.Start, path.CurrLoc), path.Visited.Count);
                if(existingStarts.Contains(path.CurrLoc.ToTuple()))
                    continue;

                graph.Add((path.Start, path.CurrLoc), path.Visited.Count);
                
                existingStarts.Add(path.CurrLoc.ToTuple());
                for(int i = 0; i < next.Count; i++)
                {
                    var path2 = new Path(path.CurrLoc);
                    path2.ContinuePath(next[i]);
                    queue.Enqueue(path2);
                }
            }
        }

        return LongestPath.GetLongestPathWithCycles(graph, _start, _end) + 2;
    }
}

class Path
{
    public HashSet<(int, int)> Visited{get;set;}
    public Point CurrLoc{get;set;}

    public Point Start{get;set;}
    public Path(Point p)
    {
        this.Start = p;
        this.CurrLoc = p;
        this.Visited = new HashSet<(int, int)>();
    }
    public Path(Point loc, HashSet<(int, int)> visited)
    {
        this.CurrLoc = loc;
        this.Visited = new HashSet<(int, int)>();
        foreach(var item in  visited)
            this.Visited.Add(item);
    }

    public List<Point> NextLocations(char[,] map)
    {
        var curr = map.Get(this.CurrLoc);

        if(curr != '.' && curr != '#')
        {
            var nextP = this.CurrLoc;
            if(curr == '>')
                nextP = this.CurrLoc.GetNeighbor(Direction.E);
            else if(curr == '<')
                nextP = this.CurrLoc.GetNeighbor(Direction.W);
            else if(curr == '^')
                nextP = this.CurrLoc.GetNeighbor(Direction.N);
            else if(curr == 'v')
                nextP = this.CurrLoc.GetNeighbor(Direction.S);
            else
                throw new Exception();
            
            if(this.Visited.Contains(nextP.ToTuple()))
                return new List<Point>();
            return new List<Point>(){nextP};
        }
        
        var next = new List<Point>();
        foreach(var p in this.CurrLoc.GetNeighbors4())
        {
            var key = p.ToTuple();
            if(map.Get(p) != '#' && !this.Visited.Contains(key))
                next.Add(p);
        }
        return next;
    }

    public void ContinuePath(Point p)
    {
        this.Visited.Add(this.CurrLoc.ToTuple());
        this.CurrLoc = p;
    }

    public Path Copy()
    {
        return new Path(this.CurrLoc, this.Visited);
    }
}