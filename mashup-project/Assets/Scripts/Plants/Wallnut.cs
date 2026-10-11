using UnityEngine;

namespace PlantsVsBloons.Plants
{
    public class Wallnut : Plant
    {
        [Header("Wallnut Stats")]
        public GameObject crackedSprite1;
        public GameObject crackedSprite2;

        protected override float MaxHealth => 400f;

        protected override void Awake()
        {
            base.Awake();
            plantType = PlantType.Wallnut;
            cost = 50;
            attackCooldown = 9999f;
            health = MaxHealth;
        }

        public override void TakeDamage(float damage)
        {
            base.TakeDamage(damage);
            UpdateCrackedVisuals();
        }

        private void UpdateCrackedVisuals()
        {
            float healthPercent = health / MaxHealth;
            SpriteRenderer sr = GetComponent<SpriteRenderer>();

            if (healthPercent < 0.33f && crackedSprite2 != null)
                sr.sprite = crackedSprite2;
            else if (healthPercent < 0.66f && crackedSprite1 != null)
                sr.sprite = crackedSprite1;
        }
    }
}
