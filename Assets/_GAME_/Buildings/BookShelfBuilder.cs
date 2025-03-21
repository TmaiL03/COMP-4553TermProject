using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Tilemaps;

public class BookShelfBuilder : MonoBehaviour
{
    public GameObject[] bookshelves;
    int bookshelfGoldCost = 30;
    private TurnManager turnManager;

    private void Start()
    {
        turnManager = FindObjectOfType<TurnManager>();
    }

    public void TryBuildBookshelf(Player_Controller player)
    {
        if (player.currency >= bookshelfGoldCost)
        {
            Debug.Log("Building bookshelf!");

            Vector3 playerPosition = player.transform.position;
            Vector3 tilePosition = player.groundTilemap.GetCellCenterWorld(player.groundTilemap.WorldToCell(playerPosition));

            Debug.Log("Player Number: " + player.playerNumber);

            Instantiate(bookshelves[player.playerNumber - 1], tilePosition, Quaternion.identity);

            player.currency -= bookshelfGoldCost;
            player.UpdateCurrencyUI();
            player.UpdateBookshelfUI();
        }
        else
        {
            Debug.Log("Not enough gold to build!");
        }
    }
}
