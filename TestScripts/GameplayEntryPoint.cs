using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Assets.Test
{
    public class GameplayEntryPoint : MonoBehaviour
    {
        [SerializeField] private Tilemap mainTilemap;
        [SerializeField] private Tilemap collisionsTilemap;
        [SerializeField] private Tilemap leddersTilemap;
        [SerializeField] private Tilemap environmentsTilemap;

        [SerializeField] private int maxBuildingsCount = 1;
        [SerializeField] private int minBuildingsCount = 0;
        [SerializeField] private int maxFloorsCount = 3;
        [SerializeField] private int minFloorsCount = 0;
        [SerializeField] private int maxBuildingsGap = 5;
        [SerializeField] private int minBuildingsGap = 2;

        [SerializeField] private int enemiesCount = 10;

        [SerializeField] private GameObject ledderPointPrefab;

        private List<Building> buildings = new List<Building>();
        private void Start()
        {
            int randomInt = UnityEngine.Random.Range(minBuildingsCount, maxBuildingsCount + 1);
            Vector3Int currentGenTransformVector = Vector3Int.zero;
            GameObject ledderPoints = new GameObject("LedderPoints");
            for (int i = 0; i < randomInt; i++)
            {
                int randomInt2 = UnityEngine.Random.Range(minFloorsCount, maxFloorsCount + 1);
                Building building = new Building(randomInt2);
                building.TransformBuilding(-currentGenTransformVector);
                building.GetAllMainTiles(out var positions, out var tiles);
                mainTilemap.SetTiles(positions, tiles);
                building.GetAllCollisionsTiles(out positions, out tiles);
                collisionsTilemap.SetTiles(positions, tiles);
                building.GetAllEnvironmentTiles(out positions, out tiles);
                environmentsTilemap.SetTiles(positions, tiles);
                building.GetAllLeddersTiles(out positions, out tiles);
                leddersTilemap.SetTiles(positions, tiles);
                randomInt2 = UnityEngine.Random.Range(minBuildingsGap, maxBuildingsGap + 1);
                currentGenTransformVector.x += building.Basement.View.MainTilemapData.Width + randomInt2;

                foreach (var ledder in building.Ledders)
                {
                    ledder.GetTiles(out var positionsList, out _);
                    List<LedderPoint> points = new List<LedderPoint>();
                    LedderPointMain pointMain = null;

                    for (int j = 0; j < positionsList.Count; j++)
                    {
                        Vector3 prefabPosition = new Vector3
                        (
                            positionsList[j].x + Mathf.Sign(positionsList[j].x) * .5f,
                            positionsList[j].y + Mathf.Sign(positionsList[j].x) * .5f
                        );
                        GameObject ledderPointInstance = Instantiate(ledderPointPrefab, prefabPosition, Quaternion.identity, ledderPoints.transform);
                        if (j == 0)
                        {
                            pointMain = ledderPointInstance.AddComponent<LedderPointMain>();
                            continue;
                        }
                        LedderPoint ledderPoint = ledderPointInstance.AddComponent<LedderPoint>();
                        ledderPoint.MainPoint = pointMain;
                        points.Add(ledderPoint);
                    }
                    pointMain.Points = points;
                }

                buildings.Add(building);
            }

            Vector3 playerPosition = new Vector3(1, -(buildings[0].Spans[^1].TransformVector.y) + 3f, 0);
            GameObject player = Instantiate(Resources.Load<GameObject>("Player"), playerPosition, Quaternion.identity);
            CollisionsController playerCollisions = player.GetComponent<CollisionsController>();
            CameraFollow cameraFollow = FindAnyObjectByType<CameraFollow>();
            cameraFollow.target = playerCollisions;

            int curEnemyIndex = 0;
            int continueIndex = 0;
            List<SpanData> spansWithEnemy = new List<SpanData>(enemiesCount);
            Dictionary<SpanData, GameObject> spansAssociatedEnemies = new Dictionary<SpanData, GameObject>(enemiesCount);
            GameObject enemy = Resources.Load<GameObject>("Turret");
            while (curEnemyIndex < enemiesCount)
            {
                randomInt = UnityEngine.Random.Range(0, buildings.Count);
                int randomInt2 = UnityEngine.Random.Range(0, buildings[randomInt].Spans.Count);
                SpanData span = buildings[randomInt].Spans[randomInt2];
                if (spansWithEnemy.Contains(span))
                {
                    continueIndex++;
                    if (continueIndex >= 5)
                    {
                        continueIndex = 0;
                        curEnemyIndex++;
                    }
                    continue;
                }
                span.GetTiles(out var positions, out _);
                randomInt = UnityEngine.Random.Range(1, positions.Count);
                Vector3Int selectedPosition = positions[randomInt];
                Vector3 enemyPosition = new Vector3(selectedPosition.x, selectedPosition.y + 1.5f);
                GameObject enemyInstance = Instantiate(enemy, enemyPosition, Quaternion.identity);
                spansWithEnemy.Add(span);
                spansAssociatedEnemies.Add(span, enemyInstance);
                curEnemyIndex++;
            }

            if (spansWithEnemy.Contains(buildings[0].Spans[^1]))
            {
                Destroy(spansAssociatedEnemies[buildings[0].Spans[^1]]);
            }

            Destroy(gameObject, 5f);
        }

    }
}
