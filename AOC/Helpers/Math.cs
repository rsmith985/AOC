using System.Numerics;

namespace rsmith985.AOC;

public class MyMath
{
    public static long LCM(params long[] nums)
    {
        var rv = (long)1;;
        for(int i = 0; i < nums.Length; i++)
            rv = LCM(rv, nums[i]);
        return rv;
    }
    public static long LCM(long a, long b)
    {
        var c = GCD(a, b);
        return a > b ? (a / c)*b : (b / c)*a;
    }
    public static long GCD(long a, long b)
    {
        while(a != 0 && b != 0)
        {
            if(a > b) a%=b;
            else b%=a;
        }
        return a|b;
    }
}
