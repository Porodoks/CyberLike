using UnityEngine;
using UnityEngine.Tilemaps;

namespace Assets.Test
{
    public class FloorView : BuildingPartView
    {
        public Tilemap Annotations => _annotations;
        [SerializeField] private Tilemap _annotations;
        public TilemapData AnnotationsTilemapData
        {
            get
            {
                if (!_annotationsTilemapData.IsReady)
                {
                    BoundsInt mainTilemapBounds = TilemapHelper.GetTilemapBounds(MainTilemap);
                    _annotationsTilemapData = new TilemapData(Annotations, Id, new Vector3Int(mainTilemapBounds.xMin, mainTilemapBounds.yMin));
                }
                return _annotationsTilemapData;
            }
        }
        private TilemapData _annotationsTilemapData;
        public FloorData GetData()
        {
            return new FloorData(this);
        }
    }
}
