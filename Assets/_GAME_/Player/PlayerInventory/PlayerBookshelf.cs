using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerBookshelf : MonoBehaviour
{
    public TextMeshProUGUI bookshelfText;
    private Player_Controller playerController;

    private void Start()
    {
        // playerController = FindObjectOfType<Player_Controller>();

        // if (playerController == null)
        // {
        //     Debug.LogError("Player_Controller not found in the scene!: Bookshelf");
        //     return;
        // }
        StartCoroutine(WaitForPlayer());
    }

    IEnumerator WaitForPlayer() {
        Player_Controller player = null;
        
        while (player == null) // Keep checking until Player_Controller exists
        {
            player = FindObjectOfType<Player_Controller>();
            yield return null; // Wait for the next frame
        }

        Debug.Log("Player_Controller found in wood: " + player.name);
        playerController = player;
        UpdateBookshelfUI(playerController.food);
        // Now safely reference player and continue execution
    }

    public void UpdateBookshelfUI(int amount)
    {
        bookshelfText.text = amount.ToString() + "/3";
    }
}
