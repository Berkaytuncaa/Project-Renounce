using UnityEngine;

public class Openchrome : MonoBehaviour
{
    [SerializeField] private GameObject panel; // Reference to your panel

    public void OpenPanel()
    {
        if (panel != null)
        {
            panel.SetActive(true);
        }
    }

    public void ClosePanel()
    {
        if (panel != null)
        {
            panel.SetActive(false);
        }
    }
}