namespace Ploc.Ploud.Library
{
    public static class ICryptoProviderFactory
    {
        public static ICryptoProvider CreateProvider()
        {
            return new AesCryptoProvider();
        }
    }
}
