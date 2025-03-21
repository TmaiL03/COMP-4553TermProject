using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerSpawner : MonoBehaviour
{
    // Reference to Player prefab.
    public GameObject[] players;

    // Centering offset for Player GameObject.
    private const float CENTRE_OFFSET = 0.5f;

    // Reference to GameLogic GameObject.
    [SerializeField] GameObject logic;

    // Start is called before the first frame update
    void Start()
    {
        // Obtaining reference to GameLogicScript.cs, then getting number of players.
        GameLogicScript logicScript = logic.GetComponent<GameLogicScript>();
        int numberOfPlayers = logicScript.NumberOfPlayers;
        
        // Spawning in the corresponding number of Player GameObjects.
        for(int i = 0; i < numberOfPlayers; i++) {
            
            string name = "Player " + (i + 1);
            spawnPlayer(name, i + 1);
            
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Changed so player spawns in center of tile
    void spawnPlayer(string name, int playerNum) {
        // Reference to the tilemap (ensure it's assigned correctly in the scene)
        Tilemap groundTilemap = GameObject.Find("Ground")?.GetComponent<Tilemap>();

        Vector3Int tilePosition;
        Vector3 worldPosition;

        do {
            // Generate a random **grid-aligned** tile position within bounds
            int tileX = Random.Range(-10, 10); // Integer values within tilemap bounds
            int tileY = Random.Range(-3, 3);
            tilePosition = new Vector3Int(tileX, tileY, 0);

            // Convert from grid coordinates to world position (centering offset is handled by CellToWorld)
            worldPosition = groundTilemap.GetCellCenterWorld(tilePosition);

        } while (!groundTilemap.HasTile(tilePosition)); // Ensure the tile is valid

        // Instantiate player at snapped world position
        Debug.Log("Plyr spawned" + players[playerNum - 1].name);

        GameObject plyr = GameObject.Find(name);
        if (plyr == null) {
            GameObject player = Instantiate(players[playerNum - 1], worldPosition, Quaternion.identity);
            player.name = name;

            Player_Controller playerController = player.GetComponent<Player_Controller>();
            if (playerController != null) {
                playerController.playerNumber = playerNum;

                if (playerNum == 1) {
                    playerController.turn = true;
                }        
            }
        } else {
            Debug.LogWarning("Player already exists: " + name);
        }

        // // Assign player number
        // Player_Controller playerController = plyr.GetComponent<Player_Controller>();
        // if (playerController != null) {
        //     playerController.playerNumber = playerNum;

        //     // Gives player 1 controls to move
        //     if (playerNum == 1) {
        //         playerController.turn = true;
        //     }
        // }
    }

}
