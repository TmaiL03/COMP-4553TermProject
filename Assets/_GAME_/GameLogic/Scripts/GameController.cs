using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    
    // // Call this function when the player clicks "Play"
    // public void LoadMainGameScene()
    // {
    //     // Start loading the scene asynchronously
    //     StartCoroutine(LoadSceneAsync("HomeTown"));
    // }

     public void UpdatePlayerCount(int index) {
        index += 2;
        PlayerPrefs.SetInt("PlayerCount", index);
        PlayerPrefs.Save();
        Debug.Log("Player Count Saved: " + PlayerPrefs.GetInt("PlayerCount"));
        
    }

    public void GoToScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    // Coroutine to load the scene asynchronously
    // public IEnumerator LoadSceneAsync(string sceneName)
    // {
    //     // Display loading screen or animation here if desired

    //     AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);

    //     // Wait until the asynchronous scene fully loads
    //     while (!asyncLoad.isDone)
    //     {
    //         // Optionally update a loading bar or display progress
    //         float progress = Mathf.Clamp01(asyncLoad.progress / 0.9f);  // 0.9 is where the scene is considered loaded
    //         // Update your loading bar here (if you have one)
    //         yield return null;
    //     }
    // }
}
