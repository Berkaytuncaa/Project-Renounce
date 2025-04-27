using UnityEngine;

public class TextTrigger : MonoBehaviour
{
    [SerializeField] private GameObject dialogueBox;

    void OnTriggerEnter2D(Collider2D collision)
    {
        dialogueBox.SetActive(true);
    }
}
