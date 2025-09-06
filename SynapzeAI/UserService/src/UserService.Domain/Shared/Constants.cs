namespace UserService.Domain.Shared
{
    public class Constants
    {
        public const int MAX_LOW_TEXT_LENGTH = 100;
        public const int MAX_HIGH_TEXT_LENGTH = 1000;
        public const int USER_UNIQUE_CODE_LENGTH = 6;
    }

    public class VerificationConstants
    {
        public const int MAX_ATTEMPTS = 5;
        public const int ExpiresMinutes = 5;
    }
}
