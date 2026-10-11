using UnityEngine;

namespace PlantsVsBloons.Plants
{
    public class Peashooter : Plant
    {
        [Header("Peashooter Stats")]
        public float damage = 1f;
        public float projectileSpeed = 10f;
        public GameObject projectilePrefab;
        public Transform firePoint;

        protected override float MaxHealth => 100f;

        protected override void Awake()
        {
            base.Awake();
            plantType = PlantType.Peashooter;
            cost = 100;
            attackCooldown = 1.4f;
            health = MaxHealth;
        }

        protected override void OnCooldownReady()
        {
            bool hasTarget = GameManager.Instance.GetBloonsInLane(Lane).Count > 0;
            if (!hasTarget) return;

            FireProjectile();
        }

        protected virtual void FireProjectile()
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
