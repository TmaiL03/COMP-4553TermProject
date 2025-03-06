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


    //This integer determines which player can currently move
    //public int playing = 0;

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
            spawnPlayer(name, i);
            
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Instantiates a new Player GameObject.
    void spawnPlayer(string name, int i) {

        // Arbitrary map bounds so player only generates on a tile on screen.
        int minX = -10;
        int maxX = 10;
        int minY = -3;
        int maxY = 3;
        
        // Instantiating Player GameObject, renaming to distinguish instantiations.
        GameObject plyr = Instantiate(player, new Vector3(Random.Range(minX, maxX) * 1.0f + CENTRE_OFFSET, Random.Range(minY, maxY) * 1.0f + CENTRE_OFFSET, 0f), transform.rotation);
        plyr.name = name;
        plyr.GetComponent<Player_Controller>().setPlayerID(i);

    }
}
