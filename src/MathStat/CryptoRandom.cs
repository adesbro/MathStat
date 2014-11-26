using System.Security.Cryptography;

namespace MathStat
{
    public class CryptoRandom : Random<RNGCryptoServiceProvider>
    {
        public CryptoRandom()
        {            
        }

        public CryptoRandom(CspParameters cspParams)
        {
            RandomNumberGenerator = new RNGCryptoServiceProvider(cspParams);
        }
    }
}