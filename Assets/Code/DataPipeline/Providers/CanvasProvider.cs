using System.Collections.Generic;
using Assets.Code.Logic.Logging;
using UnityEngine;

namespace Assets.Code.DataPipeline.Providers
{
    class CanvasProvider : IResolvableItem
    {
        private readonly Logger _logger;
        private readonly Dictionary<string, Canvas> _canvases;

        public CanvasProvider(Logger logger)
        {
            _logger = logger;
            _canvases = new Dictionary<string, Canvas>();
        }

        public void AddCanvas(Canvas canvas)
        {
            _canvases.Add(canvas.name, canvas);
        }

        public Canvas GetCanvas(string name)
        {
            if (!_canvases.ContainsKey(name))
            {
                _logger.Log("WARNING! canvas " + name + " does not exist", true);
                return null;
            }

            return _canvases[name];
        }
    }
}
