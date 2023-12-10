namespace rsmith985.AOC.Y2023;

public class Day07 : Day
{
    public override object Part1_() =>
        new Func<List<long>, long>(bids =>  Enumerable.Range(0, bids.Count).Sum(i => (i + 1) * bids[i])
        )(File.ReadAllLines(_file)
            .Select(line => ( long.Parse(line[5..]),
                line[..5].Select(c => c == 'A' ? 14 : c == 'K' ? 13 :  c == 'Q' ? 12 : c == 'J' ? 11 : c == 'T' ? 10 : c - 48).ToArray() ) )
            .Select(item =>
                (   item.Item1, 
                    new Func<List<IGrouping<int, int>>, int>(cards =>
                        cards.Count == 1 ? 6 :
                        cards.Count == 2 ?
                            (cards.First().Count() == 4 || cards.First().Count() == 1 ? 5 : 4) :
                        cards.Count == 3 ?
                            (cards.ToList()[0].Count() == 3 || cards.ToList()[1].Count() == 3 || cards.ToList()[2].Count() == 3 ? 3 : 2) :
                        cards.Count == 4 ? 1 : 0
                    )(new List<IGrouping<int, int>>(item.Item2.GroupBy(i => i))),
                    item.Item2[0] * 50625 + item.Item2[1] * 3375 + item.Item2[2] * 225 + item.Item2[3] * 15 + item.Item2[4]
                ) )
            .OrderBy(i => i.Item2)
            .ThenBy(i => i.Item3)
            .Select(i => i.Item1)
            .ToList());
    
    public override object Part2_() =>
        new Func<List<long>, long>(bids => Enumerable.Range(0, bids.Count).Sum(i => (i + 1) * bids[i])
        )(File.ReadAllLines(_file)
            .Select(line => ( long.Parse(line[5..]),
                line[..5].Select(c => c == 'A' ? 14 : c == 'K' ? 13 :  c == 'Q' ? 12 : c == 'J' ? 1 : c == 'T' ? 10 : c - 48).ToArray() ) )
            .Select(item =>
                (item.Item1, 
                    new Func<List<IGrouping<int, int>>, int, int>((cards, numNonJ) =>
                        numNonJ == 0 || cards.Count == 1 ? 6 :
                        cards.Count == 2 ?
                            (cards.First().Count() == (numNonJ - 1) || cards.Last().Count() == (numNonJ - 1)  ? 5 : 4) :
                        cards.Count == 3 ?
                            (cards.ToList()[0].Count() == (numNonJ - 2)  || cards.ToList()[1].Count() == (numNonJ - 2) || cards.ToList()[2].Count() == (numNonJ - 2) ? 3 : 2) :
                        cards.Count == 4 ? 1 : 0
                    )(new List<IGrouping<int, int>>(item.Item2.Where(i => i != 1).GroupBy(i => i)), item.Item2.Count(i => i != 1)),
                    item.Item2[0] * 50625 + item.Item2[1] * 3375 + item.Item2[2] * 225 + item.Item2[3] * 15 + item.Item2[4]
                ) )
            .OrderBy(i => i.Item2)
            .ThenBy(i => i.Item3)
            .Select(i => i.Item1)
            .ToList());

    public override object Part1()
    {
        var hands = new List<Hand>();

        foreach(var line in this.GetLines())
        {
            var cards = line[..5];
            var bid = long.Parse(line[5..]);
            hands.Add(new Hand(cards, bid));
        }

        hands.Sort();

        long score = 0;
        for(int i = 1; i <= hands.Count; i++)
            score += (hands[i - 1].Bid * i);

        return score;
    }

    public override object Part2()
    {
        var hands = new List<Hand>();

        foreach(var line in this.GetLines())
        {
            var cards = line[..5];
            var bid = long.Parse(line[5..]);
            hands.Add(new Hand(cards, bid, true));
        }

        hands.Sort();

        long score = 0;
        for(int i = 1; i <= hands.Count; i++)
            score += (hands[i - 1].Bid * i);

        return score;
    }
}

class Hand : IComparable
{
    public List<int> Values{get;}

    public int TypeScore{get;}

    public long Bid{get;}

    public Hand(string hand, long bid, bool jWild = false)
    {
        this.Bid = bid;
        this.Values = new List<int>();
        foreach(var c in hand)
        {
            if(c == 'A') this.Values.Add(14);
            else if(c == 'K') this.Values.Add(13);
            else if(c == 'Q') this.Values.Add(12);
            else if(c == 'J')  this.Values.Add(jWild ? 1 : 11);
            else if(c == 'T') this.Values.Add(10);
            else this.Values.Add(c - 48);
        }
        
        if(hand.Contains('J'))
            ;

        var nonJoker = this.Values.Where(i => i != 1).ToList();
        var nonJokerUnique = new HashSet<int>(nonJoker);

        var tot = nonJoker.Count;
        if(tot == 0)
            this.TypeScore = 6;
        else if(nonJokerUnique.Count == 1)
            this.TypeScore = 6;
        else if(nonJokerUnique.Count == 2)
        {
            var num1 = nonJokerUnique.First();
            var num2 = nonJokerUnique.Last();
            var c1 = this.Values.Count(i => i == num1);
            var c2 = this.Values.Count(i => i == num2);
            if(c1 == (tot-1) || c2 == (tot-1))
                this.TypeScore = 5;
            else
                this.TypeScore = 4;
        } 
        else if(nonJokerUnique.Count == 3)
        {
            var list = nonJokerUnique.ToList();
            var num1 = list[0];
            var num2 = list[1];
            var num3 = list[2];
            var c1 = this.Values.Count(i => i == num1);
            var c2 = this.Values.Count(i => i == num2);
            var c3 = this.Values.Count(i => i == num3);
            if(c1 == (tot-2) || c2 == (tot-2) || c3 == (tot-2))
                this.TypeScore = 3;
            else
                this.TypeScore = 2;
        }
        else if(nonJokerUnique.Count == 4)
            this.TypeScore = 1;
        else
            this.TypeScore = 0;
    }
    public int CompareTo(object obj)
    {
        var other = obj as Hand;

        if(this.TypeScore > other.TypeScore)
            return 1;
        if(this.TypeScore < other.TypeScore)
            return -1;
        
        for(int i = 0; i < this.Values.Count; i++)
        {
            if(this.Values[i] > other.Values[i])
                return 1;
            if(other.Values[i] > this.Values[i])
                return -1;
        }
        return 0;
    }
}
