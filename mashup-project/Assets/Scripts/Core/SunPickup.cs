using UnityEngine;

namespace PlantsVsBloons.Core
{
    public class SunPickup : MonoBehaviour
    {
        public int sunValue = 25;
        public float lifetime = 10f;
        public float floatSpeed = 2f;
        public float floatHeight = 0.5f;

        private float spawnTime;
        private Vector3 startPos;
        private bool isCollected = false;

        public void Initialize(int value)
        {
            sunValue = value;
            spawnTime = Time.time;
            startPos = transform.position;
        }

        private void Update()
        {
            if (isCollected) return;

            float elapsed = Time.time - spawnTime;
            transform.position = startPos + Vector3.up * Mathf.Sin(elapsed * floatSpeed) * floatHeight;

            if (elapsed > lifetime)
            {
                Destroy(gameObject);
            }
        }

        private void OnMouseDown()
        {
            if (isCollected) return;
            isCollected = true;
            GameManager.Instance.AddSun(sunValue);
            Destroy(gameObject);
        }
    }
}
