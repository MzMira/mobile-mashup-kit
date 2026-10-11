using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace PlantsVsBloons.UI
{
    public class UIManager : MonoBehaviour
    {
        [Header("HUD")]
        public TextMeshProUGUI sunText;
        public TextMeshProUGUI livesText;
        public TextMeshProUGUI waveText;

        [Header("Plant Selection")]
        public PlantButton[] plantButtons;
        public GameObject plantSelectionPanel;

        [Header("Screens")]
        public GameObject gameOverPanel;
        public GameObject victoryPanel;
        public GameObject pausePanel;

        [Header("Messages")]
        public TextMeshProUGUI messageText;
        public float messageDuration = 2f;

        private void Start()
        {
            UpdateSunDisplay(GameManager.Instance.Sun);
            UpdateLivesDisplay(GameManager.Instance.Lives);
            UpdateWaveDisplay(1, 10);
        }

        public void UpdateSunDisplay(int sun)
        {
            if (sunText != null) sunText.text = sun.ToString();
        }

        public void UpdateLivesDisplay(int lives)
        {
            if (livesText != null) livesText.text = lives.ToString();
        }

        public void UpdateWaveDisplay(int current, int total)
        {
            if (waveText != null) waveText.text = $"Wave {current}/{total}";
        }

        public void ShowGameOver()
        {
            if (gameOverPanel != null) gameOverPanel.SetActive(true);
        }

        public void ShowVictory()
        {
            if (victoryPanel != null) victoryPanel.SetActive(true);
        }

        public void ShowPause()
        {
            if (pausePanel != null) pausePanel.SetActive(true);
        }

        public void HidePause()
        {
            if (pausePanel != null) pausePanel.SetActive(false);
        }

        public void ShowMessage(string message)
        {
            if (messageText == null) return;
            messageText.text = message;
            messageText.gameObject.SetActive(true);
            CancelInvoke(nameof(HideMessage));
            Invoke(nameof(HideMessage), messageDuration);
        }

        private void HideMessage()
        {
            if (messageText != null) messageText.gameObject.SetActive(false);
        }

        public void OnPlantButtonClicked(PlantType type)
        {
            PlantButton button = System.Array.Find(plantButtons, b => b.plantType == type);
            if (button != null) button.Select();
        }
    }
}
