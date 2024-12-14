using System;

namespace rsmith985.AOC.Y2024;

public class Day13 : Day
{
    //protected override bool _useDefaultTestFile => true;

    public override object Part1()
    {
        var input = parseInput(0);

        long sum = 0;
        foreach(var eq in input)
        {
            var ans = eq.Solve();
            sum += (ans.X*3 + ans.Y);
        }
        return sum;
    }

    public override object Part2()
    {
        var input = parseInput(10000000000000);

        long sum = 0;
        foreach(var eq in input)
        {
            var ans = eq.Solve();
            sum += (ans.X*3 + ans.Y);
        }
        return sum;
    }

    private List<Equation> parseInput(long extra)
    {
        var rv = new List<Equation>();
        var lines = this.GetLines();
        for(int i = 0; i < lines.Length; i=i+4)
        {
            var l1 = lines[i];
            var l2 = lines[i+1];
            var l3 = lines[i+2];

            var ax = long.Parse(l1[(l1.IndexOf("X+")+2)..l1.IndexOf(',')]);
            var ay = long.Parse(l1[(l1.IndexOf("Y+")+2)..]);

            var bx = long.Parse(l2[(l2.IndexOf("X+")+2)..l2.IndexOf(',')]);
            var by = long.Parse(l2[(l2.IndexOf("Y+")+2)..]);

            var x = long.Parse(l3[(l3.IndexOf("X=")+2)..l3.IndexOf(',')]) + extra;
            var y = long.Parse(l3[(l3.IndexOf("Y=")+2)..]) + extra;

            rv.Add(new Equation(ax, ay, bx, by, x, y));
        }
        return rv;
    }
}

class Equation
{
    public long Ax{get;set;}
    public long Ay{get;set;}
    public long Bx{get;set;}
    public long By{get;set;}
    public long X{get;set;}
    public long Y{get;set;}

    public Equation(long ax, long ay, long bx, long by, long x, long y)
    {
        this.Ax = ax;
        this.Ay = ay;
        this.Bx = bx;
        this.By = by;
        this.X = x;
        this.Y = y;
    }

    public Coord Solve()
    {
        var d = (double)Ay/Ax;

        var top = Y - d*X;
        var bot = By - d*Bx;

        var B = (long)Math.Round(top / bot);
        var A = (long)Math.Round((X-(double)Bx*B)/Ax);

        if(validate(A, B))
            return new Coord(A, B);
        return new Coord(0,0);
    }

    private bool validate(long a, long b)
    {
        return ((Ax*a + Bx*b) == X) && ((Ay*a + By*b) == Y);
    }
}
