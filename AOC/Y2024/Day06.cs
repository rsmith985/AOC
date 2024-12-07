
namespace rsmith985.AOC.Y2024;

public class Day06 : Day
{
    //protected override bool _useDefaultTestFile => true;

    public override object Part1()
    {
        var map = this.GetLines().PadBorder('O');
        var visited = getVisited(map);
        return visited.LoopAll().Count(i => i);
    }

    public override object Part2()
    {
        var map = this.GetLines().PadBorder('O');
        var start = getStart(map);
        var vistited = getVisited(map);
        var size = map.GetSize();
        var toTest = size.GetPointsInGrid().Where(p => vistited[p.X, p.Y]).ToList();

        var count = 0;
        foreach(var p in toTest)
        {
            if(p == start) continue;

            var mapCopy = map.ToList();
            var charArray = mapCopy[p.Y].ToList();
            charArray[p.X] = '#';
            mapCopy[p.Y] = new string(charArray.ToArray());

            if(containsLoop(mapCopy))
                count++;
        }
        return count;
    }

    private bool[,] getVisited(List<string> map)
    {
        var size = map.GetSize();
        var visited = new bool[size.Width, size.Height];

        var currDir = Direction.N;
        var currPos = getStart(map);
        while(true)
        {
            visited[currPos.X, currPos.Y] = true;

            var nextPos = currPos.GetNeighbor(currDir);
            var nextVal = map.GetValueAt(nextPos);
            if(nextVal == '#')
            {
                currDir = currDir.Rotate4_CW();
                nextPos = currPos.GetNeighbor(currDir);
                nextVal = map.GetValueAt(nextPos);
            }

            if(nextVal == 'O')
            {
                break;
            }

            currPos = nextPos;
        }
        return visited;
    }

    private bool containsLoop(List<string> map)
    {
        var size = map.GetSize();
        var visited = new bool[size.Width, size.Height];
        var graph = new DirectedGraph<Point>();

        var currDir = Direction.N;
        var currPos = getStart(map);
        while(true)
        {
            visited[currPos.X, currPos.Y] = true;

            var nextPos = currPos.GetNeighbor(currDir);
            var nextVal = map.GetValueAt(nextPos);
            while(nextVal == '#')
            {
                currDir = currDir.Rotate4_CW();
                nextPos = currPos.GetNeighbor(currDir);
                nextVal = map.GetValueAt(nextPos);
                if(graph.Edges.ContainsKey(DirectedGraph<Point>.GetEdgeKey((currPos, nextPos))))
                {
                    return true;
                }
                graph.Add(currPos, nextPos);
            }

            if(nextVal == 'O')
            {
                return false;
            }

            currPos = nextPos;
        }
    }

    private Point getStart(List<string> map)
    {
        for(int y = 0; y < map.Count; y++)
        {
            for(int x = 0; x < map[0].Length; x++)
            {
                if(map[y][x] == '^')
                {
                    map[y].Replace('^', '.');
                    return new Point(x,y);
                }
            }
        }
        throw new Exception();
    }
}
