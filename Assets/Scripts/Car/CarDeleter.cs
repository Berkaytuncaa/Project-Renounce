using UnityEngine;

namespace Car
{
    public class CarDeleter : MonoBehaviour
    {
        private AudioManager audioManager;

        private void Awake()
        {
            audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            Debug.Log("EnteredTrigger");
            audioManager.PlaySFX(audioManager.carPassing);
            // Check if the entering object has the same tag we used for spawned objects
            if (other.CompareTag("SpawnedCar"))
            {
                Destroy(other.gameObject); 
            }
        }
    }
}