using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameLogicScript : MonoBehaviour
{
    [Header("Game Settings")]
    // Used to specify the number of players participating in the game.
    public int NumberOfPlayers;
    // Reference to the TileMap.
    public Tilemap GridMap;
    // Reference list containing all permissible Tile types.
    public Tile[] TileList;

    public int NumberOfResources = 3;   
    public int AmountOfWood = 3;
    public int AmountOfFood = 3;

    void Awake()
    {

        if (PlayerPrefs.HasKey("PlayerCount")) {
            NumberOfPlayers = PlayerPrefs.GetInt("PlayerCount");
            Debug.Log("Loaded Player Count: " + NumberOfPlayers);
        } else {
            Debug.Log("Used default num of players: ");
            NumberOfPlayers = 2; // Default value
        }
        // NumberOfPlayers = MainMenu.playerCount;
        //Debug.Log("Number of Players Set to: " + NumberOfPlayers);

    }

}
