using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerNumber : MonoBehaviour
{
    public TextMeshProUGUI playerNumberText;
    private Player_Controller playerController;

    private void Start()
    {
        StartCoroutine(FindPlayerController());
    }

    private IEnumerator FindPlayerController()
    {
        while (playerController == null)
        {
            playerController = FindObjectOfType<Player_Controller>();
            yield return null;
        }
    }

    public void UpdatePlayerNumberUI(int number)
    {
        playerNumberText.text = "Player: " + number.ToString();
    }
}
