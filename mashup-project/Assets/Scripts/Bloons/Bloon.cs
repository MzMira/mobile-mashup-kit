using UnityEngine;

namespace PlantsVsBloons.Core
{
    public enum BloonType
    {
        Red,
        Blue,
        Green,
        Yellow,
        Pink,
        Black,
        White,
        Rainbow,
        Ceramic,
        MOAB
    }

    [RequireComponent(typeof(SpriteRenderer))]
    public class Bloon : MonoBehaviour
    {
        [Header("Stats")]
        public BloonType bloonType;
        public float baseHealth = 1f;
        public float speed = 2f;
        public int damage = 1;
        public Color bloonColor = Color.red;

        [Header("Visuals")]
        public Sprite bloonSprite;
        public float rotationSpeed = 50f;

        protected float health;
        protected int lane;
        protected float speedMultiplier = 1f;
        protected float healthMultiplier = 1f;
        protected SpriteRenderer spriteRenderer;
        protected bool isAlive = true;
        protected bool reachedEnd = false;

        public int Lane => lane;
        public Vector3 Position => transform.position;
        public float Health => health;
        public BloonType Type => bloonType;
        public bool IsAlive => isAlive;
        public bool ReachedEnd => reachedEnd;

        protected virtual void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            if (bloonSprite != null) spriteRenderer.sprite = bloonSprite;
            spriteRenderer.color = bloonColor;
        }

        public void Initialize(BloonType type, int laneNum, float speedMult = 1f, float healthMult = 1f)
        {
            bloonType = type;
            lane = laneNum;
            speedMultiplier = speedMult;
            healthMultiplier = healthMult;

            health = baseHealth * healthMult;
            speed *= speedMult;
        }

        public virtual void Tick()
        {
            if (!isAlive || reachedEnd) return;

            transform.position += Vector3.left * speed * Time.deltaTime;
            transform.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime);

            if (transform.position.x < -6f)
            {
                reachedEnd = true;
                GameManager.Instance.LoseLife(damage);
                GameManager.Instance.RemoveBloon(this);
                Destroy(gameObject);
            }
        }

        public virtual void TakeDamage(float damage)
        {
            health -= damage;
            if (health <= 0)
            {
                Pop();
            }
        }

        protected virtual void Pop()
        {
            isAlive = false;
            GameManager.Instance.RemoveBloon(this);
            Destroy(gameObject);
        }

        public int GetLayer()
        {
            return Mathf.RoundToInt(health);
        }
    }
}
