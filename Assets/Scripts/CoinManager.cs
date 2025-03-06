using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinManager : MonoBehaviour
{
    public PlayerCurrency playerCurrency;
    public Player player;

    public void AddCoins(int amount)
    {
        player.AddCoins(amount);
        playerCurrency.SetCurrency(player);
    }
}
