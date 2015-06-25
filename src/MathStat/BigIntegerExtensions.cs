using System.Numerics;

namespace MathStat
{
    public static class BigIntegerExtensions
    {
        // Taken (and converted to C#) from here:
        // https://social.msdn.microsoft.com/Forums/en-US/dedae341-178b-43fd-9d6e-9fcdfcf6fe1b/rsa-key-pair-problem-incl-modular-inverse-etc?forum=vblanguage
        public static BigInteger ModInverse(this BigInteger x, BigInteger m)
        {
            BigInteger x0 = x;
            BigInteger m0 = m;
            BigInteger t0 = 0;
            BigInteger t = 1;

            var q = m0 / x0;
            var r = m0 - q * x0;

            while (r > 0)
            {
                var temp = t0 - q * t;

                if (temp >= 0)
                {
                    temp = temp % m;
                }

                if (temp < 0)
                {
                    temp = m - ((-temp) % m);
                }

                t0 = t;
                t = temp;
                m0 = x0;
                x0 = r;
                q = m0 / x0;
                r = m0 - q * x0;
            }

            return t % m;
        }
    }
}