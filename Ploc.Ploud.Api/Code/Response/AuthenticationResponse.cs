namespace Ploc.Ploud.Api
{
    public class AuthenticationResponse
    {
        public bool IsAuthenticated { get; set; }

        public string ErrorMessage { get; set; }

        public string HashedMd5Email { get; set; } // If you want to add a security layer.

        public string FolderName { get; set; }

        public string FileName { get; set; }
    }
}
