using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using TMPro;
// using static UnityEditor.Experimental.GraphView.GraphView;
using UnityEngine.SceneManagement;
//using System.Numerics;

public class TurnManager : MonoBehaviour
{
    private List<Vector3> settlementPositions = new List<Vector3>();
    private List<Vector3> farmPositions = new List<Vector3>();
    private List<Vector3> bookshelfPositions = new List<Vector3>();

    public List<Player_Controller> players = new List<Player_Controller>();
    public TextMeshProUGUI turnText;
    private int currentPlayerIndex = 0;

    public GameObject notEnoughWoodPanel;
    public GameObject notEnoughMeatPanel;
    public GameObject notEnoughGoldPanel;
    public GameObject storyPanel;

    public GameObject[] settlements;
    public GameObject[] farms;
    public GameObject[] bookshelves;

    public Tilemap tilemap;
    public Tile tileToChange;
    public Color changeColor = Color.red;

    private int turnCount = 0;

    private Queue<Vector3Int> spreadQueue = new Queue<Vector3Int>();
    private HashSet<Vector3Int> visitedTiles = new HashSet<Vector3Int>();

    public TextMeshProUGUI storyUI;
    public float typingSpeed = 0.05f;

    string[] apocalypseStory =
    {
        "They told us we had time. A hundred years, they said. That it wouldn't reach us. That we'd be fine... They were wrong.",
        "The ships were distant-just shadows beyond the stars. Light-years away, they assured us. They called them The Watchers. We called it the end.",
        "Their ships landed within days of our discovery. With them came something sinister. Something alive. We don't know if it's a weapon or a plague. We only know its name-The Blight."
    };
    

    void Start()
    {
        notEnoughWoodPanel.SetActive(false);
        notEnoughMeatPanel.SetActive(false);
        notEnoughGoldPanel.SetActive(false);
        storyPanel.SetActive(false);

        StartCoroutine(WaitForPlayers());

        tilemap = GameObject.Find("Ground")?.GetComponent<Tilemap>();
        // Get the center position of the Tilemap in grid coordinates
        Vector3 test = tilemap.cellBounds.center;
        Vector3Int centerInGrid = tilemap.WorldToCell(test);

        // Add the center tile to the spread queue to start spreading from there
        spreadQueue.Enqueue(centerInGrid);
        visitedTiles.Add(centerInGrid);

        // Start the spreading process
        StartCoroutine(SpreadBlight());
    }

    private IEnumerator WaitForPlayers()
    {
        yield return null;

        players = new List<Player_Controller>(FindObjectsOfType<Player_Controller>());

        players.Sort((player1, player2) => player1.playerNumber.CompareTo(player2.playerNumber));

        if (players.Count > 0)
        {
            Player_Controller playerOne = players.Find(player => player.playerNumber == 1);

            if (playerOne != null)
            {
                currentPlayerIndex = players.IndexOf(playerOne);
                players[currentPlayerIndex].turn = true;

                players[currentPlayerIndex].moves = 5;

                UpdateTurnUI();
                players[currentPlayerIndex].UpdateCurrencyUI();
                players[currentPlayerIndex].UpdateMovesUI();
                players[currentPlayerIndex].UpdateWoodUI();
                players[currentPlayerIndex].UpdatePlayerNumberUI();
            } else
            {
                Debug.Log("Player One not found in the scene");
            }
        } else
        {
            Debug.LogError("No players found in the scene!");
        }
    }

    // Activated when the 'End Turn' button is clicked
    public void EndTurn()
    {
        turnCount++;

        if (players.Count == 0)
        {
            return;
        }

        if (players[currentPlayerIndex].infection > 0) {
            int newCurrency = players[currentPlayerIndex].currency - players[currentPlayerIndex].infection;
            if (newCurrency >= 0) {
                players[currentPlayerIndex].currency = newCurrency;
            } else {
                players[currentPlayerIndex].currency = 0;
            }
            players[currentPlayerIndex].UpdateCurrencyUI();
        }

        if (turnCount % players.Count == 0) {
            turnCount = 0;
            StartCoroutine(SpreadBlight());
        }

        // Disable controls for the current player
        players[currentPlayerIndex].SetInputState(false);
        players[currentPlayerIndex].turn = false;

        currentPlayerIndex = (currentPlayerIndex + 1) % players.Count;

        players[currentPlayerIndex].moves += 5;

        // Enable controls for the new active player
        players[currentPlayerIndex].SetInputState(true);
        players[currentPlayerIndex].turn = true;

        int numOfSettlements = players[currentPlayerIndex].settlements;
        int numOfFarms = players[currentPlayerIndex].farms;
        int numOfBookshelves = players[currentPlayerIndex].bookshelves;

        // For each settlement a player has, they get 3 coins at the start of each turn
        for (int i = 0; i < numOfSettlements; i++)
        {
            players[currentPlayerIndex].currency += 3;
        }

        for (int i = 0; i < numOfFarms; i++)
        {
            players[currentPlayerIndex].currency += 3;
        }

        for (int i = 0; i < numOfBookshelves; i++)
        {
            players[currentPlayerIndex].moves += 3;
        }

        players[currentPlayerIndex].UpdateCurrencyUI();
        players[currentPlayerIndex].UpdateWoodUI();
        players[currentPlayerIndex].UpdateFoodUI();
        players[currentPlayerIndex].UpdateMovesUI();
        players[currentPlayerIndex].UpdateBookshelfUI();
        UpdateTurnUI();
    }

    public void TryBuildHouse()
    {
        if (players[currentPlayerIndex].wood >= players[currentPlayerIndex].settlementWoodCost)
        {
            Player_Controller player = players[currentPlayerIndex];

            Vector3 playerPosition = player.transform.position;
            Vector3 tilePosition = player.groundTilemap.GetCellCenterWorld(player.groundTilemap.WorldToCell(playerPosition));

            if (settlementPositions.Contains(tilePosition) || farmPositions.Contains(tilePosition) || bookshelfPositions.Contains(tilePosition))
            {
                Debug.Log("Cannot Build Here! Something is Already Placed on This Tile!");
                return;
            }

            Instantiate(settlements[player.playerNumber - 1], tilePosition, Quaternion.identity);

            players[currentPlayerIndex].settlements += 1;
            players[currentPlayerIndex].wood -= players[currentPlayerIndex].settlementWoodCost;
            players[currentPlayerIndex].UpdateWoodUI();
            settlementPositions.Add(tilePosition);
        }
        else
        {
            Debug.Log("Not enough wood to build!");

            notEnoughWoodPanel.SetActive(true);
            StartCoroutine(HideWoodPanelAfterDelay(2f));
        }
    }

    public void TryBuildFarm()
    {
        if (players[currentPlayerIndex].food >= players[currentPlayerIndex].farmMeatCost)
        {
            Player_Controller player = players[currentPlayerIndex];

            Vector3 playerPosition = player.transform.position;
            Vector3 tilePosition = player.groundTilemap.GetCellCenterWorld(player.groundTilemap.WorldToCell(playerPosition));

            if (settlementPositions.Contains(tilePosition) || farmPositions.Contains(tilePosition) || bookshelfPositions.Contains(tilePosition))
            {
                Debug.Log("Cannot Build Here! Something is Already Placed on This Tile!");
                return;
            }

            Instantiate(farms[player.playerNumber - 1], tilePosition, Quaternion.identity);

            players[currentPlayerIndex].farms += 1;
            players[currentPlayerIndex].food -= players[currentPlayerIndex].farmMeatCost;
            players[currentPlayerIndex].UpdateFoodUI();
            farmPositions.Add(tilePosition);
        }
        else
        {
            Debug.Log("Not enough meat to build!");

            notEnoughMeatPanel.SetActive(true);
            StartCoroutine(HideFoodPanelAfterDelay(2f));
        }
    }

    public void TryBuildBookshelf()
    {
        if (players[currentPlayerIndex].currency >= players[currentPlayerIndex].bookshelfGoldCost)
        {
            Player_Controller player = players[currentPlayerIndex];

            Vector3 playerPosition = player.transform.position;
            Vector3 tilePosition = player.groundTilemap.GetCellCenterWorld(player.groundTilemap.WorldToCell(playerPosition));

            if (settlementPositions.Contains(tilePosition) || farmPositions.Contains(tilePosition) || bookshelfPositions.Contains(tilePosition))
            {
                Debug.Log("Cannot Build Here! Something is Already Placed on This Tile!");
                return;
            }

            Instantiate(bookshelves[player.playerNumber - 1], tilePosition, Quaternion.identity);

            players[currentPlayerIndex].bookshelves += 1;
            players[currentPlayerIndex].currency -= players[currentPlayerIndex].bookshelfGoldCost;

            // Play story fragment based on how many are built
            if (players[currentPlayerIndex].bookshelves <= apocalypseStory.Length)
            {
                StartCoroutine(DisplayStory(apocalypseStory[players[currentPlayerIndex].bookshelves - 1]));
            }
                
            players[currentPlayerIndex].UpdateCurrencyUI();
            players[currentPlayerIndex].UpdateBookshelfUI();
            bookshelfPositions.Add(tilePosition);
        }
        else
        {
            Debug.Log("Not enough gold to build!");

            notEnoughGoldPanel.SetActive(true);
            StartCoroutine(HideGoldPanelAfterDelay(2f));
        }
    }

    private IEnumerator DisplayStory(string storyText)
    {
        storyUI.text = "";
        storyPanel.SetActive(true);

        
        foreach (char letter in storyText.ToCharArray())
        {
            storyUI.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }        

        StartCoroutine(HideStoryPanelAfterDelay(2f));
    }

    private IEnumerator HideStoryPanelAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (storyPanel != null)
        {
            storyPanel.SetActive(false);
        }
    }

    private IEnumerator HideWoodPanelAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (notEnoughWoodPanel != null)
        {
            notEnoughWoodPanel.SetActive(false);
        }
    }

    private IEnumerator HideFoodPanelAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (notEnoughMeatPanel != null)
        {
            notEnoughMeatPanel.SetActive(false);
        }
    }

    private IEnumerator HideGoldPanelAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (notEnoughGoldPanel != null)
        {
            notEnoughGoldPanel.SetActive(false);
        }
    }

    // Updates the UI to show whose turn it is
    public void UpdateTurnUI()
    {
        if (turnText != null)
        {
            turnText.text = "Player: " + (players[currentPlayerIndex].playerNumber);
        }
    }
    public void GoToScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
    
    public IEnumerator SpreadBlight() {
        int currentQueueSize = spreadQueue.Count;

        // Process all tiles in the current cycle (all tiles that need to spread this turn)
        for (int i = 0; i < currentQueueSize; i++)
        {
            Vector3Int currentTile = spreadQueue.Dequeue();

            // Change the current tile's color (or perform any modification you want)
            TileBase tileBase = tilemap.GetTile(currentTile);
            if (tileBase != null)
            {
                Tile currentTileObj = tileBase as Tile;
                if (currentTileObj != null)
                {
                    Tile newTile = ScriptableObject.CreateInstance<Tile>();
                    newTile.sprite = currentTileObj.sprite;  // Keep the same sprite
                    newTile.color = changeColor;             // Set the new color

                    // Set the new tile at the current position
                    tilemap.SetTile(currentTile, newTile);
                }
            }

            // Expand to adjacent tiles (up, down, left, right, and diagonals)
            ExpandToAdjacentTiles(currentTile);
        }
            yield return null;
    }

    void ExpandToAdjacentTiles(Vector3Int currentTile) {
        // Directions: up, down, left, and right
        Vector3Int[] directions = new Vector3Int[]
        {
            Vector3Int.up,         // Up
            Vector3Int.down,       // Down
            Vector3Int.left,       // Left
            Vector3Int.right      // Right
        };

        Vector3Int direction1 = directions[Random.Range(0, directions.Length)];
        Vector3Int direction2;

        do {
            direction2 = directions[Random.Range(0, directions.Length)];
        } while (direction1 == direction2);

        Vector3Int[] chosenDirections = { direction1, direction2 };

        // Iterate over each direction and add the neighboring tile to the spread queue
        foreach (var direction in chosenDirections)
        {
            Vector3Int neighborPosition = currentTile + direction;

            // Check if the tile is already visited or if it's outside the Tilemap bounds
            if (!visitedTiles.Contains(neighborPosition) && tilemap.HasTile(neighborPosition))
            {
                // Add the neighbor tile to the spread queue and mark it as visited
                spreadQueue.Enqueue(neighborPosition);
                visitedTiles.Add(neighborPosition);
            }
        }
    }
}
