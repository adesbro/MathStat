using System.Security.Cryptography;

namespace MathStat
{
    public class CryptoRandom : Random<RNGCryptoServiceProvider>
    {
        private readonly CspParameters _cspParams;

        public CryptoRandom()
        {            
        }

        public CryptoRandom(CspParameters cspParams)
        {
            _cspParams = cspParams;
        }

        protected override RNGCryptoServiceProvider CreateRandomNumberGenerator()
        {
            return new RNGCryptoServiceProvider(_cspParams);
        }
    }
}