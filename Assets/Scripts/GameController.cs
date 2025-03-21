using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    // Call this function when the player clicks "Play"
    public void LoadMainGameScene()
    {
        // Start loading the scene asynchronously
        StartCoroutine(LoadSceneAsync("HomeTown"));
    }

    // Coroutine to load the scene asynchronously
    private IEnumerator LoadSceneAsync(string sceneName)
    {
        // Display loading screen or animation here if desired

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            // Optionally update a loading bar or display progress
            float progress = Mathf.Clamp01(asyncLoad.progress / 0.9f);  // 0.9 is where the scene is considered loaded
            // Update your loading bar here (if you have one)
            yield return null;
        }
    }
}
