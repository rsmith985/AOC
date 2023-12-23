using System.Drawing;

namespace rsmith985.AOC.Y2023;

public class Day21 : Day
{
    //protected override bool _useDefaultTestFile => true;

    private char[,] _input;
    private Size _inputSize;
    private Point _start;

    public override void Init()
    {
        _input = this.GetLines().To2DArray();
        _inputSize = _input.GetSize();

        foreach(var p in _inputSize.GetPointsInGrid())
        {
            if(_input.Get(p) == 'S')
            {
                _start = p;
                _input.Set(p, '.');
                return;
            }
        }
    }

   // private int _testSteps = 101;
    public override object Part1()
    {
        //return countInfinite(_start, _testSteps);
        return count(_start, 64);
    }

    public override object Part2()
    {
        var steps = 26501365;
        var len = _inputSize.Width;     // everything assumes this has to be odd

        var numFullJumps = steps / len;
        var numRemainSteps = steps % len;

        var stepsForDiamondCorners1 = steps % len + len /2; 
        var stepsForDiamondCorners2 =  stepsForDiamondCorners1 - len; 
        var stepsForDiamondEdge1 = steps % len - 1;
        var stepsForDiamondEdge2 = stepsForDiamondEdge1 + len;

        (var countFullOdd, var countFullEven) = countBoth(_start, len * 2);          // Just need enough steps that we know it will fully fill a grid

        var countCornerN1 = count(new Point(_start.X, 0), stepsForDiamondCorners1);
        var countCornerS1 = count(new Point(_start.X, len - 1), stepsForDiamondCorners1);
        var countCornerE1 = count(new Point(len - 1, _start.Y), stepsForDiamondCorners1);
        var countCornerW1 = count(new Point(0, _start.Y), stepsForDiamondCorners1);

        var countCornerN2 = count(new Point(_start.X, 0), stepsForDiamondCorners2);
        var countCornerS2 = count(new Point(_start.X, len - 1), stepsForDiamondCorners2);
        var countCornerE2 = count(new Point(len - 1, _start.Y), stepsForDiamondCorners2);
        var countCornerW2 = count(new Point(0, _start.Y), stepsForDiamondCorners2);

        var countEdgeNW1 = count(new Point(0,0), stepsForDiamondEdge1);
        var countEdgeNW2 = count(new Point(0,0), stepsForDiamondEdge2);
        var countEdgeNE1 = count(new Point(len-1,0), stepsForDiamondEdge1);
        var countEdgeNE2 = count(new Point(len-1,0), stepsForDiamondEdge2);
        var countEdgeSW1 = count(new Point(0,len-1), stepsForDiamondEdge1);
        var countEdgeSW2 = count(new Point(0,len-1), stepsForDiamondEdge2);
        var countEdgeSE1 = count(new Point(len-1,len-1), stepsForDiamondEdge1);
        var countEdgeSE2 = count(new Point(len-1,len-1), stepsForDiamondEdge2);

        (var numFullOdd, var numFullEven) = calcNumFullGrid(numFullJumps);

        var numEdge1 = calcNumEdge1(numFullJumps);
        var numEdge2 = calcNumEdge2(numFullJumps);

        var totFull = numFullOdd * countFullOdd + numFullEven * countFullEven;
        var totCorners1 = countCornerN1 + countCornerS1 + countCornerE1 + countCornerW1;
        var totCorners2 = countCornerN2 + countCornerS2 + countCornerE2 + countCornerW2;
        var totEdges1 = (countEdgeNE1 + countEdgeNW1 + countEdgeSE1 + countEdgeSW1) * numEdge1;
        var totEdges2 = (countEdgeNE2 + countEdgeNW2 + countEdgeSE2 + countEdgeSW2) * numEdge2;

        return totFull + totCorners1 + totCorners2 + totEdges1 + totEdges2;
    }

    public (long odd, long even) calcNumFullGrid(int jumps)
    {
        long odd = 1;
        long even = 0;
        var onOdd = false;
        for(int x = 1; x < jumps; x++)
        {
            var nextRing = 4 * x;
            if(onOdd)
                odd += nextRing;
            else
                even += nextRing;
            onOdd = !onOdd;
        }    
        return (odd, even);
    }

    private long calcNumEdge1(int jumps) => jumps;
    private long calcNumEdge2(int jumps) => jumps == 0 ? 0 : jumps  - 1;


    private long count(Point start, int steps)
    {
        if(steps < 0) return 0;
        if(steps == 0) return 1;

        (var odd, var even) = countBoth(start, steps);
        return steps % 2 == 0 ? even : odd;
    }

    private (long odd, long even) countBoth(Point start, int steps)
    {
        var oddSpots = new HashSet<(int, int)>();
        var evenSpots = new HashSet<(int, int)>();

        var bounds = new Rectangle(Point.Empty, _inputSize);
        var curr = new List<Point>(){start};
        var onEvenStep = false;
        for(int i = 0; i < steps && curr.Any(); i++)
        {
            var next = new List<Point>();
            foreach(var p in curr)
            {
                foreach(var neighbor in p.GetNeighbors4(bounds))
                {
                    if(_input.Get(neighbor) == '.')
                    {
                        var tuple = neighbor.ToTuple();
                        if(onEvenStep)
                        {
                            if(evenSpots.Contains(tuple)) continue;
                            evenSpots.Add(tuple);

                            next.Add(neighbor);
                        }
                        else 
                        {
                            if(oddSpots.Contains(tuple)) continue;
                            oddSpots.Add(tuple);

                            next.Add(neighbor);
                        }
                    }
                }
            }
            onEvenStep = !onEvenStep;
            curr = next;
        }

        return (oddSpots.Count, evenSpots.Count);
    }

    #region Debugging
    private long countInfinite(Point start, int steps)
    {
        var oddSpots = new HashSet<(int, int)>();
        var evenSpots = new HashSet<(int, int)>();

        var bounds = new Rectangle(Point.Empty, _inputSize);
        var curr = new List<Point>(){_start};
        var onEvenStep = false;
        for(int i = 0; i < steps; i++)
        {
            var next = new List<Point>();
            foreach(var p in curr)
            {
                foreach(var neighbor in p.GetNeighbors4())
                {
                    if(infiniteInGarden(neighbor))
                    {
                        var tuple = neighbor.ToTuple();
                        if(onEvenStep)
                        {
                            if(evenSpots.Contains(tuple)) continue;
                            evenSpots.Add(tuple);

                            next.Add(neighbor);
                        }
                        else 
                        {
                            if(oddSpots.Contains(tuple)) continue;
                            oddSpots.Add(tuple);

                            next.Add(neighbor);
                        }
                    }
                }
            }
            onEvenStep = !onEvenStep;
            curr = next;
        }

        return steps % 2 == 0 ? evenSpots.Count : oddSpots.Count;
    }

    private Point infiniteToGrid(Point p)
    {
        var x = p.X;
        var y = p.Y;

        while(x < 0) x += _inputSize.Width;
        while(y < 0) y += _inputSize.Height;

        if(x >= _inputSize.Width) x = x % _inputSize.Width;
        if(y >= _inputSize.Height) y = y % _inputSize.Height;

        return new Point(x, y);
    }
    private bool infiniteInGarden(Point p)
    {
        var p2 = infiniteToGrid(p);

        return _input.Get(p2) == '.';
    }
    #endregion
}
