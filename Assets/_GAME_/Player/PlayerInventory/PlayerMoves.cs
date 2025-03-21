using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMoves : MonoBehaviour
{
    public TextMeshProUGUI movesText;
    private Player_Controller playerController;

    private void Start()
    {
        // playerController = FindObjectOfType<Player_Controller>();

        // if (playerController == null)
        // {
        //     Debug.LogError("Player_Controller not found in the scene!: player moves");
        //     return;
        // }
        StartCoroutine(WaitForPlayer());
        UpdateMovesUI(playerController.moves);
    }

        IEnumerator WaitForPlayer() {
            Player_Controller player = null;
        
            while (player == null) // Keep checking until Player_Controller exists
            {
                player = FindObjectOfType<Player_Controller>();
                yield return null; // Wait for the next frame
            }

            Debug.Log("Player_Controller found in wood: " + player.name);
        
        // Now safely reference player and continue execution
        }

    public void UpdateMovesUI(int amount)
    {
        movesText.text = "Moves: " + amount.ToString();
    }
}
