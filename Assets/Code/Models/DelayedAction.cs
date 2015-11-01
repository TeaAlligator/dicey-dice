using System;

namespace Assets.Code.Models
{
    class DelayedAction
    {
        public float RemainingTime { get; set; }
        public Action Payload { get; set; }
        public bool IgnorePaused { get; set; }
    }
}
