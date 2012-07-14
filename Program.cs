using System;

class Program
{
    static void Main(string[] args)
    {
        for (var i = 0; i < 10000; i++)
        {
            var isPrime = IsPrime(i);
            if (SolovayStrassen(i) != isPrime)
                Console.WriteLine(isPrime ? "ERROR {0}" : "Mistake {0}", i);
        }
    }

    static bool IsPrime(int n)
    {
        if (n == 2) return true;
        else if (n < 2 || (n & 1) == 0) return false;

        var u = (int)Math.Floor(Math.Sqrt(n)) + 1;
        for (var t = 3; t < u; t++)
            if (n % t == 0) return false;
        return true;
    }

    static bool SolovayStrassen(int n)
    {
        if (n == 2) return true;
        else if (n < 2 || (n & 1) == 0) return false;

        var rnd = new Random();
        var a = rnd.Next(1, n);

        if (Gcd(a, n) != 1) return false;
        else return PowMod(a, (n - 1) / 2, n) == ((Jacobi(a, n) % n) + n) % n;
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
        if (a < 0)
            switch (n % 4)
            {
                case 1: return Jacobi(-a, n);
                case 3: return -Jacobi(-a, n);
            }
        if (a < 2) return a;
        if ((a & 1) == 0)
        {
            switch (n % 8)
            {
                case 1:
                case 7: return Jacobi(a / 2, n);
                case 3:
                case 5: return -Jacobi(a / 2, n);
            }
        }
        else if (a % 4 == 3 && n % 4 == 3) return -Jacobi(n % a, a);
        return Jacobi(n % a, a);
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
