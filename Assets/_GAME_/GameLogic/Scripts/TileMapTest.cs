using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileMapTest : MonoBehaviour
{

    // Reference to the TileMap.
    public Tilemap myTileMap;
    // Reference list containing all permissible Tile types.
    public Tile[] TileList;

    // Sets the Tile at the given Vector3 position.
    public void SetTileAtPosition(Vector3Int position) {

        // Obtaining the current tile 
        TileBase currentTile = myTileMap.GetTile(position);

        // Checking in a Tile exists at the provided position.
        if(currentTile != null) {
            
            // Setting the Tile at the provided position.
            myTileMap.SetTile(position, TileList[Random.Range(0, TileList.Length)]);
        } else {

            //Debug.LogWarning("No tile found at position " + position);

        }

    }

    void Start() {

        // Obtaining the dimensions of the TileMap.
        int xDim = myTileMap.cellBounds.size.x;
        int yDim = myTileMap.cellBounds.size.y;

        // Iterating over each Tile in the TileMap instance and selecting a random texture type.
        for(int x = -(xDim / 2); x < (xDim / 2); x++) {

            for(int y = -(yDim / 2); y < (yDim / 2); y++) {

                SetTileAtPosition(new Vector3Int(x, y, 0));

            }

        }

    }

}
