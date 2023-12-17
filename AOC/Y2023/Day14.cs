namespace rsmith985.AOC.Y2023;

public class Day14 : Day
{
/*
    protected override string _testString => 
@"O....#....
O.OO#....#
.....##...
OO.#O....O
.O.....O#.
O.#..O.#.#
..O..#O..O
.......O..
#....###..
#OO..#....";
*/

    public override object Part1()
    {
        var input = this.GetLines();
        tilt(input, Direction.N);
        return calcWeight(input, Direction.N);
    }

    public override object Part2()
    {
        var input = this.GetLines();

        var repeat = new Dictionary<int, int>();
        bool foundCycle = false;
        int extra = 0;
        int neededExtra = 100;
        long numIter = 1000000000;
        for(int i = 0; extra < neededExtra; i++)
        {
            tilt(input, Direction.N);
            tilt(input, Direction.W);
            tilt(input, Direction.S);
            tilt(input, Direction.E);

            if(!foundCycle)
            {
                var id = 0;
                for(int x = 0; x < input[0].Length; x++)
                {
                    for(int y = 0; y < input[1].Length; y++)
                    {
                        if(input[y][x] == 'O')
                            id += x*100+y;
                    }
                }
                if(repeat.ContainsKey(id))
                {
                    foundCycle = true;
                    var numInCycle = repeat.Count - repeat[id];
                    neededExtra = (int)((numIter - repeat.Count) % numInCycle) - 1;
                }
                else
                {
                    repeat.Add(id, i);
                }
            }
            else
            {
                extra++;
            }
            
        }
        return calcWeight(input, Direction.N);
    }

    private long calcWeight(string[] input, Direction dir)
    {
        var vert = dir.IsVert();
        var numRows = vert ? input[0].Length : input.Length;

        long tot = 0;
        for(int i = 0; i < numRows; i++)
        {
            var row = vert ? input.GetCol(i) : input[i];

            for(int j = 0; j < row.Length; j++)
            {
                if(row[j] == 'O')
                    tot += row.Length - j;
            }
        }
        return tot;
    }

    private void tilt(string[] input, Direction dir)
    {
        var vert = dir.IsVert();
        var numRows = vert ? input[0].Length : input.Length;

        for(int i = 0; i < numRows; i++)
        {
            var row = vert ? input.GetCol(i) : input[i];
            var rev = dir == Direction.E || dir == Direction.S;
            if(rev) row = new string(row.Reverse().ToArray());
            row = shiftLeft(row);
            if(rev) row = new string(row.Reverse().ToArray());
            
            if(vert)
                input.SetCol(i, row);
            else
                input[i] = row;
        }
    }

    private string shiftLeft(string str)
    {
        var rv = str.ToArray();
        var currOpen = 0;
        for(int i = 0; i < str.Length; i++)
        {
            var c = str[i];
            if(c == '#')
            {
                currOpen = i+1;
            }
            else if(c == 'O')
            {
                rv[i] = '.';
                rv[currOpen] = 'O';
                currOpen++;
            }
        }
        return new string(rv);
    }
}
