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

    [SerializeField] Tilemap groundTilemap;
    [SerializeField] Tilemap oceanTilemap;

    [Header("Player Info")]
    public int playerNumber;
    public bool turn;

    [Header("Player Inventory")]
    public int currency = 0;
    public int moves = 0;

    [Header("Movement Attributes")]
    [SerializeField] float _moveSpeed = 5f;
    
    [Header("Dependencies")]
    [SerializeField] Rigidbody2D _rb;

    [SerializeField] int winCurrency = 20;

    private PlayerCurrency playerCurrencyUI;
    private PlayerNumber playerNumberUI;
    private PlayerMoves playerMovesUI;

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

    
    private void Start() {
        // gets refrences to the ocean and ground tile maps as they are dynamic
        // Make global variable in gamelogic script?
        groundTilemap = GameObject.Find("Ground")?.GetComponent<Tilemap>();
        oceanTilemap = GameObject.Find("Ocean")?.GetComponent<Tilemap>();

        // Using unities built in system input, no need for movepoint
        controls.Main.Movement.performed += ctx => Move(ctx.ReadValue<Vector2>());

        playerCurrencyUI = FindObjectOfType<PlayerCurrency>();
        playerNumberUI = FindObjectOfType<PlayerNumber>();
        playerMovesUI = FindObjectOfType<PlayerMoves>();

        UpdateCurrencyUI();
        UpdatePlayerNumberUI();
        UpdateMovesUI();

        SetInputState(turn);
    }

    // Moves player
    private void Move(Vector2 direction) {
        if (CanMove(direction)) {
            transform.position += (Vector3)direction;
            move();
        }
    }

    // Checks player is allowed to move there
    private bool CanMove(Vector2 direction) {
        Vector3Int gridPosition = groundTilemap.WorldToCell(transform.position + (Vector3)direction);

        if (!groundTilemap.HasTile(gridPosition) || oceanTilemap.HasTile(gridPosition)) {
            return false;
        } else 
        {
            if (moves > 0)
                return true;
            else
                return false;
        }
    }

    public void UpdateCurrencyUI()
    {
        if (playerCurrencyUI != null)
        {
            playerCurrencyUI.UpdateCurrencyUI(currency);

            if (currency >= winCurrency)
            {
                SaveScores();
                GoToScene("GameOver");
            }
        }
    }

    public void SaveScores()
    {
        for (int i = 0; i < FindObjectsOfType<Player_Controller>().Length; i++)
        {
            Player_Controller player = FindObjectsOfType<Player_Controller>()[i];
            int score = player.currency;
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
