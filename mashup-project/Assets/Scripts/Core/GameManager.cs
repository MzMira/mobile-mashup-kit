using UnityEngine;
using System.Collections.Generic;

namespace PlantsVsBloons.Core
{
    public enum GameState
    {
        Menu,
        Playing,
        Paused,
        GameOver,
        Victory
    }

    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        [Header("Game Settings")]
        public int startingSun = 150;
        public int lives = 20;
        public float waveInterval = 30f;

        [Header("References")]
        public GridManager grid;
        public WaveManager waveManager;
        public UIManager uiManager;

        public int Sun { get; private set; }
        public int Lives => lives;
        public GameState State { get; private set; }

        private List<Plant> plants = new List<Plant>();
        private List<Bloon> bloons = new List<Bloon>();
        private List<Projectile> projectiles = new List<Projectile>();

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
        }

        private void Start()
        {
            Sun = startingSun;
            State = GameState.Playing;
            waveManager.StartWaves();
        }

        private void Update()
        {
            if (State != GameState.Playing) return;

            UpdatePlants();
            UpdateBloons();
            UpdateProjectiles();
            CheckGameOver();
        }

        private void UpdatePlants()
        {
            foreach (var plant in plants)
            {
                plant.Tick();
            }
        }

        private void UpdateBloons()
        {
            for (int i = bloons.Count - 1; i >= 0; i--)
            {
                if (bloons[i] == null)
                {
                    bloons.RemoveAt(i);
                    continue;
                }
                bloons[i].Tick();
            }
        }

        private void UpdateProjectiles()
        {
            for (int i = projectiles.Count - 1; i >= 0; i--)
            {
                if (projectiles[i] == null)
                {
                    projectiles.RemoveAt(i);
                    continue;
                }
                projectiles[i].Tick();
            }
        }

        private void CheckGameOver()
        {
            if (lives <= 0)
            {
                State = GameState.GameOver;
                uiManager.ShowGameOver();
            }
        }

        public void AddPlant(Plant plant)
        {
            plants.Add(plant);
        }

        public void RemovePlant(Plant plant)
        {
            plants.Remove(plant);
        }

        public void AddBloon(Bloon bloon)
        {
            bloons.Add(bloon);
        }

        public void RemoveBloon(Bloon bloon)
        {
            bloons.Remove(bloon);
        }

        public void AddProjectile(Projectile projectile)
        {
            projectiles.Add(projectile);
        }

        public void RemoveProjectile(Projectile projectile)
        {
            projectiles.Remove(projectile);
        }

        public void AddSun(int amount)
        {
            Sun += amount;
            uiManager.UpdateSunDisplay(Sun);
        }

        public bool SpendSun(int amount)
        {
            if (Sun >= amount)
            {
                Sun -= amount;
                uiManager.UpdateSunDisplay(Sun);
                return true;
            }
            return false;
        }

        public void LoseLife(int damage)
        {
            lives -= damage;
            uiManager.UpdateLivesDisplay(lives);
            if (lives <= 0)
            {
                State = GameState.GameOver;
                uiManager.ShowGameOver();
            }
        }

        public List<Bloon> GetBloonsInLane(int lane)
        {
            return bloons.FindAll(b => b != null && b.Lane == lane);
        }

        public List<Bloon> GetBloonsInRange(int lane, float minX, float maxX)
        {
            return bloons.FindAll(b => b != null && b.Lane == lane && b.Position.x >= minX && b.Position.x <= maxX);
        }
    }
}
