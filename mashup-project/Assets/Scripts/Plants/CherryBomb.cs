using UnityEngine;

namespace PlantsVsBloons.Plants
{
    public class CherryBomb : Plant
    {
        [Header("Cherry Bomb Stats")]
        public float explosionRadius = 1.5f;
        public float explosionDamage = 5f;
        public float fuseTime = 1.2f;
        public GameObject explosionEffectPrefab;

        private float fuseTimer;
        private bool hasExploded = false;

        protected override float MaxHealth => 100f;

        protected override void Awake()
        {
            base.Awake();
            plantType = PlantType.CherryBomb;
            cost = 150;
            health = MaxHealth;
            fuseTimer = fuseTime;
        }

        public override void Tick()
        {
            if (!isAlive || hasExploded) return;

            fuseTimer -= Time.deltaTime;
            float scale = 1f + (1f - fuseTimer / fuseTime) * 0.5f;
            transform.localScale = Vector3.one * scale;

            if (fuseTimer <= 0)
            {
                Explode();
            }
        }

        private void Explode()
        {
            hasExploded = true;

            if (explosionEffectPrefab != null)
            {
                Instantiate(explosionEffectPrefab, transform.position, Quaternion.identity);
            }

            var allBloons = FindObjectsOfType<Bloon>();
            foreach (var bloon in allBloons)
            {
                float dist = Vector2.Distance(transform.position, bloon.transform.position);
                if (dist <= explosionRadius)
                {
                    bloon.TakeDamage(explosionDamage);
                }
            }

            GameManager.Instance.RemovePlant(this);
            Destroy(gameObject);
        }
    }
}
