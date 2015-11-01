using System.Collections.Generic;
using Assets.Code.Logic.Logging;
using UnityEngine;

namespace Assets.Code.DataPipeline.Providers
{
    class MaterialProvider : IResolvableItem
    {
        private readonly Logger _logger;
        private readonly Dictionary<string, Material> _materials;

        public MaterialProvider(Logger logger)
        {
            _logger = logger;
            _materials = new Dictionary<string, Material>();
        }

        public void AddMaterial(Material material)
        {
            if (material == null)
            {
                _logger.Log("WARNING! null material detected");
                return;
            }

            _materials.Add(material.name, material);
        }

        public Material GetMaterial(string name)
        {
            if (!_materials.ContainsKey(name))
            {
                _logger.Log("WARNING! material " + name + " does not exist");
                return null;
            }

            return _materials[name];
        }
    }
}
