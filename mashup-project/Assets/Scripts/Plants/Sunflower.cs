using UnityEngine;

namespace PlantsVsBloons.Plants
{
    public class Sunflower : Plant
    {
        [Header("Sunflower Stats")]
        public int sunAmount = 25;
        public float sunInterval = 8f;
        public GameObject sunPickupPrefab;
        public Transform spawnPoint;

        private float sunTimer;

        protected override float MaxHealth => 80f;

        protected override void Awake()
        {
            base.Awake();
            plantType = PlantType.Sunflower;
            cost = 50;
            attackCooldown = sunInterval;
            health = MaxHealth;
            sunTimer = sunInterval;
        }

        protected override void OnCooldownReady()
        {
            ProduceSun();
        }

        protected virtual void ProduceSun()
        {
            GameManager.Instance.AddSun(sunAmount);

            if (sunPickupPrefab != null)
            {
                Vector3 spawnPos = spawnPoint != null ? spawnPoint.position : transform.position + Vector3.up * 0.3f;
                GameObject sunObj = Instantiate(sunPickupPrefab, spawnPos, Quaternion.identity);
                SunPickup sun = sunObj.GetComponent<SunPickup>();
                if (sun == null) sun = sunObj.AddComponent<SunPickup>();
                sun.Initialize(sunAmount);
            }
        }
    }
}
