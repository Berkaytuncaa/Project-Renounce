using UnityEngine;
using UnityEngine.UI;

public class RandomPanelSpawner : MonoBehaviour
{
    [SerializeField] private GameObject panelPrefab;
    [SerializeField] private RectTransform canvasRectTransform;
    [SerializeField] private float spawnPadding = 50f;
    
    [SerializeField] private bool autoSpawn = false;
    [SerializeField] private float spawnInterval = 2f;
    
    private void Start()
    {
        if (autoSpawn)
        {
            InvokeRepeating("SpawnPanel", 0f, spawnInterval);
        }
    }
    
    public void SpawnPanel()
    {
        if (panelPrefab == null || canvasRectTransform == null)
        {
            Debug.LogError("Please assign Panel Prefab and Canvas RectTransform!");
            return;
        }

        // Instantiate the panel as a child of the canvas
        GameObject newPanel = Instantiate(panelPrefab, canvasRectTransform);
        RectTransform panelRect = newPanel.GetComponent<RectTransform>();

        // Get the panel's size
        float panelWidth = panelRect.rect.width;
        float panelHeight = panelRect.rect.height;

        // Get the canvas dimensions
        float canvasWidth = canvasRectTransform.rect.width;
        float canvasHeight = canvasRectTransform.rect.height;

        // Calculate the maximum position values to keep the panel in bounds
        float maxX = (canvasWidth - panelWidth) / 2 - spawnPadding;
        float maxY = (canvasHeight - panelHeight) / 2 - spawnPadding;

        // Calculate random position within the safe bounds
        float randomX = Random.Range(-maxX, maxX);
        float randomY = Random.Range(-maxY, maxY);

        // Set the random position
        panelRect.anchoredPosition = new Vector2(randomX, randomY);

        // Ensure the panel stays within bounds by clamping its position
        ClampPanelPosition(panelRect);
    }

    private void ClampPanelPosition(RectTransform panelRect)
    {
        // Get the current position
        Vector2 position = panelRect.anchoredPosition;

        // Get the panel's size
        float panelWidth = panelRect.rect.width;
        float panelHeight = panelRect.rect.height;

        // Get the canvas dimensions
        float canvasWidth = canvasRectTransform.rect.width;
        float canvasHeight = canvasRectTransform.rect.height;

        // Calculate the bounds
        float maxX = (canvasWidth - panelWidth) / 2 - spawnPadding;
        float maxY = (canvasHeight - panelHeight) / 2 - spawnPadding;

        // Clamp the position
        position.x = Mathf.Clamp(position.x, -maxX, maxX);
        position.y = Mathf.Clamp(position.y, -maxY, maxY);

        // Apply the clamped position
        panelRect.anchoredPosition = position;
    }

    // Call this method to manually spawn a panel
    public void SpawnPanelButton()
    {
        SpawnPanel();
    }
}