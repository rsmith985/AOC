using System;

namespace rsmith985.AOC.Y2024;

public class Day17 : Day
{
    //protected override bool _useDefaultTestFile => true;

    public override object Part1()
    {
        (var nums, var reg) = parseInput();

        var output = runProgram(nums, reg);

        return output.Print();
    }

    public override object Part2()
    {
        (var nums, var reg) = parseInput();

        var valid = new List<long>();
        var goal = nums.ToList();
        long minA = 35184890545666L;
        long maxA = 281474976710655L;
        long regA = minA;
        var count = 0;
        while(count < 1000000)
        {
            var r = reg.Copy();
            r.A = regA;

            var output = runProgram(nums, r);

            var b = true;
            for(int i = 0; i <= 15; i++)
            {
                if(output[i] != goal[i])
                {
                    b = false;
                    break;
                }
            }
            if(b)
                valid.Add(regA);
            
            if(output.Count > goal.Count)
                break;
            regA += 536870912L;
            count++;
        }

        for(int i = 0; i < valid.Count - 1; i++)
        {
            Console.WriteLine(valid[i] + ": " + (valid[i+1] - valid[i]));
        }

        return 0;
    }

    private List<long> runProgram(int[] nums, Registers reg)
    {
        var output = new List<long>();
        var ptr = 0;
        while(ptr < nums.Length)
        {
            var code = nums[ptr];
            var operand = nums[ptr+1];

            ptr = performOp(code, operand, ptr, reg, output);
        }
        return output;
    }

    private int performOp(long code, long operand, int ptr, Registers reg, List<long> output)
    {
        if(code == 0)
        {
            reg.A = doDv(operand, reg);
            //Console.WriteLine("A: " + reg.A);
        }
        else if(code == 1)
        {
            reg.B = reg.B ^ operand;
        }
        else if(code == 2)
        {
            reg.B = getCombo(operand, reg) % 8;
        }
        else if(code == 3)
        {
            if(reg.A != 0)
                return (int)operand;
        }
        else if(code == 4)
        {
            reg.B = reg.B ^ reg.C;
        }
        else if(code == 5)
        {
            var val = getCombo(operand, reg) % 8;
            output.Add((int)val);
        }
        else if(code == 6)
        {
            reg.B = doDv(operand, reg);
        }
        else if(code == 7)
        {
            reg.C = doDv(operand, reg);
        }
        return ptr + 2;
    }

    private long doDv(long operand, Registers reg)
    {
        var d = Math.Pow(2, getCombo(operand, reg));
        return (long)(reg.A / d);
    }
    private long getCombo(long operand, Registers reg)
    {
        if(operand <= 3) return operand;
        if(operand == 4) return reg.A;
        if(operand == 5) return reg.B;
        if(operand == 6) return reg.C;

        throw new Exception();
    }

    private (int[] numbers, Registers reg) parseInput()
    {
        var lines = this.GetLines();
        var reg = new Registers();
        reg.A = int.Parse(lines[0][12..]);
        reg.B = int.Parse(lines[1][12..]);
        reg.C = int.Parse(lines[2][12..]);

        var numbers = lines[4][9..].Split(int.Parse, ",").ToArray();

        return (numbers, reg);
    }

    class Registers
    {
        public long A;
        public long B;
        public  long C;

        public Registers(){}
        public Registers(long a, long b, long c)
        {
                this.A = a; this.B = b; this.C = c;
        }

        public Registers Copy()
        {
                return new Registers(this.A, this.B, this.C);
        }
    }
}


