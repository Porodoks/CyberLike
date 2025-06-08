using Assets.Test;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "BuildingPresets", menuName = "Scriptable Objects/BuildingPresets")]
public class BuildingPresets : ScriptableObject
{
    public List<BasementView> Basements => _basements;
    [SerializeField] private List<BasementView> _basements;

    public Tile SpanLeftTile;
    public Tile SpanRightTile;
    public Tile SpanCenterTile;

    public Tile LedderRegularTile;
    public Tile LedderLastTile;

    public List<WindowView> Windows = new List<WindowView>(4);

    public Tile A_WindowGenerationStartPoint;
}
