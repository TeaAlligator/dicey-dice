using Assets.Code.DataPipeline;
using Assets.Code.Extensions;
using Assets.Code.Logic.Player;
using Assets.Code.Messaging;
using Assets.Code.Messaging.Messages;
using Assets.Code.Messaging.Messages.Menu;
using Assets.Code.Models.Player.LogIn;
using Assets.Code.Models.Player.Registration;
using Assets.Code.Utilities;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Code.Ui.CanvasControllers.Menu
{
    class CreateAccountCanvasController : BaseCanvasController
    {
        /* REFERENCES */
        private readonly Messager _messager;
        private readonly UserAccountManager _user;

        private readonly InputField _usernameInputField;
        private readonly InputField _passwordInputField;
        private readonly InputField _passwordConfirmationInputField;
        private readonly Button _confirm_button;
        private readonly Button _backButton;

        /* PROPERTIES */
        private CreateAccountSelectedMessage _currentAccountCreationSession;

        /* TOKENS */
        private readonly MessagingToken _onCreateAccountSelected;

        public CreateAccountCanvasController(IoCResolver resolver, Canvas canvasView) : base(resolver, canvasView)
        {
            // resolve
            resolver.Resolve(out _messager);
            resolver.Resolve(out _user);

            ResolveElement(out _usernameInputField, "username_input_field");
            ResolveElement(out _passwordInputField, "password_input_field");
            ResolveElement(out _passwordConfirmationInputField, "password_confirmation_input_field");
            ResolveElement(out _confirm_button, "confirm_button");
            ResolveElement(out _backButton, "back_button");

            // initialize
            ShowCanvas = false;

            _usernameInputField.text = "";
            _passwordInputField.text = "";

            // subscribe
            _onCreateAccountSelected = _messager.Subscribe<CreateAccountSelectedMessage>(message =>
            {
                ShowCanvas = true;
                _currentAccountCreationSession = message;

                _usernameInputField.text = "";
                _passwordInputField.text = "";

                _confirm_button.onClick.RemoveAllListeners();
                _backButton.onClick.RemoveAllListeners();

                _confirm_button.onClick.AddListener(RegisterAccount);
                _backButton.onClick.AddListener(() => message.OnCancelled());
            });
        }

        public void RegisterAccount()
        {
            // make sure passwords are the same
            if (_passwordInputField.text != _passwordConfirmationInputField.text)
            {
                ShowCanvas = false;
                _messager.Publish(new ShowPopUpDialogueMessage
                {
                    MainDialogue = LanguageStrings.PasswordMismatchError,
                    AllowCancel = false,
                    OnConfirmed = () => ShowCanvas = true
                });

                return;
            }

            // send out our username and password over to serverland
            var login = new SignInCredentials
            {
                Username = _usernameInputField.text,
                Password = _passwordInputField.text
            };

            // TODO: send the sign in details to the server and get result
            var error = RegistrationAttemptError.None;
            // TODO: for now we just user no error

            if (error == RegistrationAttemptError.None)
            {
                ShowCanvas = false;
                _messager.Publish(new ShowPopUpDialogueMessage
                {
                    MainDialogue = LanguageStrings.AccountCreationSuccess,
                    AllowCancel = false,
                    OnConfirmed = () => _currentAccountCreationSession.OnConfirmed(login)
                });
            }
            else
            {
                ShowCanvas = false;
                _messager.Publish(new ShowPopUpDialogueMessage
                {
                    MainDialogue = error.ToString().ToFormalString(),
                    AllowCancel = false,
                    OnConfirmed = () => ShowCanvas = true
                });
            }
        }

        public override void TearDown()
        {
            _messager.CancelSubscription(_onCreateAccountSelected);

            _confirm_button.onClick.RemoveAllListeners();
            _backButton.onClick.RemoveAllListeners();

            base.TearDown();
        }
    }
}
