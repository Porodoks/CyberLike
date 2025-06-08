using System;
using UnityEngine;

namespace Assets.Test
{
    public static class Presets
    {
        static Presets()
        {
            _buildings = Resources.Load<BuildingPresets>("PresetsHolder");
            if (_buildings == null)
                throw new NullReferenceException("Не удалось загрузить держатель пресетов");
        }
        private readonly static BuildingPresets _buildings;
        public static BuildingPresets Buildings => _buildings;
    }
}
