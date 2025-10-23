namespace Luxprop.Services
{
    // Test commit to fix login behavior

    public class AuthService
    {
        public bool IsAuthenticated { get; private set; }
        public string? UserEmail { get; private set; }

        public bool Login(string email, string password)
        {
            // Simple hardcoded authentication - in a real app, you'd check against a database
            if (email == "admin@example.com" && password == "password")
            {
                IsAuthenticated = true;
                UserEmail = email;
                return true;
            }
            return false;
        }

        public void Logout()
        {
            IsAuthenticated = false;
            UserEmail = null;
        }
    }
}
