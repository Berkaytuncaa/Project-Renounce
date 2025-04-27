using UnityEngine;
using UnityEngine.UI;

public class WifiController : MonoBehaviour
{
    [SerializeField] private GameObject popupPanel; // Reference to your popup panel
    private bool isPopupVisible = false;
    
    public void TogglePopup()
    {
        if (popupPanel != null)
        {
            isPopupVisible = !isPopupVisible;
            popupPanel.SetActive(isPopupVisible);
        }
    }
}