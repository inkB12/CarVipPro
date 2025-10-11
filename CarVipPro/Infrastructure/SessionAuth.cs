namespace CarVipPro.APrenstationLayer.Infrastructure
{
    public static class SessionKeys
    {
        public const string UserId = "Auth.UserId";
        public const string Email = "Auth.Email";
        public const string Name = "Auth.Name";
        public const string Role = "Auth.Role";
    }

    public static class SessionAuthExtensions
    {
        public static bool IsLoggedIn(this ISession s) =>
            s.GetInt32(SessionKeys.UserId).HasValue;

        public static void SignIn(this ISession s, int id, string email, string name, string role)
        {
            s.SetInt32(SessionKeys.UserId, id);
            s.SetString(SessionKeys.Email, email ?? "");
            s.SetString(SessionKeys.Name, string.IsNullOrWhiteSpace(name) ? (email ?? "") : name);
            s.SetString(SessionKeys.Role, string.IsNullOrWhiteSpace(role) ? "Staff" : role);
        }

        public static void SignOut(this ISession s) => s.Clear();
    }
}
