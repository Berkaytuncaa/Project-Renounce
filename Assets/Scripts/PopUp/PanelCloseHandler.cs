using UnityEngine;
using UnityEngine.UI;

public class PanelClose : MonoBehaviour
{
    private void Start()
    {
        // Get the Button component and add a listener to its onClick event
        Button closeButton = GetComponent<Button>();
        if (closeButton != null)
        {
            closeButton.onClick.AddListener(ClosePanel);
        }
    }

    private void ClosePanel()
    {
        // Destroy the parent panel GameObject
        Destroy(transform.parent.gameObject);
    }

    private void OnDestroy()
    {
        // Clean up the listener when the object is destroyed
        Button closeButton = GetComponent<Button>();
        if (closeButton != null)
        {
            closeButton.onClick.RemoveListener(ClosePanel);
        }
    }
}