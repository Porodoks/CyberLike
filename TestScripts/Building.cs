using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Assets.Test
{
    public class Building
    {
        public Building(int floorsCount)
        {
            CreateBasement();
            CreateFloors(floorsCount);
            CreateLedders();
        }
        public BasementData Basement;
        public List<FloorData> Floors = new(2);
        public List<SpanData> Spans = new(2);
        public List<LedderData> Ledders = new(2);
        private void CreateBasement()
        {
            int randomNumber = UnityEngine.Random.Range(0, Presets.Buildings.Basements.Count);
            Basement = Presets.Buildings.Basements[randomNumber].GetData();
            SpanData span = new SpanData(Basement.View.MainTilemapData.Width + 2);
            span.TransformVector = new Vector3Int(1, -(Basement.View.MainTilemapData.Height));
            Spans.Add(span);
        }
        private void CreateFloors(int count)
        {
            int currentGenHeight = Basement.View.MainTilemapData.Height + 1;
            for (int i = 0; i < count; i++)
            {
                int randomNumer = UnityEngine.Random.Range(0, Basement.View.AssociatedFloors.Count);
                FloorData floorData = Basement.View.AssociatedFloors[randomNumer].GetData();
                floorData.TransformVector = new Vector3Int(0, -currentGenHeight);
                floorData.TransformWindows();
                Floors.Add(floorData);
                SpanData span = new SpanData(floorData.View.MainTilemapData.Width + 2);
                currentGenHeight += floorData.View.MainTilemapData.Height;
                span.TransformVector = new Vector3Int(1, -currentGenHeight);
                Spans.Add(span);
                currentGenHeight++;
                //Debug.Log($"Created new {nameof(FloorData)} with tilemap id: {floorData.View.MainTilemapData.Id}");
            }
        }
        private void CreateLedders()
        {
            for (int i = 0; i < Spans.Count; i++)
            {
                if (i == 0)
                    continue;

                LedderData ledderData = null;
                bool genEnd = false;
                while (!genEnd)
                {
                    int height = Mathf.Abs(Spans[i].TransformVector.y) - Mathf.Abs(Spans[i - 1].TransformVector.y);
                    ledderData = new LedderData(height);
                    int randomInt = UnityEngine.Random.Range(0, Spans[i].Width - 2);
                    ledderData.TransformVector = new Vector3Int(-randomInt, Spans[i].TransformVector.y + height - 1);
                    ledderData.GetTiles(out var ledderPositions, out var _);
                    bool breaked = false;
                    foreach (var window in Floors[i-1].Windows)
                    {
                        if (breaked)
                            break;

                        window.GetTiles(out var windowPositions, out _);
                        foreach (var position in ledderPositions)
                        {
                            Vector3Int intersectedVector = windowPositions.FirstOrDefault(v => v.x == position.x);
                            if (intersectedVector != Vector3Int.zero)
                            {
                                breaked = true;
                                break;
                            }
                        }
                    }
                    if (!breaked)
                        genEnd = true;
                }
                Ledders.Add(ledderData);
            }
        }
        public void TransformBuilding(Vector3Int transformVector)
        {
            Basement.TransformVector += transformVector;
            foreach (var floor in Floors)
            {
                floor.TransformVector += transformVector;
                floor.TransformWindows(transformVector);
            }
            foreach (var span in Spans)
            {
                span.TransformVector += transformVector;
            }
            foreach (var ledder in Ledders)
            {
                ledder.TransformVector += transformVector;
            }
        }

        public void GetAllMainTiles(out Vector3Int[] positions, out TileBase[] tiles)
        {
            List<Vector3Int> allPositions = new List<Vector3Int>(256);
            List<TileBase> allTiles = new List<TileBase>(256);

            List<Vector3Int> tempPositions = new List<Vector3Int>(256);
            List<TileBase> tempTiles = new List<TileBase>(256);

            Basement.GetTiles(out tempPositions, out tempTiles);
            allPositions.AddRange(tempPositions);
            allTiles.AddRange(tempTiles);

            foreach (var floor in Floors)
            {
                floor.GetTiles(out tempPositions, out tempTiles);
                allPositions.AddRange(tempPositions);
                allTiles.AddRange(tempTiles);
            }

            positions = allPositions.ToArray();
            tiles = allTiles.ToArray();
        }

        public void GetAllCollisionsTiles(out Vector3Int[] positions, out TileBase[] tiles)
        {
            List<Vector3Int> allPositions = new List<Vector3Int>(256);
            List<TileBase> allTiles = new List<TileBase>(256);

            List<Vector3Int> tempPositions = new List<Vector3Int>(256);
            List<TileBase> tempTiles = new List<TileBase>(256);

            foreach (var span in Spans)
            {
                span.GetTiles(out tempPositions, out tempTiles);
                allPositions.AddRange(tempPositions);
                allTiles.AddRange(tempTiles);
            }

            positions = allPositions.ToArray();
            tiles = allTiles.ToArray();
        }

        public void GetAllLeddersTiles(out Vector3Int[] positions, out TileBase[] tiles)
        {
            List<Vector3Int> allPositions = new List<Vector3Int>(256);
            List<TileBase> allTiles = new List<TileBase>(256);

            List<Vector3Int> tempPositions = new List<Vector3Int>(256);
            List<TileBase> tempTiles = new List<TileBase>(256);

            foreach (var ledder in Ledders)
            {
                ledder.GetTiles(out tempPositions, out tempTiles);
                allPositions.AddRange(tempPositions);
                allTiles.AddRange(tempTiles);
            }

            positions = allPositions.ToArray();
            tiles = allTiles.ToArray();
        }

        public void GetAllEnvironmentTiles(out Vector3Int[] positions, out TileBase[] tiles)
        {
            List<Vector3Int> allPositions = new List<Vector3Int>(256);
            List<TileBase> allTiles = new List<TileBase>(256);

            List<Vector3Int> tempPositions = new List<Vector3Int>(256);
            List<TileBase> tempTiles = new List<TileBase>(256);

            foreach (var floor in Floors)
            {
                foreach (var window in floor.Windows)
                {
                    window.GetTiles(out tempPositions, out tempTiles);
                    allPositions.AddRange(tempPositions);
                    allTiles.AddRange(tempTiles);
                }
            }

            positions = allPositions.ToArray();
            tiles = allTiles.ToArray();
        }
    }
}
