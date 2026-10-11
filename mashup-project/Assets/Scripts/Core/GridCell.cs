using UnityEngine;

namespace PlantsVsBloons.Core
{
    public class GridCell : MonoBehaviour
    {
        public int Lane { get; private set; }
        public int Column { get; private set; }
        public Vector3 Position { get; private set; }
        public bool IsEmpty => plant == null;
        public Plant Plant => plant;

        private Plant plant;

        public void Initialize(int lane, int column, Vector3 position)
        {
            Lane = lane;
            Column = column;
            Position = position;
        }

        public void PlacePlant(Plant newPlant)
        {
            plant = newPlant;
        }

        public void RemovePlant()
        {
            if (plant != null)
            {
                Destroy(plant.gameObject);
                plant = null;
            }
        }
    }
}
