using System;

namespace rsmith985.AOC.Y2024;

public class Day15 : Day
{
    //protected override bool _useDefaultTestFile => true;

    public override object Part1()
    {
        (var grid, var moves) = parseInput();

        var pos = getStartPos(grid);
        foreach(var move in moves)
        {
            pos = performMove(grid, pos, move);
        }

        return grid
                .GetAllValuesAndLocations()
                .Where(i => i.item == 'O')
                .Sum(i => i.loc.Y*100 + i.loc.X);
    }

    public override object Part2()
    {
        (var grid, var moves) = parseInput2();

        var pos = getStartPos(grid);
        foreach(var move in moves)
        {
            pos = performMove2(grid, pos, move);
        }

        return grid
                .GetAllValuesAndLocations()
                .Where(i => i.item == '[')
                .Sum(i => i.loc.Y*100 + i.loc.X);
    }

    private Point performMove(char[,] grid, Point pos, Direction dir)
    {
        var nextPos = pos.GetNeighbor(dir);
        var nextVal = grid.Get(nextPos);

        if(nextVal == '.') return nextPos;
        if(nextVal == '#') return pos;

        var startPos = nextPos;
        while(nextVal == 'O')
        {
            nextPos = nextPos.GetNeighbor(dir);
            nextVal = grid.Get(nextPos);
        }

        if(nextVal == '#') return pos;

        grid.Set(nextPos, 'O');
        grid.Set(startPos, '.');
        return startPos;
    }

    private Point performMove2(char[,] grid, Point pos, Direction dir)
    {
        if(dir.IsHorz())
            return performMove2_EW(grid, pos, dir);
        return performMove2_NS(grid, pos, dir);
    }

    private Point performMove2_EW(char[,] grid, Point pos, Direction dir)
    {
        var nextPos = pos.GetNeighbor(dir);
        var nextVal = grid.Get(nextPos);

        if(nextVal == '.') return nextPos;
        if(nextVal == '#') return pos;

        var startPos = nextPos;
        while(nextVal == '[' || nextVal == ']')
        {
            nextPos = nextPos.GetNeighbor(dir);
            nextVal = grid.Get(nextPos);
        }

        if(nextVal == '#') return pos;

        var endPos = nextPos;
        var opp = dir.GetOpposite();
        for(Point p = endPos; p != startPos; )
        {
            var n = p.GetNeighbor(opp);
            grid.Set(p, grid.Get(n));
            p = n;
        }
        grid.Set(startPos, '.');
        return startPos;
    }
    private Point performMove2_NS(char[,] grid, Point pos, Direction dir)
    {
        var nextPos = pos.GetNeighbor(dir);
        var nextVal = grid.Get(nextPos);

        if(nextVal == '.') return nextPos;
        if(nextVal == '#') return pos;

        var toCheck = new List<Point>();
        if(nextVal == '[')
        {
            toCheck.Add(nextPos);
            toCheck.Add(nextPos.GetNeighbor(Direction.E));
        }
        else if(nextVal == ']')
        {
            toCheck.Add(nextPos);
            toCheck.Add(nextPos.GetNeighbor(Direction.W));
        }
        else
        {
            throw new Exception();
        }

        var moveList = new List<Point>();
        if(!canMoveNS(grid, toCheck, moveList, dir))
            return pos;
        
        var vacated = new HashSet<(int, int)>();
        var filled = new HashSet<(int, int)>();
        foreach(var p in moveList.Reverse<Point>())
        {
            var next = p.GetNeighbor(dir);
            grid.Set(next, grid.Get(p));

            vacated.Add(p.ToTuple());
            filled.Add(next.ToTuple());
        }

        foreach(var v in vacated)
        {
            if(filled.Contains(v)) continue;
            grid.Set(v.ToPoint(), '.');
        }

        return nextPos;
    }
    private bool canMoveNS(char[,] grid, List<Point> toCheck, List<Point> finalList, Direction dir)
    {
        var nextToCheck = new List<Point>();
        foreach(var p in toCheck)
        {
            var nextPos = p.GetNeighbor(dir);
            var nextVal = grid.Get(nextPos);

            if(nextVal == '#') return false;

            var currVal = grid.Get(p);
            if(currVal == '[')
            {
                if(nextVal == '[') nextToCheck.Add(nextPos);
                if(nextVal == ']')
                {
                    nextToCheck.Add(nextPos);
                    nextToCheck.Add(nextPos.GetNeighbor(Direction.W));
                }
            }
            else if(currVal == ']')
            {
                if(nextVal == ']') nextToCheck.Add(nextPos);
                if(nextVal == '[')
                {
                    nextToCheck.Add(nextPos);
                    nextToCheck.Add(nextPos.GetNeighbor(Direction.E));
                }
            }
            else
                throw new Exception();
        }
        if(!toCheck.Any())
            return true;

        finalList.AddRange(toCheck);
        return canMoveNS(grid, nextToCheck, finalList, dir);
    }

    private Point getStartPos(char[,] grid)
    {
        for(int x = 0; x < grid.GetLength(0); x++)
        {
            for(int y = 0; y < grid.GetLength(1); y++)
            {
                if(grid[x,y] == '@')
                {
                    grid[x,y] = '.';
                    return new Point(x,y);
                }
            }
        }
        throw new Exception();
    }

    private (char[,] grid, List<Direction> moves) parseInput()
    {
        var gridLines = new List<string>();
        var moveLines = new List<string>();

        foreach(var line in this.GetLines())
        {
            if(line.StartsWith('#'))
                gridLines.Add(line);
            else if(line.Length > 1)
                moveLines.Add(line);
        }

        var grid = gridLines.ToGrid();

        var moves = new List<Direction>();
        foreach(var line in moveLines)
        {
            foreach(var c in line)
            {
                moves.Add(c.ToDirection());
            }
        }
        return (grid, moves);
    }
    private (char[,] grid, List<Direction> moves) parseInput2()
    {
        var gridLines = new List<string>();
        var moveLines = new List<string>();

        foreach(var line in this.GetLines())
        {
            if(line.StartsWith('#'))
            {
                var l = line.Replace("#", "##");
                l = l.Replace("O", "[]");
                l = l.Replace(".", "..");
                l = l.Replace("@", "@.");
                gridLines.Add(l);
            }
            else if(line.Length > 1)
                moveLines.Add(line);
        }

        var grid = gridLines.ToGrid();

        var moves = new List<Direction>();
        foreach(var line in moveLines)
        {
            foreach(var c in line)
            {
                moves.Add(c.ToDirection());
            }
        }
        return (grid, moves);
    }
}
