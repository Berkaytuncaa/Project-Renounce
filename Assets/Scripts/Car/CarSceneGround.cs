using UnityEngine;

namespace Car
{
    public class CarSceneGround : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                other.GetComponent<PlayerController>().Die();
            }
        }
    }
}