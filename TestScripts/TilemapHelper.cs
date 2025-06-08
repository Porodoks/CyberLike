using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Assets.Test
{
    public static class TilemapHelper
    {
        public static BoundsInt GetTilemapBounds(Tilemap tilemap)
        {
            tilemap.CompressBounds();
            return tilemap.cellBounds;
        }
        public static void GetTiles(Tilemap tilemap, out Vector3Int[] tilesPosition, out TileBase[] tiles)
        {
            BoundsInt tilemapBounds = GetTilemapBounds(tilemap);
            GetTiles(tilemap, out tilesPosition, out tiles, new Vector3Int(tilemapBounds.xMin, tilemapBounds.yMin));
        }
        public static void GetTiles(Tilemap tilemap, out Vector3Int[] tilesPosition, out TileBase[] tiles, Vector3Int transformVector)
        {
            List<Vector3Int> tilesPositionTempList = new List<Vector3Int>(32);
            List<TileBase> tilesTempList = new List<TileBase>(32);

            BoundsInt tilemapBounds = GetTilemapBounds(tilemap);
            for (int i = tilemapBounds.xMin; i < tilemapBounds.xMax; i++)
            {
                for (int j = tilemapBounds.yMin; j < tilemapBounds.yMax; j++)
                {
                    Vector3Int tempTilePosition = new Vector3Int(i, j);
                    if (tilemap.HasTile(tempTilePosition))
                    {
                        tilesPositionTempList.Add(tempTilePosition);
                        tilesTempList.Add(tilemap.GetTile(tempTilePosition));
                    }
                }
            }

            tilesPosition = TransformPositions(tilesPositionTempList.ToArray(), transformVector);
            tiles = tilesTempList.ToArray();
        }
        public static void TransformPositions(ref Vector3Int[] positions, Vector3Int transformVector)
        {
            for (int i = 0; i < positions.Length; i++)
            {
                positions[i] = new Vector3Int(positions[i].x - transformVector.x, positions[i].y - transformVector.y);
            }
        }
        public static void TransformPositions(Vector3Int[] positions, Vector3Int transformVector, out Vector3Int[] transformedPositions)
        {
            transformedPositions = new Vector3Int[positions.Length];
            for (int i = 0; i < positions.Length; i++)
            {
                transformedPositions[i] = new Vector3Int(positions[i].x - transformVector.x, positions[i].y - transformVector.y);
            }
        }
        public static void TransformPositions(ref List<Vector3Int> positions, Vector3Int transformVector)
        {
            for (int i = 0; i < positions.Count; i++)
            {
                positions[i] = new Vector3Int(positions[i].x - transformVector.x, positions[i].y - transformVector.y);
            }
        }
        public static void TransformPositions(List<Vector3Int> positions, Vector3Int transformVector, out List<Vector3Int> transformedPositions)
        {
            transformedPositions = new List<Vector3Int>(positions.Count);
            for (int i = 0; i < positions.Count; i++)
            {
                transformedPositions.Add(new Vector3Int(positions[i].x - transformVector.x, positions[i].y - transformVector.y));
            }
        }
        public static Vector3Int[] TransformPositions(Vector3Int[] positions, Vector3Int transformVector)
        {
            TransformPositions(positions, transformVector, out var transformedPositions);
            return transformedPositions;
        }
        public static List<Vector3Int> TransformPositions(List<Vector3Int> positions, Vector3Int transformVector)
        {
            return TransformPositions(positions.ToArray(), transformVector).ToList();
        }
    }
}
