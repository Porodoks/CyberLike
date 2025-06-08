using System.Collections.Generic;
using UnityEngine;

namespace Assets.Test
{
    public class BasementView : BuildingPartView
    {
        public List<FloorView> AssociatedFloors => _associatedFloors;
        [SerializeField] private List<FloorView> _associatedFloors = new List<FloorView>();

        public BasementData GetData()
        {
            return new BasementData(this);
        }
    }
}
