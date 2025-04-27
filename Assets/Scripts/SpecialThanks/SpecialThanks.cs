using UnityEngine;
using TMPro;

public class SpecialThanks : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI creditsText;
    [SerializeField] private float scrollSpeed = 20f;
    [SerializeField] private float tiltAngle = 45f;
    
    private RectTransform textRectTransform;
    private Vector3 startPosition;
    private float textHeight;

    void Start()
    {
        if (creditsText == null)
        {
            Debug.LogError("Credits Text reference is missing!");
            return;
        }

        textRectTransform = creditsText.GetComponent<RectTransform>();
        // Start position below the screen
        startPosition = textRectTransform.position;
        textHeight = textRectTransform.rect.height;
        
        // Set the rotation for the Star Wars effect
        textRectTransform.rotation = Quaternion.Euler(tiltAngle, 0, 0);
    }

    void Update()
    {
        // Move the text upward
        textRectTransform.Translate(Vector3.up * scrollSpeed * Time.deltaTime);

        // If the text has scrolled completely off screen, you might want to:
        // 1. Either reset it to start position
        // 2. Or destroy/deactivate it
        if (textRectTransform.position.y > (startPosition.y + textHeight + Screen.height))
        {
            // Option 1: Reset position
            textRectTransform.position = startPosition;
            
            // Option 2: Deactivate
            // gameObject.SetActive(false);
        }
    }
}