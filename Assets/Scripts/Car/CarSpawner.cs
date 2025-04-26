using UnityEngine;

namespace Car
{
    public class CarSpawner : MonoBehaviour
    {
        [SerializeField] private GameObject[] prefabsToSpawn; // Array to store your prefabs
        [SerializeField] private float spawnInterval = 2f;    // Time between spawns
        [SerializeField] private float moveSpeed = 5f;        // Speed of movement

        private float nextSpawnTime;

        void Update()
        {
            // Check if it's time to spawn
            if (Time.time >= nextSpawnTime)
            {
                SpawnObject();
                nextSpawnTime = Time.time + spawnInterval;
            }

            // Move all spawned objects
            GameObject[] spawnedObjects = GameObject.FindGameObjectsWithTag("SpawnedObject");
            foreach (GameObject obj in spawnedObjects)
            {
                obj.transform.Translate(Vector3.left * moveSpeed * Time.deltaTime);
            }
        }

        void SpawnObject()
        {
            // Randomly select a prefab from the array
            if (prefabsToSpawn.Length > 0)
            {
                int randomIndex = Random.Range(0, prefabsToSpawn.Length);
                GameObject selectedPrefab = prefabsToSpawn[randomIndex];

                // Spawn the object at the spawner's position
                GameObject spawnedObject = Instantiate(selectedPrefab, transform.position, Quaternion.identity);
                spawnedObject.tag = "SpawnedObject"; // Add tag for movement tracking
            }
        }
    }
}