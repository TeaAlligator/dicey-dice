using System;

namespace Assets.Code.Messaging.Messages
{
    public class ShowPopUpDialogueMessage : IMessage
    {
        public string MainDialogue;
        public string ConfirmDialogue;
        public string CancelDialogue;
        public bool AllowCancel;

        public Action OnConfirmed;
        public Action OnCancelled;
    }
}
