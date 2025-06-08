using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Assets.Test
{
    public class LedderData
    {
        public LedderData(int height)
        {
            CreateLedder(height);
        }

        private List<Vector3Int> tilesPosition = new List<Vector3Int>();
        private List<TileBase> tiles = new List<TileBase>();
        public Vector3Int TransformVector;

        public void CreateLedder(int height)
        {
            Vector3Int[] tempPositionsArray = new Vector3Int[height];
            TileBase[] tempTilesArray = new TileBase[height];

            for (int i = 0; i < height; i++)
            {
                tempPositionsArray[i] = new Vector3Int(0, i);
            }

            Array.Fill(tempTilesArray, Presets.Buildings.LedderRegularTile);
            tempTilesArray[0] = Presets.Buildings.LedderLastTile;

            tilesPosition = tempPositionsArray.ToList();
            tiles = tempTilesArray.ToList();
        }
        public void GetTiles(out List<Vector3Int> positions, out List<TileBase> tiles)
        {
            positions = TilemapHelper.TransformPositions(tilesPosition, TransformVector);
            tiles = this.tiles;
        }
    }
}
