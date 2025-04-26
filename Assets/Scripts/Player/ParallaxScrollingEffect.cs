using UnityEngine;

public class ParallaxEffect : MonoBehaviour
{
    [SerializeField] private float parallaxEffect = 1f;
    private float startPos;
    private float length;
    private Camera mainCamera;
    private Vector2 startCameraPos;

    void Start()
    {
        startPos = transform.position.x;
        length = GetComponent<SpriteRenderer>().bounds.size.x;
        mainCamera = Camera.main;
        startCameraPos = mainCamera.transform.position;
    }

    void Update()
    {
        float temp = mainCamera.transform.position.x * (1 - parallaxEffect);
        float dist = mainCamera.transform.position.x * parallaxEffect;
        
        transform.position = new Vector3(startPos + dist, transform.position.y, transform.position.z);

        // Infinite scrolling (optional)
        if (temp > startPos + length)
        {
            startPos += length;
        }
        else if (temp < startPos - length)
        {
            startPos -= length;
        }
    }
}