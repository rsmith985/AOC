using System.Data;
using System.Runtime.Intrinsics.X86;
using Microsoft.Z3;

namespace rsmith985.AOC.Y2023;

public class Day24 : Day
{
    /*
    protected override string _testString => @"19, 13, 30 @ -2,  1, -2
18, 19, 22 @ -1, -1, -2
20, 25, 34 @ -2, -2, -4
12, 31, 28 @ -1, -2, -1
20, 19, 15 @  1, -5, -3";
*/

    private List<Hail> _hail;
    public override void Init()
    {
        _hail = new List<Hail>();
        foreach(var line in this.GetLines())
        {
            var parts = line.Split('@');

            var pItems = parts[0].Split(',');
            var pX = long.Parse(pItems[0].Trim());
            var pY = long.Parse(pItems[1].Trim());
            var pZ = long.Parse(pItems[2].Trim());

            var vItems = parts[1].Split(',');
            var vX = long.Parse(vItems[0].Trim());
            var vY = long.Parse(vItems[1].Trim());
            var vZ = long.Parse(vItems[2].Trim());

            _hail.Add(new Hail((pX, pY, pZ), (vX, vY, vZ)));
        }
    }

    public override object Part1() 
    {
        var min = 200000000000000;
        var max = 400000000000000;
        var count = 0;
        for(int i = 0; i < _hail.Count - 1; i++)
        {
            var h1 = _hail[i];
            var m1 = h1.Slope;
            var b1 = h1.Intercept;
            var x1 = h1.Location.X;
            var y1 = h1.Location.Y;
            var vx1 = h1.Velocity.X;
            var vy1 = h1.Velocity.Y;
            for(int j = i+1; j < _hail.Count; j++)
            {
                var h2 = _hail[j];
                var m2 = h2.Slope;
                var b2 = h2.Intercept;

                if(m1 == m2) continue;

                // solve y/x for y = mx + b
                var delta = m2 - m1;
                var x = (b1 - b2) / delta;
                var y = (m2 * b1 - m1 * b2) / delta;

                // check if within bounds
                if(x < min || x > max || y < min || y > max) continue;

                var x2 = h2.Location.X;
                var y2 = h2.Location.Y;
                var vx2 = h2.Velocity.X;
                var vy2 = h2.Velocity.Y;

                // Verify intersection didn't happen before initial position
                if(x - x1 < 0 != vx1 < 0 || y - y1 < 0 != vy1 < 0)
                    continue;
                if(x - x2 < 0 != vx2 < 0 || y - y2 < 0 != vy2 < 0)
                    continue;
                
                count++;
            }
        }

        return count;
    }

    public override object Part2()
    {
        return "This is stupid";
        //return solveP2();
    }

/*
    private long solve()
    {
        var tried = new HashSet<(int, int, int)>();
        for(int i = 0; i < 1000; i++)
        {
            for(int vx = -i; vx <= i; vx++)
            {
                for(int vy = -i; vy <= i; vy++)
                {
                    for(int vz = -i; vy <= i; vz++)
                    {
                        var key = (vx, vy, vz);
                        if(tried.Contains(key)) continue;
                        tried.Add(key);


                    }
                }
            }
        }
        for(int vx = 0; vx < 500; vx++)

        return 0;
    }
    */


    /// <summary>
    /// Stole someone else's code that just uses an system of equation solver.
    /// </summary>
    /// <returns></returns>
    private long solveP2()
    {
        var ctx = new Context();
        var solver = ctx.MkSolver();
    
        // Coordinates of the stone
        var x = ctx.MkIntConst("x");
        var y = ctx.MkIntConst("y");
        var z = ctx.MkIntConst("z");
    
        // Velocity of the stone
        var vx = ctx.MkIntConst("vx");
        var vy = ctx.MkIntConst("vy");
        var vz = ctx.MkIntConst("vz");
    
        // For each iteration, we will add 3 new equations and one new condition to the solver.
        // We want to find 9 variables (x, y, z, vx, vy, vz, t0, t1, t2) that satisfy all the equations, so a system of 9 equations is enough.
        for (var i = 0; i < 3; i++)
        {
            var t = ctx.MkIntConst($"t{i}"); // time for the stone to reach the hail
            var hail = _hail[i];
    
            var px = ctx.MkInt(hail.Location.X);
            var py = ctx.MkInt(hail.Location.Y);
            var pz = ctx.MkInt(hail.Location.Z);
            
            var pvx = ctx.MkInt(hail.Velocity.X);
            var pvy = ctx.MkInt(hail.Velocity.Y);
            var pvz = ctx.MkInt(hail.Velocity.Z);
            
            var xLeft = ctx.MkAdd(x, ctx.MkMul(t, vx)); // x + t * vx
            var yLeft = ctx.MkAdd(y, ctx.MkMul(t, vy)); // y + t * vy
            var zLeft = ctx.MkAdd(z, ctx.MkMul(t, vz)); // z + t * vz
    
            var xRight = ctx.MkAdd(px, ctx.MkMul(t, pvx)); // px + t * pvx
            var yRight = ctx.MkAdd(py, ctx.MkMul(t, pvy)); // py + t * pvy
            var zRight = ctx.MkAdd(pz, ctx.MkMul(t, pvz)); // pz + t * pvz
    
            solver.Add(t >= 0); // time should always be positive - we don't want solutions for negative time
            solver.Add(ctx.MkEq(xLeft, xRight)); // x + t * vx = px + t * pvx
            solver.Add(ctx.MkEq(yLeft, yRight)); // y + t * vy = py + t * pvy
            solver.Add(ctx.MkEq(zLeft, zRight)); // z + t * vz = pz + t * pvz
        }
    
        solver.Check();

        var model = solver.Model;
        var rx = model.Eval(x);
        var ry = model.Eval(y);
        var rz = model.Eval(z);
    
        return Convert.ToInt64(rx.ToString()) + Convert.ToInt64(ry.ToString()) + Convert.ToInt64(rz.ToString());
    }
}

class Hail
{
    public (long X, long Y, long Z) Location{get;}
    public (long X, long Y, long Z) Velocity{get;}

    public double Slope{get;}
    public double Intercept{get;}

    public Hail((long, long, long) loc, (long, long, long) velocity)
    {
        this.Location = loc;
        this.Velocity = velocity;
        
        var p1 = new Coord(this.Location.X, this.Location.Y);
        var p2 = p1.Plus(this.Velocity.X, this.Velocity.Y);

        this.Slope = (double)(p2.Y - p1.Y) / (p2.X - p1.X);
        this.Intercept = p2.Y - this.Slope * p2.X;
    }
}