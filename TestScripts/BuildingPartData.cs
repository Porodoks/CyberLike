using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Assets.Test
{
    public abstract class BuildingPartData<T> where T : BuildingPartView
    {
        public BuildingPartData(T view)
        {
            if (view == null)
                throw new NullReferenceException($"Переменная {nameof(view)} является null");

            View = view;
        }
        public readonly T View;
        public Vector3Int TransformVector;
        public void GetTiles(out List<Vector3Int> positions, out List<TileBase> tiles)
        {
            positions = TilemapHelper.TransformPositions(View.MainTilemapData.TilesPositions, TransformVector);
            tiles = View.MainTilemapData.Tiles;
        }
    }
}
