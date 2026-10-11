using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace PlantsVsBloons.UI
{
    public class PlantButton : MonoBehaviour
    {
        [Header("Settings")]
        public PlantType plantType;
        public int cost = 100;
        public float cooldown = 5f;

        [Header("Visuals")]
        public Image iconImage;
        public TextMeshProUGUI costText;
        public Image cooldownOverlay;
        public GameObject selectedOutline;

        [Header("Plant Prefab")]
        public GameObject plantPrefab;

        private bool isOnCooldown = false;
        private bool isSelected = false;
        private float cooldownTimer = 0f;

        public PlantType Type => plantType;
        public bool IsOnCooldown => isOnCooldown;
        public bool IsSelected => isSelected;

        private void Start()
        {
            if (costText != null) costText.text = cost.ToString();
        }

        private void Update()
        {
            if (isOnCooldown)
            {
                cooldownTimer -= Time.deltaTime;
                if (cooldownOverlay != null)
                    cooldownOverlay.fillAmount = cooldownTimer / cooldown;

                if (cooldownTimer <= 0)
                {
                    isOnCooldown = false;
                    if (cooldownOverlay != null) cooldownOverlay.fillAmount = 0f;
                }
            }
        }

        public void Select()
        {
            if (isOnCooldown) return;
            if (GameManager.Instance.Sun < cost) return;

            DeselectAll();
            isSelected = true;
            if (selectedOutline != null) selectedOutline.SetActive(true);
        }

        public void Deselect()
        {
            isSelected = false;
            if (selectedOutline != null) selectedOutline.SetActive(false);
        }

        private void DeselectAll()
        {
            var buttons = FindObjectsOfType<PlantButton>();
            foreach (var btn in buttons) btn.Deselect();
        }

        public void PlacePlant(int lane, int col)
        {
            if (!isSelected || isOnCooldown) return;
            if (!GameManager.Instance.SpendSun(cost)) return;

            Vector3 pos = GameManager.Instance.grid.GetCellPosition(lane, col);
            GameObject plantObj = Instantiate(plantPrefab, pos, Quaternion.identity);
            Plant plant = plantObj.GetComponent<Plant>();
            if (plant == null) return;

            GameManager.Instance.grid.PlacePlant(plant, lane, col);
            plant.Initialize(GameManager.Instance.grid.GetCell(lane, col));
            GameManager.Instance.AddPlant(plant);

            isOnCooldown = true;
            cooldownTimer = cooldown;
            Deselect();
        }
    }
}
