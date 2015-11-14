using Assets.Code.DataPipeline;
using Assets.Code.DataPipeline.Providers;
using Assets.Code.Messaging;
using Assets.Code.Messaging.Messages.Menu;
using Assets.Code.Ui;
using Assets.Code.Ui.CanvasControllers;
using Assets.Code.Ui.CanvasControllers.Menu;

namespace Assets.Code.States
{
    class MenuState : BaseState
    {
        /* REFERENCES */

        /* PROPERTIES */
        private readonly Messager _messager;
        private readonly CanvasProvider _canvasProvider;
        private readonly UiManager _uiManager;

        /* TOKENS */
        private MessagingToken _onHostGameClicked;

        public MenuState(IoCResolver resolver) : base(resolver)
        {
            resolver.Resolve(out _messager);
            resolver.Resolve(out _canvasProvider);
            resolver.Resolve(out _uiManager);
        }

        public override void Initialize()
        {
            // initialize
            _uiManager.RegisterUi(new SignInCanvasController(_resolver, _canvasProvider.GetCanvas("sign_in_canvas")));
            _uiManager.RegisterUi(new CreateAccountCanvasController(_resolver, _canvasProvider.GetCanvas("create_account_canvas")));
            _uiManager.RegisterUi(new MainMenuCanvasController(_resolver, _canvasProvider.GetCanvas("main_menu_canvas")));
            _uiManager.RegisterUi(new DialoguePopUpCanvasController(_resolver, _canvasProvider.GetCanvas("pop_up_dialogue_canvas")));

            _onHostGameClicked = _messager.Subscribe<HostGameClickedMessage>(message =>
            {
                SwitchState(new PlayState(_resolver));
            });
        }

        public override void Update() {}

        public override void HandleInput() {}

        public override void TearDown()
        {
            _messager.CancelSubscription(_onHostGameClicked);
        }
    }
}
