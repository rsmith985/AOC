using System.Drawing;
using Emgu.CV.Structure;

namespace rsmith985.AOC.Y2023;

public class Day22 : Day
{
    //protected override bool _useDefaultTestFile => true;

    private List<Brick> _bricks;
    private List<Layer> _layers;
    public override void Init()
    {
        _bricks = new List<Brick>();
        _layers = new List<Layer>();
        foreach(var line in this.GetLines())
        {
            var parts = line.Split('~');
            var nums1 = parts[0].Split(',').Select(i => int.Parse(i)).ToTuple3();
            var nums2 = parts[1].Split(',').Select(i => int.Parse(i)).ToTuple3();
            _bricks.Add(new Brick(nums1, nums2));
        }

        var maxX = Math.Max(_bricks.Select(i => i.End1.X).Max(), _bricks.Select(i => i.End2.X).Max());
        var maxY = Math.Max(_bricks.Select(i => i.End1.Y).Max(), _bricks.Select(i => i.End2.Y).Max());
        var maxZ = Math.Max(_bricks.Select(i => i.End1.Z).Max(), _bricks.Select(i => i.End2.Z).Max());

        var size = new Size(maxX + 1, maxY + 1);
        for(int i = 1; i <= maxZ; i++)
            _layers.Add(new Layer(i, size));

        foreach(var brick in _bricks)
        {
            for(int z = brick.MinZ; z <= brick.MaxZ; z++)
            {
                _layers[z-1].AddBrick(brick);
                brick.Layers.Add(_layers[z-1]);
            }
        }
        dropBricks();
        addSupports();
    }

    public override object Part1()
    {
        return _bricks.Count(b => b.CanDisintegrate());
    }

    public override object Part2()
    {
        var cantDisintegrate = _bricks.Where(b => !b.CanDisintegrate());

        long tot = 0;
        foreach(var brick in cantDisintegrate)
        {
            var set = new HashSet<Brick>();
            set.Add(brick);

            var queue = new Queue<Brick>();
            queue.Enqueue(brick);
            while(queue.Any())
            {
                var curr = queue.Dequeue();
                foreach(var b in curr.Supporting)
                {
                    if(b.WouldFall(set))
                    {
                        set.Add(b);
                        queue.Enqueue(b);
                    }
                }
            }
            tot += set.Count - 1;
        }
        return tot;
    }

    private void dropBricks()
    {
        for(int i = 1; i < _layers.Count; i++)
        {
            var layer = _layers[i];
            foreach(var brick in layer.All.ToList())
            {
                for(int j = i - 1; j >= 0; j--)
                {
                    var below = _layers[j];
                    if(!below.TryDrop(brick)) break;
                }
            }
        }
    }

    private void addSupports()
    {
        for(int i = 1; i < _layers.Count; i++)
        {
            var below = _layers[i-1];
            var above = _layers[i];

            foreach(var brick in below.All)
            {
                for(int x = brick.MinX; x <= brick.MaxX; x++)
                {
                    for(int y = brick.MinY; y <= brick.MaxY; y++)
                    {
                        if(above.Grid[x,y] != null && above.Grid[x,y] != brick)
                        {
                            brick.Supporting.Add(above.Grid[x,y]);
                            above.Grid[x,y].SupportedBy.Add(brick);
                        }
                    }
                }
            }
        }
    }
}

class Brick
{
    private static char _debugName = 'A';
    public char DebugName{get;private set;}

    public (int X, int Y, int Z) End1{get; private set;}
    public (int X, int Y, int Z) End2{get; private set;}

    public int MinZ => Math.Min(this.End1.Z, this.End2.Z);
    public int MaxZ => Math.Max(this.End1.Z, this.End2.Z);
    public int MinX => Math.Min(this.End1.X, this.End2.X);
    public int MaxX => Math.Max(this.End1.X, this.End2.X);
    public int MinY => Math.Min(this.End1.Y, this.End2.Y);
    public int MaxY => Math.Max(this.End1.Y, this.End2.Y);

    public List<Layer> Layers{get;private set;}

    public HashSet<Brick> Supporting{get;private set;}
    public HashSet<Brick> SupportedBy{get;private set;}

    public Layer Top => this.Layers.Last();
    public Layer Bot => this.Layers.First();

    public Brick((int, int, int) end1, (int, int, int) end2)
    {
        this.DebugName = _debugName;
        _debugName++;

        this.End1 = end1;
        this.End2 = end2;
        this.Layers = new List<Layer>();

        this.Supporting = new HashSet<Brick>();
        this.SupportedBy = new HashSet<Brick>();
    }

    public void MoveDown(Layer bottom)
    {
        this.End1 = (this.End1.X, this.End1.Y, this.End1.Z - 1);
        this.End2 = (this.End2.X, this.End2.Y, this.End2.Z - 1);
        this.Top.RemoveBrick(this);
        bottom.AddBrick(this);
        this.Layers.Remove(this.Top);
        this.Layers.Insert(0, bottom);
    }

    public bool CanDisintegrate()
    {
        if(this.Supporting.Count == 0) return true;

        return this.Supporting.All(b => b.SupportedBy.Count >= 2);
    }
    public bool WouldFall(HashSet<Brick> removed)
    {
        return this.SupportedBy.All(i => removed.Contains(i));
    }
}

class Layer
{
    public int Z{get;private set;}

    public Brick[,] Grid{get; private set;}

    public HashSet<Brick> All{get;private set;}

    public Layer(int z, Size s)
    {
        this.Z = z;
        this.All = new HashSet<Brick>();
        this.Grid = new Brick[s.Width, s.Height];
    }

    public void AddBrick(Brick brick)
    {
        this.All.Add(brick);
        for(int x = brick.MinX; x <= brick.MaxX; x++)
        {
            for(int y = brick.MinY; y <= brick.MaxY; y++)
                this.Grid[x,y] = brick;
        }
    }

    public void RemoveBrick(Brick brick)
    {
        this.All.Remove(brick);
        for(int x = brick.MinX; x <= brick.MaxX; x++)
        {
            for(int y = brick.MinY; y <= brick.MaxY; y++)
                this.Grid[x,y] = null;
        }
    }

    public bool TryDrop(Brick brick)
    {
        for(int x = brick.MinX; x <= brick.MaxX; x++)
        {
            for(int y = brick.MinY; y <= brick.MaxY; y++)
            {
                if(this.Grid[x,y] != null)
                    return false;
            }
        }

        brick.MoveDown(this);
        return true;
    }

    public void DebugPrint()
    {
        for(int y = 0; y < this.Grid.GetLength(1); y++)
        {
            Console.Write(y + " ");
            for(int x = 0; x < this.Grid.GetLength(0); x++)
            {
                Console.Write(this.Grid[x,y]?.DebugName ?? '.');
            }
            Console.WriteLine();
        }
    }
}