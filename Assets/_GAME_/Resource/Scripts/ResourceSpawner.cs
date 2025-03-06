using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceSpawner : MonoBehaviour
{
    // Reference to Resource prefab.
    public GameObject resource;

    // Centering offset for Resource GameObject.
    private const float CENTRE_OFFSET = 0.5f;

    // Reference to GameLogic GameObject.
    [SerializeField] GameObject logic;

    int numberOfResources;

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
        // Instantiating Player GameObject, renaming to distinguish instantiations.
        GameObject rsrc = Instantiate(resource, findLocation(), transform.rotation);
    }

    Vector3 findLocation()
    {
        // Arbitrary map bounds so player only generates on a tile on screen.
        /*float minX = -10.5f;
        float maxX = 10.5f;
        float minY = -3.5f;
        float maxY = 3.5f;*/

        
        //return new Vector3(Random.Range(minX, maxX) + CENTRE_OFFSET, Random.Range(minY, maxY) + CENTRE_OFFSET, 0f);

        /*
        *   Changed randomization to random position on a tile, hard-coded based on the current map size
        */

        float x_coord = Random.Range(1, 22) - 11.5f;
        float y_coord = Random.Range(1, 8) - 4.5f;

        return new Vector3(x_coord, y_coord, 0.0f);
    }
}
