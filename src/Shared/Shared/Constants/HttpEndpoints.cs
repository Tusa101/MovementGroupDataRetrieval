namespace Shared.Constants;

public static class HttpEndpoints
{
    public static class Account
    {
        public const string RegisterUser = "register-user";
        public const string LoginUser = "login-user";
        public const string RefreshToken = "refresh-token";
        public const string RevokeTokens = "revoke-tokens";
    }
    public static class StoredData
    {
        public const string GetAll = "get-all";
    }
}
