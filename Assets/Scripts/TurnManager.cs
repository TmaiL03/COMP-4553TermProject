using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using static UnityEditor.Experimental.GraphView.GraphView;

public class TurnManager : MonoBehaviour
{
    public List<Player_Controller> players = new List<Player_Controller>();
    public TextMeshProUGUI turnText;
    private int currentPlayerIndex = 0;
    public GameObject notEnoughWoodPanel;

    void Start()
    {
        notEnoughWoodPanel.SetActive(false);
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
                players[currentPlayerIndex].UpdateWoodUI();
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

        // Disable controls for the current player
        players[currentPlayerIndex].SetInputState(false);
        players[currentPlayerIndex].turn = false;

        currentPlayerIndex = (currentPlayerIndex + 1) % players.Count;

        players[currentPlayerIndex].moves += 5;

        // Enable controls for the new active player
        players[currentPlayerIndex].SetInputState(true);
        players[currentPlayerIndex].turn = true;

        players[currentPlayerIndex].UpdateCurrencyUI();
        players[currentPlayerIndex].UpdateWoodUI();
        players[currentPlayerIndex].UpdateMovesUI();
        UpdateTurnUI();
    }

    public void TryBuildHouse()
    {
        if (players[currentPlayerIndex].wood >= players[currentPlayerIndex].settlementWoodCost)
        {
            Vector3 playerPosition = players[currentPlayerIndex].transform.position;
            Vector3 tilePosition = players[currentPlayerIndex].groundTilemap.GetCellCenterWorld(players[currentPlayerIndex].groundTilemap.WorldToCell(playerPosition));

            Instantiate(players[currentPlayerIndex].settlementPrefab, tilePosition, Quaternion.identity);

            players[currentPlayerIndex].wood -= players[currentPlayerIndex].settlementWoodCost;
            players[currentPlayerIndex].UpdateWoodUI();
        }
        else
        {
            Debug.Log("Not enough wood to build!");

            notEnoughWoodPanel.SetActive(true);
            StartCoroutine(HidePanelAfterDelay(2f));
        }
    }

    private IEnumerator HidePanelAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (notEnoughWoodPanel != null)
        {
            notEnoughWoodPanel.SetActive(false);
        }
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
