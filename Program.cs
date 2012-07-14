using System;

class Program
{
    static Random rnd = new Random(100);

    static void Main(string[] args)
    {
        var primes = Eratosthenes(40000);
        for (var i = 3; i < 40000; i += 2)
            if (SolovayStrassen(i) != primes[i])
                Console.WriteLine(primes[i] ? "ERROR {0}" : "Mistake {0}", i);
    }

    static bool[] Eratosthenes(int n)
    {
        var ret = new bool[n];
        ret[2] = true;
        for (var i = 3; i < n; i += 2) ret[i] = true;

        var u = (int)Math.Floor(Math.Sqrt(n)) + 1;
        for (var i = 3; i < u; i += 2)
            for (var j = i * i; j < n; j += i)
                ret[j] = false;

        return ret;
    }

    static bool SolovayStrassen(int n)
    {
        if (n == 2) return true;
        else if (n < 2 || (n & 1) == 0) return false;

        var a = rnd.Next(1, n);

        if (Gcd(a, n) != 1) return false;
        else return PowMod(a, (n - 1) / 2, n) == Jacobi(a, n);
    }

    static int Gcd(int a, int b)
    {
        if (a <= 0 || b <= 0)
            throw new ArgumentOutOfRangeException();
        else if (a > b) goto StartB;

    StartA:
        // b >= a > 0
        b %= a;
        if (b == 0) return a;

    StartB:
        // a > b > 0
        a %= b;
        if (a == 0) return b;
        goto StartA;
    }

    static int Jacobi(int a, int n)
    {
        var flag = false;
        var N = n;

    Start:
        if (a < 2) return flag ? N - a : a;
        if ((a & 1) == 0)
        {
            switch (n % 8)
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

    static int PowMod(int @base, int pow, int mod)
    {
        if (@base < 0 || pow < 0 || mod <= 0 || mod > 46340)
            throw new ArgumentOutOfRangeException();
        @base %= mod;

        var ret = 1;
        while (pow != 0)
        {
            if ((pow & 1) == 1)
                ret = (ret * @base) % mod;
            @base = (@base * @base) % mod;
            pow >>= 1;
        }
        return ret;
    }
}
