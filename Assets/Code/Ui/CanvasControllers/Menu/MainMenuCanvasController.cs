using System;
using Assets.Code.DataPipeline;
using Assets.Code.Logic.Player;
using Assets.Code.Messaging;
using Assets.Code.Messaging.Messages;
using Assets.Code.Messaging.Messages.Menu;
using Assets.Code.Utilities;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Code.Ui.CanvasControllers.Menu
{
    class MainMenuCanvasController : BaseCanvasController
    {
        /* REFERENCES */
        private readonly Messager _messager;
        private readonly UserAccountManager _user;

        /* PROPERTIES */
        private readonly Text _welcomeText;
        private readonly Button _hostGameButton;
        private readonly Button _joinGameButton;
        private readonly Button _exitGameButton;

        /* SUBSCRIPTIONS */
        private readonly MessagingToken _onMainMenuSelected;

        public MainMenuCanvasController(IoCResolver resolver, Canvas canvasView) : base(resolver, canvasView)
        {
            // resolve
            resolver.Resolve(out _messager);
            resolver.Resolve(out _user);

            ResolveElement(out _welcomeText, "welcome_text");
            ResolveElement(out _hostGameButton, "host_game_button");
            ResolveElement(out _joinGameButton, "join_game_button");
            ResolveElement(out _exitGameButton, "exit_game_button");

            // initialize
            ShowCanvas = false;

            _hostGameButton.onClick.AddListener(() =>
            {
                _messager.Publish(new HostGameClickedMessage());
            });

            _exitGameButton.onClick.AddListener(() =>
            {
                _messager.Publish(new ExitMessage());
            });

            // subscribe
            _onMainMenuSelected = _messager.Subscribe<MainMenuSelectedMessage>(message =>
            {
                ShowCanvas = true;
                _welcomeText.text = String.Format(LanguageStrings.MainMenuWelcome, _user.Username);
            });
        }

        public override void TearDown()
        {
            _hostGameButton.onClick.RemoveAllListeners();
            _joinGameButton.onClick.RemoveAllListeners();
            _exitGameButton.onClick.RemoveAllListeners();

            base.TearDown();
        }
    }
}
