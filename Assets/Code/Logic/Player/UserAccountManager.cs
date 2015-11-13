using Assets.Code.DataPipeline;
using Assets.Code.Models.Player;
using Assets.Code.Models.Player.LogIn;

namespace Assets.Code.Logic.Player
{
    class UserAccountManager : IResolvableItem
    {
        private UserAccountData _userAccountData { get; set; }

        public bool IsLoggedIn { get { return _userAccountData != null; } }

        public string Username
        { get { return _userAccountData != null ? _userAccountData.Username : null; } }

        public string Password
        { get { return _userAccountData != null ? _userAccountData.Password : null; } }

        public SignInError AttemptSignIn(SignInCredentials login)
        {
            // TODO: talk to server

            // for now we'll just build data and assume all is dandy
            _userAccountData = new UserAccountData
            {
                Username = login.Username,
                Password = login.Password
            };

            return SignInError.None;
        }
    }
}
