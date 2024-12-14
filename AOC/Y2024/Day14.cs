using System;

namespace rsmith985.AOC.Y2024;

public class Day14 : Day
{
    private static int LIMIT_X = 101;
    private static int LIMIT_Y = 103;

    //protected override bool _useDefaultTestFile => true;

    public override object Part1()
    {
        var robots = parseInput();
        iterate(robots, 100);

        //display(robots);
        return calculateQuadrantScore(robots);
    }

    public override object Part2()
    {
        var robots = parseInput();
        int maxVal = 0;
        var maxIdx = 0;
        var maxRobots = new List<Robot>();
        foreach(var r in robots)
            maxRobots.Add(new Robot(r.Location, r.Move));

        for(int i = 0; i < 10000; i++)
        {
            iterate(robots, 1);
            var score = adjacencyScore(robots);

            if(score > maxVal)
            {
                maxIdx = i;
                maxVal = score;
                maxRobots.Clear();
                foreach(var r in robots)
                    maxRobots.Add(new Robot(r.Location, r.Move));
                
                //Console.WriteLine($"{i}: {score}");
            }
        }

        //display(maxRobots);

        return maxIdx+1;
    }

    private void display(List<Robot> robots)
    {
        for(int y = 0; y < LIMIT_Y; y++)
        {
            for(int x = 0; x < LIMIT_X; x++)
            {
                var num = robots.Count(i => i.Location == (x,y));
                Console.Write(num);
            }
            Console.WriteLine();
        }
    }

    private void iterate(List<Robot> robots, int num)
    {
        foreach(var robot in robots)
        {
            var x = (robot.Location.x + (robot.Move.x + LIMIT_X) * num) % LIMIT_X;
            var y = (robot.Location.y + (robot.Move.y + LIMIT_Y) * num) % LIMIT_Y;

            robot.Location = (x,y);
        }
    }

    private int adjacencyScore(List<Robot> robots)
    {
        var set = new HashSet<(int x, int y)>();
        robots.Perform(i => set.Add(i.Location));

        int tot = 0;
        foreach(var p in set)
        {
            if(set.Contains(p.GetNeighbor(Direction.N)))
                tot++;
            if(set.Contains(p.GetNeighbor(Direction.S)))
                tot++;
            if(set.Contains(p.GetNeighbor(Direction.E)))
                tot++;
            if(set.Contains(p.GetNeighbor(Direction.W)))
                tot++;
        }
        return tot;
    }

    private long calculateQuadrantScore(List<Robot> robots)
    {
        var q1 = 0;
        var q2 = 0; 
        var q3 = 0;
        var q4 = 0;
        var midX = LIMIT_X / 2;
        var midY = LIMIT_Y / 2;
        foreach(var robot in robots)
        {
            var x = robot.Location.x;
            var y = robot.Location.y;

            if(x < midX && y < midY)
                q1++;
            else if(x > midX && y < midY)
                q2++;
            else if(x < midX && y > midY)
                q3++;
            else if(x > midX && y > midY)
                q4++;
        }
        return (long)q1 * q2 * q3 * q4;
    }

    private List<Robot> parseInput()
    {
        var rv = new List<Robot>();
        foreach(var line in this.GetLines())
        {
            var idx = line.IndexOf("v=");

            var p = line[2..(idx-1)].Split2(int.Parse, ",");
            var v = line[(idx+2)..].Split2(int.Parse, ",");

            rv.Add(new Robot(p, v));
        }
        return rv;
    }
    class Robot
    {
        public (int x, int y) Location{get; set;}
        public (int x, int y) Move{get;set;}

        public Robot((int x, int y) loc, (int x, int y) move)
        {
            this.Location = loc;
            this.Move = move;
        }
    }
}
