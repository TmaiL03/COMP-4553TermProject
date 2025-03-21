using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Tilemaps;
using UnityEngine.SceneManagement;

[SelectionBase]
public class Player_Controller : MonoBehaviour
{
    #region Editor Data

    public Tilemap groundTilemap;
    [SerializeField] Tilemap oceanTilemap;
    public GameObject settlementPrefab;
    public GameObject farmPrefab;
    public GameObject bookshelfPrefab;

    [Header("Player Info")]
    public int playerNumber;
    public bool turn;

    [Header("Player Inventory")]
    public int currency = 0;
    public int moves = 0;
    public int wood = 0;
    public int food = 0;
    public int settlements = 0;
    public int farms = 0;
    public int bookshelves = 0;
    public int infection = 0;

    [Header("Movement Attributes")]
    [SerializeField] float _moveSpeed = 5f;
    
    [Header("Dependencies")]
    [SerializeField] Rigidbody2D _rb;

    int winNumOfBookshelves = 3; 
    public int settlementWoodCost = 10;
    public int farmMeatCost = 10;
    public int bookshelfGoldCost = 10;

    private PlayerCurrency playerCurrencyUI;
    private PlayerNumber playerNumberUI;
    private PlayerMoves playerMovesUI;
    private PlayerWood playerWoodUI;
    private PlayerFood playerFoodUI;
    private PlayerBookshelf playerBookshelfUI;

    private PlayerMovement controls;

    #endregion

    private void Awake() {
        controls = new PlayerMovement();
    }

    private void OnEnable() {
        controls.Enable();
    }

    private void OnDisable() {
        controls.Disable();
    }

    // Enables and disables player controls for tunr based control
    public void SetInputState(bool isActive) {
        if (isActive)
        {
            controls.Enable();
        }
        else
        {
            controls.Disable();
        }
    }

    private Animator animator;
    private void Start() {

        animator = GetComponent<Animator>();

        // gets refrences to the ocean and ground tile maps as they are dynamic
        // Make global variable in gamelogic script?
        groundTilemap = GameObject.Find("Ground")?.GetComponent<Tilemap>();
        oceanTilemap = GameObject.Find("Ocean")?.GetComponent<Tilemap>();

        // Using unities built in system input, no need for movepoint
        controls.Main.Movement.performed += ctx => Move(ctx.ReadValue<Vector2>());

        playerCurrencyUI = FindObjectOfType<PlayerCurrency>();
        playerNumberUI = FindObjectOfType<PlayerNumber>();
        playerMovesUI = FindObjectOfType<PlayerMoves>();
        playerWoodUI = FindObjectOfType<PlayerWood>();
        playerFoodUI = FindObjectOfType<PlayerFood>();
        playerBookshelfUI = FindObjectOfType<PlayerBookshelf>();

        UpdateCurrencyUI();
        UpdatePlayerNumberUI();
        UpdateMovesUI();

        SetInputState(turn);
    }

    // Moves player
    private void Move(Vector2 direction) {
        if (CanMove(direction)) {
            toggleInfection();
            if (direction.x > 0)
            {
                transform.localScale = new Vector3(-0.75f, 0.75f, 0.75f);
                transform.Find("Name").localScale = new Vector3(-1.333333f, 1.333333f, 1.333333f);

            }
            else
            {
                transform.localScale = new Vector3(0.75f, 0.75f, 0.75f);
                transform.Find("Name").localScale = new Vector3(1.333333f, 1.333333f, 1.333333f);
            }

            move();

            StartCoroutine(MoveSmooth(transform.position + (Vector3)direction));
        }
    }

    private IEnumerator MoveSmooth(Vector3 targetPosition)
    {
        if (animator.GetBool("isWalking")) yield break;

        animator.SetBool("isWalking", true);

        while (Vector3.Distance(transform.position, targetPosition) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, _moveSpeed * Time.deltaTime);
            yield return null;
        }

        transform.position = targetPosition;

        animator.SetBool("isWalking", false);
    }

    // Checks player is allowed to move there
    private bool CanMove(Vector2 direction) {
        Vector3Int gridPosition = groundTilemap.WorldToCell(transform.position + (Vector3)direction);

        if (!groundTilemap.HasTile(gridPosition) || oceanTilemap.HasTile(gridPosition)) {
            return false;
        } else 
        {
            if (moves > 0) {
                if (groundTilemap.GetColor(gridPosition) == Color.red && currency > 0) {
                    infection += 1;
                } else if (!(groundTilemap.GetColor(gridPosition) == Color.red) && infection > 0) {
                    infection -= 1;
                }
                return true;
            } else
                return false;
        }
    }

    public void toggleInfection() {
        GameObject infectionLevel = transform.Find("Body/Infection").gameObject;
        if (infection > 0) {
            TextMeshPro infectionText = infectionLevel.GetComponentInChildren<TextMeshPro>();
            infectionText.text = infection.ToString();
            infectionLevel.SetActive(true);
        } else {
            infectionLevel.SetActive(false);
        }
    }

    public void UpdateCurrencyUI()
    {
        if (playerCurrencyUI != null)
        {
            playerCurrencyUI.UpdateCurrencyUI(currency);
        }
    }

    public void UpdateWoodUI()
    {
        if (playerWoodUI != null)
        {
            playerWoodUI.UpdateWoodUI(wood);
        }
    }

    public void UpdateFoodUI()
    {
        if (playerFoodUI != null)
        {
            playerFoodUI.UpdateFoodUI(food);
        }
    }

    public void UpdateBookshelfUI()
    {
        if (playerBookshelfUI != null)
        {
            playerBookshelfUI.UpdateBookshelfUI(bookshelves);
        }

        if (bookshelves >= winNumOfBookshelves)
        {
            SaveScores();

            StartCoroutine(WaitAndGoToGameOver(12f));
        }
    }

    private IEnumerator WaitAndGoToGameOver(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        GoToScene("GameOverScript");
    }

    public void SaveScores()
    {
        for (int i = 0; i < FindObjectsOfType<Player_Controller>().Length; i++)
        {
            Player_Controller player = FindObjectsOfType<Player_Controller>()[i];
            int score = player.bookshelves;
            PlayerPrefs.SetInt("Player" + player.playerNumber + "_Score", score);
        }

        PlayerPrefs.SetInt("TotalPlayers", FindObjectsOfType<Player_Controller>().Length);
        PlayerPrefs.Save();
    }

    public void UpdatePlayerNumberUI()
    {
        if (playerNumberUI != null)
        {
            playerNumberUI.UpdatePlayerNumberUI(playerNumber);
        }
    }

    public void UpdateMovesUI()
    {
        if (playerMovesUI != null)
        {
            playerMovesUI.UpdateMovesUI(moves);
        }
    }

    #region Internal Data

    // Prefixed "_" denotes private/protected field to this class.
    private Vector2 _moveDir = Vector2.zero;

    #endregion
    
    #region Movement Logic

    public void addScore(int amount)
    {
        currency += amount;
        UpdateCurrencyUI();
    }

    public void addWood(int amount)
    {
        wood += amount;
        UpdateWoodUI();
    }

    public void addFood(int amount)
    {
        food += amount;
        UpdateFoodUI();
    }

    public void move()
    {
        moves --;
        UpdateMovesUI();
    }

    public void endTurn()
    {
        UpdatePlayerNumberUI();
        turn = false;
    }

    public void GoToScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    #endregion 

}
