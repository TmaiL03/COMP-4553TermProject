using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using static UnityEditor.Experimental.GraphView.GraphView;
using UnityEngine.SceneManagement;
//using System.Numerics;

public class TurnManager : MonoBehaviour
{
    private List<Vector3> settlementPositions = new List<Vector3>();
    private List<Vector3> farmPositions = new List<Vector3>();
    public List<Player_Controller> players = new List<Player_Controller>();
    public TextMeshProUGUI turnText;
    private int currentPlayerIndex = 0;
    public GameObject notEnoughWoodPanel;
    public GameObject notEnoughMeatPanel;
    public GameObject[] settlements;
    public GameObject[] farms;

    void Start()
    {
        notEnoughWoodPanel.SetActive(false);
        notEnoughMeatPanel.SetActive(false);

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

        int numOfSettlements = players[currentPlayerIndex].settlements;
        int numOfFarms = players[currentPlayerIndex].farms;

        // For each settlement a player has, they get 3 coins at the start of each turn
        for (int i = 0; i < numOfSettlements; i++)
        {
            players[currentPlayerIndex].currency += 3;
        }

        for (int i = 0; i < numOfFarms; i++)
        {
            players[currentPlayerIndex].currency += 3;
        }

        players[currentPlayerIndex].UpdateCurrencyUI();
        players[currentPlayerIndex].UpdateWoodUI();
        players[currentPlayerIndex].UpdateFoodUI();
        players[currentPlayerIndex].UpdateMovesUI();
        UpdateTurnUI();
    }

    public void TryBuildHouse()
    {
        if (players[currentPlayerIndex].wood >= players[currentPlayerIndex].settlementWoodCost)
        {
            Player_Controller player = players[currentPlayerIndex];

            Vector3 playerPosition = player.transform.position;
            Vector3 tilePosition = player.groundTilemap.GetCellCenterWorld(player.groundTilemap.WorldToCell(playerPosition));

            if (settlementPositions.Contains(tilePosition) || farmPositions.Contains(tilePosition))
            {
                Debug.Log("Cannot Build Here! A Settlement is Already Placed on This Tile!");
                return;
            }

            Instantiate(settlements[player.playerNumber - 1], tilePosition, Quaternion.identity);

            players[currentPlayerIndex].settlements += 1;
            players[currentPlayerIndex].wood -= players[currentPlayerIndex].settlementWoodCost;
            players[currentPlayerIndex].UpdateWoodUI();
            settlementPositions.Add(tilePosition);
        }
        else
        {
            Debug.Log("Not enough wood to build!");

            notEnoughWoodPanel.SetActive(true);
            StartCoroutine(HideWoodPanelAfterDelay(2f));
        }
    }

    public void TryBuildFarm()
    {
        if (players[currentPlayerIndex].food >= players[currentPlayerIndex].farmMeatCost)
        {
            Player_Controller player = players[currentPlayerIndex];

            Vector3 playerPosition = player.transform.position;
            Vector3 tilePosition = player.groundTilemap.GetCellCenterWorld(player.groundTilemap.WorldToCell(playerPosition));

            if (settlementPositions.Contains(tilePosition) || farmPositions.Contains(tilePosition))
            {
                Debug.Log("Cannot Build Here! A Settlement or Farm is Already Placed on This Tile!");
                return;
            }

            Instantiate(farms[player.playerNumber - 1], tilePosition, Quaternion.identity);

            players[currentPlayerIndex].farms += 1;
            players[currentPlayerIndex].food -= players[currentPlayerIndex].farmMeatCost;
            players[currentPlayerIndex].UpdateFoodUI();
            farmPositions.Add(tilePosition);
        }
        else
        {
            Debug.Log("Not enough meat to build!");

            notEnoughMeatPanel.SetActive(true);
            StartCoroutine(HideFoodPanelAfterDelay(2f));
        }
    }

    private IEnumerator HideWoodPanelAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (notEnoughWoodPanel != null)
        {
            notEnoughWoodPanel.SetActive(false);
        }
    }

    private IEnumerator HideFoodPanelAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (notEnoughMeatPanel != null)
        {
            notEnoughMeatPanel.SetActive(false);
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

    public void GoToScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
