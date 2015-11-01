using System.Collections.Generic;
using Assets.Code.DataPipeline;
using Assets.Code.DataPipeline.Providers;
using Assets.Code.Logic.Logging;
using Assets.Code.UnityBehaviours.Pooling;

namespace Assets.Code.Logic.Pooling
{
    class PoolingObjectManager : IResolvableItem
    {
        /* REFERENCES */
        private readonly Logger _logger;
        private readonly PrefabProvider _prefabProvider;

        /* PROPERTIES */
        private readonly Dictionary<string, ObjectPool> _pools;

        public PoolingObjectManager(Logger logger, PrefabProvider prefabProvider)
        {
            _pools = new Dictionary<string, ObjectPool>();

            _logger = logger;
            _prefabProvider = prefabProvider;
        }

        public PoolingBehaviour Instantiate(string prefabName)
        {
            if (!_pools.ContainsKey(prefabName))
                _pools.Add(prefabName, new ObjectPool(_logger, _prefabProvider.GetPrefab(prefabName)));

            return _pools[prefabName].Instantiate();
        }

        public void TearDown()
        {
            foreach(var pool in _pools)
                pool.Value.TearDown();
        }
    }
}
