﻿using System.Collections.Generic;
using System.Linq;
using Assets.Code.Extensions;
using Assets.Code.Logic.Logging;
using UnityEngine;

namespace Assets.Code.DataPipeline.Providers
{
    class SpriteProvider : IResolvableItem
    {
        private readonly Logger _logger;
        private readonly Dictionary<string, Sprite> _sprites;

        public SpriteProvider(Logger logger)
        {
            _logger = logger;
            _sprites = new Dictionary<string, Sprite>();
        }

        public void AddSprite(Sprite sprite)
        {
            if (sprite == null)
            {
                _logger.Log("WARNING! null sprite detected", true);
                return;
            }

            _sprites.Add(sprite.name, sprite);
        }

        public Sprite GetSprite(string name)
        {
            if (!_sprites.ContainsKey(name))
            {
                _logger.Log("WARNING! sprite " + name + " does not exist", true);
                return null;
            }

            return _sprites[name];
        }

        public List<Sprite> GetSpritesOfType(string prefix)
        {
            var lowerCasePrefix = prefix.ToFormalString().ToLower();

            return _sprites.Where (sprite => sprite.Key.ToLower().StartsWith(lowerCasePrefix))
                           .Select(sprite => sprite.Value)
                           .ToList();
        }

        public Sprite GetRandomSpriteOfType(string prefix)
        {
            var lowerCasePrefix = prefix.ToFormalString().ToLower();

            return _sprites.Where(sprite => sprite.Key.ToLower().StartsWith(lowerCasePrefix))
                           .Select(sprite => sprite.Value)
                           .ToList()
                           .GetRandomItem();
        }
    }
}
