using System;

namespace Assets.Code.Models.Pooling
{
    class ParticleLoopToken
    {
        public Action<ActiveParticleLoop> Replace;
        public Action End;
    }
}
