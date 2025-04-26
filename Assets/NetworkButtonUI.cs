using UnityEngine;
using UnityEngine.UI;

public class NetworkButtonUI : MonoBehaviour
{
    [SerializeField] private Image wifiIcon;
    [SerializeField] private Text networkNameText;
    
    public void SetNetworkName(string name)
    {
        networkNameText.text = name;
    }
}