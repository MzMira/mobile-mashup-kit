using UnityEngine;

namespace PlantsVsBloons.Core
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class Projectile : MonoBehaviour
    {
        [Header("Stats")]
        public float damage = 1f;
        public float speed = 8f;
        public int lane;
        public bool destroyOnHit = true;
        public float lifetime = 5f;

        [Header("Visuals")]
        public Sprite projectileSprite;
        public Color projectileColor = Color.green;

        protected float spawnTime;
        protected SpriteRenderer spriteRenderer;
        protected bool isActive = true;

        protected virtual void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            if (projectileSprite != null) spriteRenderer.sprite = projectileSprite;
            spriteRenderer.color = projectileColor;
            spawnTime = Time.time;
        }

        public void Initialize(float projDamage, int projLane, float projSpeed)
        {
            damage = projDamage;
            lane = projLane;
            speed = projSpeed;
        }

        public virtual void Tick()
        {
            if (!isActive) return;

            transform.position += Vector3.right * speed * Time.deltaTime;

            if (Time.time - spawnTime > lifetime || transform.position.x > 7f)
            {
                Deactivate();
            }
        }

        protected virtual void OnTriggerEnter2D(Collider2D other)
        {
            if (!isActive) return;

            Bloon bloon = other.GetComponent<Bloon>();
            if (bloon != null && bloon.IsAlive && bloon.Lane == lane)
            {
                HitBloon(bloon);
            }
        }

        protected virtual void HitBloon(Bloon bloon)
        {
            bloon.TakeDamage(damage);
            if (destroyOnHit)
            {
                Deactivate();
            }
        }

        protected virtual void Deactivate()
        {
            isActive = false;
            GameManager.Instance.RemoveProjectile(this);
            Destroy(gameObject);
        }
    }
}
