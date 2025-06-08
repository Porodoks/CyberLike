using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Assets.Test
{
    public struct TilemapData
    {
        public TilemapData(Tilemap tilemap, string id, Vector3Int transformPosition)
        {
            Id = id;
            BoundsInt tilemapBounds = TilemapHelper.GetTilemapBounds(tilemap);
            Width = tilemapBounds.size.x;
            Height = tilemapBounds.size.y;

            if (transformPosition == Vector3Int.zero)
                transformPosition = new Vector3Int(tilemapBounds.xMin, tilemapBounds.yMin);

            TilemapHelper.GetTiles(tilemap, out var positionsTempArray, out var tilesTempArray, transformPosition);
            TilesPositions = positionsTempArray.ToList();
            Tiles = tilesTempArray.ToList();
            IsReady = true;
        }
        public readonly string Id;
        public readonly int Width;
        public readonly int Height;
        public List<Vector3Int> TilesPositions;
        public List<TileBase> Tiles;
        public readonly bool IsReady;
    }
}
