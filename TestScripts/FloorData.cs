using System.Collections.Generic;
using UnityEngine;

namespace Assets.Test
{
    public class FloorData : BuildingPartData<FloorView>
    {
        public FloorData(FloorView view) : base(view)
        {
            CreateWindows();
        }

        public List<WindowData> Windows = new List<WindowData>(2);

        private void CreateWindows()
        {
            TilemapData tilemapData = View.AnnotationsTilemapData;
            for (int i = 0; i < tilemapData.TilesPositions.Count; i++)
            {
                if (tilemapData.Tiles[i].name.Equals(Presets.Buildings.A_WindowGenerationStartPoint.name))
                {
                    int randomNumber = UnityEngine.Random.Range(0, Presets.Buildings.Windows.Count);
                    WindowData windowData = Presets.Buildings.Windows[randomNumber].GetData();
                    windowData.TransformVector = -(tilemapData.TilesPositions[i]);
                    Windows.Add(windowData);
                }
            }
        }
        public void TransformWindows()
        {
            foreach (var window in Windows)
            {
                window.TransformVector += TransformVector;
            }
        }

        public void TransformWindows(Vector3Int transformVector)
        {
            foreach (var window in Windows)
            {
                window.TransformVector += transformVector;
            }
        }
    }
}
