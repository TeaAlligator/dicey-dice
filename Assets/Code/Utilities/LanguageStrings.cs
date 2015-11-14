
namespace Assets.Code.Utilities
{
    static class LanguageStrings
    {
        /* ACCOUNT CREATION AND SIGN IN */
        public const string AccountCreationSuccess = "account created successfully!";
        public const string SignInSuccess = "signed in successfully!";
        public const string PasswordMismatchError = "your passwords do not match!";

        /// <summary>
        /// [0]: min password length
        /// </summary>
        public const string InsufficientPasswordLength = "your password must be at least {0} characters!";
        public const string InvalidUserLoginDetails = "invalid credentials!";

        /* MAIN MENU */
        /// <summary>
        /// [0]: username
        /// </summary>
        public const string MainMenuWelcome = "welcome {0}!";
    }
}
