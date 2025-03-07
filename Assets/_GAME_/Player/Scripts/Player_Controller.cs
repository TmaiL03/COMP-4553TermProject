using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Tilemaps;

[SelectionBase]
public class Player_Controller : MonoBehaviour
{
    #region Editor Data

    [SerializeField] Tilemap groundTilemap;
    [SerializeField] Tilemap oceanTilemap;

    [Header("Player Info")]
    [SerializeField] public int playerNumber;
    [SerializeField] public bool turn;

    [Header("Player Inventory")]
    [SerializeField] public int victoryPoints = 0;
    [SerializeField] int developmentPoints = 0;

    [Header("Movement Attributes")]
    [SerializeField] float _moveSpeed = 5f;
    
    [Header("Dependencies")]
    [SerializeField] Rigidbody2D _rb;

    private PlayerCurrency playerCurrencyUI;
    private PlayerNumber playerNumberUI;

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

        UpdateCurrencyUI();
        UpdatePlayerNumberUI();

        SetInputState(turn);
    }

    // Moves player
    private void Move(Vector2 direction) {
        if (CanMove(direction)) {
            transform.position += (Vector3)direction;
        }
    }

    // Checks player is allowed to move there
    private bool CanMove(Vector2 direction) {
        Vector3Int gridPosition = groundTilemap.WorldToCell(transform.position + (Vector3)direction);

        if (!groundTilemap.HasTile(gridPosition) || oceanTilemap.HasTile(gridPosition)) {
            return false;
        } else { return true; }
    }

    public void UpdateCurrencyUI()
    {
        if (playerCurrencyUI != null)
        {
            playerCurrencyUI.UpdateCurrencyUI(victoryPoints);
        }
    }
    public void UpdatePlayerNumberUI()
    {
        if (playerNumberUI != null)
        {
            playerNumberUI.UpdatePlayerNumberUI(playerNumber);
        }
    }

    #region Internal Data

    // Prefixed "_" denotes private/protected field to this class.
    private Vector2 _moveDir = Vector2.zero;

    #endregion
    
    #region Movement Logic

    public void addScore(int amount)
    {
        victoryPoints += amount;
        UpdateCurrencyUI();
    }

    public void endTurn()
    {
        UpdatePlayerNumberUI();
        turn = false;
    }

    #endregion 

}
