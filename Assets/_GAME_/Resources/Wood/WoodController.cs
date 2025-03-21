using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WoodController : MonoBehaviour
{
    int worth = 1;
    bool alreadyTouched = false;
    WoodSpawner woodSpawner;
    public TextMeshPro worth_text;
    // Start is called before the first frame update

    void Start()
    {
        worth = Random.Range(1, 4);
        worth_text.text = worth.ToString();
        GameObject spawner = GameObject.FindWithTag("WoodSpawner");
        //Get the component that holds the function you want to trigger
        woodSpawner = spawner.GetComponent<WoodSpawner>();
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
                playerController.addWood(worth);

            }
            woodSpawner.spawnWood();
            Destroy(gameObject);
        }
    }
}
