using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    // Reference to Player prefab.
    public GameObject player;

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
        
        string name;

        // Spawning in the corresponding number of Player GameObjects.
        for(int i = 0; i < numberOfPlayers; i++) {
            
            name = "Player " + (i + 1);
            spawnPlayer(name);
            
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Instantiates a new Player GameObject.
    void spawnPlayer(string name) {

        // Arbitrary map bounds so player only generates on a tile on screen.
        float minX = -10.5f;
        float maxX = 10.5f;
        float minY = -3.5f;
        float maxY = 3.5f;
        
        // Instantiating Player GameObject, renaming to distinguish instantiations.
        GameObject plyr = Instantiate(player, new Vector3(Random.Range(minX, maxX) + CENTRE_OFFSET, Random.Range(minY, maxY) + CENTRE_OFFSET, 0f), transform.rotation);
        plyr.name = name;

    }
}
