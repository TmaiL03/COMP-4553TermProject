using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLogicScript : MonoBehaviour
{
    [Header("Game Settings")]
    public int NumberOfPlayers;

    void Awake()
    {
        NumberOfPlayers = MainMenu.playerCount;
        Debug.Log("Number of Players Set to: " + NumberOfPlayers);
    }
}
