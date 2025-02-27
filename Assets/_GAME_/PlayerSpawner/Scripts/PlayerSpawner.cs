using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    // Reference to Player prefab.
    public GameObject player;
    // Centering offset for Player GameObject.
    private const float CENTRE_OFFSET = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        spawnPlayer();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Instantiates a new Player GameObject.
    void spawnPlayer() {

        // Arbitrary map bounds so player only generates on a tile on screen.
        int minX = -11;
        int maxX = 10;
        int minY = -5;
        int maxY = 3;
        
        Instantiate(player, new Vector3(Random.Range(minX, maxX) + CENTRE_OFFSET, Random.Range(minY, maxY) + CENTRE_OFFSET, 0f), transform.rotation);

    }
}
