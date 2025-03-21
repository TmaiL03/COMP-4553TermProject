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

    // void Start() {

    //     // Generates TileMap on which game will be played.
    //     GenerateMap();

    // }

    // // Generates the randomly distributed TileMap.
    // void GenerateMap() {

    //     // Obtaining the dimensions of the TileMap.
    //     int xDim = GridMap.cellBounds.size.x;
    //     int yDim = GridMap.cellBounds.size.y;

    //     // Iterating over each Tile in the TileMap instance (in x and y) and selecting a random texture type.
    //     for(int x = -(xDim / 2); x < (xDim / 2); x++) {

    //         for(int y = -(yDim / 2); y < (yDim / 2); y++) {

    //             SetTileAtPosition(new Vector3Int(x, y, 0));

    //         }

    //     }

    // }

    // // Sets the Tile at the given Vector3 position.
    // public void SetTileAtPosition(Vector3Int position) {

    //     // Obtaining the current tile 
    //     TileBase currentTile = GridMap.GetTile(position);

    //     // Checking in a Tile exists at the provided position.
    //     if(currentTile != null) {
            
    //         // Setting the Tile at the provided position.
    //         GridMap.SetTile(position, TileList[Random.Range(0, TileList.Length)]);
    //         Debug.Log("Tile at " + position + " has been changed.");

    //     } else {

    //         //Debug.LogWarning("No tile found at position " + position);

    //     }

    // }

}
