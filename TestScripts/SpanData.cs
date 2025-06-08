using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Assets.Test
{
    public class SpanData
    {
        public SpanData(int width)
        {
            CreateSpan(width);
            Width = width;
        }
        private List<Vector3Int> tilesPosition = new List<Vector3Int>();
        private List<TileBase> tiles = new List<TileBase>();
        public Vector3Int TransformVector;
        public readonly int Width;

        private void CreateSpan(int width)
        {
            Vector3Int[] positions = new Vector3Int[width];
            TileBase[] tiles = new TileBase[width];

            for (int i = 0; i < width; i++)
            {
                positions[i] = new Vector3Int(i, 0);
            }
            Array.Fill(tiles, Presets.Buildings.SpanCenterTile);
            tiles[0] = Presets.Buildings.SpanLeftTile;
            tiles[^1] = Presets.Buildings.SpanRightTile;

            tilesPosition.AddRange(positions);
            this.tiles.AddRange(tiles);
        }
        public void GetTiles(out List<Vector3Int> positions, out List<TileBase> tiles)
        {
            positions = TilemapHelper.TransformPositions(tilesPosition, TransformVector);
            tiles = this.tiles;
        }
    }
}
