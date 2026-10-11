using UnityEngine;
using System.Collections.Generic;

namespace PlantsVsBloons.Core
{
    public class WaveManager : MonoBehaviour
    {
        [Header("Wave Settings")]
        public List<Wave> waves = new List<Wave>();
        public float timeBetweenWaves = 30f;
        public float spawnInterval = 1.5f;

        [Header("References")]
        public GameObject bloonPrefab;
        public Transform bloonContainer;

        private int currentWaveIndex = 0;
        private bool isSpawning = false;
        private float spawnTimer = 0f;
        private int bloonsSpawnedInWave = 0;
        private int bloonsInCurrentWave = 0;

        public int CurrentWave => currentWaveIndex + 1;
        public int TotalWaves => waves.Count;
        public bool AllWavesComplete => currentWaveIndex >= waves.Count && !isSpawning;

        public void StartWaves()
        {
            currentWaveIndex = 0;
            StartCoroutine(RunWaves());
        }

        private System.Collections.IEnumerator RunWaves()
        {
            while (currentWaveIndex < waves.Count)
            {
                Wave wave = waves[currentWaveIndex];
                yield return new WaitForSeconds(timeBetweenWaves);

                isSpawning = true;
                bloonsSpawnedInWave = 0;
                bloonsInCurrentWave = wave.GetTotalBloons();

                while (bloonsSpawnedInWave < bloonsInCurrentWave)
                {
                    SpawnBloon(wave);
                    bloonsSpawnedInWave++;
                    spawnTimer += spawnInterval;
                    yield return new WaitForSeconds(spawnInterval);
                }

                isSpawning = false;
                currentWaveIndex++;
            }
        }

        private void SpawnBloon(Wave wave)
        {
            // Pick a random bloon type from the wave
            BloonType type = wave.GetRandomBloonType();
            int lane = Random.Range(0, GameManager.Instance.grid.Lanes);

            Vector3 spawnPos = new Vector3(6f, GameManager.Instance.grid.GetCellPosition(lane, 0).y, 0);

            GameObject bloonObj = Instantiate(bloonPrefab, spawnPos, Quaternion.identity, bloonContainer);
            Bloon bloon = bloonObj.GetComponent<Bloon>();
            if (bloon == null) bloon = bloonObj.AddComponent<Bloon>();

            bloon.Initialize(type, lane, wave.GetSpeedMultiplier(), wave.GetHealthMultiplier());
            GameManager.Instance.AddBloon(bloon);
        }
    }

    [System.Serializable]
    public class WaveEntry
    {
        public BloonType bloonType;
        public int count;
        public float spawnChance = 1f;
    }

    [System.Serializable]
    public class Wave
    {
        public string waveName;
        public List<WaveEntry> entries = new List<WaveEntry>();
        public float speedMultiplier = 1f;
        public float healthMultiplier = 1f;

        public int GetTotalBloons()
        {
            int total = 0;
            foreach (var entry in entries) total += entry.count;
            return total;
        }

        public BloonType GetRandomBloonType()
        {
            float totalChance = 0f;
            foreach (var entry in entries) totalChance += entry.spawnChance;

            float roll = Random.Range(0f, totalChance);
            float cumulative = 0f;

            foreach (var entry in entries)
            {
                cumulative += entry.spawnChance;
                if (roll <= cumulative) return entry.bloonType;
            }

            return entries[0].bloonType;
        }

        public float GetSpeedMultiplier() => speedMultiplier;
        public float GetHealthMultiplier() => healthMultiplier;
    }
}
