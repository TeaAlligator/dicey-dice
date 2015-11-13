
namespace Assets.Code.Models.Player.LogIn
{
    enum SignInError { Unknown = 0, None, UserNameInvalid, WrongPassword }

    class SignInAttemptResult
    {
        public SignInError Error { get; set; }

        public UserAccountData Data { get; set; }
    }
}
