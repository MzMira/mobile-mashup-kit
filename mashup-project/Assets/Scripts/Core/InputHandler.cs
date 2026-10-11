using UnityEngine;

namespace PlantsVsBloons.Core
{
    public class InputHandler : MonoBehaviour
    {
        [Header("Settings")]
        public LayerMask plantLayer;
        public LayerMask gridLayer;
        public GameObject placementIndicator;

        [Header("References")]
        public GridManager grid;
        public PlantButton selectedButton;

        private Camera mainCamera;
        private bool isPlacing = false;

        private void Start()
        {
            mainCamera = Camera.main;
            if (placementIndicator != null) placementIndicator.SetActive(false);
        }

        private void Update()
        {
            HandleMouseInput();
            HandleTouchInput();
        }

        private void HandleMouseInput()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Vector3 mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
                TryPlacePlant(mousePos);
            }
        }

        private void HandleTouchInput()
        {
            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
            {
                Vector3 touchPos = mainCamera.ScreenToWorldPoint(Input.GetTouch(0).position);
                TryPlacePlant(touchPos);
            }
        }

        private void TryPlacePlant(Vector3 worldPos)
        {
            if (selectedButton == null || !selectedButton.IsSelected) return;

            int lane = grid.GetLaneFromWorldPosition(worldPos);
            int col = grid.GetColumnFromWorldPosition(worldPos);

            if (grid.IsCellEmpty(lane, col))
            {
                selectedButton.PlacePlant(lane, col);
            }
        }

        public void SetSelectedButton(PlantButton button)
        {
            selectedButton = button;
        }
    }
}
