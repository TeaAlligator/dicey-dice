using System;
using Assets.Code.UnityBehaviours.Pooling;
using UnityEngine;

namespace Assets.Code.Models.Pooling
{
    class PooledAudioRequest
    {
        public AudioClip Sound;
        public float Volume;
        public Vector3 Target;
        public bool IsSpatial;
        public bool IsMusic;

        public Action<PooledAudioSource> OnFinished;
    }
}
