using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ResourceSpawner : MonoBehaviour
{
    // Reference to Resource prefab.
    public GameObject resource;

    // Centering offset for Resource GameObject.
    private const float CENTRE_OFFSET = 0.5f;

    // Reference to GameLogic GameObject.
    [SerializeField] GameObject logic;

    int numberOfResources;
    
    // Defines probability a Resource is Fool's Gold.
    [SerializeField] private int foolsProb;

    // Start is called before the first frame update
    void Start()
    {
        // Obtaining reference to GameLogicScript.cs, then getting number of players.
        GameLogicScript logicScript = logic.GetComponent<GameLogicScript>();
        numberOfResources = logicScript.NumberOfResources;

        // Spawning in the corresponding number of Player GameObjects.
        for (int i = 0; i < numberOfResources; i++)
        {
            spawnResource();

        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    // Instantiates a new Player GameObject.
    public void spawnResource()
    {
        // Instantiating Resource GameObject, renaming to distinguish instantiations.
        GameObject rsrc = Instantiate(resource, findLocation(), transform.rotation);
        ResourceController fools = rsrc.GetComponent<ResourceController>();

        // Sets fool's gold Boolean depending on provided probability.
        fools.fools = setFoolsBool(foolsProb);
    }

    // Calculates if the current Resource instance will be a fool's gold instance or not.
    public bool setFoolsBool(int prob) {

        // Integer Random.Range() overload maxbound is exclusive, so +1 must be added.
        int probVal = Random.Range(1, prob + 1);

        // If generated probability value is 1, return true, else false.
        if(probVal == 1) {

            return true;

        } else {

            return false;

        }

    }

    // Changed so Resources spawn in center of tile in grid
    Vector3 findLocation()
    {
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


        return worldPosition;
    }
}
