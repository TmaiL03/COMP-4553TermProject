using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using static UnityEditor.Experimental.GraphView.GraphView;

public class TurnManager : MonoBehaviour
{
    private List<Player_Controller> players = new List<Player_Controller>();
    public TextMeshProUGUI turnText;
    private int currentPlayerIndex = 0;

    void Start()
    {
        StartCoroutine(WaitForPlayers());
    }

    private IEnumerator WaitForPlayers()
    {
        yield return null;

        players = new List<Player_Controller>(FindObjectsOfType<Player_Controller>());

        players.Sort((player1, player2) => player1.playerNumber.CompareTo(player2.playerNumber));

        if (players.Count > 0)
        {
            Player_Controller playerOne = players.Find(player => player.playerNumber == 1);

            if (playerOne != null)
            {
                currentPlayerIndex = players.IndexOf(playerOne);
                players[currentPlayerIndex].turn = true;

                players[currentPlayerIndex].moves = 5;

                UpdateTurnUI();
                players[currentPlayerIndex].UpdateCurrencyUI();
                players[currentPlayerIndex].UpdateMovesUI();
                players[currentPlayerIndex].UpdatePlayerNumberUI();
            } else
            {
                Debug.Log("Player One not found in the scene");
            }
        } else
        {
            Debug.LogError("No players found in the scene!");
        }
    }

    // Activated when the 'End Turn' button is clicked
    public void EndTurn()
    {
        if (players.Count == 0)
        {
            return;
        }

        players[currentPlayerIndex].moves += 5;

        // Disable controls for the current player
        players[currentPlayerIndex].SetInputState(false);
        players[currentPlayerIndex].turn = false;

        currentPlayerIndex = (currentPlayerIndex + 1) % players.Count;
        
        // Enable controls for the new active player
        players[currentPlayerIndex].SetInputState(true);
        players[currentPlayerIndex].turn = true;

        players[currentPlayerIndex].UpdateCurrencyUI();
        players[currentPlayerIndex].UpdateMovesUI();
        UpdateTurnUI();
    }

    // Updates the UI to show whose turn it is
    public void UpdateTurnUI()
    {
        if (turnText != null)
        {
            turnText.text = "Player: " + (players[currentPlayerIndex].playerNumber);
        }
    }
}
