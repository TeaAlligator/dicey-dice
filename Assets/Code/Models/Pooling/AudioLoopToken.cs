using System;

namespace Assets.Code.Models.Pooling
{
    class AudioLoopToken
    {
        public Func<PooledAudioRequest, AudioLoopToken> Replace;
        public Action End;
    }
}
