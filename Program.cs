using System;
using System.Numerics;

class Program
{
    static Random rnd = new Random();

    static void Main(string[] args)
    {
        const int mersenneUpper = 1000;
        const int testIteration = 3;

        // from http://en.wikipedia.org/wiki/Mersenne_prime
        var mersenneP = new[] { 2, 3, 5, 7, 13, 17, 19, 31, 61, 89, 107, 127,
            521, 607, 1279, 2203, 2281, 3217, 4253, 4423, 9689, 9941, 11213,
            19937, 21701, 23209, 44497, 86243, 110503, 132049, 216091, 756839,
            859433, 1257787, 1398269, 2976221, 3021377, 6972593, 13466917,
            20996011, 24036583, 25964951, 30402457, 32582657, 37156667,
            42643801, 43112609 };

        for (var i = 0; i < mersenneUpper; i++)
        //foreach (var i in mersenneP)
        {
            var n = BigInteger.Pow(2, i) - 1;

            var start = DateTime.Now;
            var j = 0;
            for (; j < testIteration; j++)
                if (!SolovayStrassen(n)) break;
            var end = DateTime.Now;

            Console.WriteLine("{0}{1} {2} after {3}{4} iterations by {5}",
                new string(' ', 8 - i.ToString().Length),
                i.ToString(),
                j == testIteration ? "Probably PRIME" : "     COMPOSITE",
                new string(' ', testIteration.ToString().Length - j.ToString().Length),
                j.ToString(),
                end - start);
        }
    }

    static bool SolovayStrassen(BigInteger n)
    {
        if (n == 2) return true;
        else if (n < 2 || n.IsEven) return false;

        var a = BigRandom(n);

        if (BigInteger.GreatestCommonDivisor(a, n) != 1) return false;
        else return BigInteger.ModPow(a, (n - 1) / 2, n) == Jacobi(a, n);
    }

    static BigInteger BigRandom(BigInteger n)
    {
        var array = n.ToByteArray();
        int last;

        byte temp;
        while ((temp = array[last = array.Length - 1]) == 0)
            Array.Resize<byte>(ref array, last);
        rnd.NextBytes(array);
        array[last] = (byte)rnd.Next(last == 0 ? 2 : 0, temp);

        return new BigInteger(array);
    }

    static BigInteger Jacobi(BigInteger a, BigInteger n)
    {
        var flag = false;
        var N = n;

    Start:
        if (a < 2) return flag ? N - a : a;
        if (a.IsEven)
        {
            switch ((int)(n % 8))
            {
                case 1:
                case 7: a /= 2; goto Start;
                case 3:
                case 5: flag = !flag; a /= 2; goto Start;
            }
        }
        else if (a % 4 == 3 && n % 4 == 3) flag = !flag;
        var t = a;
        a = n % a;
        n = t;
        goto Start;
    }
}
