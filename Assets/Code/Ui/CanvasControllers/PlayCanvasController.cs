using Assets.Code.DataPipeline;
using Assets.Code.Messaging;
using Assets.Code.Messaging.Messages;
using Assets.Code.Messaging.Messages.Play;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Code.Ui.CanvasControllers
{
    class PlayCanvasController : BaseCanvasController
    {
        /* REFERENCES */
        private readonly Messager _messager;

        /* PROPERTIES */
        private readonly Button _rollDiceButton;
        private readonly Button _exitGameButton;

        public PlayCanvasController(IoCResolver resolver, Canvas canvasView) : base(resolver, canvasView)
        {
            // resolve
            resolver.Resolve(out _messager);

            ResolveElement(out _rollDiceButton, "roll_dice_button");
            ResolveElement(out _exitGameButton, "exit_game_button");

            // initialize
            _rollDiceButton.onClick.AddListener(() =>
            {
                _messager.Publish(new RollDiceClickedMessage());
            });

            _exitGameButton.onClick.AddListener(() =>
            {
                _messager.Publish(new ExitPlayClickedMessage());
            });
        }

        public override void TearDown()
        {
            _rollDiceButton.onClick.RemoveAllListeners();
            _exitGameButton.onClick.RemoveAllListeners();

            base.TearDown();
        }
    }
}
