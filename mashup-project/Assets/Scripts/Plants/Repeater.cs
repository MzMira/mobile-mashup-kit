using UnityEngine;

namespace PlantsVsBloons.Plants
{
    public class Repeater : Plant
    {
        [Header("Repeater Stats")]
        public float damage = 1f;
        public float projectileSpeed = 10f;
        public int burstCount = 2;
        public float burstDelay = 0.15f;
        public GameObject projectilePrefab;
        public Transform firePoint;

        protected override float MaxHealth => 100f;

        protected override void Awake()
        {
            base.Awake();
            plantType = PlantType.Repeater;
            cost = 200;
            attackCooldown = 1.4f;
            health = MaxHealth;
        }

        protected override void OnCooldownReady()
        {
            bool hasTarget = GameManager.Instance.GetBloonsInLane(Lane).Count > 0;
            if (!hasTarget) return;

            StartCoroutine(BurstFire());
        }

        private System.Collections.IEnumerator BurstFire()
        {
            for (int i = 0; i < burstCount; i++)
            {
                FireOnce();
                yield return new WaitForSeconds(burstDelay);
            }
        }

        private void FireOnce()
        {
            if (projectilePrefab == null) return;

            Vector3 spawnPos = firePoint != null ? firePoint.position : transform.position + Vector3.right * 0.5f;
            GameObject projObj = Instantiate(projectilePrefab, spawnPos, Quaternion.identity);
            Projectile proj = projObj.GetComponent<Projectile>();
            if (proj == null) proj = projObj.AddComponent<Projectile>();

            proj.Initialize(damage, Lane, projectileSpeed);
            GameManager.Instance.AddProjectile(proj);
        }
    }
}
