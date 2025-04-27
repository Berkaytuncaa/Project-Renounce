using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.Serialization;

public class ButtonListGenerator : MonoBehaviour
{
    [SerializeField] private GameObject buttonPrefab; // Assign your button prefab
    [SerializeField] private Transform contentParent; // Assign the Content object of your ScrollView
    
    [SerializeField] private GameObject passwordField; // Reference to your popup panel
    private bool isPopupVisible = false;
    
    // You can modify this array with your desired button texts
    private string[] buttonTexts = new string[] 
    { "Totally Free WiFi",
        "ODTU_Teknokent_5G",
        "Cyberpark_Free",
        "AnkaraTeknokent_Secure",
        "TGB_Misafir_2.4GHz",
        "Teknopark Ankara Misafir",
        "Teknopark_Staff",
        "Bilkent_Cyberpark",
        "AR-GE_Network",
        "StartupAnkara_WiFi",
        "Innovation_Hub_5G"
    };

    void Start()
    {
        SetupScrollViewLayout();
        GenerateButtons();
    }

    private void SetupScrollViewLayout()
    {
        // Add Vertical Layout Group if it doesn't exist
        VerticalLayoutGroup verticalLayout = contentParent.gameObject.GetComponent<VerticalLayoutGroup>();
        if (verticalLayout == null)
        {
            verticalLayout = contentParent.gameObject.AddComponent<VerticalLayoutGroup>();
        }
        
        // Configure the Vertical Layout Group
        verticalLayout.spacing = 10f; // Space between buttons
        verticalLayout.padding = new RectOffset(10, 10, 10, 10); // Padding around all buttons
        verticalLayout.childAlignment = TextAnchor.UpperCenter;
        verticalLayout.childControlHeight = false; // Don't control height
        verticalLayout.childControlWidth = false;
        verticalLayout.childForceExpandHeight = false;
        verticalLayout.childForceExpandWidth = false;

        // Add Content Size Fitter if it doesn't exist
        ContentSizeFitter contentSizeFitter = contentParent.gameObject.GetComponent<ContentSizeFitter>();
        if (contentSizeFitter == null)
        {
            contentSizeFitter = contentParent.gameObject.AddComponent<ContentSizeFitter>();
        }
        
        // Configure the Content Size Fitter
        contentSizeFitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
        contentSizeFitter.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
    }

    private void GenerateButtons()
    {
        foreach (string buttonText in buttonTexts)
        {
            // Instantiate the button prefab
            GameObject newButton = Instantiate(buttonPrefab, contentParent);
            
            // Get the Text component of the button and set its text
            TMPro.TextMeshProUGUI buttonTextComponent = newButton.GetComponentInChildren<TMPro.TextMeshProUGUI>();
            if (buttonTextComponent != null)
            {
                buttonTextComponent.text = buttonText;
            }
            
            // Get the Button component and add onClick listener
            Button buttonComponent = newButton.GetComponent<Button>();
            if (buttonComponent != null)
            {
                string currentText = buttonText;
                buttonComponent.onClick.AddListener(() => OnButtonClick(currentText));
            }
        }
    }

    private void OnButtonClick(string buttonText)
    {
        Debug.Log($"Button clicked: {buttonText}");
        // Add your button functionality here
        
        if (passwordField != null)
        {
            isPopupVisible = !isPopupVisible;
            passwordField.SetActive(isPopupVisible);
            passwordField.GetComponentInChildren<PasswordFieldScript>().wifiName.text = buttonText;
        }
    }
}