using UnityEngine;

namespace Car
{
    public class CarSpawner : MonoBehaviour
    {
        [SerializeField] private GameObject[] prefabsToSpawn;
        [SerializeField] private float spawnInterval = 2f;
        [SerializeField] private float moveSpeed = 5f;

        private float nextSpawnTime;

        void Update()
        {
            if (Time.time >= nextSpawnTime)
            {
                SpawnObject();
                nextSpawnTime = Time.time + spawnInterval;
            }
        }

        void SpawnObject()
        {
            if (prefabsToSpawn.Length > 0)
            {
                int randomIndex = Random.Range(0, prefabsToSpawn.Length);
                GameObject selectedPrefab = prefabsToSpawn[randomIndex];

                GameObject spawnedObject = Instantiate(selectedPrefab, transform.position, Quaternion.identity);
                spawnedObject.tag = "SpawnedCar";
                
                // Add Rigidbody2D if it doesn't exist
                Rigidbody2D rb = spawnedObject.GetComponent<Rigidbody2D>();
                if (rb == null)
                {
                    rb = spawnedObject.AddComponent<Rigidbody2D>();
                }
                
                // Configure the Rigidbody2D
                rb.bodyType = RigidbodyType2D.Kinematic;
                rb.linearVelocity = new Vector2(-moveSpeed, 0);
            }
        }
    }
}