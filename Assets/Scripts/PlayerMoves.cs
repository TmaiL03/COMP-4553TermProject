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
        playerController = FindObjectOfType<Player_Controller>();

        if (playerController == null)
        {
            Debug.LogError("Player_Controller not found in the scene!");
            return;
        }

        UpdateMovesUI(playerController.moves);
    }

    public void UpdateMovesUI(int amount)
    {
        movesText.text = "Moves: " + amount.ToString();
    }
}
