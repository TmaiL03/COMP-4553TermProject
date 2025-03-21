using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Tilemaps;

public class FarmBuilder : MonoBehaviour
{
    public GameObject[] farms;
    int farmMeatCost = 10;
    private TurnManager turnManager;

    private void Start()
    {
        turnManager = FindObjectOfType<TurnManager>();
    }

    public void TryBuildSettlement(Player_Controller player)
    {
        if (player.food >= farmMeatCost)
        {
            Debug.Log("Building farm!");

            Vector3 playerPosition = player.transform.position;
            Vector3 tilePosition = player.groundTilemap.GetCellCenterWorld(player.groundTilemap.WorldToCell(playerPosition));

            Debug.Log("Player Number: " + player.playerNumber);

            Instantiate(farms[player.playerNumber - 1], tilePosition, Quaternion.identity);

            player.food -= farmMeatCost;
            player.UpdateFoodUI();
        }
        else
        {
            Debug.Log("Not enough meat to build!");
        }
    }
}
