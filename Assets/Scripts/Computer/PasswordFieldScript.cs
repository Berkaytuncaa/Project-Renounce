using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class PasswordFieldScript : MonoBehaviour
{
    public TMPro.TextMeshProUGUI wifiName;
    
    public Sprite newWifiIconSprite;
    public Image wifiImage;
    
    public TMPro.TMP_InputField passwordField;

    public GameObject internetPanel;

    public GameObject panelSpawner;

    public void ClickConfirmButton()
    {
        if (passwordField.text == "password123" && wifiName.text == "Teknopark Ankara Misafir")
        {
            wifiImage.sprite = newWifiIconSprite;
            internetPanel.SetActive(false);
            panelSpawner.SetActive(true);
        }
    }
}
