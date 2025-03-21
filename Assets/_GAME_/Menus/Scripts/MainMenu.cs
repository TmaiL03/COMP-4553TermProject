using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class MainMenu : MonoBehaviour
{
    public TMP_Dropdown playerCountDropdown;
    public static int playerCount = 2;

    private void Start()
    {
        if (playerCountDropdown != null)
        {
            playerCountDropdown.onValueChanged.AddListener(UpdatePlayerCount);
        }
    }

    public void UpdatePlayerCount(int index)
    {
        playerCount = index + 2;
        PlayerPrefs.SetInt("PlayerCount", playerCount);
        PlayerPrefs.Save();
        Debug.Log("Player Count Saved: " + playerCount);
    }

    public void GoToScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void QuitApp()
    {
        Application.Quit();
        Debug.Log("Application has quit.");
    }
}
