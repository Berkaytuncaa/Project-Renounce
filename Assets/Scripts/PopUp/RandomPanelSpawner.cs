using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.Serialization;

public class RandomPanelSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] panelPrefabs; // Changed to array of prefabs
    [SerializeField] private RectTransform boundingPanelRectTransform;
    [SerializeField] private float spawnPadding = 50f;
    
    [SerializeField] private bool autoSpawn = false;
    [SerializeField] private float spawnInterval = 2f;

    [SerializeField] private GameObject noInternetPanel;
    [SerializeField] private GameObject yesInternetPanel;

    private List<GameObject> activePanels = new List<GameObject>();
    private string spawnInvokeMethod = "SpawnPanel";
    
    private void Start()
    {
        if (autoSpawn)
        {
            InvokeRepeating(spawnInvokeMethod, 0f, spawnInterval);
        }
    }

    private void Update()
    {
        // Remove any destroyed panels from our list
        activePanels.RemoveAll(panel => panel == null);

        // If no panels remain and we have an active invoke, cancel it
        if (activePanels.Count == 0 && IsInvoking(spawnInvokeMethod))
        {
            CancelInvoke(spawnInvokeMethod);
            Debug.Log("All panels have been destroyed. Stopping spawn.");
            noInternetPanel.SetActive(false);
            yesInternetPanel.SetActive(true);
        }
    }
    
    public void SpawnPanel()
    {
        if (panelPrefabs == null || panelPrefabs.Length == 0 || boundingPanelRectTransform == null)
        {
            Debug.LogError("Please assign Panel Prefabs and Bounding Panel RectTransform!");
            return;
        }

        // Select a random prefab from the array
        GameObject selectedPrefab = panelPrefabs[Random.Range(0, panelPrefabs.Length)];
        
        if (selectedPrefab == null)
        {
            Debug.LogError("Selected prefab is null!");
            return;
        }

        // Instantiate the panel as a child of the bounding panel
        GameObject newPanel = Instantiate(selectedPrefab, boundingPanelRectTransform);
        RectTransform panelRect = newPanel.GetComponent<RectTransform>();

        // Add the new panel to our tracking list
        activePanels.Add(newPanel);

        // Get the panel's size
        float panelWidth = panelRect.rect.width;
        float panelHeight = panelRect.rect.height;

        // Get the bounding panel dimensions
        float boundingWidth = boundingPanelRectTransform.rect.width;
        float boundingHeight = boundingPanelRectTransform.rect.height;

        // Calculate the maximum position values to keep the panel in bounds
        float maxX = (boundingWidth - panelWidth) / 2 - spawnPadding;
        float maxY = (boundingHeight - panelHeight) / 2 - spawnPadding;

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

        // Get the bounding panel dimensions
        float boundingWidth = boundingPanelRectTransform.rect.width;
        float boundingHeight = boundingPanelRectTransform.rect.height;

        // Calculate the bounds
        float maxX = (boundingWidth - panelWidth) / 2 - spawnPadding;
        float maxY = (boundingHeight - panelHeight) / 2 - spawnPadding;

        // Clamp the position
        position.x = Mathf.Clamp(position.x, -maxX, maxX);
        position.y = Mathf.Clamp(position.y, -maxY, maxY);

        // Apply the clamped position
        panelRect.anchoredPosition = position;
    }

    public void SpawnPanelButton()
    {
        SpawnPanel();
    }

    public void RestartSpawning()
    {
        if (!IsInvoking(spawnInvokeMethod) && autoSpawn)
        {
            InvokeRepeating(spawnInvokeMethod, 0f, spawnInterval);
        }
    }
}