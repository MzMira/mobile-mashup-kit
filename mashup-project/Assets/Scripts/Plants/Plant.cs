using UnityEngine;

namespace PlantsVsBloons.Core
{
    public enum PlantType
    {
        Peashooter,
        Sunflower,
        Wallnut,
        SnowPea,
        Repeater,
        CherryBomb
    }

    [RequireComponent(typeof(SpriteRenderer))]
    public abstract class Plant : MonoBehaviour
    {
        [Header("Base Stats")]
        public PlantType plantType;
        public int cost = 100;
        public float health = 100f;
        public float attackCooldown = 1.5f;
        public int sunProduction = 0;

        [Header("Visuals")]
        public Sprite plantSprite;
        public Color healthyColor = Color.white;
        public Color damagedColor = Color.red;

        protected float cooldownTimer = 0f;
        protected bool isAlive = true;
        protected SpriteRenderer spriteRenderer;
        protected GridCell currentCell;

        public int Lane => currentCell != null ? currentCell.Lane : -1;
        public float Health => health;
        public bool IsAlive => isAlive;

        protected virtual void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            if (plantSprite != null) spriteRenderer.sprite = plantSprite;
            if (healthyColor != Color.white) spriteRenderer.color = healthyColor;
        }

        public virtual void Initialize(GridCell cell)
        {
            currentCell = cell;
            cooldownTimer = attackCooldown;
        }

        public virtual void Tick()
        {
            if (!isAlive) return;

            if (cooldownTimer > 0)
            {
                cooldownTimer -= Time.deltaTime;
            }
            else
            {
                OnCooldownReady();
                cooldownTimer = attackCooldown;
            }
        }

        protected virtual void OnCooldownReady() { }

        public virtual void TakeDamage(float damage)
        {
            health -= damage;
            if (health <= 0)
            {
                Die();
            }
            else
            {
                UpdateVisuals();
            }
        }

        protected virtual void UpdateVisuals()
        {
            float healthPercent = health / MaxHealth;
            spriteRenderer.color = Color.Lerp(damagedColor, healthyColor, healthPercent);
        }

        protected virtual float MaxHealth => 100f;

        protected virtual void Die()
        {
            isAlive = false;
            if (currentCell != null) currentCell.RemovePlant();
            GameManager.Instance.RemovePlant(this);
            Destroy(gameObject);
        }
    }
}
