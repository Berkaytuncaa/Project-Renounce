using UnityEngine;

public class BeytepeBird : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5.0f;
    [SerializeField] private float zigzagAmplitude = 1.0f;
    [SerializeField] private float zigzagFrequency = 2.0f;
    [SerializeField] private LayerMask platformLayer;

    private bool movingRight = true;

    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        MoveZigzag();
    }

    private void MoveZigzag()
    {
        float horizontalMovement = moveSpeed * Time.deltaTime;
        float verticalMovement = Mathf.Sin(Time.time * zigzagFrequency) * zigzagAmplitude * Time.deltaTime;

        transform.Translate(new Vector3(horizontalMovement * (movingRight ? 1 : -1), verticalMovement, 0));

        if (movingRight && !spriteRenderer.flipX)
        {
            spriteRenderer.flipX = true;
        }
        else if (!movingRight && spriteRenderer.flipX)
        {
            spriteRenderer.flipX = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Platform"))
        {
            movingRight = !movingRight;
            Debug.Log("UH");
        }
    }
}
