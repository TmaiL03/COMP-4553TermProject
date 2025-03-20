using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ResourceController : MonoBehaviour
{
    int worth = 1;
    bool alreadyTouched = false;
    public bool fools;

    List<GameObject> players;

    ResourceSpawner resourceSpawner;

    public TextMeshPro worth_text;
    // Start is called before the first frame update

    void Start()
    {

        worth = Random.Range(1, 4);
        worth_text.text = worth.ToString();
        GameObject spawner = GameObject.FindWithTag("ResourceSpawner");
        //Get the component that holds the function you want to trigger
        resourceSpawner = spawner.GetComponent<ResourceSpawner>();

        // If fools Boolean == true, obtain list of all Player GameObjects.
        if(fools) {

            GameObject[] playersArr = GameObject.FindGameObjectsWithTag("Player");
            players = new List<GameObject>(playersArr);

        }

    }

    void Update()
    {
        
        // Checks if fools Boolean is true.
        if(fools) {

            // Iterates over each Player in the game to obtain distance from this instance.
            foreach(GameObject player in players) {

                // Obtaining Player's distance from Resource instance.
                float playerDist = Vector3.Distance(transform.position, player.transform.position);
                
                // Checks if distance is equal to or less than 1 away from the a Player.
                if(playerDist < 1.1f) {

                    // Spawn new resource, then destroy this object.
                    resourceSpawner.spawnResource();
                    Destroy(this.gameObject);

                } else {

                    continue;

                }

            }

        }

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!alreadyTouched)
        {

            alreadyTouched = true;
            Player_Controller playerController = other.GetComponent<Player_Controller>();

            if (playerController != null)
            {
                // Increment the player's score
                playerController.addScore(worth);

            }

            resourceSpawner.spawnResource();
            Destroy(gameObject);
        }
    }
}
