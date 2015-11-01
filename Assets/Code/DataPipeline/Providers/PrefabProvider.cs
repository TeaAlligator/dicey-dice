using System.Collections.Generic;
using Assets.Code.Logic.Logging;
using UnityEngine;

namespace Assets.Code.DataPipeline.Providers
{
    class PrefabProvider : IResolvableItem
    {
        private readonly Logger _logger;
        private readonly Dictionary<string, GameObject> _prefabs;

        public PrefabProvider(Logger logger)
        {
            _logger = logger;
            _prefabs = new Dictionary<string, GameObject>();
        }

        public void AddPrefab(GameObject newPrefab)
        {
            _prefabs.Add(newPrefab.name, newPrefab);
        }

        public GameObject GetPrefab(string name)
        {
            if (!_prefabs.ContainsKey(name))
            {
                _logger.Log("WARNING! prefab " + name + " does not exist", true);
                return null;
            }

            return _prefabs[name];
        }
    }
}
