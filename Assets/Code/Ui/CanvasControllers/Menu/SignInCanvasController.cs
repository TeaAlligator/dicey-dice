using Assets.Code.DataPipeline;
using Assets.Code.Extensions;
using Assets.Code.Logic.Player;
using Assets.Code.Messaging;
using Assets.Code.Messaging.Messages;
using Assets.Code.Messaging.Messages.Menu;
using Assets.Code.Models.Player.LogIn;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Code.Ui.CanvasControllers.Menu
{
    class SignInCanvasController : BaseCanvasController
    {
        /* PROPERTIES */
        private readonly Messager _messager;
        private readonly UserAccountManager _user;

        private readonly InputField _usernameInputField;
        private readonly InputField _passwordInputField;
        private readonly Button _signInButton;
        private readonly Button _createAccountButton;

        public SignInCanvasController(IoCResolver resolver, Canvas canvasView) : base(resolver, canvasView)
        {
            // resolve
            resolver.Resolve(out _messager);
            resolver.Resolve(out _user);

            ResolveElement(out _usernameInputField, "username_input_field");
            ResolveElement(out _passwordInputField, "password_input_field");
            ResolveElement(out _signInButton, "sign_in_button");
            ResolveElement(out _createAccountButton, "create_account_button");

            // initialize
            _createAccountButton.onClick.AddListener(() =>
            {
                ShowCanvas = false;

                _messager.Publish(new CreateAccountSelectedMessage
                {
                    OnCancelled = () => ShowCanvas = true,
                    OnConfirmed = credentials =>
                    {
                        ShowCanvas = true;

                        _usernameInputField.text = credentials.Username;
                        _passwordInputField.text = credentials.Password;
                    }
                });
            });
            _signInButton.onClick.AddListener(AttemptSignIn);
        }

        private void AttemptSignIn()
        {
            var credentials = new SignInCredentials
            {
                Username = _usernameInputField.text,
                Password = _passwordInputField.text
            };

            var result = _user.AttemptSignIn(credentials);

            if (result == SignInError.None)
            {
                ShowCanvas = false;
                _messager.Publish(new MainMenuSelectedMessage());
            }
            else
            {
                ShowCanvas = false;
                _messager.Publish(new ShowPopUpDialogueMessage
                {
                    AllowCancel = false,
                    MainDialogue = result.ToString().ToFormalString(),
                    OnConfirmed = () => ShowCanvas = true
                });
            }
        }

        public override void TearDown()
        {
            _signInButton.onClick.RemoveAllListeners();
            _createAccountButton.onClick.RemoveAllListeners();

            base.TearDown();
        }
    }
}
