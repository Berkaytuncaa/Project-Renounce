using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    [SerializeField] private GameObject unsereText;

    private void Start()
    {
        StartCoroutine(LoadNextSceneAfterDelay());
    }

    private IEnumerator LoadNextSceneAfterDelay()
    {
        StartCoroutine(RevealText());
        yield return new WaitForSeconds(4f);
      
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
        else
        {
            SceneManager.LoadScene("MainMenu");
        }
    }

    private IEnumerator RevealText()
    {
        yield return new WaitForSeconds(2.3f);
        unsereText.SetActive(true);
    }
}
