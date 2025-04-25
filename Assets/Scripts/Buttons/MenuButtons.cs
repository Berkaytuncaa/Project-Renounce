using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButtons : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] protected int buttonNum;

    public void ClickButton()
    {
        switch (buttonNum)
        {
            case 0:
                StartButton();
                break;
            case 1:
                SoundButton();
                break;
            case 2:
                ExitButton();
                break;
        }
    }

    void StartButton()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    void SoundButton()
    {
        // Add a set that active-deactivate soundbar
    }
    void ExitButton()
    {
        Application.Quit();
    }
}
