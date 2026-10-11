using UnityEngine;

namespace PlantsVsBloons.Core
{
    public class GridManager : MonoBehaviour
    {
        [Header("Grid Settings")]
        public int lanes = 5;
        public int columns = 9;
        public float cellSize = 1.2f;
        public Vector2 gridOrigin = new Vector2(-4.5f, -2.5f);

        [Header("References")]
        public GameObject cellPrefab;
        public GameObject gridOverlay;

        private GridCell[,] cells;

        public int Lanes => lanes;
        public int Columns => columns;

        private void Awake()
        {
            InitializeGrid();
        }

        private void InitializeGrid()
        {
            cells = new GridCell[lanes, columns];

            for (int lane = 0; lane < lanes; lane++)
            {
                for (int col = 0; col < columns; col++)
                {
                    Vector3 pos = GetCellPosition(lane, col);
                    GameObject cellObj = Instantiate(cellPrefab, pos, Quaternion.identity, transform);
                    cellObj.name = $"Cell_{lane}_{col}";

                    GridCell cell = cellObj.GetComponent<GridCell>();
                    if (cell == null) cell = cellObj.AddComponent<GridCell>();
                    cell.Initialize(lane, col, pos);
                    cells[lane, col] = cell;
                }
            }
        }

        public Vector3 GetCellPosition(int lane, int col)
        {
            float x = gridOrigin.x + col * cellSize;
            float y = gridOrigin.y + lane * cellSize;
            return new Vector3(x, y, 0);
        }

        public GridCell GetCell(int lane, int col)
        {
            if (lane < 0 || lane >= lanes || col < 0 || col >= columns) return null;
            return cells[lane, col];
        }

        public bool IsCellEmpty(int lane, int col)
        {
            GridCell cell = GetCell(lane, col);
            return cell != null && cell.IsEmpty;
        }

        public bool PlacePlant(Plant plant, int lane, int col)
        {
            if (!IsCellEmpty(lane, col)) return false;

            GridCell cell = GetCell(lane, col);
            cell.PlacePlant(plant);
            plant.transform.position = GetCellPosition(lane, col);
            return true;
        }

        public void RemovePlantAt(int lane, int col)
        {
            GridCell cell = GetCell(lane, col);
            if (cell != null) cell.RemovePlant();
        }

        public int GetLaneFromWorldPosition(Vector3 worldPos)
        {
            float relativeY = worldPos.y - gridOrigin.y;
            int lane = Mathf.RoundToInt(relativeY / cellSize);
            return Mathf.Clamp(lane, 0, lanes - 1);
        }

        public int GetColumnFromWorldPosition(Vector3 worldPos)
        {
            float relativeX = worldPos.x - gridOrigin.x;
            int col = Mathf.RoundToInt(relativeX / cellSize);
            return Mathf.Clamp(col, 0, columns - 1);
        }
    }
}
