using System.Drawing;

namespace rsmith985.AOC.Y2023;

public class Day16 : Day
{
    public override object Part1()
    {
        var lines = this.GetLines();
        var input = lines.PadBorder('#').To2DArray();
        var visited = new bool[input.GetLength(0), input.GetLength(1)];
        var set = new HashSet<(int, int, int)>();

        var currEnds = new Queue<(Point p, Direction d)>();
        currEnds.Enqueue((new Point(1, 1), Direction.E));

        while(currEnds.Any())
        {
            var curr = currEnds.Dequeue();

            var id  = identifier(curr);
            if(set.Contains(id))
                continue;

            set.Add(id);

            var c = input[curr.p.X, curr.p.Y];
            if(c == '#')
                continue;

            visited[curr.p.X, curr.p.Y] = true;
            if(c == '.')
            {
                currEnds.Enqueue((curr.p.GetNeighbor(curr.d), curr.d));
            }
            else if(c == '|')
            {
                if(curr.d.IsVert())
                    currEnds.Enqueue((curr.p.GetNeighbor(curr.d), curr.d));
                else
                {
                    currEnds.Enqueue((curr.p.GetNeighbor(Direction.N), Direction.N));
                    currEnds.Enqueue((curr.p.GetNeighbor(Direction.S), Direction.S));
                }
            }
            else if( c == '-')
            {
                if(curr.d.IsHorz())
                    currEnds.Enqueue((curr.p.GetNeighbor(curr.d), curr.d));
                else
                {
                    currEnds.Enqueue((curr.p.GetNeighbor(Direction.E), Direction.E));
                    currEnds.Enqueue((curr.p.GetNeighbor(Direction.W), Direction.W));
                }
            }
            else if (c == '\\')
            {
                if(curr.d == Direction.E)
                    currEnds.Enqueue((curr.p.GetNeighbor(Direction.S), Direction.S));
                else if(curr.d == Direction.N)
                    currEnds.Enqueue((curr.p.GetNeighbor(Direction.W), Direction.W));
                else if(curr.d == Direction.W)
                    currEnds.Enqueue((curr.p.GetNeighbor(Direction.N), Direction.N));
                else if(curr.d == Direction.S)
                    currEnds.Enqueue((curr.p.GetNeighbor(Direction.E), Direction.E));
                else
                    throw new Exception("");
            }
            else if (c == '/')
            {
                if(curr.d == Direction.E)
                    currEnds.Enqueue((curr.p.GetNeighbor(Direction.N), Direction.N));
                else if(curr.d == Direction.N)
                    currEnds.Enqueue((curr.p.GetNeighbor(Direction.E), Direction.E));
                else if(curr.d == Direction.W)
                    currEnds.Enqueue((curr.p.GetNeighbor(Direction.S), Direction.S));
                else if(curr.d == Direction.S)
                    currEnds.Enqueue((curr.p.GetNeighbor(Direction.W), Direction.W));
                else
                    throw new Exception("");
            }
        }

        var debug = new string[visited.GetLength(1)];
        var count = 0;
        for(int y = 0; y < visited.GetLength(1); y++)
        {
            var str = y.ToString().PadLeft(2) + ": ";
            for(int x = 0; x < visited.GetLength(0); x++)
            {
                if(visited[x,y]) 
                {
                    str += "#";
                    count++;
                }
                else
                {
                    str += ".";
                }
            }
            debug[y] = str;
        }
        //Console.WriteLine(debug.PrintLines());

        return count;
    }

    private (int, int, int) identifier((Point p, Direction d) item) => (item.p.X, item.p.Y, (int)item.d);

    public override object Part2()
    {
        var lines = this.GetLines();
        var input = lines.PadBorder('#').To2DArray();

        var initPos = new List<(Point, Direction)>();
        var b = input.GetLength(1) - 2;
        var r = input.GetLength(0) - 2;
        for(int x = 1; x < input.GetLength(0) - 1; x++)
        {
            initPos.Add((new Point(x, 1), Direction.S));
            initPos.Add((new Point(x, b), Direction.N));
        }
        for(int y = 1; y < input.GetLength(1) - 1; y++)
        {
            initPos.Add((new Point(1, y), Direction.E));
            initPos.Add((new Point(r, y), Direction.W));
        }

        int max = 0;
        foreach(var start in initPos)
        {
            var num = test(input, start);
            if(num > max)
                max = num;
        }

        return max;
    }

    private int test(char[,] input, (Point p, Direction d) start)
    {
        var visited = new bool[input.GetLength(0), input.GetLength(1)];
        var set = new HashSet<(int, int, int)>();

        var currEnds = new Queue<(Point p, Direction d)>();
        currEnds.Enqueue(start);

        while(currEnds.Any())
        {
            var curr = currEnds.Dequeue();

            var id  = identifier(curr);
            if(set.Contains(id))
                continue;

            set.Add(id);

            var c = input[curr.p.X, curr.p.Y];
            if(c == '#')
                continue;

            visited[curr.p.X, curr.p.Y] = true;
            if(c == '.')
            {
                currEnds.Enqueue((curr.p.GetNeighbor(curr.d), curr.d));
            }
            else if(c == '|')
            {
                if(curr.d.IsVert())
                    currEnds.Enqueue((curr.p.GetNeighbor(curr.d), curr.d));
                else
                {
                    currEnds.Enqueue((curr.p.GetNeighbor(Direction.N), Direction.N));
                    currEnds.Enqueue((curr.p.GetNeighbor(Direction.S), Direction.S));
                }
            }
            else if( c == '-')
            {
                if(curr.d.IsHorz())
                    currEnds.Enqueue((curr.p.GetNeighbor(curr.d), curr.d));
                else
                {
                    currEnds.Enqueue((curr.p.GetNeighbor(Direction.E), Direction.E));
                    currEnds.Enqueue((curr.p.GetNeighbor(Direction.W), Direction.W));
                }
            }
            else if (c == '\\')
            {
                if(curr.d == Direction.E)
                    currEnds.Enqueue((curr.p.GetNeighbor(Direction.S), Direction.S));
                else if(curr.d == Direction.N)
                    currEnds.Enqueue((curr.p.GetNeighbor(Direction.W), Direction.W));
                else if(curr.d == Direction.W)
                    currEnds.Enqueue((curr.p.GetNeighbor(Direction.N), Direction.N));
                else if(curr.d == Direction.S)
                    currEnds.Enqueue((curr.p.GetNeighbor(Direction.E), Direction.E));
                else
                    throw new Exception("");
            }
            else if (c == '/')
            {
                if(curr.d == Direction.E)
                    currEnds.Enqueue((curr.p.GetNeighbor(Direction.N), Direction.N));
                else if(curr.d == Direction.N)
                    currEnds.Enqueue((curr.p.GetNeighbor(Direction.E), Direction.E));
                else if(curr.d == Direction.W)
                    currEnds.Enqueue((curr.p.GetNeighbor(Direction.S), Direction.S));
                else if(curr.d == Direction.S)
                    currEnds.Enqueue((curr.p.GetNeighbor(Direction.W), Direction.W));
                else
                    throw new Exception("");
            }
        }
        
       // var debug = new string[visited.GetLength(1)];
        var count = 0;
        for(int y = 0; y < visited.GetLength(1); y++)
        {
            //var str = y.ToString().PadLeft(2) + ": ";
            for(int x = 0; x < visited.GetLength(0); x++)
            {
                if(visited[x,y]) 
                {
                    //str += "#";
                    count++;
                }
                else
                {
                   // str += ".";
                }
            }
            //debug[y] = str;
        }
        //Console.WriteLine(debug.PrintLines());
        

        return count;
    }
}
