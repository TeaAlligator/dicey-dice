using System;
using Assets.Code.Models.Player.LogIn;

namespace Assets.Code.Messaging.Messages.Menu
{
    class CreateAccountSelectedMessage : IMessage
    {
        public Action<SignInCredentials> OnConfirmed;
        public Action OnCancelled;
    }
}
