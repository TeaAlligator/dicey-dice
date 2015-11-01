using Assets.Code.DataPipeline;
using Assets.Code.Messaging;
using Assets.Code.Messaging.Messages;
using Assets.Code.Messaging.Messages.Menu;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Code.Ui.CanvasControllers
{
    class MenuCanvasController : BaseCanvasController
    {
        /* REFERENCES */
        private readonly Messager _messager;

        /* PROPERTIES */
        private readonly Button _hostGameButton;
        private readonly Button _joinGameButton;
        private readonly Button _exitGameButton;

        public MenuCanvasController(IoCResolver resolver, Canvas canvasView) : base(resolver, canvasView)
        {
            // resolve
            resolver.Resolve(out _messager);

            ResolveElement(out _hostGameButton, "host_game_button");
            ResolveElement(out _joinGameButton, "join_game_button");
            ResolveElement(out _exitGameButton, "exit_game_button");

            // initialize
            _hostGameButton.onClick.AddListener(() =>
            {
                _messager.Publish(new HostGameClickedMessage());
            });

            _exitGameButton.onClick.AddListener(() =>
            {
                _messager.Publish(new ExitMessage());
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
