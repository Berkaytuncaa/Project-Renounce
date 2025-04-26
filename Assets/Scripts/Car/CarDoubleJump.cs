using System;
using UnityEngine;

public class CarDoubleJump : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<PlayerController>().ExtraJump = 1;
        }
    }
}
