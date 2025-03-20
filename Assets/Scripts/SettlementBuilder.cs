using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Tilemaps;

public class SettlementBuilder : MonoBehaviour
{
    public GameObject[] settlements;
    int settlementWoodCost = 10;
    private TurnManager turnManager;

    private void Start()
    {
        turnManager = FindObjectOfType<TurnManager>();
    }

    public void TryBuildSettlement(Player_Controller player)
    {
        if (player.wood >= settlementWoodCost)
        {
            Debug.Log("Building house!");

            Vector3 playerPosition = player.transform.position;
            Vector3 tilePosition = player.groundTilemap.GetCellCenterWorld(player.groundTilemap.WorldToCell(playerPosition));

            Debug.Log("Player Number: " + player.playerNumber);

            Instantiate(settlements[player.playerNumber - 1], tilePosition, Quaternion.identity);

            player.wood -= settlementWoodCost;
            player.UpdateWoodUI();
        }
        else
        {
            Debug.Log("Not enough wood to build!");
        }
    }
}
