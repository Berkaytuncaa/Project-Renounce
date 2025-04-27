using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadAfterAnim : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(LoadNextSceneAfterDelay());
    }

    private IEnumerator LoadNextSceneAfterDelay()
    {
        yield return new WaitForSeconds(5.3f);

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
}
