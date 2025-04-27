using UnityEngine;

public class BouncePad : MonoBehaviour
{
    public float bounceForce = 15f;
    private AudioManager audioManager;

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            audioManager.PlaySFX(audioManager.boing4);
            foreach (ContactPoint2D contact in collision.contacts)
            {
                if (contact.normal.y <= -0.5f)
                {
                    Rigidbody2D playerRb = collision.gameObject.GetComponent<Rigidbody2D>();
                    if (playerRb != null)
                    {
                        playerRb.linearVelocity = new Vector2(playerRb.linearVelocity.x, 0);
                        playerRb.AddForce(Vector2.up * bounceForce, ForceMode2D.Impulse);
                    }
                }
            }
        }
    }
}
