using System;

namespace rsmith985.AOC.Y2024;

public class Day09 : Day
{
    //protected override bool _useDefaultTestFile => true;

    public override object Part1()
    {
        (var fileBlocks, var empty) = parseInput();

        var emptyBlocks = new Queue<Block>(empty);
        var reverse = new Queue<Block>(fileBlocks.Reverse<Block>());

        var finalList = new List<Block>();

        var toMove = reverse.Dequeue();
        var available = emptyBlocks.Dequeue();
        while(available.Start < toMove.Start)
        {
            if(available.FreeSpace >= toMove.Length)
            {
                var newBlock = new Block(toMove.ID, available.FreeStart, toMove.Length);
                finalList.Add(newBlock);

                available.FreeSpace = available.FreeSpace - toMove.Length;

                toMove = reverse.Dequeue();
            }
            else
            {
                var newBlock = new Block(toMove.ID, available.FreeStart, available.FreeSpace);
                finalList.Add(newBlock);

                toMove = new Block(toMove.ID, toMove.Start, toMove.Length - newBlock.Length);
                available.FreeSpace = 0;
            }

            if(available.FreeSpace ==0)
            {
                /*
                if(!emptyBlocks.Any())
                {
                    finalList.Add(toMove);

                    break;
                }
                */
                available = emptyBlocks.Dequeue();
            }
        }

        finalList.Add(toMove);
        while(reverse.Any())
        {
            finalList.Add(reverse.Dequeue());
        }
        //print(finalList.OrderBy(i => i.Start).ToList());
        return checksum(finalList.OrderBy(i => i.Start).ToList());
    }

    public override object Part2()
    {
        (var fileBlocks, var emptyBlocks) = parseInput();

        var reverse = new Queue<Block>(fileBlocks.Reverse<Block>());

        var finalList = new List<Block>();

        foreach(var toMove in reverse)
        {           
            var empty = emptyBlocks.FirstOrDefault(i => i.Length >= toMove.Length);

            if(empty == null || empty.Start > toMove.Start)
            {
                finalList.Add(toMove);
                continue;
            }

            var newBlock = new Block(toMove.ID, empty.Start, toMove.Length);
            finalList.Add(newBlock);

            if(empty.Length > newBlock.Length)
            {
                empty.Start = newBlock.Start + newBlock.Length;
                empty.Length = empty.Length - newBlock.Length;
            }
            else
            {
                emptyBlocks.Remove(empty);
            }
        }


        //print(finalList.OrderBy(i => i.Start).ToList());
        //Console.WriteLine();
        return checksum(finalList.OrderBy(i => i.Start).ToList());
    }

    private void print(List<Block> blocks)
    {
        foreach(var block in blocks)
        {
            for(int i = 0; i < block.Length; i++)
            {
                Console.Write(block.ID);
            }
        }
    }

    private long checksum(List<Block> blocks)
    {
        var currBlockIdx = 0;
        var endIdx = blocks[^1].Start + blocks[^1].Length;

        long tot = 0;
        for(int i = 0; i < endIdx; )
        {
            if(!blocks[currBlockIdx].WithinRange(i))
                currBlockIdx++;
            
            while(blocks[currBlockIdx].Start > i)
                i++;
            
            var id = blocks[currBlockIdx].ID;

            tot += id*i++;
        }
        return tot;
    }

    private (List<Block> files, List<Block> free) parseInput()
    {
        var files = new List<Block>();
        var free = new List<Block>();

        var id = 0;
        var idx = 0;
        var onFile = true;
        foreach(var c in this.GetReader().ReadToEnd())
        {
            var num = (int)(c - 48);
            if(onFile)
            {
                files.Add(new Block(id++, idx, num));
            }
            else
            {
                free.Add(new Block(-1, idx, num));
            }
            onFile = !onFile;
            idx += num;
        }

        return (files, free);
    }
}

class Block
{
    public int ID{get;set;}
    public int Start{get;set;}
    public int Length{get;set;}
    public int FreeSpace {get;set;} = 0;

    public int FreeStart => this.Start+(this.Length-this.FreeSpace);

    public Block(int id, int start, int length)
    {
        ID = id;
        Start = start;
        Length = length;
        if(id < 0) this.FreeSpace = this.Length;
    }

    public bool WithinRange(int idx)
    {
        return idx >= this.Start && idx < (this.Start + this.Length);
    }
}
