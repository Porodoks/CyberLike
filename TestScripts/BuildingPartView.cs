using UnityEngine;
using UnityEngine.Tilemaps;

namespace Assets.Test
{
    public abstract class BuildingPartView : MonoBehaviour
    {
        public Tilemap MainTilemap => _tilemap;
        [SerializeField]
        private Tilemap _tilemap;
        public string Id => _id;
        [SerializeField] private string _id;

        public TilemapData MainTilemapData
        {
            get
            {
                if (!_mainTilemapData.IsReady)
                {
                    _mainTilemapData = new TilemapData(MainTilemap, Id, Vector3Int.zero);
                    Debug.Log($"Created new {nameof(TilemapData)} with id: {_mainTilemapData.Id}");
                }
                return _mainTilemapData;
            }
        }
        private TilemapData _mainTilemapData;
    }
}
