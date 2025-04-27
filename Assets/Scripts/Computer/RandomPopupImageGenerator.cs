using UnityEngine;
using UnityEngine.UI; // Required for Image component

public class RandomPopupImageGenerator : MonoBehaviour
{
    [SerializeField] private Sprite[] popupImages;
    private Image imageComponent;

    private void Start()
    {
        // Get the Image component
        imageComponent = GetComponent<Image>();
        
        if (imageComponent == null)
        {
            Debug.LogError("No Image component found on this GameObject!");
            return;
        }

        AssignRandomSprite();
    }

    private void AssignRandomSprite()
    {
        if (popupImages == null || popupImages.Length == 0)
        {
            Debug.LogError("No sprites assigned to popupImages array!");
            return;
        }

        // Get a random index
        int randomIndex = Random.Range(0, popupImages.Length);
        
        // Assign the random sprite to the Image component
        imageComponent.sprite = popupImages[randomIndex];
    }
}