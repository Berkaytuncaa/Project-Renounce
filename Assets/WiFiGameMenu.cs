using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class WiFiGameMenu : MonoBehaviour
{
    [SerializeField] private Sprite disconnectedWifiIcon; // Bağlı değilken gösterilecek sprite
    [SerializeField] private Sprite connectedWifiIcon;    // Bağlandıktan sonra gösterilecek sprite
    private Image wifiButtonImage;  // Buton görüntüsü için referans
    // ... diğer değişkenler
    private GameObject wifiPanel;
    private Button wifiButton;
    private TMP_InputField passwordInput;
    private Button connectButton;
    private TextMeshProUGUI messageText;
    private Transform networkListContent;

    private string selectedNetwork = "";
    private bool isConnected = false;

    void Start()
    {
        if (FindObjectOfType<EventSystem>() == null)
        {
            GameObject eventSystem = new GameObject("EventSystem");
            eventSystem.AddComponent<EventSystem>();
            eventSystem.AddComponent<StandaloneInputModule>();
        }

        SetupUI();
    }

    void SetupUI()
    {
        GameObject canvasObj = new GameObject("WiFiCanvas");
        Canvas canvas = canvasObj.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvasObj.AddComponent<CanvasScaler>();
        canvasObj.AddComponent<GraphicRaycaster>();

        CreateWiFiButton(canvasObj);
        CreateWiFiPanel(canvasObj);
    }

void CreateWiFiButton(GameObject parent)
{
    GameObject buttonObj = new GameObject("WiFiButton");
    buttonObj.transform.SetParent(parent.transform, false);
    wifiButton = buttonObj.AddComponent<Button>();

    // Image component'ini kaydet
    wifiButtonImage = buttonObj.AddComponent<Image>();
    wifiButtonImage.sprite = disconnectedWifiIcon; // Başlangıçta bağlı değil sprite'ı
    wifiButtonImage.type = Image.Type.Simple;
    wifiButtonImage.preserveAspect = true;
    wifiButtonImage.raycastTarget = true;
    
    RectTransform rect = buttonObj.GetComponent<RectTransform>();
    rect.anchorMin = new Vector2(1, 0);
    rect.anchorMax = new Vector2(1, 0);
    rect.pivot = new Vector2(1, 0);
    rect.sizeDelta = new Vector2(80, 80);
    rect.anchoredPosition = new Vector2(-20, 20);

    ColorBlock colors = wifiButton.colors;
    colors.normalColor = Color.white;
    colors.highlightedColor = new Color(0.9f, 0.9f, 0.9f, 1f);
    colors.pressedColor = new Color(0.7f, 0.7f, 0.7f, 1f);
    wifiButton.colors = colors;

    wifiButton.onClick.AddListener(() =>
    {
        RectTransform wifiButtonRect = wifiButton.GetComponent<RectTransform>();
        ToggleWiFiPanel(wifiButtonRect);
    });
}

    void CreateWiFiPanel(GameObject parent)
    {
        wifiPanel = new GameObject("WiFiPanel");
        wifiPanel.transform.SetParent(parent.transform, false);

        Image panelImage = wifiPanel.AddComponent<Image>();
        panelImage.color = new Color(0.1f, 0.1f, 0.1f, 0.95f);

        RectTransform panelRect = wifiPanel.GetComponent<RectTransform>();
        panelRect.anchorMin = new Vector2(0.5f, 0.5f);
        panelRect.anchorMax = new Vector2(0.5f, 0.5f);
        panelRect.pivot = new Vector2(0.5f, 0.5f);
        panelRect.sizeDelta = new Vector2(400, 600);
        panelRect.anchoredPosition = Vector2.zero;

        CreatePanelTitle();
        CreateNetworkList();
        CreatePasswordField();
        CreateConnectButton();
        CreateMessageText();

        wifiPanel.SetActive(false);
    }

    void CreatePanelTitle()
    {
        GameObject titleObj = new GameObject("Title");
        titleObj.transform.SetParent(wifiPanel.transform, false);

        TextMeshProUGUI titleText = titleObj.AddComponent<TextMeshProUGUI>();
        titleText.text = "WiFi";
        titleText.fontSize = 24;
        titleText.alignment = TextAlignmentOptions.Center;
        titleText.color = Color.white;

        RectTransform titleRect = titleObj.GetComponent<RectTransform>();
        titleRect.anchorMin = new Vector2(0, 1);
        titleRect.anchorMax = new Vector2(1, 1);
        titleRect.sizeDelta = new Vector2(0, 50);
        titleRect.anchoredPosition = new Vector2(0, -5);
    }

    void CreateNetworkList()
    {
        GameObject listObj = new GameObject("NetworkList");
        //Add RectTransform component to listObj
        RectTransform listRect = listObj.AddComponent<RectTransform>();
        listObj.transform.SetParent(wifiPanel.transform, false);
        //RectTransform listRect = listObj.GetComponent<RectTransform>();

        // NetworkList container pozisyonu
        listRect.anchorMin = new Vector2(0, 0);
        listRect.anchorMax = new Vector2(1, 1);
        listRect.offsetMin = new Vector2(20, 100); // Alt boşluk
        listRect.offsetMax = new Vector2(-20, -20); // Üst boşluk

        // Scroll View oluştur
        GameObject scrollView = new GameObject("ScrollView", typeof(RectTransform));
        scrollView.transform.SetParent(listObj.transform, false);
        ScrollRect scrollRect = scrollView.AddComponent<ScrollRect>();
        Image scrollImage = scrollView.AddComponent<Image>();
        scrollImage.color = new Color(0.1f, 0.1f, 0.1f, 0.9f);
        RectTransform scrollRectTransform = scrollView.GetComponent<RectTransform>();

        // ScrollView pozisyonu
        scrollRectTransform.anchorMin = Vector2.zero;
        scrollRectTransform.anchorMax = Vector2.one;
        scrollRectTransform.sizeDelta = Vector2.zero;
        scrollRectTransform.offsetMin = Vector2.zero;
        scrollRectTransform.offsetMax = Vector2.zero;

        GameObject viewport = new GameObject("Viewport", typeof(RectTransform));
        viewport.transform.SetParent(scrollView.transform, false);

        // Add RectTransform
        RectTransform viewportRect = viewport.GetComponent<RectTransform>();

        // Add Image (transparent background)
        Image viewportImage = viewport.AddComponent<Image>();
        viewportImage.color = Color.clear;
        viewportImage.raycastTarget = false; // <-- important

        // Only add RectMask2D, NOT Mask
        viewport.AddComponent<RectMask2D>();

        // Viewport pozisyonu
        viewportRect.anchorMin = Vector2.zero;
        viewportRect.anchorMax = Vector2.one;
        viewportRect.sizeDelta = Vector2.zero;
        viewportRect.offsetMin = Vector2.zero;
        viewportRect.offsetMax = Vector2.zero;
        
        // Content oluştur
        GameObject content = new GameObject("Content", typeof(RectTransform));
        content.transform.SetParent(viewport.transform, false);
        RectTransform contentRect = content.GetComponent<RectTransform>();
        VerticalLayoutGroup verticalLayout = content.AddComponent<VerticalLayoutGroup>();
        ContentSizeFitter contentSizeFitter = content.AddComponent<ContentSizeFitter>();

        // Content ayarları
        verticalLayout.padding = new RectOffset(10, 10, 10, 10);
        verticalLayout.spacing = 10;
        verticalLayout.childAlignment = TextAnchor.UpperCenter;
        verticalLayout.childControlHeight = false;
        verticalLayout.childForceExpandHeight = false;

        contentSizeFitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;

        // Content pozisyonu
        contentRect.anchorMin = new Vector2(0, 1);
        contentRect.anchorMax = new Vector2(1, 1);
        contentRect.pivot = new Vector2(0.5f, 1);
        contentRect.sizeDelta = new Vector2(0, 0);

        // ScrollRect bileşenlerini ayarla
        scrollRect.viewport = viewportRect;
        scrollRect.content = contentRect;
        scrollRect.horizontal = false;
        scrollRect.vertical = true;
        scrollRect.scrollSensitivity = 15;
        scrollRect.movementType = ScrollRect.MovementType.Clamped;

        // Örnek WiFi ağları
        string[] networks = new string[] 
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

        float buttonHeight = 50f;
        float spacing = 10f;

        networkListContent = content.transform;

        for (int i = 0; i < networks.Length; i++)
        {
            CreateNetworkButton(networks[i], i, buttonHeight, spacing);
        }

        // Scrollbar oluştur
        GameObject scrollbarGO = new GameObject("Scrollbar", typeof(RectTransform));
        scrollbarGO.transform.SetParent(scrollView.transform, false);
        Scrollbar scrollbar = scrollbarGO.AddComponent<Scrollbar>();
        Image scrollbarImage = scrollbarGO.AddComponent<Image>();
        scrollbarImage.color = new Color(0.3f, 0.3f, 0.3f, 1f);

        // Scrollbar handle
        GameObject scrollbarHandle = new GameObject("Handle", typeof(RectTransform));
        scrollbarHandle.transform.SetParent(scrollbarGO.transform, false);
        Image handleImage = scrollbarHandle.AddComponent<Image>();
        handleImage.color = new Color(0.7f, 0.7f, 0.7f, 1f);

        // Scrollbar ayarları
        scrollbar.handleRect = scrollbarHandle.GetComponent<RectTransform>();
        scrollbar.direction = Scrollbar.Direction.BottomToTop;
        scrollRect.verticalScrollbar = scrollbar;

        // Scrollbar pozisyonu
        RectTransform scrollbarRect = scrollbarGO.GetComponent<RectTransform>();
        scrollbarRect.anchorMin = new Vector2(1, 0);
        scrollbarRect.anchorMax = new Vector2(1, 1);
        scrollbarRect.pivot = new Vector2(1, 1);
        scrollbarRect.sizeDelta = new Vector2(10, 0);
        scrollbarRect.anchoredPosition = new Vector2(0, 0);

        // Handle pozisyonu
        RectTransform handleRect = scrollbarHandle.GetComponent<RectTransform>();
        handleRect.sizeDelta = new Vector2(0, 0);
        handleRect.anchorMin = new Vector2(0, 0);
        handleRect.anchorMax = new Vector2(1, 1);
    }

void CreateNetworkButton(string networkName, int index, float height, float spacing)
{
    // Button GameObject'i oluştur
    GameObject buttonObj = new GameObject("Network_" + networkName, typeof(RectTransform));
    buttonObj.transform.SetParent(networkListContent, false);
    
    // Button bileşenleri
    RectTransform buttonRect = buttonObj.GetComponent<RectTransform>();
    Button button = buttonObj.AddComponent<Button>();
    Image buttonImage = buttonObj.AddComponent<Image>();
    
    // Button görünüm ayarları
    buttonImage.color = new Color(0.2f, 0.2f, 0.2f, 1f);

    // Button boyut ve pozisyon ayarları
    buttonRect.anchorMin = new Vector2(0, 1);
    buttonRect.anchorMax = new Vector2(1, 1);
    buttonRect.pivot = new Vector2(0.5f, 1);
    buttonRect.sizeDelta = new Vector2(-20, height);
    buttonRect.anchoredPosition = new Vector2(0, -index * (height + spacing));

    // Text GameObject'i oluştur
    GameObject textObj = new GameObject("Text", typeof(RectTransform));
    textObj.transform.SetParent(buttonObj.transform, false);
    
    // Text bileşenleri ve ayarları
    RectTransform textRect = textObj.GetComponent<RectTransform>();
    TextMeshProUGUI buttonText = textObj.AddComponent<TextMeshProUGUI>();
    buttonText.text = networkName;
    buttonText.fontSize = 18;
    buttonText.alignment = TextAlignmentOptions.Left;
    buttonText.color = Color.white;
    
    // Text pozisyon ayarları
    textRect.anchorMin = Vector2.zero;
    textRect.anchorMax = Vector2.one;
    textRect.offsetMin = new Vector2(10, 2);
    textRect.offsetMax = new Vector2(-10, -2);

    // Button hover ve pressed renk ayarları
    ColorBlock colors = button.colors;
    colors.normalColor = new Color(0.2f, 0.2f, 0.2f, 1f);
    colors.highlightedColor = new Color(0.3f, 0.3f, 0.3f, 1f);
    colors.pressedColor = new Color(0.15f, 0.15f, 0.15f, 1f);
    colors.selectedColor = new Color(0.25f, 0.25f, 0.25f, 1f);
    button.colors = colors;

    // WiFi ikonu ekle
    GameObject wifiIcon = new GameObject("WifiIcon", typeof(RectTransform));
    wifiIcon.transform.SetParent(buttonObj.transform, false);
    RectTransform wifiRect = wifiIcon.GetComponent<RectTransform>();
    TextMeshProUGUI wifiSymbol = wifiIcon.AddComponent<TextMeshProUGUI>();
    wifiSymbol.text = "📶"; // WiFi emoji sembolü
    wifiSymbol.fontSize = 20;
    wifiSymbol.alignment = TextAlignmentOptions.Right;
    wifiSymbol.color = new Color(0.7f, 0.7f, 0.7f, 1f);

    // WiFi ikon pozisyonu
    wifiRect.anchorMin = new Vector2(1, 0);
    wifiRect.anchorMax = new Vector2(1, 1);
    wifiRect.pivot = new Vector2(1, 0.5f);
    wifiRect.sizeDelta = new Vector2(30, 0);
    wifiRect.anchoredPosition = new Vector2(-5, 0);

    // Button tıklama olayı
    string network = networkName;
    button.onClick.AddListener(() => {
        SelectNetwork(network);
        // Seçili butonun rengini değiştir
        foreach (Transform child in networkListContent)
        {
            Button btn = child.GetComponent<Button>();
            if (btn != null)
            {
                btn.GetComponent<Image>().color = new Color(0.2f, 0.2f, 0.2f, 1f);
            }
        }
        buttonImage.color = new Color(0.4f, 0.4f, 0.8f, 1f);
    });
}

// Ağ seçme fonksiyonu
void SelectNetwork(string networkName)
{
    selectedNetwork = networkName;
    // Removed the ShowMessage call
}

void CreatePasswordField()
{
    GameObject inputObj = new GameObject("PasswordInput");
    inputObj.transform.SetParent(wifiPanel.transform, false);

    passwordInput = inputObj.AddComponent<TMP_InputField>();
    Image inputImage = inputObj.AddComponent<Image>();
    inputImage.color = Color.white;

    // Simple progress bar background
    GameObject barBg = new GameObject("ProgressBarBackground");
    barBg.transform.SetParent(inputObj.transform, false);
    Image barBgImage = barBg.AddComponent<Image>();
    barBgImage.color = new Color(0.2f, 0.2f, 0.2f, 0.8f);
    
    GameObject barFill = new GameObject("ProgressBarFill");
    barFill.transform.SetParent(barBg.transform, false);
    Image barFillImage = barFill.AddComponent<Image>();
    barFillImage.color = new Color(0.3f, 0.3f, 0.3f, 1f); // Simple gray fill

    // Set up the progress bar position
    RectTransform barBgRect = barBg.GetComponent<RectTransform>();
    barBgRect.anchorMin = new Vector2(0, 0);
    barBgRect.anchorMax = new Vector2(1, 0);
    barBgRect.sizeDelta = new Vector2(0, 3);
    barBgRect.anchoredPosition = new Vector2(0, -5);

    RectTransform barFillRect = barFill.GetComponent<RectTransform>();
    barFillRect.anchorMin = Vector2.zero;
    barFillRect.anchorMax = Vector2.one;
    barFillRect.sizeDelta = Vector2.zero;
    barFillRect.localScale = new Vector3(0, 1, 1);

    GameObject placeholderObj = new GameObject("Placeholder");
    placeholderObj.transform.SetParent(inputObj.transform, false);
    TextMeshProUGUI placeholder = placeholderObj.AddComponent<TextMeshProUGUI>();
    placeholder.text = "WiFi Şifresi";
    placeholder.fontSize = 18;
    placeholder.alignment = TextAlignmentOptions.Left;
    placeholder.color = new Color(0.5f, 0.5f, 0.5f);

    GameObject textObj = new GameObject("Text");
    textObj.transform.SetParent(inputObj.transform, false);
    TextMeshProUGUI inputText = textObj.AddComponent<TextMeshProUGUI>();
    inputText.fontSize = 18;
    inputText.alignment = TextAlignmentOptions.Left;
    inputText.color = Color.black;

    RectTransform placeholderRect = placeholderObj.GetComponent<RectTransform>();
    placeholderRect.anchorMin = Vector2.zero;
    placeholderRect.anchorMax = Vector2.one;
    placeholderRect.offsetMin = new Vector2(10, 0);
    placeholderRect.offsetMax = new Vector2(-10, 0);

    RectTransform textRect = textObj.GetComponent<RectTransform>();
    textRect.anchorMin = Vector2.zero;
    textRect.anchorMax = Vector2.one;
    textRect.offsetMin = new Vector2(10, 0);
    textRect.offsetMax = new Vector2(-10, 0);

    passwordInput.placeholder = placeholder;
    passwordInput.textComponent = inputText;
    passwordInput.contentType = TMP_InputField.ContentType.Password;

    // Position the input field lower
    RectTransform inputRect = inputObj.GetComponent<RectTransform>();
    inputRect.anchorMin = new Vector2(0.5f, 0);
    inputRect.anchorMax = new Vector2(0.5f, 0);
    inputRect.sizeDelta = new Vector2(300, 40);
    inputRect.anchoredPosition = new Vector2(0, 70);

    // Simple progress update
    passwordInput.onValueChanged.AddListener((value) => {
        float progress = Mathf.Min(1.0f, value.Length / 8.0f);
        barFillRect.localScale = new Vector3(progress, 1, 1);
    });
}

    void CreateConnectButton()
    {
        GameObject buttonObj = new GameObject("ConnectButton");
        buttonObj.transform.SetParent(wifiPanel.transform, false);

        connectButton = buttonObj.AddComponent<Button>();
        Image buttonImage = buttonObj.AddComponent<Image>();
        buttonImage.color = new Color(0.3f, 0.8f, 0.3f);

        GameObject textObj = new GameObject("Text");
        textObj.transform.SetParent(buttonObj.transform, false);
        TextMeshProUGUI buttonText = textObj.AddComponent<TextMeshProUGUI>();
        buttonText.text = "Bağlan";
        buttonText.fontSize = 20;
        buttonText.alignment = TextAlignmentOptions.Center;
        buttonText.color = Color.white;

        RectTransform textRect = textObj.GetComponent<RectTransform>();
        textRect.anchorMin = Vector2.zero;
        textRect.anchorMax = Vector2.one;
        textRect.offsetMin = Vector2.zero;
        textRect.offsetMax = Vector2.zero;

        RectTransform buttonRect = buttonObj.GetComponent<RectTransform>();
        buttonRect.anchorMin = new Vector2(0.5f, 0);
        buttonRect.anchorMax = new Vector2(0.5f, 0);
        buttonRect.sizeDelta = new Vector2(200, 50);
        buttonRect.anchoredPosition = new Vector2(0, 20);

        connectButton.onClick.AddListener(TryConnect);
    }

    void CreateMessageText()
    {
        GameObject messageObj = new GameObject("MessageText");
        messageObj.transform.SetParent(wifiPanel.transform, false);

        messageText = messageObj.AddComponent<TextMeshProUGUI>();
        messageText.fontSize = 18;
        messageText.alignment = TextAlignmentOptions.Center;
        messageText.color = Color.white;
        messageText.text = "";

        RectTransform rect = messageObj.GetComponent<RectTransform>();
        rect.anchorMin = new Vector2(0, 0);
        rect.anchorMax = new Vector2(1, 0);
        rect.sizeDelta = new Vector2(0, 30);
        rect.anchoredPosition = new Vector2(0, 10);
    }

    public void ToggleWiFiPanel(RectTransform wifiButtonRect)
    {
        if (!wifiPanel.activeInHierarchy)
        {
            // Position panel above and to the left of the WiFi button
            RectTransform panelRect = wifiPanel.GetComponent<RectTransform>();
            Vector2 buttonPosition = wifiButtonRect.position;
        
            // Adjust these offset values to fine-tune the position
            float leftOffset = 175f;  // Move left by 100 units
            float verticalOffset = panelRect.sizeDelta.y - 200f;  // Lower the panel by 50 units
        
            wifiPanel.transform.position = new Vector3(
                buttonPosition.x - leftOffset, 
                buttonPosition.y + verticalOffset, 
                0
            );
        }
        wifiPanel.SetActive(!wifiPanel.activeInHierarchy);
    }

    void TryConnect()
    {
        if (string.IsNullOrEmpty(selectedNetwork))
        {
            ShowMessage("Lütfen bir ağ seçin!", Color.red);
            return;
        }

        if (string.IsNullOrEmpty(passwordInput.text))
        {
            ShowMessage("Lütfen şifre girin!", Color.red);
            return;
        }

        if (selectedNetwork == "Teknopark Ankara Misafir" && passwordInput.text == "123")
        {
            isConnected = true;
            ShowMessage("Başarılı! Doğru WiFi ağına bağlandınız!", Color.green);
            // Close the panel after successful connection
            wifiButtonImage.sprite = connectedWifiIcon;
            wifiButtonImage.SetAllDirty(); // Görüntüyü yenilemeye zorla
            wifiPanel.SetActive(false);

        }
        else
        {
            ShowMessage("Yanlış şifre veya yanlış ağ!", Color.red);
        }
    }

    void ShowMessage(string message, Color color)
    {
        messageText.text = message;
        messageText.color = color;
    }
}