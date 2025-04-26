using UnityEngine;

namespace Car
{
    public class CarDeleter : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D other)
        {
            Debug.Log("EnteredTrigger");
            // Check if the entering object has the same tag we used for spawned objects
            if (other.CompareTag("SpawnedCar"))
            {
                Destroy(other.gameObject); 
            }
        }
    }
}