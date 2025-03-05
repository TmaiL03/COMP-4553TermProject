using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class MainMenu : MonoBehaviour
{
    public TMP_Dropdown playerCountDropdown;
    public static int playerCount = 3;

    private void Start()
    {
        playerCountDropdown.onValueChanged.AddListener(UpdatePlayerCount);
    }

    public void UpdatePlayerCount(int index)
    {
        playerCount = index + 2;
        Debug.Log("Player Count Set To: " + playerCount);
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
