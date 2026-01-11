namespace AccountService.Domain.Constants
{
    public static class RedisCacheNames
    {
        public const string GetAccountsByClientId = "accounts:client:{0}";
    }
}