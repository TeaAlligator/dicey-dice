using Assets.Code.DataPipeline;
using Assets.Code.Messaging;
using Assets.Code.Messaging.Messages;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Code.Ui.CanvasControllers
{
    class DialoguePopUpCanvasController : BaseCanvasController
    {
        /* REFERENCES */
        private readonly Messager _messager;

        private readonly Text _dialogueText;
        private readonly Text _cancelText;
        private readonly Text _confirmText;
        private readonly Text _recognitionText;
        private readonly Button _cancelButton;
        private readonly Button _confirmButton;
        private readonly Button _recognitionButton;

        /* PROPERTIES */
        private ShowPopUpDialogueMessage _currentMessage;

        /* TOKENS */
        private readonly MessagingToken _onShowConfirmationDialogue;

        public DialoguePopUpCanvasController(IoCResolver resolver, Canvas canvasView) : base(resolver, canvasView)
        {
            // resolve references
            resolver.Resolve(out _messager);

            ResolveElement(out _dialogueText, "dialogue_text");
            ResolveElement(out _cancelText, "cancel_button/cancel_text");
            ResolveElement(out _confirmText, "confirm_button/confirm_text");
            ResolveElement(out _recognitionText, "recognition_button/recognition_text");
            ResolveElement(out _cancelButton, "cancel_button");
            ResolveElement(out _confirmButton, "confirm_button");
            ResolveElement(out _recognitionButton, "recognition_button");

            _cancelButton.onClick.AddListener(OnCancelButtonClicked);
            _confirmButton.onClick.AddListener(OnConfirmButtonClicked);
            _recognitionButton.onClick.AddListener(OnConfirmButtonClicked);

            // initialize
            _canvasView.gameObject.SetActive(false);

            _cancelButton.onClick.AddListener(OnCancelButtonClicked);
            _confirmButton.onClick.AddListener(OnConfirmButtonClicked);
            _recognitionButton.onClick.AddListener(OnConfirmButtonClicked);

            // subscribe
            _onShowConfirmationDialogue = _messager.Subscribe<ShowPopUpDialogueMessage>(message =>
            {
                ShowCanvas = true;

                _dialogueText.text = message.MainDialogue;
                _cancelText.text = message.CancelDialogue ?? "no";
                _confirmText.text = message.ConfirmDialogue ?? "yes";
                _recognitionText.text = message.ConfirmDialogue ?? "ok";

                _cancelButton.gameObject.SetActive(message.AllowCancel);
                _confirmButton.gameObject.SetActive(message.AllowCancel);
                _recognitionButton.gameObject.SetActive(!message.AllowCancel);

                _currentMessage = message;
            });
        }

        private void OnCancelButtonClicked()
        {
            _canvasView.gameObject.SetActive(false);

            if (_currentMessage.OnCancelled != null)
                _currentMessage.OnCancelled();
        }

        private void OnConfirmButtonClicked()
        {
            _canvasView.gameObject.SetActive(false);

            if (_currentMessage.OnConfirmed != null)
                _currentMessage.OnConfirmed();
        }

        public override void TearDown()
        {
            _messager.CancelSubscription(_onShowConfirmationDialogue);

            _cancelButton.onClick.RemoveAllListeners();
            _confirmButton.onClick.RemoveAllListeners();
            _recognitionButton.onClick.RemoveAllListeners();

            _cancelButton.gameObject.SetActive(true);
            _confirmButton.gameObject.SetActive(true);
            _recognitionButton.gameObject.SetActive(true);

            base.TearDown();
        }
    }
}
